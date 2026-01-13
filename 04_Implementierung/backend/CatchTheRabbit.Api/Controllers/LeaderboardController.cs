using CatchTheRabbit.Api.DTOs;
using CatchTheRabbit.Core.Models;
using CatchTheRabbit.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatchTheRabbit.Api.Controllers;

[ApiController]
[Route("api/leaderboard")]
public class LeaderboardController : ControllerBase
{
    private readonly ILeaderboardService _leaderboardService;
    private readonly ILogger<LeaderboardController> _logger;

    public LeaderboardController(ILeaderboardService leaderboardService, ILogger<LeaderboardController> logger)
    {
        _leaderboardService = leaderboardService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<LeaderboardResponse>> GetLeaderboard()
    {
        _logger.LogInformation("Getting leaderboard");

        var rabbitEntries = await _leaderboardService.GetTopEntriesAsync(PlayerRole.Rabbit);
        var childrenEntries = await _leaderboardService.GetTopEntriesAsync(PlayerRole.Children);

        return Ok(new LeaderboardResponse
        {
            Rabbit = rabbitEntries.Select((e, i) => new LeaderboardEntryDto(e, i + 1)).ToList(),
            Children = childrenEntries.Select((e, i) => new LeaderboardEntryDto(e, i + 1)).ToList()
        });
    }

    [HttpPost]
    public async Task<ActionResult<AddEntryResponse>> AddEntry([FromBody] AddLeaderboardEntryRequest request)
    {
        _logger.LogInformation("Adding leaderboard entry for game {GameId}", request.GameId);

        // Validate nickname
        if (string.IsNullOrWhiteSpace(request.Nickname))
        {
            return BadRequest(new AddEntryResponse { Success = false, Error = "Nickname cannot be empty" });
        }

        // For now, we'll trust the client about the game result
        // In production, you'd verify this against stored game data
        var entry = new LeaderboardEntry
        {
            Nickname = request.Nickname,
            Role = PlayerRole.Rabbit, // This should come from actual game state
            ThinkingTimeMs = 0 // This should come from actual game state
        };

        // Try to get game state if available
        if (GameStorage.TryGet(request.GameId, out var gameState) && gameState != null)
        {
            entry.Role = gameState.PlayerRole;
            entry.ThinkingTimeMs = gameState.PlayerThinkingTimeMs;
            _logger.LogInformation("Found game state: Role={Role}, ThinkingTime={ThinkingTime}ms",
                gameState.PlayerRole, gameState.PlayerThinkingTimeMs);
        }
        else
        {
            _logger.LogWarning("Game state not found for GameId {GameId}", request.GameId);
        }

        try
        {
            var id = await _leaderboardService.AddEntryAsync(entry);
            var rank = await _leaderboardService.GetRankAsync(entry.Role, entry.ThinkingTimeMs);

            _logger.LogInformation("Entry added with ID {Id}, rank {Rank}", id, rank);

            return Ok(new AddEntryResponse
            {
                Success = true,
                Rank = rank,
                Entry = new LeaderboardEntryDto(entry, rank)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding leaderboard entry");
            return StatusCode(500, new AddEntryResponse { Success = false, Error = "Failed to add entry" });
        }
    }
}
