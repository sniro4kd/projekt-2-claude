using CatchTheRabbit.Api.Hubs;
using CatchTheRabbit.Core.Interfaces;
using CatchTheRabbit.Core.Services;
using CatchTheRabbit.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

// Core services
builder.Services.AddSingleton<IGameService, GameService>();
builder.Services.AddSingleton<IAIService>(sp =>
{
    var gameService = sp.GetRequiredService<IGameService>();
    return new AIService(gameService);
});

// Infrastructure
var connectionString = builder.Configuration.GetConnectionString("Sqlite")
    ?? "Data Source=data/game.db";
builder.Services.AddSingleton<ILeaderboardRepository>(sp =>
    new SqliteLeaderboardRepository(connectionString));
builder.Services.AddSingleton<ILeaderboardService, LeaderboardService>();

// Controllers
builder.Services.AddControllers();

// SignalR
builder.Services.AddSignalR();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost", "http://localhost:5173", "http://localhost:80")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var repository = scope.ServiceProvider.GetRequiredService<ILeaderboardRepository>();
    await repository.InitializeDatabaseAsync();
}

// Configure the HTTP request pipeline
app.UseCors();

app.MapControllers();
app.MapHub<GameHub>("/gamehub");

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

app.Run();
