using CatchTheRabbit.Core.Models;

namespace CatchTheRabbit.Core.Services;

/// <summary>
/// Service interface for game logic operations.
/// </summary>
public interface IGameService
{
    /// <summary>
    /// Creates a new game with the specified player role.
    /// </summary>
    GameState CreateGame(PlayerRole playerRole);

    /// <summary>
    /// Validates if a move is legal.
    /// </summary>
    ValidationResult ValidateMove(GameState state, Move move);

    /// <summary>
    /// Applies a move to the game state and returns the new state.
    /// </summary>
    GameState ApplyMove(GameState state, Move move);

    /// <summary>
    /// Gets all valid moves for a specific piece.
    /// </summary>
    List<Position> GetValidMoves(GameState state, PieceType pieceType, int? pieceIndex = null);

    /// <summary>
    /// Checks the game end conditions and returns the current status.
    /// </summary>
    GameStatus CheckGameEnd(GameState state);
}
