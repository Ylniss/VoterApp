using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using VoterApp.Application.Contracts;
using VoterApp.Infrastructure.Parsers;
using VoterApp.Infrastructure.PsqlDb;
using VoterApp.Infrastructure.PsqlDb.Repositories;

namespace VoterApp.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddScoped<IPsqlDbContext, PsqlDbContext>();

        services.AddScoped<IVoterRepository, VoterRepository>();
        services.AddScoped<ICandidateRepository, CandidateRepository>();

        services.AddScoped<IConnectionStringParser, ConnectionStringParser>();

        return services;
    }
}