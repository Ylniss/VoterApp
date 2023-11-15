using System.Data;
using Dapper;
using VoterApp.Application.Contracts;
using VoterApp.Application.Features.Elections.Commands.CreateElection;
using VoterApp.Domain.Entities;

namespace VoterApp.Infrastructure.PsqlDb.Repositories;

public class ElectionRepository : IElectionRepository
{
    private readonly ICandidateRepository _candidateRepository;
    private readonly IPsqlDbContext _psqlDbContext;
    private readonly IVoterRepository _voterRepository;

    public ElectionRepository(IPsqlDbContext psqlDbContext, ICandidateRepository candidateRepository,
        IVoterRepository voterRepository)
    {
        _psqlDbContext = psqlDbContext;
        _candidateRepository = candidateRepository;
        _voterRepository = voterRepository;
    }

    public async Task<Election?> Get(int id, IDbTransaction? transaction = null)
    {
        var sql = @"
                    SELECT e.*, c.*, v.*
                    FROM Elections e
                    LEFT JOIN Candidates c ON c.ElectionId = e.Id
                    LEFT JOIN Voters v ON v.ElectionId = e.Id
                    WHERE e.Id = @Id;
                    ";

        using var connection = _psqlDbContext.CreateConnection();

        var electionDictionary = new Dictionary<int, Election>();

        try
        {
            var elections =
                (await connection.QueryAsync(
                    sql,
                    MapElection(electionDictionary),
                    new { id },
                    splitOn: "Id,Id"
                )).Distinct().ToList();

            var election = elections.FirstOrDefault();

            if (election == null) return election;

            election.Candidates = (await _candidateRepository.GetAll(id)).ToList();
            election.Voters = (await _voterRepository.GetAll(id)).ToList();

            return election;
        }
        catch (InvalidOperationException ex) when (ex.Message is "Sequence contains no elements")
        {
            return null;
        }
    }

    public async Task<Election?> GetByRoomCode(Guid roomCode, IDbTransaction? transaction = null)
    {
        var sql = @"
                    SELECT e.*, c.*, v.*
                    FROM Elections e
                    LEFT JOIN Candidates c ON c.ElectionId = e.Id
                    LEFT JOIN Voters v ON v.ElectionId = e.Id
                    WHERE e.RoomCode = @RoomCode;
                    ";

        using var connection = _psqlDbContext.CreateConnection();

        var electionDictionary = new Dictionary<int, Election>();

        try
        {
            var elections =
                (await connection.QueryAsync(
                    sql, MapElection(electionDictionary),
                    new { roomCode },
                    splitOn: "Id,Id"
                )).Distinct().ToList();

            var election = elections.FirstOrDefault();

            if (election == null) return election;

            election.Candidates = (await _candidateRepository.GetAll(election.Id)).ToList();
            election.Voters = (await _voterRepository.GetAll(election.Id)).ToList();

            return election;
        }
        catch (InvalidOperationException ex) when (ex.Message is "Sequence contains no elements")
        {
            return null;
        }
    }

    public async Task<int> Create(CreateElectionCommand createCommand, IDbTransaction? transaction = null)
    {
        var sql = "INSERT INTO Elections(Topic, Archived) VALUES (@Topic, false) RETURNING Id";

        using var connection = _psqlDbContext.CreateConnection();

        var id = await connection.ExecuteScalarAsync(sql, createCommand, transaction);

        return (int)(id ?? 0);
    }

    private static Func<Election, Candidate?, Voter?, Election> MapElection(
        Dictionary<int, Election> electionDictionary)
    {
        return (election, candidate, voter) =>
        {
            if (!electionDictionary.TryGetValue(election.Id, out var electionEntry))
            {
                electionEntry = election;
                electionDictionary.Add(electionEntry.Id, electionEntry);
            }

            if (voter == null) return electionEntry;

            electionEntry.Voters.Add(voter);
            voter.Election = election;
            voter.VotedCandidate = candidate;

            return electionEntry;
        };
    }
}