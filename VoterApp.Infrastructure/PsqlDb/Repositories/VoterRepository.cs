using System.Data;
using Dapper;
using VoterApp.Application.Contracts;
using VoterApp.Application.Features.Voters.Commands.CreateVoter;
using VoterApp.Application.Features.Voters.Commands.UpdateVoterName;
using VoterApp.Domain.Entities;

namespace VoterApp.Infrastructure.PsqlDb.Repositories;

public class VoterRepository : IVoterRepository
{
    private readonly IPsqlDbContext _psqlDbContext;

    public VoterRepository(IPsqlDbContext psqlDbContext) => _psqlDbContext = psqlDbContext;

    public async Task<Voter?> Get(int id, IDbTransaction? transaction = null)
    {
        var sql = @"
                SELECT v.*, e.*, c.*
                FROM Voters v
                JOIN Elections e ON v.ElectionId = e.Id
                LEFT JOIN Candidates c ON c.Id = v.VotedCandidateId
                WHERE v.Id = @Id";

        using var connection = _psqlDbContext.CreateConnection();

        var voterDictionary = new Dictionary<int, Voter>();

        try
        {
            var voters = (await connection.QueryAsync<Voter, Election, Candidate, Voter>(
                sql,
                (voter, election, candidate) => MapVoter(voterDictionary, voter, election, candidate),
                new { id },
                transaction,
                splitOn: "Id,Id"
            )).Distinct().ToList();

            return voters.FirstOrDefault();
        }
        catch (InvalidOperationException ex) when (ex.Message is "Sequence contains no elements")
        {
            return null;
        }
    }

    public async Task<IEnumerable<Voter>> GetAll(IDbTransaction? transaction = null)
    {
        var sql = @"
                SELECT v.*, e.*, c.*
                FROM Voters v
                JOIN Elections e ON v.ElectionId = e.Id
                LEFT JOIN Candidates c ON c.Id = v.VotedCandidateId;";

        using var connection = _psqlDbContext.CreateConnection();

        var voterDictionary = new Dictionary<int, Voter>();

        var voters = (await connection.QueryAsync<Voter, Election, Candidate?, Voter>(
            sql,
            (voter, election, candidate) => MapVoter(voterDictionary, voter, election, candidate),
            transaction: transaction,
            splitOn: "Id,Id"
        )).Distinct().ToList();

        return voters;
    }

    public async Task<IEnumerable<Voter>> GetAll(int electionId, IDbTransaction? transaction = null)
    {
        var sql = @"
                SELECT v.*, e.*, c.*
                FROM Voters v
                JOIN Elections e ON v.ElectionId = e.Id
                LEFT JOIN Candidates c ON c.Id = v.VotedCandidateId
                WHERE e.Id = @ElectionId;";

        using var connection = _psqlDbContext.CreateConnection();

        var voterDictionary = new Dictionary<int, Voter>();

        var voters = (await connection.QueryAsync<Voter, Election, Candidate?, Voter>(
            sql,
            (voter, election, candidate) => MapVoter(voterDictionary, voter, election, candidate),
            new { electionId },
            transaction,
            splitOn: "Id,Id"
        )).Distinct().ToList();

        return voters;
    }

    public async Task<int> Create(CreateVoterCommand createCommand, IDbTransaction? transaction = null)
    {
        var sql =
            @"INSERT INTO Voters(Name, ElectionId, KeyPhrase) VALUES (@Name, @ElectionId, @KeyPhrase) RETURNING Id";

        using var connection = _psqlDbContext.CreateConnection();

        var id = await connection.ExecuteScalarAsync(sql, createCommand, transaction);

        return (int)(id ?? 0);
    }

    public async Task Update(UpdateVoterCommand updateCommand, IDbTransaction? transaction = null)
    {
        var sql = @"UPDATE Voters
                        SET Name = @Name,
                            VotedCandidateId = @VotedCandidateId
                        WHERE Id = @Id";

        using var connection = _psqlDbContext.CreateConnection();

        await connection.ExecuteAsync(sql, updateCommand, transaction);
    }

    public async Task Delete(int id, IDbTransaction? transaction = null)
    {
        var sql = "DELETE FROM Voters WHERE Id = @Id";

        var connection = _psqlDbContext.CreateConnection();

        await connection.ExecuteAsync(sql, new { id }, transaction);
    }

    private Voter MapVoter(Dictionary<int, Voter> voterDictionary, Voter voter,
        Election election, Candidate? candidate)
    {
        if (!voterDictionary.TryGetValue(voter.Id, out var voterEntry))
        {
            voterEntry = voter;
            voterEntry.Election = election;
            election.Voters.Add(voterEntry);
            voterDictionary.Add(voterEntry.Id, voterEntry);
        }

        voter.VotedCandidate = candidate;
        voter.Election = voterEntry.Election;

        return voterEntry;
    }
}