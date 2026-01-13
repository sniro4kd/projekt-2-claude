using CatchTheRabbit.Core.Models;
using CatchTheRabbit.Infrastructure.Repositories;

namespace CatchTheRabbit.Tests.Unit;

public class LeaderboardRepositoryTests : IAsyncLifetime
{
    private readonly string _testDbPath;
    private readonly SqliteLeaderboardRepository _sut;

    public LeaderboardRepositoryTests()
    {
        _testDbPath = Path.Combine(Path.GetTempPath(), $"test_{Guid.NewGuid()}.db");
        _sut = new SqliteLeaderboardRepository($"Data Source={_testDbPath}");
    }

    public async Task InitializeAsync()
    {
        await _sut.InitializeDatabaseAsync();
    }

    public Task DisposeAsync()
    {
        if (File.Exists(_testDbPath))
        {
            try { File.Delete(_testDbPath); } catch { }
        }
        return Task.CompletedTask;
    }

    #region KT-LB-001: Eintrag speichern

    [Fact]
    public async Task AddEntryAsync_ValidEntry_SavesSuccessfully()
    {
        // Arrange
        var entry = CreateTestEntry("TestPlayer", PlayerRole.Rabbit, 500);

        // Act
        var id = await _sut.AddEntryAsync(entry);

        // Assert
        id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task AddEntryAsync_MultipleEntries_AllSaved()
    {
        // Arrange
        var entries = new[]
        {
            CreateTestEntry("Player1", PlayerRole.Rabbit, 100),
            CreateTestEntry("Player2", PlayerRole.Children, 200),
            CreateTestEntry("Player3", PlayerRole.Rabbit, 300)
        };

        // Act
        foreach (var entry in entries)
        {
            await _sut.AddEntryAsync(entry);
        }

        var rabbitEntries = await _sut.GetTopEntriesAsync(PlayerRole.Rabbit, 20);
        var childrenEntries = await _sut.GetTopEntriesAsync(PlayerRole.Children, 20);

        // Assert
        rabbitEntries.Count().Should().Be(2);
        childrenEntries.Count().Should().Be(1);
    }

    #endregion

    #region KT-LB-002: Top 20 abrufen

    [Fact]
    public async Task GetTopEntriesAsync_With25Entries_Returns20()
    {
        // Arrange
        for (int i = 0; i < 25; i++)
        {
            await _sut.AddEntryAsync(CreateTestEntry($"Player{i}", PlayerRole.Rabbit, 100 + i * 10));
        }

        // Act
        var result = await _sut.GetTopEntriesAsync(PlayerRole.Rabbit, 20);

        // Assert
        result.Count().Should().Be(20);
    }

    [Fact]
    public async Task GetTopEntriesAsync_SortedByThinkingTime()
    {
        // Arrange
        await _sut.AddEntryAsync(CreateTestEntry("Slow", PlayerRole.Rabbit, 900));
        await _sut.AddEntryAsync(CreateTestEntry("Fast", PlayerRole.Rabbit, 100));
        await _sut.AddEntryAsync(CreateTestEntry("Medium", PlayerRole.Rabbit, 500));

        // Act
        var result = (await _sut.GetTopEntriesAsync(PlayerRole.Rabbit, 20)).ToList();

        // Assert
        result.Should().BeInAscendingOrder(e => e.ThinkingTimeMs);
        result.First().Nickname.Should().Be("Fast");
        result.Last().Nickname.Should().Be("Slow");
    }

    [Fact]
    public async Task GetTopEntriesAsync_EmptyDatabase_ReturnsEmptyList()
    {
        // Act
        var result = await _sut.GetTopEntriesAsync(PlayerRole.Rabbit, 20);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetTopEntriesAsync_FiltersByRole()
    {
        // Arrange
        await _sut.AddEntryAsync(CreateTestEntry("RabbitPlayer", PlayerRole.Rabbit, 100));
        await _sut.AddEntryAsync(CreateTestEntry("ChildPlayer", PlayerRole.Children, 200));

        // Act
        var rabbitResult = (await _sut.GetTopEntriesAsync(PlayerRole.Rabbit, 20)).ToList();
        var childResult = (await _sut.GetTopEntriesAsync(PlayerRole.Children, 20)).ToList();

        // Assert
        rabbitResult.Should().HaveCount(1);
        rabbitResult.First().Nickname.Should().Be("RabbitPlayer");
        childResult.Should().HaveCount(1);
        childResult.First().Nickname.Should().Be("ChildPlayer");
    }

    #endregion

    #region KT-LB-003: Rang berechnen

    [Fact]
    public async Task CountBetterEntriesAsync_ReturnsCorrectCount()
    {
        // Arrange - 10 Einträge mit bekannten Zeiten
        for (int i = 1; i <= 10; i++)
        {
            await _sut.AddEntryAsync(CreateTestEntry($"Player{i}", PlayerRole.Rabbit, i * 100));
        }

        // Act - Count entries better than 550ms
        var count = await _sut.CountBetterEntriesAsync(PlayerRole.Rabbit, 550);

        // Assert - Should be 5 (100, 200, 300, 400, 500 are all < 550)
        count.Should().Be(5);
    }

    [Fact]
    public async Task CountBetterEntriesAsync_BestTime_ReturnsZero()
    {
        // Arrange
        await _sut.AddEntryAsync(CreateTestEntry("Second", PlayerRole.Rabbit, 200));
        await _sut.AddEntryAsync(CreateTestEntry("Third", PlayerRole.Rabbit, 300));

        // Act - Count entries better than 50ms (none exist)
        var count = await _sut.CountBetterEntriesAsync(PlayerRole.Rabbit, 50);

        // Assert
        count.Should().Be(0);
    }

    #endregion

    #region Helper Methods

    private LeaderboardEntry CreateTestEntry(string nickname, PlayerRole role, long thinkingTimeMs)
    {
        return new LeaderboardEntry
        {
            Nickname = nickname,
            Role = role,
            ThinkingTimeMs = thinkingTimeMs,
            CreatedAt = DateTime.UtcNow
        };
    }

    #endregion
}
