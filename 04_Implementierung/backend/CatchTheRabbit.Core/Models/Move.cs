namespace CatchTheRabbit.Core.Models;

/// <summary>
/// Represents a move in the game.
/// </summary>
public class Move
{
    /// <summary>
    /// The type of piece being moved.
    /// </summary>
    public PieceType PieceType { get; set; }

    /// <summary>
    /// The index of the child piece (0-3). Only relevant when PieceType is Child.
    /// </summary>
    public int? PieceIndex { get; set; }

    /// <summary>
    /// The starting position of the move.
    /// </summary>
    public Position From { get; set; }

    /// <summary>
    /// The target position of the move.
    /// </summary>
    public Position To { get; set; }

    /// <summary>
    /// The timestamp when the move was made.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
