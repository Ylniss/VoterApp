using VoterApp.Api;
using VoterApp.Api.Middleware;
using VoterApp.Application;
using VoterApp.Infrastructure;
using VoterApp.Infrastructure.PsqlDb;

// Log.Logger = new LoggerConfiguration()
//     .WriteTo.Console()
//     .WriteTo.File("/app/logs/log.log", rollingInterval: RollingInterval.Day)
//     .CreateLogger();

// try
// {
// Log.Information("Starting VoterApp API");
var builder = WebApplication.CreateBuilder(args);

// builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices()
    .AddApiServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors("CorsPolicy");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<IPsqlDbContext>();
    await context.Init();
}


app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
//}
// catch (Exception ex)
// {
//     Log.Fatal(ex, "Application terminated unexpectedly");
// }
// finally
// {
//     Log.CloseAndFlush();
// }

// Program must be public for integration tests custom web application factory
public partial class Program
{
}