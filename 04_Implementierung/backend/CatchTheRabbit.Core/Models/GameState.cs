namespace CatchTheRabbit.Core.Models;

/// <summary>
/// Represents the complete state of a game.
/// </summary>
public class GameState
{
    /// <summary>
    /// Unique identifier for this game.
    /// </summary>
    public Guid GameId { get; set; } = Guid.NewGuid();

    /// <summary>
    /// The role chosen by the human player.
    /// </summary>
    public PlayerRole PlayerRole { get; set; }

    /// <summary>
    /// Whose turn it is currently.
    /// </summary>
    public PlayerRole CurrentTurn { get; set; }

    /// <summary>
    /// The position of the rabbit.
    /// </summary>
    public Position Rabbit { get; set; }

    /// <summary>
    /// The positions of the four children.
    /// </summary>
    public Position[] Children { get; set; } = new Position[4];

    /// <summary>
    /// The current status of the game.
    /// </summary>
    public GameStatus Status { get; set; } = GameStatus.Playing;

    /// <summary>
    /// The accumulated thinking time of the human player in milliseconds.
    /// </summary>
    public long PlayerThinkingTimeMs { get; set; }

    /// <summary>
    /// The history of all moves made in this game.
    /// </summary>
    public List<Move> MoveHistory { get; set; } = new();

    /// <summary>
    /// Creates a deep clone of this game state.
    /// </summary>
    public GameState Clone()
    {
        return new GameState
        {
            GameId = GameId,
            PlayerRole = PlayerRole,
            CurrentTurn = CurrentTurn,
            Rabbit = Rabbit,
            Children = (Position[])Children.Clone(),
            Status = Status,
            PlayerThinkingTimeMs = PlayerThinkingTimeMs,
            MoveHistory = new List<Move>(MoveHistory)
        };
    }

    /// <summary>
    /// Checks if a position is occupied by any piece.
    /// </summary>
    public bool IsOccupied(Position pos)
    {
        if (Rabbit == pos)
            return true;

        foreach (var child in Children)
        {
            if (child == pos)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Gets all piece positions on the board.
    /// </summary>
    public IEnumerable<Position> GetAllPieces()
    {
        yield return Rabbit;
        foreach (var child in Children)
        {
            yield return child;
        }
    }
}
