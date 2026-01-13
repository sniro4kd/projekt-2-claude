using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CatchTheRabbit.Api.DTOs;

namespace CatchTheRabbit.Tests.Integration;

public class GameControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public GameControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    #region ST-API-001: Spiel starten als Hase

    [Fact]
    public async Task CreateGame_AsRabbit_ReturnsSuccess()
    {
        // Arrange
        var request = new CreateGameRequest { PlayerRole = "rabbit" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/game/new", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<GameStateResponse>(_jsonOptions);
        result.Should().NotBeNull();
        result!.GameId.Should().NotBeNullOrEmpty();
        result.PlayerRole.Should().Be("rabbit");
        result.CurrentTurn.Should().Be("rabbit");
        result.Status.Should().Be("playing");
    }

    [Fact]
    public async Task CreateGame_AsRabbit_RabbitOnValidPosition()
    {
        // Arrange
        var request = new CreateGameRequest { PlayerRole = "rabbit" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/game/new", request);
        var result = await response.Content.ReadFromJsonAsync<GameStateResponse>(_jsonOptions);

        // Assert
        result!.Rabbit.Should().NotBeNull();
        result.Rabbit.X.Should().BeInRange(0, 9);
        result.Rabbit.Y.Should().BeInRange(0, 9);
    }

    [Fact]
    public async Task CreateGame_AsRabbit_FourChildrenPresent()
    {
        // Arrange
        var request = new CreateGameRequest { PlayerRole = "rabbit" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/game/new", request);
        var result = await response.Content.ReadFromJsonAsync<GameStateResponse>(_jsonOptions);

        // Assert
        result!.Children.Should().HaveCount(4);
    }

    #endregion

    #region ST-API-002: Spiel starten als Kinder

    [Fact]
    public async Task CreateGame_AsChildren_ReturnsSuccess()
    {
        // Arrange
        var request = new CreateGameRequest { PlayerRole = "children" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/game/new", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<GameStateResponse>(_jsonOptions);
        result!.PlayerRole.Should().Be("children");
        // When player is children, AI (rabbit) makes first move, then it's children's turn
        result.CurrentTurn.Should().Be("children");
    }

    #endregion

    #region ST-API-003: Spiel starten mit ungültiger Rolle

    [Fact]
    public async Task CreateGame_InvalidRole_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateGameRequest { PlayerRole = "invalid" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/game/new", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region ST-API-004: Gültiger Spielzug

    [Fact]
    public async Task MakeMove_ValidMove_ReturnsSuccess()
    {
        // Arrange - Create game first
        var createRequest = new CreateGameRequest { PlayerRole = "rabbit" };
        var createResponse = await _client.PostAsJsonAsync("/api/game/new", createRequest);
        var gameState = await createResponse.Content.ReadFromJsonAsync<GameStateResponse>(_jsonOptions);
        var gameId = gameState!.GameId;

        // Get valid moves for rabbit
        var movesResponse = await _client.GetAsync($"/api/game/{gameId}/valid-moves?pieceType=rabbit");
        var validMoves = await movesResponse.Content.ReadFromJsonAsync<List<PositionDto>>(_jsonOptions);

        if (validMoves != null && validMoves.Count > 0)
        {
            var moveRequest = new MakeMoveRequest
            {
                PieceType = "rabbit",
                To = validMoves[0],
                ThinkingTimeMs = 100
            };

            // Act
            var response = await _client.PostAsJsonAsync($"/api/game/{gameId}/move", moveRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }

    #endregion

    #region ST-API-009: Spielzustand abrufen (gültige ID)

    [Fact]
    public async Task GetState_ValidId_ReturnsGameState()
    {
        // Arrange - Create game
        var createRequest = new CreateGameRequest { PlayerRole = "rabbit" };
        var createResponse = await _client.PostAsJsonAsync("/api/game/new", createRequest);
        var gameState = await createResponse.Content.ReadFromJsonAsync<GameStateResponse>(_jsonOptions);
        var gameId = gameState!.GameId;

        // Act
        var response = await _client.GetAsync($"/api/game/{gameId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<GameStateResponse>(_jsonOptions);
        result.Should().NotBeNull();
        result!.GameId.Should().Be(gameId);
    }

    #endregion

    #region ST-API-010: Spielzustand abrufen (ungültige ID)

    [Fact]
    public async Task GetState_InvalidId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync($"/api/game/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region ST-ERR-002: Health Check

    [Fact]
    public async Task HealthCheck_ReturnsHealthy()
    {
        // Act
        var response = await _client.GetAsync("/health");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    #endregion
}
