using CatchTheRabbit.Core.Interfaces;
using CatchTheRabbit.Core.Models;

namespace CatchTheRabbit.Core.Services;

/// <summary>
/// Implementation of leaderboard operations.
/// </summary>
public class LeaderboardService : ILeaderboardService
{
    private readonly ILeaderboardRepository _repository;

    public LeaderboardService(ILeaderboardRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<LeaderboardEntry>> GetTopEntriesAsync(PlayerRole role, int count = LeaderboardConstants.MaxEntries)
    {
        var entries = await _repository.GetTopEntriesAsync(role, count);
        return entries.ToList();
    }

    public async Task<int> AddEntryAsync(LeaderboardEntry entry)
    {
        // Validate nickname
        if (string.IsNullOrWhiteSpace(entry.Nickname))
            throw new ArgumentException("Nickname cannot be empty");

        // Truncate nickname if too long
        if (entry.Nickname.Length > LeaderboardConstants.MaxNicknameLength)
            entry.Nickname = entry.Nickname[..LeaderboardConstants.MaxNicknameLength];

        // Sanitize nickname (remove leading/trailing whitespace)
        entry.Nickname = entry.Nickname.Trim();

        entry.CreatedAt = DateTime.UtcNow;

        return await _repository.AddEntryAsync(entry);
    }

    public async Task<int> GetRankAsync(PlayerRole role, long thinkingTimeMs)
    {
        var betterCount = await _repository.CountBetterEntriesAsync(role, thinkingTimeMs);
        return betterCount + 1;
    }
}
