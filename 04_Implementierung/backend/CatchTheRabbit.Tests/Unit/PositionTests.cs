using CatchTheRabbit.Core.Models;

namespace CatchTheRabbit.Tests.Unit;

public class PositionTests
{
    #region KT-POS-001: Position Validierung

    [Theory]
    [InlineData(0, 0, true)]
    [InlineData(9, 9, true)]
    [InlineData(5, 5, true)]
    [InlineData(0, 9, true)]
    [InlineData(9, 0, true)]
    public void IsValid_ValidPositions_ReturnsTrue(int x, int y, bool expected)
    {
        // Arrange
        var position = new Position(x, y);

        // Act
        var result = position.IsValid();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(-1, -1)]
    [InlineData(10, 0)]
    [InlineData(0, 10)]
    [InlineData(10, 10)]
    [InlineData(11, 5)]
    [InlineData(5, 11)]
    public void IsValid_InvalidPositions_ReturnsFalse(int x, int y)
    {
        // Arrange
        var position = new Position(x, y);

        // Act
        var result = position.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region IsBlackField Tests

    [Theory]
    [InlineData(0, 1, true)]  // 0+1=1 is odd
    [InlineData(1, 0, true)]  // 1+0=1 is odd
    [InlineData(2, 3, true)]  // 2+3=5 is odd
    [InlineData(0, 0, false)] // 0+0=0 is even
    [InlineData(2, 2, false)] // 2+2=4 is even
    [InlineData(4, 4, false)] // 4+4=8 is even
    public void IsBlackField_ReturnsCorrectValue(int x, int y, bool expected)
    {
        // Arrange
        var position = new Position(x, y);

        // Act
        var result = position.IsBlackField();

        // Assert
        result.Should().Be(expected);
    }

    #endregion

    #region KT-POS-002: Position Gleichheit

    [Fact]
    public void Equals_SamePosition_ReturnsTrue()
    {
        // Arrange
        var pos1 = new Position(5, 5);
        var pos2 = new Position(5, 5);

        // Act & Assert
        pos1.Equals(pos2).Should().BeTrue();
        (pos1 == pos2).Should().BeTrue();
    }

    [Fact]
    public void Equals_DifferentX_ReturnsFalse()
    {
        // Arrange
        var pos1 = new Position(5, 5);
        var pos2 = new Position(6, 5);

        // Act & Assert
        pos1.Equals(pos2).Should().BeFalse();
        (pos1 == pos2).Should().BeFalse();
    }

    [Fact]
    public void Equals_DifferentY_ReturnsFalse()
    {
        // Arrange
        var pos1 = new Position(5, 5);
        var pos2 = new Position(5, 6);

        // Act & Assert
        pos1.Equals(pos2).Should().BeFalse();
        (pos1 != pos2).Should().BeTrue();
    }

    [Fact]
    public void GetHashCode_SamePosition_SameHashCode()
    {
        // Arrange
        var pos1 = new Position(5, 5);
        var pos2 = new Position(5, 5);

        // Act & Assert
        pos1.GetHashCode().Should().Be(pos2.GetHashCode());
    }

    #endregion

    #region Position ToString

    [Fact]
    public void ToString_ReturnsReadableFormat()
    {
        // Arrange
        var position = new Position(3, 7);

        // Act
        var result = position.ToString();

        // Assert
        result.Should().Contain("3");
        result.Should().Contain("7");
    }

    #endregion

    #region Position als Dictionary-Key

    [Fact]
    public void Position_CanBeUsedAsDictionaryKey()
    {
        // Arrange
        var dict = new Dictionary<Position, string>();
        var pos = new Position(5, 5);

        // Act
        dict[pos] = "TestValue";
        var samePos = new Position(5, 5);

        // Assert
        dict.Should().ContainKey(samePos);
        dict[samePos].Should().Be("TestValue");
    }

    [Fact]
    public void Position_InHashSet_WorksCorrectly()
    {
        // Arrange
        var set = new HashSet<Position>();
        var pos1 = new Position(5, 5);
        var pos2 = new Position(5, 5);

        // Act
        set.Add(pos1);
        set.Add(pos2);

        // Assert
        set.Should().HaveCount(1);
        set.Should().Contain(pos1);
    }

    #endregion
}
