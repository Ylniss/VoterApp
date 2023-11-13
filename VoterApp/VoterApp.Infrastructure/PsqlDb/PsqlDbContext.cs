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

    public async Task Init()
    {
        await CreateDatabaseIfDoesntExist();
        await CreateTables();
    }

    public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);

    public IDbConnection CreateConnection(string connectionString, string dbName)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString)
        {
            Database = dbName
        };

        return new NpgsqlConnection(builder.ToString());
    }

    private async Task CreateDatabaseIfDoesntExist()
    {
        // before we can use our database, first we have to create it using default 'postgres' db
        using var connection = CreateConnection(_connectionString, "postgres");

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
        connection.Open();

        using var transaction = connection.BeginTransaction();
        try
        {
            await CreateTables(connection, transaction);
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test")
                await CreateTestData(connection, transaction);

            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }

    private static async Task CreateTables(IDbConnection connection, IDbTransaction transaction)
    {
        List<string> queries = new()
        {
            @"    
                    CREATE TABLE IF NOT EXISTS Elections (
                        Id SERIAL PRIMARY KEY,
                        Topic VARCHAR NOT NULL,
                        Archived BOOLEAN NOT NULL,
                        RoomNumber UUID NOT NULL UNIQUE
                );",
            @"               
                    CREATE TABLE IF NOT EXISTS Candidates (
                        Id SERIAL PRIMARY KEY,
                        Name VARCHAR NOT NULL,
                        ElectionId INTEGER NOT NULL REFERENCES Elections(Id)
                );",
            @"    
                    CREATE TABLE IF NOT EXISTS Voters (
                        Id SERIAL PRIMARY KEY,
                        Name VARCHAR NOT NULL,
                        ElectionId INTEGER NOT NULL REFERENCES Elections(Id),
                        VotedCandidateId INTEGER REFERENCES Candidates(Id) ON DELETE SET NULL,
                        KeyPhrase VARCHAR NOT NULL,
                        UNIQUE (ElectionId, KeyPhrase)
                );"
        };
        await ExecuteQueriesInTransaction(connection, transaction, queries);
    }

    private static async Task CreateTestData(IDbConnection connection, IDbTransaction transaction)
    {
        List<string> queries = new()
        {
            @"
                        INSERT INTO elections(topic, archived, roomnumber)
                        VALUES 
                            ('Choose your man', false, 'c7f8b63d-4ca7-41f8-bd28-54ff5d41dc13'),
                            ('Select yo characta', false, '4de96b78-c5d8-4cad-8c57-42ad89b4c9b3');",
            @"
                        INSERT INTO candidates(name, electionid)
                        VALUES 
                            ('Bogdan', 1),
                            ('Gobdan', 2),
                            ('Dobgan', 1),
                            ('Topgun', 2);",
            @"
                        INSERT INTO voters(name, electionid, votedcandidateid, keyphrase) 
                        VALUES 
                            ('Chillman', 1, 1, '123'),
                            ('Lilchan',1, 1, '321'),
                            ('Nalchil',1, 3, '111');"
        };
        await ExecuteQueriesInTransaction(connection, transaction, queries);
    }

    private static async Task ExecuteQueriesInTransaction(IDbConnection connection, IDbTransaction transaction,
        IEnumerable<string> queries)
    {
        foreach (var sql in queries) await connection.ExecuteAsync(sql, transaction: transaction);
    }
}