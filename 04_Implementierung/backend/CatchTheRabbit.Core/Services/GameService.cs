using CatchTheRabbit.Core.Models;

namespace CatchTheRabbit.Core.Services;

/// <summary>
/// Implementation of game logic operations.
/// </summary>
public class GameService : IGameService
{
    private readonly Random _random = new();

    public GameState CreateGame(PlayerRole playerRole)
    {
        var state = new GameState
        {
            PlayerRole = playerRole,
            CurrentTurn = PlayerRole.Rabbit // Rabbit always starts
        };

        // Generate random positions for children (top 3 rows)
        var usedPositions = new HashSet<Position>();
        var childPositions = new Position[GameConstants.ChildrenCount];

        for (int i = 0; i < GameConstants.ChildrenCount; i++)
        {
            Position pos;
            do
            {
                pos = GenerateRandomPosition(
                    GameConstants.ChildrenStartMinY,
                    GameConstants.ChildrenStartMaxY,
                    usedPositions);
            } while (!pos.IsBlackField());

            childPositions[i] = pos;
            usedPositions.Add(pos);
        }

        state.Children = childPositions;

        // Generate random position for rabbit (bottom 3 rows)
        Position rabbitPos;
        do
        {
            rabbitPos = GenerateRandomPosition(
                GameConstants.RabbitStartMinY,
                GameConstants.RabbitStartMaxY,
                usedPositions);
        } while (!rabbitPos.IsBlackField());

        state.Rabbit = rabbitPos;

        return state;
    }

    public ValidationResult ValidateMove(GameState state, Move move)
    {
        // Check if the right player is moving
        if (move.PieceType == PieceType.Rabbit && state.CurrentTurn != PlayerRole.Rabbit)
            return ValidationResult.Fail("Not rabbit's turn");

        if (move.PieceType == PieceType.Child && state.CurrentTurn != PlayerRole.Children)
            return ValidationResult.Fail("Not children's turn");

        // Get current position
        Position from;
        if (move.PieceType == PieceType.Rabbit)
        {
            from = state.Rabbit;
        }
        else
        {
            if (!move.PieceIndex.HasValue || move.PieceIndex < 0 || move.PieceIndex >= GameConstants.ChildrenCount)
                return ValidationResult.Fail("Invalid piece index for child");

            from = state.Children[move.PieceIndex.Value];
        }

        // Check if target is within board
        if (!move.To.IsValid())
            return ValidationResult.Fail("Target position is outside the board");

        // Check if target is a black field
        if (!move.To.IsBlackField())
            return ValidationResult.Fail("Target position is not a black field");

        // Check if target is not occupied
        if (state.IsOccupied(move.To))
            return ValidationResult.Fail("Target position is occupied");

        // Check diagonal movement (exactly 1 field)
        int deltaX = Math.Abs(move.To.X - from.X);
        int deltaY = Math.Abs(move.To.Y - from.Y);

        if (deltaX != 1 || deltaY != 1)
            return ValidationResult.Fail("Must move diagonally by exactly one field");

        // Check movement direction for children (only downward)
        if (move.PieceType == PieceType.Child)
        {
            if (move.To.Y <= from.Y)
                return ValidationResult.Fail("Children can only move downward");
        }

        return ValidationResult.Success();
    }

    public GameState ApplyMove(GameState state, Move move)
    {
        var newState = state.Clone();

        // Get current position and update
        if (move.PieceType == PieceType.Rabbit)
        {
            move.From = newState.Rabbit;
            newState.Rabbit = move.To;
        }
        else
        {
            move.From = newState.Children[move.PieceIndex!.Value];
            newState.Children[move.PieceIndex.Value] = move.To;
        }

        // Add move to history
        newState.MoveHistory.Add(move);

        // Switch turn
        newState.CurrentTurn = newState.CurrentTurn == PlayerRole.Rabbit
            ? PlayerRole.Children
            : PlayerRole.Rabbit;

        // Check game end
        newState.Status = CheckGameEnd(newState);

        return newState;
    }

    public List<Position> GetValidMoves(GameState state, PieceType pieceType, int? pieceIndex = null)
    {
        var validMoves = new List<Position>();

        Position from;
        if (pieceType == PieceType.Rabbit)
        {
            from = state.Rabbit;
        }
        else
        {
            if (!pieceIndex.HasValue)
                return validMoves;

            from = state.Children[pieceIndex.Value];
        }

        // Possible diagonal directions
        int[] dx = { -1, 1, -1, 1 };
        int[] dy = { -1, -1, 1, 1 };

        for (int i = 0; i < 4; i++)
        {
            // Children can only move downward (dy > 0)
            if (pieceType == PieceType.Child && dy[i] <= 0)
                continue;

            var to = new Position(from.X + dx[i], from.Y + dy[i]);

            // Check if move is valid (without turn validation)
            if (IsValidMovePosition(state, from, to, pieceType))
            {
                validMoves.Add(to);
            }
        }

        return validMoves;
    }

    /// <summary>
    /// Checks if a move from one position to another is valid,
    /// without checking whose turn it is (used for game end check).
    /// </summary>
    private bool IsValidMovePosition(GameState state, Position from, Position to, PieceType pieceType)
    {
        // Check if target is within board
        if (!to.IsValid())
            return false;

        // Check if target is a black field
        if (!to.IsBlackField())
            return false;

        // Check if target is not occupied
        if (state.IsOccupied(to))
            return false;

        // Check diagonal movement (exactly 1 field)
        int deltaX = Math.Abs(to.X - from.X);
        int deltaY = Math.Abs(to.Y - from.Y);

        if (deltaX != 1 || deltaY != 1)
            return false;

        // Check movement direction for children (only downward)
        if (pieceType == PieceType.Child)
        {
            if (to.Y <= from.Y)
                return false;
        }

        return true;
    }

    public GameStatus CheckGameEnd(GameState state)
    {
        // Rabbit wins if it reaches the top row
        if (state.Rabbit.Y == GameConstants.RabbitWinY)
            return GameStatus.RabbitWins;

        // Children win if rabbit has no valid moves
        var rabbitMoves = GetValidMoves(state, PieceType.Rabbit);
        if (rabbitMoves.Count == 0)
            return GameStatus.ChildrenWin;

        return GameStatus.Playing;
    }

    private Position GenerateRandomPosition(int minY, int maxY, HashSet<Position> excluded)
    {
        Position pos;
        int attempts = 0;
        const int maxAttempts = 100;

        do
        {
            int x = _random.Next(0, GameConstants.BoardSize);
            int y = _random.Next(minY, maxY + 1);
            pos = new Position(x, y);
            attempts++;

            if (attempts > maxAttempts)
                throw new InvalidOperationException("Could not generate unique position");

        } while (excluded.Contains(pos) || !pos.IsBlackField());

        return pos;
    }
}
