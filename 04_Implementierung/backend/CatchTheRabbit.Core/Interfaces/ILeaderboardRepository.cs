using CatchTheRabbit.Core.Models;

namespace CatchTheRabbit.Core.Interfaces;

/// <summary>
/// Repository interface for leaderboard data access.
/// </summary>
public interface ILeaderboardRepository
{
    /// <summary>
    /// Gets the top entries for a specific role.
    /// </summary>
    Task<IEnumerable<LeaderboardEntry>> GetTopEntriesAsync(PlayerRole role, int count);

    /// <summary>
    /// Adds a new entry to the leaderboard.
    /// </summary>
    /// <returns>The ID of the new entry.</returns>
    Task<int> AddEntryAsync(LeaderboardEntry entry);

    /// <summary>
    /// Counts entries with a time better than the given time.
    /// </summary>
    Task<int> CountBetterEntriesAsync(PlayerRole role, long thinkingTimeMs);

    /// <summary>
    /// Initializes the database schema.
    /// </summary>
    Task InitializeDatabaseAsync();
}
