using CatchTheRabbit.Api.DTOs;
using CatchTheRabbit.Core;
using CatchTheRabbit.Core.Models;
using CatchTheRabbit.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatchTheRabbit.Api.Controllers;

[ApiController]
[Route("api/game")]
public class GameController : ControllerBase
{
    private readonly IGameService _gameService;
    private readonly IAIService _aiService;
    private readonly ILogger<GameController> _logger;

    public GameController(IGameService gameService, IAIService aiService, ILogger<GameController> logger)
    {
        _gameService = gameService;
        _aiService = aiService;
        _logger = logger;
    }

    [HttpPost("new")]
    public ActionResult<GameStateResponse> CreateGame([FromBody] CreateGameRequest request)
    {
        _logger.LogInformation("Creating new game with player role: {PlayerRole}", request.PlayerRole);

        if (!Enum.TryParse<PlayerRole>(request.PlayerRole, ignoreCase: true, out var playerRole))
        {
            return BadRequest(new { error = "Invalid player role. Must be 'rabbit' or 'children'." });
        }

        var gameState = _gameService.CreateGame(playerRole);

        // Store the game
        GameStorage.Store(gameState);

        _logger.LogInformation("Game created with ID: {GameId}", gameState.GameId);

        // If player chose children, AI (rabbit) makes the first move
        if (playerRole == PlayerRole.Children)
        {
            var aiMove = _aiService.CalculateBestMove(gameState, PlayerRole.Rabbit, AIConstants.DefaultMaxTimeMs);
            gameState = _gameService.ApplyMove(gameState, aiMove);
            GameStorage.Store(gameState);
            _logger.LogInformation("AI made first move as rabbit");
        }

        return Ok(new GameStateResponse(gameState));
    }

    [HttpPost("{gameId}/move")]
    public ActionResult<MoveResponse> MakeMove(Guid gameId, [FromBody] MakeMoveRequest request)
    {
        _logger.LogInformation("Move request for game {GameId}", gameId);

        if (!GameStorage.TryGet(gameId, out var gameState) || gameState == null)
        {
            return NotFound(new MoveResponse { Success = false, Error = "Game not found" });
        }

        if (gameState.Status != GameStatus.Playing)
        {
            return BadRequest(new MoveResponse { Success = false, Error = "Game is already over" });
        }

        // Parse piece type
        if (!Enum.TryParse<PieceType>(request.PieceType, ignoreCase: true, out var pieceType))
        {
            return BadRequest(new MoveResponse { Success = false, Error = "Invalid piece type" });
        }

        // Get current position for the move
        Position from;
        if (pieceType == PieceType.Rabbit)
        {
            from = gameState.Rabbit;
        }
        else
        {
            if (!request.PieceIndex.HasValue || request.PieceIndex < 0 || request.PieceIndex >= GameConstants.ChildrenCount)
            {
                return BadRequest(new MoveResponse { Success = false, Error = "Invalid piece index for child" });
            }
            from = gameState.Children[request.PieceIndex.Value];
        }

        var move = new Move
        {
            PieceType = pieceType,
            PieceIndex = request.PieceIndex,
            From = from,
            To = request.To.ToPosition()
        };

        // Validate move
        var validation = _gameService.ValidateMove(gameState, move);
        if (!validation.IsValid)
        {
            return BadRequest(new MoveResponse { Success = false, Error = validation.ErrorMessage });
        }

        // Update player thinking time
        gameState.PlayerThinkingTimeMs += request.ThinkingTimeMs;

        // Apply player move
        gameState = _gameService.ApplyMove(gameState, move);
        GameStorage.Store(gameState);

        _logger.LogInformation("Player move applied: {From} -> {To}", move.From, move.To);

        MoveDto? aiMoveDto = null;

        // If game is still playing, let AI make a move
        if (gameState.Status == GameStatus.Playing)
        {
            var aiRole = gameState.PlayerRole == PlayerRole.Rabbit ? PlayerRole.Children : PlayerRole.Rabbit;
            var aiMove = _aiService.CalculateBestMove(gameState, aiRole, AIConstants.DefaultMaxTimeMs);
            gameState = _gameService.ApplyMove(gameState, aiMove);
            GameStorage.Store(gameState);
            aiMoveDto = new MoveDto(aiMove);

            _logger.LogInformation("AI move applied: {From} -> {To}", aiMove.From, aiMove.To);
        }

        return Ok(new MoveResponse
        {
            Success = true,
            GameState = new GameStateResponse(gameState),
            AiMove = aiMoveDto
        });
    }

    [HttpGet("{gameId}")]
    public ActionResult<GameStateResponse> GetGame(Guid gameId)
    {
        if (!GameStorage.TryGet(gameId, out var gameState) || gameState == null)
        {
            return NotFound(new { error = "Game not found" });
        }

        return Ok(new GameStateResponse(gameState));
    }

    [HttpGet("{gameId}/valid-moves")]
    public ActionResult<List<PositionDto>> GetValidMoves(Guid gameId, [FromQuery] string pieceType, [FromQuery] int? pieceIndex)
    {
        if (!GameStorage.TryGet(gameId, out var gameState) || gameState == null)
        {
            return NotFound(new { error = "Game not found" });
        }

        if (!Enum.TryParse<PieceType>(pieceType, ignoreCase: true, out var type))
        {
            return BadRequest(new { error = "Invalid piece type" });
        }

        var validMoves = _gameService.GetValidMoves(gameState, type, pieceIndex);
        return Ok(validMoves.Select(p => new PositionDto(p)).ToList());
    }
}
