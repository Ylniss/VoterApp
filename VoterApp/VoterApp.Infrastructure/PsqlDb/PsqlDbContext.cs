using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using VoterApp.Infrastructure.Parsers;

namespace VoterApp.Infrastructure.PsqlDb;

public class PsqlDbContext : IPsqlDbContext
{
    private readonly string? _connectionString;
    private readonly string _dbName;

    public PsqlDbContext(IConfiguration config, IConnectionStringParser connectionStringParser)
    {
        _connectionString = config.GetConnectionString("PsqlConnection");
        _dbName = connectionStringParser.GetValue("Database", _connectionString);
    }

    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }

    public async Task Init()
    {
        await CreateDatabaseIfDoesntExist();
        await CreateTables();
    }

    private async Task CreateDatabaseIfDoesntExist()
    {
        using var connection = CreateConnection();
        var sqlDbCount = $@"SELECT COUNT(*) FROM pg_database WHERE datname = '{_dbName}';";
        var dbCount = await connection.ExecuteScalarAsync<int>(sqlDbCount);
        if (dbCount == 0)
        {
            var sql = $"""CREATE DATABASE "{_dbName}" """;
            await connection.ExecuteAsync(sql);
        }
    }

    private async Task CreateTables()
    {
        using var connection = CreateConnection();
        await CreateCandidatesTable(connection);
        await CreateVotersTable(connection);
    }

    private static async Task CreateCandidatesTable(IDbConnection connection)
    {
        await connection.ExecuteAsync("""
                CREATE TABLE IF NOT EXISTS Candidates (
                    Id SERIAL PRIMARY KEY,
                    Name VARCHAR NOT NULL
                );
            """);
    }

    private static async Task CreateVotersTable(IDbConnection connection)
    {
        await connection.ExecuteAsync("""
                CREATE TABLE IF NOT EXISTS Voters (
                    Id SERIAL PRIMARY KEY,
                    Name VARCHAR NOT NULL,
                    VotedCandidate INTEGER REFERENCES Candidates(Id),
                    KeyPhrase VARCHAR NOT NULL
                );
            """);
    }
}