using System.Data;

namespace VoterApp.Infrastructure.PsqlDb;

public interface IPsqlDbContext
{
    IDbConnection CreateConnection();
    IDbConnection CreateConnection(string connectionString, string dbName);

    Task CleanTablesAsync(string connectionString);
    Task Init();
}