using System.Diagnostics;
using CatchTheRabbit.Core.Models;

namespace CatchTheRabbit.Core.Services;

/// <summary>
/// AI service using Minimax with Alpha-Beta pruning.
/// </summary>
public class AIService : IAIService
{
    private readonly IGameService _gameService;
    private readonly Random _random = new();
    private readonly int _maxDepth;
    private readonly int _maxTimeMs;

    public AIService(IGameService gameService, int maxDepth = AIConstants.DefaultMaxDepth, int maxTimeMs = AIConstants.DefaultMaxTimeMs)
    {
        _gameService = gameService;
        _maxDepth = maxDepth;
        _maxTimeMs = maxTimeMs;
    }

    public Move CalculateBestMove(GameState state, PlayerRole aiRole, int timeLimitMs)
    {
        var stopwatch = Stopwatch.StartNew();
        Move? bestMove = null;

        // Iterative deepening
        for (int depth = 1; depth <= _maxDepth; depth++)
        {
            if (stopwatch.ElapsedMilliseconds > timeLimitMs * 0.9)
                break;

            var currentBest = MinimaxRoot(state, depth, aiRole, timeLimitMs - (int)stopwatch.ElapsedMilliseconds);

            if (currentBest != null)
                bestMove = currentBest;
        }

        // Fallback: random valid move
        if (bestMove == null)
        {
            var moves = GetAllMoves(state, aiRole);
            if (moves.Count > 0)
                bestMove = moves[_random.Next(moves.Count)];
        }

        return bestMove ?? throw new InvalidOperationException("No valid moves available");
    }

    private Move? MinimaxRoot(GameState state, int depth, PlayerRole aiRole, int remainingTimeMs)
    {
        var moves = GetAllMoves(state, aiRole);
        if (moves.Count == 0)
            return null;

        Move? bestMove = null;
        int bestValue = int.MinValue;

        foreach (var move in moves)
        {
            var newState = _gameService.ApplyMove(state, move);
            int value = Minimax(newState, depth - 1, int.MinValue, int.MaxValue, false, aiRole);

            if (value > bestValue)
            {
                bestValue = value;
                bestMove = move;
            }
        }

        return bestMove;
    }

    private int Minimax(GameState state, int depth, int alpha, int beta, bool maximizing, PlayerRole aiRole)
    {
        // Base case: depth 0 or game over
        if (depth == 0 || state.Status != GameStatus.Playing)
            return Evaluate(state, aiRole);

        var currentRole = maximizing
            ? aiRole
            : (aiRole == PlayerRole.Rabbit ? PlayerRole.Children : PlayerRole.Rabbit);

        var moves = GetAllMoves(state, currentRole);

        if (maximizing)
        {
            int maxEval = int.MinValue;

            foreach (var move in moves)
            {
                var newState = _gameService.ApplyMove(state, move);
                int eval = Minimax(newState, depth - 1, alpha, beta, false, aiRole);
                maxEval = Math.Max(maxEval, eval);
                alpha = Math.Max(alpha, eval);

                if (beta <= alpha)
                    break; // Beta cutoff
            }

            return maxEval;
        }
        else
        {
            int minEval = int.MaxValue;

            foreach (var move in moves)
            {
                var newState = _gameService.ApplyMove(state, move);
                int eval = Minimax(newState, depth - 1, alpha, beta, true, aiRole);
                minEval = Math.Min(minEval, eval);
                beta = Math.Min(beta, eval);

                if (beta <= alpha)
                    break; // Alpha cutoff
            }

            return minEval;
        }
    }

    private int Evaluate(GameState state, PlayerRole aiRole)
    {
        // Terminal states
        if (state.Status == GameStatus.RabbitWins)
            return aiRole == PlayerRole.Rabbit ? AIConstants.WinScore : -AIConstants.WinScore;

        if (state.Status == GameStatus.ChildrenWin)
            return aiRole == PlayerRole.Children ? AIConstants.WinScore : -AIConstants.WinScore;

        int score = 0;

        // Factor 1: Rabbit progress (higher = closer to goal)
        int rabbitProgress = (9 - state.Rabbit.Y) * AIConstants.ProgressWeight;

        // Factor 2: Rabbit mobility
        var rabbitMoves = _gameService.GetValidMoves(state, PieceType.Rabbit);
        int mobilityScore = rabbitMoves.Count * AIConstants.MobilityWeight;

        // Factor 3: Encirclement
        int encirclement = CalculateEncirclement(state.Rabbit, state.Children);
        int encirclementScore = encirclement * AIConstants.EncirclementWeight;

        // Factor 4: Children formation
        int formation = CalculateFormation(state.Children);
        int formationScore = formation * AIConstants.FormationWeight;

        // Calculate score based on AI role
        if (aiRole == PlayerRole.Rabbit)
        {
            score = rabbitProgress + mobilityScore - encirclementScore;
        }
        else
        {
            score = encirclementScore + formationScore - rabbitProgress - mobilityScore;
        }

        return score;
    }

    private int CalculateEncirclement(Position rabbit, Position[] children)
    {
        int count = 0;

        // Check all 4 diagonal neighbors of the rabbit
        int[] dx = { -1, 1, -1, 1 };
        int[] dy = { -1, -1, 1, 1 };

        for (int i = 0; i < 4; i++)
        {
            int nx = rabbit.X + dx[i];
            int ny = rabbit.Y + dy[i];

            // Outside board counts as blocked
            if (nx < 0 || nx > 9 || ny < 0 || ny > 9)
            {
                count++;
                continue;
            }

            // Child on this field?
            foreach (var child in children)
            {
                if (child.X == nx && child.Y == ny)
                {
                    count++;
                    break;
                }
            }
        }

        return count;
    }

    private int CalculateFormation(Position[] children)
    {
        // Bonus for children being horizontally close
        int minX = children.Min(c => c.X);
        int maxX = children.Max(c => c.X);
        int spread = maxX - minX;

        // Tighter formation = better (max 7 points at spread of 3)
        return Math.Max(0, 10 - spread);
    }

    private List<Move> GetAllMoves(GameState state, PlayerRole role)
    {
        var moves = new List<Move>();

        if (role == PlayerRole.Rabbit)
        {
            var validPositions = _gameService.GetValidMoves(state, PieceType.Rabbit);
            foreach (var pos in validPositions)
            {
                moves.Add(new Move
                {
                    PieceType = PieceType.Rabbit,
                    From = state.Rabbit,
                    To = pos
                });
            }
        }
        else
        {
            for (int i = 0; i < GameConstants.ChildrenCount; i++)
            {
                var validPositions = _gameService.GetValidMoves(state, PieceType.Child, i);
                foreach (var pos in validPositions)
                {
                    moves.Add(new Move
                    {
                        PieceType = PieceType.Child,
                        PieceIndex = i,
                        From = state.Children[i],
                        To = pos
                    });
                }
            }
        }

        return moves;
    }
}
