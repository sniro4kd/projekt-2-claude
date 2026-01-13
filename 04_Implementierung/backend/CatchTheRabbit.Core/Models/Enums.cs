namespace CatchTheRabbit.Core.Models;

/// <summary>
/// The role a player can take in the game.
/// </summary>
public enum PlayerRole
{
    Rabbit,
    Children
}

/// <summary>
/// The type of a game piece.
/// </summary>
public enum PieceType
{
    Rabbit,
    Child
}

/// <summary>
/// The current status of a game.
/// </summary>
public enum GameStatus
{
    Playing,
    RabbitWins,
    ChildrenWin
}
