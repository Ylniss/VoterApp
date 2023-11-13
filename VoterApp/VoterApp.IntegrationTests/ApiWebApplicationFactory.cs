using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.PostgreSql;

namespace VoterApp.IntegrationTests;

public class ApiWebApplicationFactory
    : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithDatabase("voterAppDbIntegrationTests")
        .WithUsername("myUsername")
        .WithPassword("myPassword")
        .Build();

    public async Task InitializeAsync() => await _postgreSqlContainer.StartAsync();

    public async Task DisposeAsync()
    {
        await _postgreSqlContainer.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ConnectionString", _postgreSqlContainer.GetConnectionString());
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");

        builder.UseEnvironment("Development");
    }
}