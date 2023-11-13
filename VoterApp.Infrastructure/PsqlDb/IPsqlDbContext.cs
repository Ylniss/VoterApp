using System.Data;

namespace VoterApp.Infrastructure.PsqlDb;

public interface IPsqlDbContext
{
    IDbConnection CreateConnection();
    Task Init();
}