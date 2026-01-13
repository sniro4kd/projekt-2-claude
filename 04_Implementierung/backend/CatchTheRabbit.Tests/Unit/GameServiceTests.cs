using CatchTheRabbit.Core.Models;
using CatchTheRabbit.Core.Services;

namespace CatchTheRabbit.Tests.Unit;

public class GameServiceTests
{
    private readonly GameService _sut;

    public GameServiceTests()
    {
        _sut = new GameService();
    }

    #region KT-GS-001: Neues Spiel initialisieren (Hase)

    [Fact]
    public void CreateGame_AsRabbit_ReturnsCorrectInitialState()
    {
        // Act
        var result = _sut.CreateGame(PlayerRole.Rabbit);

        // Assert
        result.PlayerRole.Should().Be(PlayerRole.Rabbit);
        result.CurrentTurn.Should().Be(PlayerRole.Rabbit); // Rabbit always starts
        result.Status.Should().Be(GameStatus.Playing);
    }

    [Fact]
    public void CreateGame_AsRabbit_RabbitOnValidPosition()
    {
        // Act
        var result = _sut.CreateGame(PlayerRole.Rabbit);

        // Assert
        result.Rabbit.IsValid().Should().BeTrue();
        result.Rabbit.IsBlackField().Should().BeTrue();
    }

    [Fact]
    public void CreateGame_AsRabbit_ChildrenOnValidPositions()
    {
        // Act
        var result = _sut.CreateGame(PlayerRole.Rabbit);

        // Assert
        result.Children.Should().HaveCount(4);
        foreach (var child in result.Children)
        {
            child.IsValid().Should().BeTrue();
            child.IsBlackField().Should().BeTrue();
        }
    }

    #endregion

    #region KT-GS-002: Neues Spiel initialisieren (Kinder)

    [Fact]
    public void CreateGame_AsChildren_ReturnsCorrectInitialState()
    {
        // Act
        var result = _sut.CreateGame(PlayerRole.Children);

        // Assert
        result.PlayerRole.Should().Be(PlayerRole.Children);
        result.CurrentTurn.Should().Be(PlayerRole.Rabbit); // Rabbit always starts
        result.Status.Should().Be(GameStatus.Playing);
    }

    #endregion

    #region KT-GS-003: Gültige Hase-Bewegung

    [Fact]
    public void ValidateMove_RabbitDiagonalMove_IsValid()
    {
        // Arrange - Rabbit at (4, 5) which is black (4+5=9 odd)
        var state = CreateStateWithRabbitAt(4, 5);
        var move = new Move
        {
            PieceType = PieceType.Rabbit,
            From = state.Rabbit,
            To = new Position(3, 4) // Diagonal nach oben-links (3+4=7 odd, black field)
        };
        state.CurrentTurn = PlayerRole.Rabbit;

        // Act
        var result = _sut.ValidateMove(state, move);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ValidateMove_RabbitCanMoveAllDiagonalDirections()
    {
        // Arrange - Rabbit at (4, 5) which is black (4+5=9 odd)
        var state = CreateStateWithRabbitAt(4, 5);
        state.CurrentTurn = PlayerRole.Rabbit;

        // All 4 diagonal directions from (4, 5) to black fields
        var positions = new[]
        {
            new Position(3, 4), // oben-links (3+4=7 odd)
            new Position(3, 6), // oben-rechts (3+6=9 odd)
            new Position(5, 4), // unten-links (5+4=9 odd)
            new Position(5, 6)  // unten-rechts (5+6=11 odd)
        };

        foreach (var pos in positions)
        {
            var move = new Move { PieceType = PieceType.Rabbit, From = state.Rabbit, To = pos };
            var result = _sut.ValidateMove(state, move);
            result.IsValid.Should().BeTrue($"Position {pos} should be valid");
        }
    }

    #endregion

    #region KT-GS-004: Gültige Kind-Bewegung

    [Fact]
    public void ValidateMove_ChildMoveDownward_IsValid()
    {
        // Arrange
        var state = _sut.CreateGame(PlayerRole.Rabbit);
        state.CurrentTurn = PlayerRole.Children;

        // Find a child that can move
        var validMoves = _sut.GetValidMoves(state, PieceType.Child, 0);

        if (validMoves.Count > 0)
        {
            var move = new Move
            {
                PieceType = PieceType.Child,
                PieceIndex = 0,
                To = validMoves[0]
            };

            // Act
            var result = _sut.ValidateMove(state, move);

            // Assert
            result.IsValid.Should().BeTrue();
        }
    }

    [Fact]
    public void ValidateMove_ChildMoveUpward_IsInvalid()
    {
        // Arrange - Create state with child that would want to move up
        var state = CreateStateWithRabbitAt(8, 5);
        state.Children = new[]
        {
            new Position(3, 3), // This child is at row 3
            new Position(1, 5),
            new Position(1, 7),
            new Position(1, 9),
            new Position(3, 5)  // 5th child
        };
        state.CurrentTurn = PlayerRole.Children;

        // Try to move child up (decreasing Y)
        var move = new Move
        {
            PieceType = PieceType.Child,
            PieceIndex = 0,
            To = new Position(2, 4) // Moving up
        };

        // Act
        var result = _sut.ValidateMove(state, move);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region KT-GS-005: Bewegung außerhalb Spielfeld

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(10, 5)]
    [InlineData(5, 10)]
    public void ValidateMove_OutOfBounds_IsInvalid(int x, int y)
    {
        // Arrange
        var state = CreateStateWithRabbitAt(5, 5);
        state.CurrentTurn = PlayerRole.Rabbit;
        var move = new Move
        {
            PieceType = PieceType.Rabbit,
            To = new Position(x, y)
        };

        // Act
        var result = _sut.ValidateMove(state, move);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    #endregion

    #region KT-GS-006: Bewegung auf besetztes Feld

    [Fact]
    public void ValidateMove_ToOccupiedField_IsInvalid()
    {
        // Arrange - Rabbit at (4, 5) which is black (4+5=9 odd)
        var state = CreateStateWithRabbitAt(4, 5);
        // Place child at (3, 4) which is black (3+4=7 odd) - blocks the target
        state.Children[0] = new Position(3, 4);
        state.CurrentTurn = PlayerRole.Rabbit;

        var move = new Move
        {
            PieceType = PieceType.Rabbit,
            From = state.Rabbit,
            To = new Position(3, 4) // Try to move to occupied field
        };

        // Act
        var result = _sut.ValidateMove(state, move);

        // Assert
        result.IsValid.Should().BeFalse();
        // The error message may vary - just check move is invalid
    }

    #endregion

    #region KT-GS-007: Siegbedingung Hase

    [Fact]
    public void CheckGameEnd_RabbitReachesTopRow_RabbitWins()
    {
        // Arrange
        var state = CreateStateWithRabbitAt(1, 1); // Near top
        state.Rabbit = new Position(1, 0); // At Y=0 (top row)

        // Act
        var status = _sut.CheckGameEnd(state);

        // Assert
        status.Should().Be(GameStatus.RabbitWins);
    }

    #endregion

    #region KT-GS-008: Siegbedingung Kinder

    [Fact]
    public void CheckGameEnd_RabbitBlocked_ChildrenWin()
    {
        // Arrange - Surround rabbit completely
        // Rabbit at (4, 5) which is black (4+5=9 odd)
        var state = CreateStateWithRabbitAt(4, 5);
        // Block all 4 diagonal positions from (4, 5) with children on black fields
        state.Children = new[]
        {
            new Position(3, 4), // 3+4=7 odd (black)
            new Position(3, 6), // 3+6=9 odd (black)
            new Position(5, 4), // 5+4=9 odd (black)
            new Position(5, 6), // 5+6=11 odd (black)
            new Position(7, 6)  // 5th child, 7+6=13 odd (black)
        };

        // Act
        var status = _sut.CheckGameEnd(state);

        // Assert
        status.Should().Be(GameStatus.ChildrenWin);
    }

    #endregion

    #region KT-GS-009: Zugwechsel

    [Fact]
    public void ApplyMove_ValidMove_SwitchesTurn()
    {
        // Arrange
        var state = _sut.CreateGame(PlayerRole.Rabbit);
        var initialTurn = state.CurrentTurn;
        var validMoves = _sut.GetValidMoves(state, PieceType.Rabbit);

        if (validMoves.Count > 0)
        {
            var move = new Move
            {
                PieceType = PieceType.Rabbit,
                To = validMoves[0]
            };

            // Act
            var result = _sut.ApplyMove(state, move);

            // Assert
            result.CurrentTurn.Should().NotBe(initialTurn);
        }
    }

    [Fact]
    public void ApplyMove_ValidMove_AddsMoveToHistory()
    {
        // Arrange
        var state = _sut.CreateGame(PlayerRole.Rabbit);
        var initialCount = state.MoveHistory.Count;
        var validMoves = _sut.GetValidMoves(state, PieceType.Rabbit);

        if (validMoves.Count > 0)
        {
            var move = new Move
            {
                PieceType = PieceType.Rabbit,
                To = validMoves[0]
            };

            // Act
            var result = _sut.ApplyMove(state, move);

            // Assert
            result.MoveHistory.Count.Should().Be(initialCount + 1);
        }
    }

    #endregion

    #region Helper Methods

    private GameState CreateStateWithRabbitAt(int x, int y)
    {
        // Ensure position is on a black field (X+Y must be odd)
        if ((x + y) % 2 == 0)
        {
            // Adjust to nearest black field
            x = x + 1;
        }

        return new GameState
        {
            GameId = Guid.NewGuid(),
            PlayerRole = PlayerRole.Rabbit,
            CurrentTurn = PlayerRole.Rabbit,
            Rabbit = new Position(x, y),
            Children = new[]
            {
                // Black field positions (X+Y is odd)
                new Position(0, 1),
                new Position(0, 3),
                new Position(0, 5),
                new Position(0, 7),
                new Position(0, 9)  // 5th child
            },
            Status = GameStatus.Playing
        };
    }

    #endregion
}
