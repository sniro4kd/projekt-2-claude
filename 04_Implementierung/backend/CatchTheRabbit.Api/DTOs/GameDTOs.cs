using CatchTheRabbit.Core.Models;

namespace CatchTheRabbit.Api.DTOs;

// Request DTOs

public class CreateGameRequest
{
    public string PlayerRole { get; set; } = "rabbit";
}

public class MakeMoveRequest
{
    public string PieceType { get; set; } = "rabbit";
    public int? PieceIndex { get; set; }
    public PositionDto To { get; set; } = new();
    public long ThinkingTimeMs { get; set; }
}

public class AddLeaderboardEntryRequest
{
    public Guid GameId { get; set; }
    public string Nickname { get; set; } = string.Empty;
}

// Response DTOs

public class PositionDto
{
    public int X { get; set; }
    public int Y { get; set; }

    public PositionDto() { }

    public PositionDto(Position pos)
    {
        X = pos.X;
        Y = pos.Y;
    }

    public Position ToPosition() => new(X, Y);
}

public class MoveDto
{
    public string PieceType { get; set; } = string.Empty;
    public int? PieceIndex { get; set; }
    public PositionDto From { get; set; } = new();
    public PositionDto To { get; set; } = new();

    public MoveDto() { }

    public MoveDto(Move move)
    {
        PieceType = move.PieceType.ToString().ToLowerInvariant();
        PieceIndex = move.PieceIndex;
        From = new PositionDto(move.From);
        To = new PositionDto(move.To);
    }
}

public class GameStateResponse
{
    public string GameId { get; set; } = string.Empty;
    public string PlayerRole { get; set; } = string.Empty;
    public string CurrentTurn { get; set; } = string.Empty;
    public PositionDto Rabbit { get; set; } = new();
    public List<PositionDto> Children { get; set; } = new();
    public string Status { get; set; } = string.Empty;
    public long PlayerThinkingTimeMs { get; set; }

    public GameStateResponse() { }

    public GameStateResponse(GameState state)
    {
        GameId = state.GameId.ToString();
        PlayerRole = state.PlayerRole.ToString().ToLowerInvariant();
        CurrentTurn = state.CurrentTurn.ToString().ToLowerInvariant();
        Rabbit = new PositionDto(state.Rabbit);
        Children = state.Children.Select(c => new PositionDto(c)).ToList();
        Status = state.Status.ToString().ToLowerInvariant();
        PlayerThinkingTimeMs = state.PlayerThinkingTimeMs;
    }
}

public class MoveResponse
{
    public bool Success { get; set; }
    public GameStateResponse? GameState { get; set; }
    public MoveDto? AiMove { get; set; }
    public string? Error { get; set; }
}

public class LeaderboardResponse
{
    public List<LeaderboardEntryDto> Rabbit { get; set; } = new();
    public List<LeaderboardEntryDto> Children { get; set; } = new();
}

public class LeaderboardEntryDto
{
    public int Rank { get; set; }
    public string Nickname { get; set; } = string.Empty;
    public long ThinkingTimeMs { get; set; }

    public LeaderboardEntryDto() { }

    public LeaderboardEntryDto(LeaderboardEntry entry, int rank)
    {
        Rank = rank;
        Nickname = entry.Nickname;
        ThinkingTimeMs = entry.ThinkingTimeMs;
    }
}

public class AddEntryResponse
{
    public bool Success { get; set; }
    public int? Rank { get; set; }
    public LeaderboardEntryDto? Entry { get; set; }
    public string? Error { get; set; }
}
