using CatchTheRabbit.Core.Models;

namespace CatchTheRabbit.Core.Services;

/// <summary>
/// Service interface for leaderboard operations.
/// </summary>
public interface ILeaderboardService
{
    /// <summary>
    /// Gets the top entries for a specific role.
    /// </summary>
    Task<List<LeaderboardEntry>> GetTopEntriesAsync(PlayerRole role, int count = LeaderboardConstants.MaxEntries);

    /// <summary>
    /// Adds a new entry to the leaderboard.
    /// </summary>
    /// <returns>The ID of the new entry.</returns>
    Task<int> AddEntryAsync(LeaderboardEntry entry);

    /// <summary>
    /// Gets the rank a player would have with the given time.
    /// </summary>
    Task<int> GetRankAsync(PlayerRole role, long thinkingTimeMs);
}
