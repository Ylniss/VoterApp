using AutoMapper;
using Dapper;
using VoterApp.Application.Contracts;
using VoterApp.Application.Features.Candidates.Commands.CreateCandidate;
using VoterApp.Application.Features.Candidates.Commands.UpdateCandidate;
using VoterApp.Application.Features.Candidates.Dtos;
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
        var sql = "SELECT id, name FROM candidates WHERE id = @Id;";

        using var connection = _psqlDbContext.CreateConnection();

        try
        {
            var candidate = await connection.QueryFirstAsync<CandidateDto>(sql, new { id });
            return _mapper.Map<Candidate>(candidate);
        }
        catch (InvalidOperationException ex) when (ex.Message is "Sequence contains no elements")
        {
            return null;
        }
    }

    public async Task<IEnumerable<Candidate>> GetAll()
    {
        var sql = "SELECT id, name FROM candidates";

        using var connection = _psqlDbContext.CreateConnection();

        var candidates = await connection.QueryAsync<CandidateDto>(sql);

        return _mapper.Map<IEnumerable<Candidate>>(candidates);
    }

    public async Task<int> Create(CreateCandidateCommand createCommand)
    {
        var sql = @"INSERT INTO candidates(name) VALUES (@Name) RETURNING Id";

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
}