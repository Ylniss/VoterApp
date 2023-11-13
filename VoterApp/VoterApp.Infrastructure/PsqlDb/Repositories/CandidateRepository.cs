using AutoMapper;
using Dapper;
using VoterApp.Application.Contracts;
using VoterApp.Application.Features.Candidates.Commands.CreateCandidate;
using VoterApp.Application.Features.Candidates.Commands.UpdateCandidate;
using VoterApp.Domain.Entities;

namespace VoterApp.Infrastructure.PsqlDb.Repositories;

public class CandidateRepository : ICandidateRepository
{
    private readonly IMapper _mapper;
    private readonly IPsqlDbContext _psqlDbContext;

    public CandidateRepository(IPsqlDbContext psqlDbContext, IMapper mapper)
    {
        _psqlDbContext = psqlDbContext;
        _mapper = mapper;
    }

    public async Task<Candidate?> Get(int id)
    {
        var sql = @"
                    SELECT c.*, e.*, v.*
                    FROM Candidates c
                    JOIN Elections e ON c.ElectionId = e.Id
                    JOIN Voters v ON c.Id = v.VotedCandidateId
                    WHERE c.Id = @Id;
                    ";

        using var connection = _psqlDbContext.CreateConnection();

        var candidateDictionary = new Dictionary<int, Candidate>();

        try
        {
            var candidates = connection.Query<Candidate, Election, Voter, Candidate>(
                sql,
                (candidate, election, voter) => MapCandidate(candidateDictionary, candidate, election, voter),
                new { id },
                splitOn: "Id,Id"
            ).Distinct().ToList();

            return candidates.FirstOrDefault();
        }
        catch (InvalidOperationException ex) when (ex.Message is "Sequence contains no elements")
        {
            return null;
        }
    }

    public async Task<IEnumerable<Candidate>> GetAll()
    {
        var sql = @"
                SELECT c.*, e.*, v.*
                FROM Candidates c
                JOIN Elections e ON c.ElectionId = e.Id
                LEFT JOIN Voters v ON c.Id = v.VotedCandidateId;";

        using var connection = _psqlDbContext.CreateConnection();

        var candidateDictionary = new Dictionary<int, Candidate>();

        var candidates = connection.Query<Candidate, Election, Voter?, Candidate>(
            sql,
            (candidate, election, voter) => MapCandidate(candidateDictionary, candidate, election, voter),
            splitOn: "Id,Id"
        ).Distinct().ToList();

        return candidates;
    }

    public async Task<int> Create(CreateCandidateCommand createCommand)
    {
        var sql = @"INSERT INTO candidates(name, electionid) VALUES (@Name, @ElectionId) RETURNING Id";

        using var connection = _psqlDbContext.CreateConnection();

        var id = await connection.ExecuteScalarAsync(sql, createCommand);

        return (int)(id ?? 0);
    }

    public async Task Update(UpdateCandidateCommand updateCommand)
    {
        var sql = @"UPDATE candidates
                        SET name = @Name
                        WHERE id = @Id";

        using var connection = _psqlDbContext.CreateConnection();

        await connection.ExecuteAsync(sql, updateCommand);
    }

    public async Task Delete(int id)
    {
        var sql = "DELETE FROM candidates WHERE id = @Id";

        var connection = _psqlDbContext.CreateConnection();

        await connection.ExecuteAsync(sql, new { id });
    }

    private Candidate MapCandidate(Dictionary<int, Candidate> candidateDictionary, Candidate candidate,
        Election election, Voter? voter)
    {
        if (!candidateDictionary.TryGetValue(candidate.Id, out var candidateEntry))
        {
            candidateEntry = candidate;
            candidateEntry.Election = election;
            election.Candidates.Add(candidateEntry);
            candidateDictionary.Add(candidateEntry.Id, candidateEntry);
        }

        // Check for null in case of LEFT JOIN (if a candidate has no voters)
        if (voter != null)
        {
            candidateEntry.Voters.Add(voter);
            voter.VotedCandidate = candidate;
            voter.Election = candidateEntry.Election;
        }

        return candidateEntry;
    }
}