using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CatchTheRabbit.Api.DTOs;

namespace CatchTheRabbit.Tests.Integration;

public class LeaderboardControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public LeaderboardControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    #region ST-API-011: Leaderboard abrufen

    [Fact]
    public async Task GetLeaderboard_ReturnsSuccess()
    {
        // Act
        var response = await _client.GetAsync("/api/leaderboard");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<LeaderboardResponse>(_jsonOptions);
        result.Should().NotBeNull();
        result!.Rabbit.Should().NotBeNull();
        result.Children.Should().NotBeNull();
    }

    #endregion

    #region ST-API-014: Score eintragen (leerer Nickname)

    [Fact]
    public async Task AddEntry_EmptyNickname_ReturnsBadRequest()
    {
        // Arrange
        var request = new AddLeaderboardEntryRequest
        {
            GameId = Guid.NewGuid(),
            Nickname = ""
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/leaderboard", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region ST-API-015: Score eintragen (ungültige Game-ID)

    [Fact]
    public async Task AddEntry_NonExistentGame_StillSucceeds()
    {
        // Arrange
        var request = new AddLeaderboardEntryRequest
        {
            GameId = Guid.NewGuid(),
            Nickname = "TestPlayer"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/leaderboard", request);

        // Assert - Controller accepts entries even without matching game (uses defaults)
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    #endregion
}
