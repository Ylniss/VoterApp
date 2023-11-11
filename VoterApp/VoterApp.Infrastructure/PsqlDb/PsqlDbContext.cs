using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using VoterApp.Infrastructure.Parsers;

namespace VoterApp.Infrastructure.PsqlDb;

public class PsqlDbContext : IPsqlDbContext
{
    private readonly string? _connectionString;
    private readonly string _dbName;
    private readonly ILogger<PsqlDbContext> _logger;

    public PsqlDbContext(IConnectionStringParser connectionStringParser, ILogger<PsqlDbContext> logger)
    {
        _logger = logger;
        _connectionString = Environment.GetEnvironmentVariable("ConnectionString");
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
        var builder = new NpgsqlConnectionStringBuilder(_connectionString)
        {
            // before we can use our database, first we have to create it using default postgres db
            Database = "postgres"
        };

        await using var connection = new NpgsqlConnection(builder.ToString());

        var sqlDbCount = $@"SELECT COUNT(*) FROM pg_database WHERE datname = '{_dbName}';";
        var dbCount = await connection.ExecuteScalarAsync<int>(sqlDbCount);

        if (dbCount == 0)
        {
            _logger.LogInformation("Creating database...");
            var sql = $"""CREATE DATABASE "{_dbName}" """;
            await connection.ExecuteAsync(sql);
            _logger.LogInformation($"Database {_dbName} created.");
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