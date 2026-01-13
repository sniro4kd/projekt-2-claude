using CatchTheRabbit.Core.Models;

namespace CatchTheRabbit.Api;

/// <summary>
/// Shared in-memory storage for game states.
/// In production, this would be a distributed cache (Redis, etc.)
/// </summary>
public static class GameStorage
{
    private static readonly Dictionary<Guid, GameState> _games = new();
    private static readonly object _lock = new();

    public static void Store(GameState game)
    {
        lock (_lock)
        {
            _games[game.GameId] = game;
        }
    }

    public static bool TryGet(Guid gameId, out GameState? game)
    {
        lock (_lock)
        {
            return _games.TryGetValue(gameId, out game);
        }
    }

    public static void Remove(Guid gameId)
    {
        lock (_lock)
        {
            _games.Remove(gameId);
        }
    }
}
