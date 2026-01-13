using CatchTheRabbit.Core.Models;

namespace CatchTheRabbit.Tests.Unit;

public class GameStateTests
{
    #region KT-STATE-001: GameState Clone

    [Fact]
    public void Clone_ReturnsDeepCopy()
    {
        // Arrange
        var original = CreateTestGameState();

        // Act
        var clone = original.Clone();

        // Assert
        clone.Should().NotBeSameAs(original);
        clone.Rabbit.Should().Be(original.Rabbit);
        clone.PlayerRole.Should().Be(original.PlayerRole);
        clone.CurrentTurn.Should().Be(original.CurrentTurn);
        clone.Status.Should().Be(original.Status);
    }

    [Fact]
    public void Clone_ChildrenArray_IsIndependent()
    {
        // Arrange
        var original = CreateTestGameState();
        var originalFirstChild = original.Children[0];

        // Act
        var clone = original.Clone();
        clone.Children[0] = new Position(0, 1);

        // Assert
        original.Children[0].Should().Be(originalFirstChild);
        clone.Children[0].Should().NotBe(originalFirstChild);
    }

    [Fact]
    public void Clone_ModifyingOriginal_DoesNotAffectClone()
    {
        // Arrange
        var original = CreateTestGameState();
        var clone = original.Clone();
        var originalRabbitPos = clone.Rabbit;

        // Act
        original.Rabbit = new Position(9, 9);
        original.CurrentTurn = PlayerRole.Children;

        // Assert
        clone.Rabbit.Should().Be(originalRabbitPos);
    }

    [Fact]
    public void Clone_ModifyingClone_DoesNotAffectOriginal()
    {
        // Arrange
        var original = CreateTestGameState();
        var originalRabbitPos = original.Rabbit;
        var clone = original.Clone();

        // Act
        clone.Rabbit = new Position(9, 9);
        clone.Status = GameStatus.RabbitWins;

        // Assert
        original.Rabbit.Should().Be(originalRabbitPos);
        original.Status.Should().Be(GameStatus.Playing);
    }

    [Fact]
    public void Clone_PreservesAllChildren()
    {
        // Arrange
        var original = CreateTestGameState();

        // Act
        var clone = original.Clone();

        // Assert
        clone.Children.Should().HaveCount(original.Children.Length);
        for (int i = 0; i < original.Children.Length; i++)
        {
            clone.Children[i].Should().Be(original.Children[i]);
        }
    }

    #endregion

    #region GameState Properties

    [Fact]
    public void GameState_InitialValues_AreCorrect()
    {
        // Arrange & Act
        var state = new GameState();

        // Assert
        state.GameId.Should().NotBe(Guid.Empty);
        state.Status.Should().Be(GameStatus.Playing);
        state.MoveHistory.Should().NotBeNull();
        state.MoveHistory.Should().BeEmpty();
    }

    [Fact]
    public void GameState_Id_IsUnique()
    {
        // Arrange & Act
        var state1 = new GameState();
        var state2 = new GameState();

        // Assert
        state1.GameId.Should().NotBe(state2.GameId);
    }

    #endregion

    #region IsOccupied Tests

    [Fact]
    public void IsOccupied_PositionWithRabbit_ReturnsTrue()
    {
        // Arrange
        var state = CreateTestGameState();

        // Act
        var result = state.IsOccupied(state.Rabbit);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsOccupied_PositionWithChild_ReturnsTrue()
    {
        // Arrange
        var state = CreateTestGameState();
        var childPos = state.Children[0];

        // Act
        var result = state.IsOccupied(childPos);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsOccupied_EmptyPosition_ReturnsFalse()
    {
        // Arrange
        var state = CreateTestGameState();
        var emptyPos = new Position(5, 5); // Not occupied

        // Act
        var result = state.IsOccupied(emptyPos);

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region Helper Methods

    private GameState CreateTestGameState()
    {
        return new GameState
        {
            GameId = Guid.NewGuid(),
            Rabbit = new Position(7, 7),
            Children = new[]
            {
                new Position(1, 1),
                new Position(1, 3),
                new Position(1, 5),
                new Position(1, 7),
                new Position(1, 9)  // 5th child
            },
            PlayerRole = PlayerRole.Rabbit,
            CurrentTurn = PlayerRole.Rabbit,
            Status = GameStatus.Playing,
            PlayerThinkingTimeMs = 250
        };
    }

    #endregion
}
