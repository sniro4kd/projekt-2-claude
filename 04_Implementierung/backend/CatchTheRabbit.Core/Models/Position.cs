namespace CatchTheRabbit.Core.Models;

/// <summary>
/// Represents a position on the game board.
/// </summary>
public readonly struct Position : IEquatable<Position>
{
    public int X { get; }
    public int Y { get; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Checks if the position is within the board boundaries (0-9).
    /// </summary>
    public bool IsValid() => X >= 0 && X <= 9 && Y >= 0 && Y <= 9;

    /// <summary>
    /// Checks if the position is a black field (playable).
    /// Black fields have (x + y) % 2 == 1.
    /// </summary>
    public bool IsBlackField() => (X + Y) % 2 == 1;

    public bool Equals(Position other) => X == other.X && Y == other.Y;

    public override bool Equals(object? obj) => obj is Position other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(X, Y);

    public static bool operator ==(Position left, Position right) => left.Equals(right);

    public static bool operator !=(Position left, Position right) => !left.Equals(right);

    public override string ToString() => $"({X}, {Y})";
}
