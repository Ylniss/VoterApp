namespace VoterApp.Infrastructure.PsqlDb.InitDataProviders;

public interface IInitDataProvider
{
    IEnumerable<string> GetSchemaQueries();
    IEnumerable<string> GetInsertQueries();
}