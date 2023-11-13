using System.Security.Cryptography;

namespace VoterApp.Domain.ValueObjects;

public record KeyPhrase
{
    public KeyPhrase()
    {
        var bytes = new byte[8];

        var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);

        Key = Convert.ToBase64String(bytes);
    }

    public string Key { get; set; }
}