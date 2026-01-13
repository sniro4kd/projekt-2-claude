using CatchTheRabbit.Core.Models;
using CatchTheRabbit.Core.Services;
using System.Diagnostics;

namespace CatchTheRabbit.Tests.Unit;

public class AIServiceTests
{
    private readonly IGameService _gameService;
    private readonly AIService _sut;

    public AIServiceTests()
    {
        _gameService = new GameService();
        _sut = new AIService(_gameService);
    }

    #region KT-AI-001: KI berechnet Zug als Hase

    [Fact]
    public void CalculateBestMove_AsRabbit_ReturnsValidMove()
    {
        // Arrange
        var state = _gameService.CreateGame(PlayerRole.Children);
        // KI spielt als Hase (weil Spieler Children gewählt hat)

        // Act
        var move = _sut.CalculateBestMove(state, PlayerRole.Rabbit, 1000);

        // Assert
        move.Should().NotBeNull();
        move.PieceType.Should().Be(PieceType.Rabbit);
        move.To.IsValid().Should().BeTrue();
    }

    [Fact]
    public void CalculateBestMove_AsRabbit_MovesDiagonally()
    {
        // Arrange
        var state = _gameService.CreateGame(PlayerRole.Children);
        var rabbitPos = state.Rabbit;

        // Act
        var move = _sut.CalculateBestMove(state, PlayerRole.Rabbit, 1000);

        // Assert
        Math.Abs(move.To.X - rabbitPos.X).Should().Be(1);
        Math.Abs(move.To.Y - rabbitPos.Y).Should().Be(1);
    }

    #endregion

    #region KT-AI-002: KI berechnet Zug als Kinder

    [Fact]
    public void CalculateBestMove_AsChildren_ReturnsValidMove()
    {
        // Arrange
        var state = _gameService.CreateGame(PlayerRole.Rabbit);
        state.CurrentTurn = PlayerRole.Children;

        // Act
        var move = _sut.CalculateBestMove(state, PlayerRole.Children, 1000);

        // Assert
        move.Should().NotBeNull();
        move.PieceType.Should().Be(PieceType.Child);
        move.PieceIndex.Should().NotBeNull();
    }

    [Fact]
    public void CalculateBestMove_AsChildren_MovesDownward()
    {
        // Arrange
        var state = _gameService.CreateGame(PlayerRole.Rabbit);
        state.CurrentTurn = PlayerRole.Children;

        // Act
        var move = _sut.CalculateBestMove(state, PlayerRole.Children, 1000);

        // Assert - Children must move downward (increasing Y)
        var originalPos = state.Children[move.PieceIndex!.Value];
        move.To.Y.Should().BeGreaterThan(originalPos.Y);
    }

    #endregion

    #region KT-AI-003: KI-Berechnung Zeitlimit

    [Fact]
    public void CalculateBestMove_CompletesWithinTimeLimit()
    {
        // Arrange
        var state = _gameService.CreateGame(PlayerRole.Children);
        var timeLimitMs = 1000;

        // Act
        var stopwatch = Stopwatch.StartNew();
        var move = _sut.CalculateBestMove(state, PlayerRole.Rabbit, timeLimitMs);
        stopwatch.Stop();

        // Assert
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(timeLimitMs + 100); // Small buffer
        move.Should().NotBeNull();
    }

    [Fact]
    public void CalculateBestMove_MultipleStates_AllComplete()
    {
        // Arrange
        var timeLimitMs = 1000;

        for (int i = 0; i < 5; i++)
        {
            var state = _gameService.CreateGame(PlayerRole.Children);

            // Act
            var stopwatch = Stopwatch.StartNew();
            var move = _sut.CalculateBestMove(state, PlayerRole.Rabbit, timeLimitMs);
            stopwatch.Stop();

            // Assert
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(timeLimitMs + 200);
            move.Should().NotBeNull();
        }
    }

    #endregion

    #region KT-AI-004: KI wählt Siegzug

    [Fact]
    public void CalculateBestMove_RabbitNearWin_MovesTowardGoal()
    {
        // Arrange - Rabbit close to the goal (Y=0)
        // Rabbit at (4, 1) which is black (4+1=5 odd)
        var state = new GameState
        {
            GameId = Guid.NewGuid(),
            PlayerRole = PlayerRole.Children,
            CurrentTurn = PlayerRole.Rabbit,
            Rabbit = new Position(4, 1), // Near top (Y=1), black field
            Children = new[]
            {
                // All on black fields (X+Y odd)
                new Position(2, 5), // 2+5=7
                new Position(2, 7), // 2+7=9
                new Position(6, 5), // 6+5=11
                new Position(6, 7), // 6+7=13
                new Position(8, 5)  // 8+5=13
            },
            Status = GameStatus.Playing
        };

        // Act
        var move = _sut.CalculateBestMove(state, PlayerRole.Rabbit, 1000);

        // Assert - Should move toward Y=0
        move.To.Y.Should().Be(0, because: "Rabbit should move to win");
    }

    #endregion

    #region KT-AI-005: Alpha-Beta Pruning Effizienz

    [Fact]
    public void CalculateBestMove_ComplexState_CompletesQuickly()
    {
        // Arrange - Complex game state mid-game
        // Rabbit at (4, 5) which is black (4+5=9 odd)
        var state = new GameState
        {
            GameId = Guid.NewGuid(),
            PlayerRole = PlayerRole.Children,
            CurrentTurn = PlayerRole.Rabbit,
            Rabbit = new Position(4, 5), // 4+5=9 odd (black)
            Children = new[]
            {
                // All on black fields (X+Y odd)
                new Position(2, 3), // 2+3=5
                new Position(2, 5), // 2+5=7
                new Position(4, 3), // 4+3=7
                new Position(6, 5), // 6+5=11
                new Position(8, 3)  // 8+3=11
            },
            Status = GameStatus.Playing
        };

        // Act
        var stopwatch = Stopwatch.StartNew();
        var move = _sut.CalculateBestMove(state, PlayerRole.Rabbit, 500);
        stopwatch.Stop();

        // Assert
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(600);
        move.Should().NotBeNull();
    }

    #endregion
}
