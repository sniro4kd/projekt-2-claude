using Microsoft.AspNetCore.SignalR;

namespace CatchTheRabbit.Api.Hubs;

/// <summary>
/// SignalR hub for real-time game communication.
/// </summary>
public class GameHub : Hub
{
    private readonly ILogger<GameHub> _logger;

    public GameHub(ILogger<GameHub> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Joins a game session.
    /// </summary>
    public async Task JoinGame(string gameId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
        _logger.LogInformation("Client {ConnectionId} joined game {GameId}", Context.ConnectionId, gameId);
    }

    /// <summary>
    /// Leaves a game session.
    /// </summary>
    public async Task LeaveGame(string gameId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameId);
        _logger.LogInformation("Client {ConnectionId} left game {GameId}", Context.ConnectionId, gameId);
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("Client connected: {ConnectionId}", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("Client disconnected: {ConnectionId}", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}
