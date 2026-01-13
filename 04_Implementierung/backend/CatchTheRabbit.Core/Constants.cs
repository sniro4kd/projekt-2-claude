namespace CatchTheRabbit.Core;

/// <summary>
/// Game-related constants.
/// </summary>
public static class GameConstants
{
    public const int BoardSize = 10;
    public const int ChildrenCount = 5;

    public const int RabbitStartMinY = 7;
    public const int RabbitStartMaxY = 9;

    public const int ChildrenStartMinY = 0;
    public const int ChildrenStartMaxY = 2;

    public const int RabbitWinY = 0;
}

/// <summary>
/// AI-related constants.
/// </summary>
public static class AIConstants
{
    public const int DefaultMaxDepth = 8;
    public const int DefaultMaxTimeMs = 900;

    public const int WinScore = 10000;
    public const int ProgressWeight = 100;
    public const int MobilityWeight = 50;
    public const int EncirclementWeight = 30;
    public const int FormationWeight = 20;
}

/// <summary>
/// Leaderboard-related constants.
/// </summary>
public static class LeaderboardConstants
{
    public const int MaxEntries = 20;
    public const int MaxNicknameLength = 50;
}
