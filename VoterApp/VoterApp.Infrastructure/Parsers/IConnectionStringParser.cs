namespace VoterApp.Infrastructure.Parsers;

public interface IConnectionStringParser
{
    string GetValue(string key, string? connectionString);
}