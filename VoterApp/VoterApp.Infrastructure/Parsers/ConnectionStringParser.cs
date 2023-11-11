namespace VoterApp.Infrastructure.Parsers;

public class ConnectionStringParser : IConnectionStringParser
{
    public string GetValue(string key, string? connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Connection string not found.");

        var keyValuePairs = ParseConnectionString(connectionString);

        if (keyValuePairs.TryGetValue(key, out var value)) return value;

        throw new ArgumentException($"{key} name not found in the connection string.");
    }

    private Dictionary<string, string> ParseConnectionString(string connectionString)
    {
        var keyValuePairs = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        var parts = connectionString.Split(';');
        foreach (var part in parts)
        {
            var keyValue = part.Split('=');
            if (keyValue.Length == 2) keyValuePairs[keyValue[0].Trim()] = keyValue[1].Trim();
        }

        return keyValuePairs;
    }
}