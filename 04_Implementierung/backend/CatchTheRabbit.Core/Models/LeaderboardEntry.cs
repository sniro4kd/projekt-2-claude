namespace CatchTheRabbit.Core.Models;

/// <summary>
/// Represents an entry in the leaderboard.
/// </summary>
public class LeaderboardEntry
{
    /// <summary>
    /// The database ID of the entry.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The nickname chosen by the player.
    /// </summary>
    public string Nickname { get; set; } = string.Empty;

    /// <summary>
    /// The role the player had when winning.
    /// </summary>
    public PlayerRole Role { get; set; }

    /// <summary>
    /// The total thinking time in milliseconds.
    /// </summary>
    public long ThinkingTimeMs { get; set; }

    /// <summary>
    /// When the entry was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
