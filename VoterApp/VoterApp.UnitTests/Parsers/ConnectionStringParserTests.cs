using Shouldly;
using VoterApp.Infrastructure.Parsers;

namespace VoterApp.UnitTests.Parsers;

public class ConnectionStringParserTests
{
    [Theory]
    [InlineData("myDbName", "Database", "Server=.;Port=5432;Database=myDbName;User Id=myUsername;Password=myPassword;")]
    [InlineData("5432", "Port", "Server=.;Port=5432;Database=myDbName;User Id=myUsername;Password=myPassword;")]
    [InlineData("myUsername", "User Id",
        "Database=myDbName;  User Id= myUsername;Password=myPassword;")]
    public void GetValue_ShouldBeExpectedResult(string expectedValue, string key, string connectionString)
    {
        // Arrange
        var connectionStringParser = new ConnectionStringParser();

        // Act
        var result = connectionStringParser.GetValue(key, connectionString);

        // Assert
        result.ShouldBe(expectedValue);
    }

    [Fact]
    public void GetValue_EmptyConnectionString_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var connectionStringParser = new ConnectionStringParser();
        var key = "Database";
        var connectionString = "  ";

        // Act Assert
        Should.Throw<InvalidOperationException>(() =>
            connectionStringParser.GetValue(key, connectionString));
    }

    [Fact]
    public void GetValue_KeyDoesntExist_ShouldThrowArgumentException()
    {
        // Arrange
        var connectionStringParser = new ConnectionStringParser();
        var key = "User Id";
        var connectionString = "Server=.;Port=5432;Database=myDbName;";

        // Act Assert
        Should.Throw<ArgumentException>(() =>
            connectionStringParser.GetValue(key, connectionString));
    }
}