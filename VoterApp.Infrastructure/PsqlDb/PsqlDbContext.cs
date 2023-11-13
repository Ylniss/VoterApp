using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using VoterApp.Infrastructure.Parsers;
using VoterApp.Infrastructure.PsqlDb.InitDataProviders;

namespace VoterApp.Infrastructure.PsqlDb;

public class PsqlDbContext : IPsqlDbContext
{
    private readonly string? _connectionString;
    private readonly string _dbName;
    private readonly IInitDataProvider _initDataProvider;
    private readonly ILogger<PsqlDbContext> _logger;

    public PsqlDbContext(IConnectionStringParser connectionStringParser, IInitDataProvider initDataProvider,
        ILogger<PsqlDbContext> logger)
    {
        _initDataProvider = initDataProvider;
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
                await InsertTestData(connection, transaction);

            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }

    private async Task CreateTables(IDbConnection connection, IDbTransaction transaction)
    {
        var queries = _initDataProvider.GetSchemaQueries();
        await ExecuteQueriesInTransaction(connection, transaction, queries);
    }

    private async Task InsertTestData(IDbConnection connection, IDbTransaction transaction)
    {
        var queries = _initDataProvider.GetInsertQueries();
        await ExecuteQueriesInTransaction(connection, transaction, queries);
    }

    private static async Task ExecuteQueriesInTransaction(IDbConnection connection, IDbTransaction transaction,
        IEnumerable<string> queries)
    {
        foreach (var sql in queries) await connection.ExecuteAsync(sql, transaction: transaction);
    }
}