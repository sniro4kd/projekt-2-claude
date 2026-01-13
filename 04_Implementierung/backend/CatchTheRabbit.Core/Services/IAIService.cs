using CatchTheRabbit.Core.Models;

namespace CatchTheRabbit.Core.Services;

/// <summary>
/// Service interface for AI move calculation.
/// </summary>
public interface IAIService
{
    /// <summary>
    /// Calculates the best move for the AI player.
    /// </summary>
    /// <param name="state">The current game state.</param>
    /// <param name="aiRole">The role the AI is playing.</param>
    /// <param name="timeLimitMs">Maximum time allowed for calculation in milliseconds.</param>
    /// <returns>The best move found.</returns>
    Move CalculateBestMove(GameState state, PlayerRole aiRole, int timeLimitMs);
}
