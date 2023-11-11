using VoterApp.Infrastructure;
using VoterApp.Infrastructure.PsqlDb;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddInfrastructureServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<PsqlDbContext>();
await context.Init();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();