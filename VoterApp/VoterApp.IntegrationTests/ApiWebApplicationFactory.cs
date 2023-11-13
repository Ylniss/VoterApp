using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;
using VoterApp.Infrastructure.PsqlDb;

namespace VoterApp.IntegrationTests;

public class ApiWebApplicationFactory
    : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithDatabase("voterAppDbIntegrationTests")
        .WithUsername("myUsername")
        .WithPassword("myPassword")
        .Build();

    private DbConnection _connection = default!;

    private IPsqlDbContext _psqlDbContext;
    private Respawner _respawner = default!;


    public HttpClient Client { get; private set; } = default!;

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        _connection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        Client = CreateClient();
        // InitializeRespawner();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _connection.DisposeAsync();
    }

    public async Task ResetDatabaseAsync()
    {
        //await _respawner.ResetAsync(_connection);
        // await _psqlDbContext.CleanTablesAsync(_dbContainer.GetConnectionString());
        // await _psqlDbContext.Init();
    }

    private async Task InitializeRespawner()
    {
        await _connection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = new[] { "public" }
        });
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ConnectionString", _dbContainer.GetConnectionString());
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");

        builder.ConfigureServices(services =>
        {
            _psqlDbContext = services.BuildServiceProvider().GetRequiredService<IPsqlDbContext>();
        });

        builder.UseEnvironment("Development");
    }
}