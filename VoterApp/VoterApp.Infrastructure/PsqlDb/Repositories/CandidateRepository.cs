using System.Data;
using Dapper;
using VoterApp.Application.Contracts;
using VoterApp.Application.Features.Candidates.Commands.CreateCandidate;
using VoterApp.Application.Features.Candidates.Commands.UpdateCandidate;
using VoterApp.Domain.Entities;

namespace VoterApp.Infrastructure.PsqlDb.Repositories;

public class CandidateRepository : ICandidateRepository
{
    private readonly IPsqlDbContext _psqlDbContext;

    public CandidateRepository(IPsqlDbContext psqlDbContext) => _psqlDbContext = psqlDbContext;

    public async Task<Candidate?> Get(int id, IDbTransaction? transaction = null)
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
            var candidates = (await connection.QueryAsync<Candidate, Election, Voter, Candidate>(
                sql,
                (candidate, election, voter) => MapCandidate(candidateDictionary, candidate, election, voter),
                new { id },
                transaction,
                splitOn: "Id,Id"
            )).Distinct().ToList();

            return candidates.FirstOrDefault();
        }
        catch (InvalidOperationException ex) when (ex.Message is "Sequence contains no elements")
        {
            return null;
        }
    }

    public async Task<IEnumerable<Candidate>> GetAll(IDbTransaction? transaction = null)
    {
        var sql = @"
                SELECT c.*, e.*, v.*
                FROM Candidates c
                JOIN Elections e ON c.ElectionId = e.Id
                LEFT JOIN Voters v ON c.Id = v.VotedCandidateId;";

        using var connection = _psqlDbContext.CreateConnection();

        var candidateDictionary = new Dictionary<int, Candidate>();

        var candidates = (await connection.QueryAsync<Candidate, Election, Voter?, Candidate>(
            sql,
            (candidate, election, voter) => MapCandidate(candidateDictionary, candidate, election, voter),
            transaction: transaction,
            splitOn: "Id,Id"
        )).Distinct().ToList();

        return candidates;
    }

    public async Task<IEnumerable<Candidate>> GetAll(int electionId, IDbTransaction? transaction = null)
    {
        var sql = @"
                SELECT c.*, e.*, v.*
                FROM Candidates c
                JOIN Elections e ON c.ElectionId = e.Id
                LEFT JOIN Voters v ON c.Id = v.VotedCandidateId
                WHERE e.Id = @ElectionId";

        using var connection = _psqlDbContext.CreateConnection();

        var candidateDictionary = new Dictionary<int, Candidate>();

        var candidates = (await connection.QueryAsync<Candidate, Election, Voter?, Candidate>(
            sql,
            (candidate, election, voter) => MapCandidate(candidateDictionary, candidate, election, voter),
            new { electionId },
            transaction,
            splitOn: "Id,Id"
        )).Distinct().ToList();

        return candidates;
    }

    public async Task<int> Create(CreateCandidateCommand createCommand, IDbTransaction? transaction = null)
    {
        var sql = @"INSERT INTO Candidates(Name, ElectionId) VALUES (@Name, @ElectionId) RETURNING Id";

        using var connection = _psqlDbContext.CreateConnection();

        var id = await connection.ExecuteScalarAsync(sql, createCommand, transaction);

        return (int)(id ?? 0);
    }

    public async Task Update(UpdateCandidateCommand updateCommand, IDbTransaction? transaction = null)
    {
        var sql = @"UPDATE Candidates
                        SET Name = @Name
                        WHERE Id = @Id";

        using var connection = _psqlDbContext.CreateConnection();

        await connection.ExecuteAsync(sql, updateCommand, transaction);
    }

    public async Task Delete(int id, IDbTransaction? transaction = null)
    {
        var sql = "DELETE FROM Candidates WHERE Id = @Id";

        var connection = _psqlDbContext.CreateConnection();

        await connection.ExecuteAsync(sql, new { id }, transaction);
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