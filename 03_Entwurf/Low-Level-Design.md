# Low-Level-Design: CatchTheRabbit

| Attribut | Wert |
|----------|------|
| Projekt | CatchTheRabbit |
| Version | 1.0 |
| Datum | 21.03.2025 |
| Status | Entwurf |
| Basis | High-Level-Design v1.0, Pflichtenheft v1.0 |

---

## 1. Einleitung

### 1.1 Zweck

Dieses Dokument beschreibt das detaillierte Design der Systemkomponenten. Es enthält Klassendiagramme, Methodensignaturen, Algorithmen und Sequenzdiagramme für die Implementierung.

### 1.2 Scope

- Klassendiagramme für Backend und Frontend
- Detaillierte Methoden-Spezifikationen
- Algorithmus-Details (KI)
- Sequenzdiagramme für kritische Abläufe
- Datenstrukturen und DTOs

---

## 2. Backend-Klassendesign

### 2.1 Domain-Modell

```
┌─────────────────────────────────────────────────────────────────┐
│                         Domain Models                            │
│                                                                  │
│  ┌─────────────────────┐       ┌─────────────────────────────┐  │
│  │      Position       │       │        GameState            │  │
│  ├─────────────────────┤       ├─────────────────────────────┤  │
│  │ + X: int            │       │ + GameId: Guid              │  │
│  │ + Y: int            │       │ + PlayerRole: PlayerRole    │  │
│  ├─────────────────────┤       │ + CurrentTurn: PlayerRole   │  │
│  │ + IsValid(): bool   │       │ + Rabbit: Position          │  │
│  │ + IsBlackField():   │       │ + Children: Position[4]     │  │
│  │     bool            │       │ + Status: GameStatus        │  │
│  │ + Equals(): bool    │       │ + PlayerThinkingTimeMs: long│  │
│  └─────────────────────┘       │ + MoveHistory: List<Move>   │  │
│                                ├─────────────────────────────┤  │
│  ┌─────────────────────┐       │ + Clone(): GameState        │  │
│  │        Move         │       │ + IsOccupied(pos): bool     │  │
│  ├─────────────────────┤       │ + GetAllPieces(): Position[]│  │
│  │ + PieceType: Piece  │       └─────────────────────────────┘  │
│  │ + PieceIndex: int?  │                                        │
│  │ + From: Position    │       ┌─────────────────────────────┐  │
│  │ + To: Position      │       │     LeaderboardEntry        │  │
│  │ + Timestamp: DateTime       ├─────────────────────────────┤  │
│  └─────────────────────┘       │ + Id: int                   │  │
│                                │ + Nickname: string          │  │
│  ┌─────────────────────┐       │ + Role: PlayerRole          │  │
│  │   <<enumeration>>   │       │ + ThinkingTimeMs: long      │  │
│  │     PlayerRole      │       │ + CreatedAt: DateTime       │  │
│  ├─────────────────────┤       └─────────────────────────────┘  │
│  │ Rabbit              │                                        │
│  │ Children            │       ┌─────────────────────────────┐  │
│  └─────────────────────┘       │   <<enumeration>>           │  │
│                                │      GameStatus             │  │
│  ┌─────────────────────┐       ├─────────────────────────────┤  │
│  │   <<enumeration>>   │       │ Playing                     │  │
│  │       Piece         │       │ RabbitWins                  │  │
│  ├─────────────────────┤       │ ChildrenWin                 │  │
│  │ Rabbit              │       └─────────────────────────────┘  │
│  │ Child               │                                        │
│  └─────────────────────┘                                        │
└─────────────────────────────────────────────────────────────────┘
```

### 2.2 Service-Klassen

```
┌─────────────────────────────────────────────────────────────────┐
│                        Business Logic                            │
│                                                                  │
│  ┌─────────────────────────────────────────────────────────┐    │
│  │                    IGameService                          │    │
│  │                    «interface»                           │    │
│  ├─────────────────────────────────────────────────────────┤    │
│  │ + CreateGame(playerRole: PlayerRole): GameState         │    │
│  │ + ValidateMove(state, move): ValidationResult           │    │
│  │ + ApplyMove(state, move): GameState                     │    │
│  │ + GetValidMoves(state, piece, index?): List<Position>   │    │
│  │ + CheckGameEnd(state): GameStatus                       │    │
│  └─────────────────────────────────────────────────────────┘    │
│                              ▲                                   │
│                              │ implements                        │
│  ┌─────────────────────────────────────────────────────────┐    │
│  │                     GameService                          │    │
│  ├─────────────────────────────────────────────────────────┤    │
│  │ - _random: Random                                        │    │
│  ├─────────────────────────────────────────────────────────┤    │
│  │ + CreateGame(playerRole): GameState                     │    │
│  │ + ValidateMove(state, move): ValidationResult           │    │
│  │ + ApplyMove(state, move): GameState                     │    │
│  │ + GetValidMoves(state, piece, index?): List<Position>   │    │
│  │ + CheckGameEnd(state): GameStatus                       │    │
│  │ - GenerateRandomPosition(minY, maxY, excluded):Position │    │
│  │ - IsValidPosition(pos): bool                            │    │
│  │ - IsBlackField(x, y): bool                              │    │
│  └─────────────────────────────────────────────────────────┘    │
│                                                                  │
│  ┌─────────────────────────────────────────────────────────┐    │
│  │                     IAIService                           │    │
│  │                    «interface»                           │    │
│  ├─────────────────────────────────────────────────────────┤    │
│  │ + CalculateBestMove(state, aiRole, timeLimit): Move     │    │
│  └─────────────────────────────────────────────────────────┘    │
│                              ▲                                   │
│                              │ implements                        │
│  ┌─────────────────────────────────────────────────────────┐    │
│  │                      AIService                           │    │
│  ├─────────────────────────────────────────────────────────┤    │
│  │ - _gameService: IGameService                            │    │
│  │ - _maxDepth: int                                        │    │
│  │ - _maxTimeMs: int                                       │    │
│  ├─────────────────────────────────────────────────────────┤    │
│  │ + CalculateBestMove(state, aiRole, timeLimit): Move     │    │
│  │ - Minimax(state, depth, alpha, beta, max, aiRole): int  │    │
│  │ - Evaluate(state, aiRole): int                          │    │
│  │ - GetAllMoves(state, role): List<Move>                  │    │
│  │ - CalcRabbitProgress(rabbit): int                       │    │
│  │ - CalcMobility(state, rabbit): int                      │    │
│  │ - CalcEncirclement(rabbit, children): int               │    │
│  │ - CalcFormation(children): int                          │    │
│  └─────────────────────────────────────────────────────────┘    │
│                                                                  │
│  ┌─────────────────────────────────────────────────────────┐    │
│  │                  ILeaderboardService                     │    │
│  │                    «interface»                           │    │
│  ├─────────────────────────────────────────────────────────┤    │
│  │ + GetTopEntriesAsync(role, count): Task<List<Entry>>    │    │
│  │ + AddEntryAsync(entry): Task<int>                       │    │
│  │ + GetRankAsync(role, timeMs): Task<int>                 │    │
│  └─────────────────────────────────────────────────────────┘    │
│                              ▲                                   │
│                              │ implements                        │
│  ┌─────────────────────────────────────────────────────────┐    │
│  │                   LeaderboardService                     │    │
│  ├─────────────────────────────────────────────────────────┤    │
│  │ - _repository: ILeaderboardRepository                   │    │
│  │ - _maxEntries: int                                      │    │
│  ├─────────────────────────────────────────────────────────┤    │
│  │ + GetTopEntriesAsync(role, count): Task<List<Entry>>    │    │
│  │ + AddEntryAsync(entry): Task<int>                       │    │
│  │ + GetRankAsync(role, timeMs): Task<int>                 │    │
│  │ - ValidateNickname(nickname): ValidationResult          │    │
│  └─────────────────────────────────────────────────────────┘    │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

### 2.3 Repository-Klassen

```
┌─────────────────────────────────────────────────────────────────┐
│                        Data Access                               │
│                                                                  │
│  ┌─────────────────────────────────────────────────────────┐    │
│  │                ILeaderboardRepository                    │    │
│  │                    «interface»                           │    │
│  ├─────────────────────────────────────────────────────────┤    │
│  │ + GetTopEntriesAsync(role, count): Task<List<Entry>>    │    │
│  │ + AddEntryAsync(entry): Task<int>                       │    │
│  │ + CountEntriesAsync(role): Task<int>                    │    │
│  └─────────────────────────────────────────────────────────┘    │
│                              ▲                                   │
│                              │ implements                        │
│  ┌─────────────────────────────────────────────────────────┐    │
│  │              SqliteLeaderboardRepository                 │    │
│  ├─────────────────────────────────────────────────────────┤    │
│  │ - _connectionString: string                             │    │
│  ├─────────────────────────────────────────────────────────┤    │
│  │ + GetTopEntriesAsync(role, count): Task<List<Entry>>    │    │
│  │ + AddEntryAsync(entry): Task<int>                       │    │
│  │ + CountEntriesAsync(role): Task<int>                    │    │
│  │ - GetConnection(): IDbConnection                        │    │
│  │ + InitializeDatabaseAsync(): Task                       │    │
│  └─────────────────────────────────────────────────────────┘    │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

### 2.4 Controller und DTOs

```
┌─────────────────────────────────────────────────────────────────┐
│                    Presentation Layer                            │
│                                                                  │
│  ┌─────────────────────────────────────────────────────────┐    │
│  │                    GameController                        │    │
│  │                  [ApiController]                         │    │
│  │                  [Route("api/game")]                     │    │
│  ├─────────────────────────────────────────────────────────┤    │
│  │ - _gameService: IGameService                            │    │
│  │ - _aiService: IAIService                                │    │
│  │ - _hubContext: IHubContext<GameHub>                     │    │
│  ├─────────────────────────────────────────────────────────┤    │
│  │ [POST("new")]                                           │    │
│  │ + CreateGame(req: CreateGameRequest): ActionResult      │    │
│  │                                                          │    │
│  │ [POST("{gameId}/move")]                                 │    │
│  │ + MakeMove(gameId, req: MakeMoveRequest): ActionResult  │    │
│  └─────────────────────────────────────────────────────────┘    │
│                                                                  │
│  ┌─────────────────────────────────────────────────────────┐    │
│  │                 LeaderboardController                    │    │
│  │                  [ApiController]                         │    │
│  │                  [Route("api/leaderboard")]             │    │
│  ├─────────────────────────────────────────────────────────┤    │
│  │ - _leaderboardService: ILeaderboardService              │    │
│  ├─────────────────────────────────────────────────────────┤    │
│  │ [GET]                                                   │    │
│  │ + GetLeaderboard(): ActionResult<LeaderboardResponse>   │    │
│  │                                                          │    │
│  │ [POST]                                                  │    │
│  │ + AddEntry(req: AddEntryRequest): ActionResult          │    │
│  └─────────────────────────────────────────────────────────┘    │
│                                                                  │
│  ┌───────────────────┐  ┌───────────────────┐                   │
│  │ CreateGameRequest │  │ MakeMoveRequest   │                   │
│  ├───────────────────┤  ├───────────────────┤                   │
│  │ + PlayerRole:     │  │ + PieceType: str  │                   │
│  │     string        │  │ + PieceIndex: int?│                   │
│  └───────────────────┘  │ + To: PositionDto │                   │
│                         └───────────────────┘                   │
│  ┌───────────────────┐  ┌───────────────────┐                   │
│  │ GameStateResponse │  │ MoveResponse      │                   │
│  ├───────────────────┤  ├───────────────────┤                   │
│  │ + GameId: string  │  │ + Success: bool   │                   │
│  │ + PlayerRole: str │  │ + GameState: obj  │                   │
│  │ + CurrentTurn: str│  │ + AiMove: MoveDto?│                   │
│  │ + Rabbit: PosDto  │  │ + Error: string?  │                   │
│  │ + Children: []    │  └───────────────────┘                   │
│  │ + Status: string  │                                          │
│  └───────────────────┘                                          │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

---

## 3. Frontend-Klassendesign

### 3.1 TypeScript-Interfaces

```typescript
// types/game.ts

interface Position {
  x: number;
  y: number;
}

interface Move {
  pieceType: 'rabbit' | 'child';
  pieceIndex?: number;
  from: Position;
  to: Position;
}

interface GameState {
  gameId: string;
  playerRole: 'rabbit' | 'children';
  currentTurn: 'rabbit' | 'children';
  rabbit: Position;
  children: Position[];
  status: 'playing' | 'rabbit_wins' | 'children_win';
  playerThinkingTimeMs: number;
}

interface LeaderboardEntry {
  rank: number;
  nickname: string;
  thinkingTimeMs: number;
}

interface Leaderboard {
  rabbit: LeaderboardEntry[];
  children: LeaderboardEntry[];
}
```

### 3.2 Pinia Store Design

```typescript
// stores/gameStore.ts

interface GameStoreState {
  gameState: GameState | null;
  selectedPiece: { type: 'rabbit' | 'child'; index?: number } | null;
  validMoves: Position[];
  isLoading: boolean;
  error: string | null;
  timerInterval: number | null;
}

interface GameStoreActions {
  createGame(playerRole: 'rabbit' | 'children'): Promise<void>;
  selectPiece(type: 'rabbit' | 'child', index?: number): void;
  makeMove(to: Position): Promise<void>;
  clearSelection(): void;
  startTimer(): void;
  stopTimer(): void;
  resetGame(): void;
}

interface GameStoreGetters {
  isPlayerTurn: boolean;
  isGameOver: boolean;
  winner: 'rabbit' | 'children' | null;
  formattedTime: string;
}
```

```typescript
// stores/leaderboardStore.ts

interface LeaderboardStoreState {
  leaderboard: Leaderboard | null;
  isLoading: boolean;
  error: string | null;
}

interface LeaderboardStoreActions {
  fetchLeaderboard(): Promise<void>;
  submitScore(gameId: string, nickname: string): Promise<number>;
}
```

### 3.3 Vue-Komponenten-Struktur

```
┌─────────────────────────────────────────────────────────────────┐
│                    Vue Component Hierarchy                       │
│                                                                  │
│  App.vue                                                        │
│  ├── HeaderBar.vue                                              │
│  │   └── Props: none                                            │
│  │   └── Emits: none                                            │
│  │                                                              │
│  └── <router-view>                                              │
│      │                                                          │
│      ├── HomeView.vue                                           │
│      │   └── RoleSelect.vue                                     │
│      │       └── Props: none                                    │
│      │       └── Emits: @select(role: string)                   │
│      │                                                          │
│      ├── GameView.vue                                           │
│      │   ├── GameBoard.vue                                      │
│      │   │   └── Props: gameState, selectedPiece, validMoves   │
│      │   │   └── Emits: @cell-click(x, y)                      │
│      │   │   │                                                  │
│      │   │   └── BoardCell.vue                                  │
│      │   │       └── Props: x, y, isBlack, piece, isSelected,  │
│      │   │                  isValidMove                         │
│      │   │       └── Emits: @click                             │
│      │   │       │                                              │
│      │   │       └── GamePiece.vue                             │
│      │   │           └── Props: type('rabbit'|'child'), index  │
│      │   │                                                      │
│      │   ├── GameControls.vue                                   │
│      │   │   └── Props: thinkingTime, currentTurn, playerRole  │
│      │   │   └── Emits: @toggle-sound                          │
│      │   │   │                                                  │
│      │   │   └── Timer.vue                                     │
│      │   │       └── Props: timeMs                             │
│      │   │                                                      │
│      │   └── GameEndModal.vue                                   │
│      │       └── Props: winner, playerRole, thinkingTime       │
│      │       └── Emits: @submit-score(nickname), @new-game,    │
│      │                  @skip                                   │
│      │                                                          │
│      └── LeaderboardView.vue                                    │
│          └── LeaderboardList.vue                                │
│              └── Props: entries, title                          │
│              │                                                  │
│              └── LeaderboardItem.vue                            │
│                  └── Props: rank, nickname, time                │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

### 3.4 Service-Klassen (Frontend)

```typescript
// services/gameService.ts

class GameService {
  private baseUrl: string;

  constructor(baseUrl: string);

  async createGame(playerRole: string): Promise<GameState>;
  async makeMove(gameId: string, move: MoveRequest): Promise<MoveResponse>;
}

// services/leaderboardService.ts

class LeaderboardService {
  private baseUrl: string;

  constructor(baseUrl: string);

  async getLeaderboard(): Promise<Leaderboard>;
  async submitScore(gameId: string, nickname: string): Promise<SubmitResponse>;
}

// services/signalRService.ts

class SignalRService {
  private connection: HubConnection | null;

  async connect(hubUrl: string): Promise<void>;
  async joinGame(gameId: string): Promise<void>;
  async leaveGame(gameId: string): Promise<void>;

  onGameStateUpdated(callback: (state: GameState) => void): void;
  onAiMoveStarted(callback: () => void): void;
  onAiMoveCompleted(callback: (move: Move) => void): void;
  onGameEnded(callback: (result: GameResult) => void): void;

  async disconnect(): Promise<void>;
}
```

---

## 4. Algorithmus-Details

### 4.1 Zugvalidierung (Detail)

```csharp
// GameService.cs

public ValidationResult ValidateMove(GameState state, Move move)
{
    // 1. Prüfe ob Spieler am Zug
    if (move.PieceType == Piece.Rabbit && state.CurrentTurn != PlayerRole.Rabbit)
        return ValidationResult.Fail("Nicht am Zug");

    if (move.PieceType == Piece.Child && state.CurrentTurn != PlayerRole.Children)
        return ValidationResult.Fail("Nicht am Zug");

    // 2. Hole aktuelle Position
    Position from = move.PieceType == Piece.Rabbit
        ? state.Rabbit
        : state.Children[move.PieceIndex.Value];

    // 3. Prüfe Zielfeld im Spielfeld
    if (move.To.X < 0 || move.To.X > 9 || move.To.Y < 0 || move.To.Y > 9)
        return ValidationResult.Fail("Zielfeld außerhalb des Spielfelds");

    // 4. Prüfe schwarzes Feld
    if (!IsBlackField(move.To.X, move.To.Y))
        return ValidationResult.Fail("Zielfeld ist kein schwarzes Feld");

    // 5. Prüfe Feld nicht besetzt
    if (state.IsOccupied(move.To))
        return ValidationResult.Fail("Zielfeld ist besetzt");

    // 6. Prüfe diagonale Bewegung (1 Feld)
    int deltaX = Math.Abs(move.To.X - from.X);
    int deltaY = Math.Abs(move.To.Y - from.Y);

    if (deltaX != 1 || deltaY != 1)
        return ValidationResult.Fail("Nur diagonale Züge um 1 Feld erlaubt");

    // 7. Prüfe Bewegungsrichtung
    if (move.PieceType == Piece.Child)
    {
        if (move.To.Y <= from.Y)
            return ValidationResult.Fail("Kinder können nur nach unten ziehen");
    }

    return ValidationResult.Success();
}

private bool IsBlackField(int x, int y) => (x + y) % 2 == 1;
```

### 4.2 KI-Bewertungsfunktion (Detail)

```csharp
// AIService.cs

private int Evaluate(GameState state, PlayerRole aiRole)
{
    // Terminale Zustände
    if (state.Status == GameStatus.RabbitWins)
        return aiRole == PlayerRole.Rabbit ? 10000 : -10000;

    if (state.Status == GameStatus.ChildrenWin)
        return aiRole == PlayerRole.Children ? 10000 : -10000;

    int score = 0;

    // Faktor 1: Hasen-Fortschritt (0-9, wobei 0 = Ziel)
    // Gewichtung: 100 Punkte pro Reihe
    int rabbitProgress = (9 - state.Rabbit.Y) * 100;

    // Faktor 2: Hasen-Mobilität (0-4 mögliche Züge)
    // Gewichtung: 50 Punkte pro möglichem Zug
    int rabbitMoves = GetValidMoves(state, Piece.Rabbit, null).Count;
    int mobilityScore = rabbitMoves * 50;

    // Faktor 3: Einkreisung
    // Prüfe wie viele Kinder den Hasen "bedrohen"
    int encirclement = CalculateEncirclement(state.Rabbit, state.Children);
    int encirclementScore = encirclement * 30;

    // Faktor 4: Kinder-Formation
    // Bonus wenn Kinder eine Linie bilden
    int formation = CalculateFormation(state.Children);
    int formationScore = formation * 20;

    // Zusammenrechnen je nach KI-Rolle
    if (aiRole == PlayerRole.Rabbit)
    {
        score = rabbitProgress + mobilityScore - encirclementScore;
    }
    else
    {
        score = encirclementScore + formationScore - rabbitProgress - mobilityScore;
    }

    return score;
}

private int CalculateEncirclement(Position rabbit, Position[] children)
{
    int count = 0;

    // Prüfe alle 4 diagonalen Nachbarfelder des Hasen
    int[] dx = { -1, 1, -1, 1 };
    int[] dy = { -1, -1, 1, 1 };

    for (int i = 0; i < 4; i++)
    {
        int nx = rabbit.X + dx[i];
        int ny = rabbit.Y + dy[i];

        // Außerhalb des Spielfelds zählt als blockiert
        if (nx < 0 || nx > 9 || ny < 0 || ny > 9)
        {
            count++;
            continue;
        }

        // Kind auf diesem Feld?
        if (children.Any(c => c.X == nx && c.Y == ny))
        {
            count++;
        }
    }

    return count;
}

private int CalculateFormation(Position[] children)
{
    // Bonus wenn Kinder horizontal nahe beieinander sind
    int minX = children.Min(c => c.X);
    int maxX = children.Max(c => c.X);
    int spread = maxX - minX;

    // Je enger, desto besser (max 7 Punkte bei Spread von 3)
    return Math.Max(0, 10 - spread);
}
```

### 4.3 Minimax mit Iterative Deepening (Detail)

```csharp
// AIService.cs

public Move CalculateBestMove(GameState state, PlayerRole aiRole, int timeLimitMs)
{
    var stopwatch = Stopwatch.StartNew();
    Move bestMove = null;

    // Iterative Deepening
    for (int depth = 1; depth <= _maxDepth; depth++)
    {
        // Zeitcheck
        if (stopwatch.ElapsedMilliseconds > timeLimitMs * 0.9)
            break;

        var currentBest = MinimaxRoot(state, depth, aiRole, timeLimitMs - stopwatch.ElapsedMilliseconds);

        if (currentBest != null)
            bestMove = currentBest;
    }

    // Fallback: zufälliger gültiger Zug
    if (bestMove == null)
    {
        var moves = GetAllMoves(state, aiRole);
        bestMove = moves[_random.Next(moves.Count)];
    }

    return bestMove;
}

private Move MinimaxRoot(GameState state, int depth, PlayerRole aiRole, long remainingTimeMs)
{
    var moves = GetAllMoves(state, aiRole);
    Move bestMove = null;
    int bestValue = int.MinValue;

    foreach (var move in moves)
    {
        var newState = _gameService.ApplyMove(state, move);
        int value = Minimax(newState, depth - 1, int.MinValue, int.MaxValue, false, aiRole);

        if (value > bestValue)
        {
            bestValue = value;
            bestMove = move;
        }
    }

    return bestMove;
}

private int Minimax(GameState state, int depth, int alpha, int beta, bool maximizing, PlayerRole aiRole)
{
    // Basis: Tiefe 0 oder Spielende
    if (depth == 0 || state.Status != GameStatus.Playing)
        return Evaluate(state, aiRole);

    var currentRole = maximizing
        ? aiRole
        : (aiRole == PlayerRole.Rabbit ? PlayerRole.Children : PlayerRole.Rabbit);

    var moves = GetAllMoves(state, currentRole);

    if (maximizing)
    {
        int maxEval = int.MinValue;

        foreach (var move in moves)
        {
            var newState = _gameService.ApplyMove(state, move);
            int eval = Minimax(newState, depth - 1, alpha, beta, false, aiRole);
            maxEval = Math.Max(maxEval, eval);
            alpha = Math.Max(alpha, eval);

            if (beta <= alpha)
                break; // Beta-Cutoff
        }

        return maxEval;
    }
    else
    {
        int minEval = int.MaxValue;

        foreach (var move in moves)
        {
            var newState = _gameService.ApplyMove(state, move);
            int eval = Minimax(newState, depth - 1, alpha, beta, true, aiRole);
            minEval = Math.Min(minEval, eval);
            beta = Math.Min(beta, eval);

            if (beta <= alpha)
                break; // Alpha-Cutoff
        }

        return minEval;
    }
}
```

---

## 5. Sequenzdiagramme

### 5.1 Spielstart-Sequenz

```
┌────────┐     ┌────────┐     ┌──────────┐     ┌───────────┐     ┌─────────┐
│ User   │     │HomeView│     │gameStore │     │gameService│     │ Backend │
└───┬────┘     └───┬────┘     └────┬─────┘     └─────┬─────┘     └────┬────┘
    │              │               │                 │                │
    │ Klick        │               │                 │                │
    │ "Hase"       │               │                 │                │
    │─────────────►│               │                 │                │
    │              │               │                 │                │
    │              │ createGame    │                 │                │
    │              │ ('rabbit')    │                 │                │
    │              │──────────────►│                 │                │
    │              │               │                 │                │
    │              │               │ createGame()    │                │
    │              │               │────────────────►│                │
    │              │               │                 │                │
    │              │               │                 │ POST           │
    │              │               │                 │ /api/game/new  │
    │              │               │                 │───────────────►│
    │              │               │                 │                │
    │              │               │                 │                │ Generate
    │              │               │                 │                │ Positions
    │              │               │                 │                │
    │              │               │                 │◄───────────────│
    │              │               │                 │  GameState     │
    │              │               │                 │                │
    │              │               │◄────────────────│                │
    │              │               │   GameState     │                │
    │              │               │                 │                │
    │              │◄──────────────│                 │                │
    │              │  state updated│                 │                │
    │              │               │                 │                │
    │              │ navigate to   │                 │                │
    │              │ /game         │                 │                │
    │◄─────────────│               │                 │                │
    │ Show         │               │                 │                │
    │ GameView     │               │                 │                │
    │              │               │                 │                │
```

### 5.2 Spielzug-Sequenz (Spieler + KI)

```
┌────────┐  ┌─────────┐  ┌──────────┐  ┌───────────┐  ┌─────────┐  ┌─────────┐
│ User   │  │GameBoard│  │gameStore │  │gameService│  │ Backend │  │AIService│
└───┬────┘  └────┬────┘  └────┬─────┘  └─────┬─────┘  └────┬────┘  └────┬────┘
    │            │            │              │             │            │
    │ Klick      │            │              │             │            │
    │ Hase       │            │              │             │            │
    │───────────►│            │              │             │            │
    │            │            │              │             │            │
    │            │selectPiece │              │             │            │
    │            │('rabbit')  │              │             │            │
    │            │───────────►│              │             │            │
    │            │            │              │             │            │
    │            │            │ berechne     │             │            │
    │            │            │ validMoves   │             │            │
    │            │            │              │             │            │
    │            │◄───────────│              │             │            │
    │◄───────────│ zeige      │              │             │            │
    │ Highlight  │ Moves      │              │             │            │
    │            │            │              │             │            │
    │ Klick      │            │              │             │            │
    │ Zielfeld   │            │              │             │            │
    │───────────►│            │              │             │            │
    │            │            │              │             │            │
    │            │ makeMove   │              │             │            │
    │            │ (to)       │              │             │            │
    │            │───────────►│              │             │            │
    │            │            │              │             │            │
    │            │            │ makeMove()   │             │            │
    │            │            │─────────────►│             │            │
    │            │            │              │             │            │
    │            │            │              │ POST        │            │
    │            │            │              │ /game/move  │            │
    │            │            │              │────────────►│            │
    │            │            │              │             │            │
    │            │            │              │             │ Validate   │
    │            │            │              │             │ Move       │
    │            │            │              │             │            │
    │            │            │              │             │ Apply      │
    │            │            │              │             │ Move       │
    │            │            │              │             │            │
    │            │            │              │             │ Calculate  │
    │            │            │              │             │ AI Move    │
    │            │            │              │             │───────────►│
    │            │            │              │             │            │
    │            │            │              │             │            │ Minimax
    │            │            │              │             │            │ Search
    │            │            │              │             │            │
    │            │            │              │             │◄───────────│
    │            │            │              │             │  Best Move │
    │            │            │              │             │            │
    │            │            │              │             │ Apply      │
    │            │            │              │             │ AI Move    │
    │            │            │              │             │            │
    │            │            │              │◄────────────│            │
    │            │            │              │ Response    │            │
    │            │            │              │ (state+     │            │
    │            │            │              │  aiMove)    │            │
    │            │            │              │             │            │
    │            │            │◄─────────────│             │            │
    │            │            │  update      │             │            │
    │            │            │  state       │             │            │
    │            │◄───────────│              │             │            │
    │◄───────────│ animate    │              │             │            │
    │ Show       │ moves      │              │             │            │
    │ Animation  │            │              │             │            │
    │            │            │              │             │            │
```

### 5.3 Bestenlisten-Eintrag-Sequenz

```
┌────────┐  ┌────────────┐  ┌─────────────┐  ┌────────────────┐  ┌─────────┐
│ User   │  │GameEndModal│  │leaderboard  │  │leaderboard     │  │ Backend │
│        │  │            │  │Store        │  │Service         │  │         │
└───┬────┘  └─────┬──────┘  └──────┬──────┘  └───────┬────────┘  └────┬────┘
    │             │                │                 │                │
    │ Enter       │                │                 │                │
    │ Nickname    │                │                 │                │
    │────────────►│                │                 │                │
    │             │                │                 │                │
    │ Klick       │                │                 │                │
    │ "Eintragen" │                │                 │                │
    │────────────►│                │                 │                │
    │             │                │                 │                │
    │             │ submitScore    │                 │                │
    │             │ (gameId,       │                 │                │
    │             │  nickname)     │                 │                │
    │             │───────────────►│                 │                │
    │             │                │                 │                │
    │             │                │ submitScore()   │                │
    │             │                │────────────────►│                │
    │             │                │                 │                │
    │             │                │                 │ POST           │
    │             │                │                 │ /api/          │
    │             │                │                 │ leaderboard    │
    │             │                │                 │───────────────►│
    │             │                │                 │                │
    │             │                │                 │                │ Validate
    │             │                │                 │                │ Save
    │             │                │                 │                │ Get Rank
    │             │                │                 │                │
    │             │                │                 │◄───────────────│
    │             │                │                 │  { rank: 5 }   │
    │             │                │                 │                │
    │             │                │◄────────────────│                │
    │             │                │   rank          │                │
    │             │◄───────────────│                 │                │
    │◄────────────│ "Platz 5!"     │                 │                │
    │ Show        │                │                 │                │
    │ Success     │                │                 │                │
    │             │                │                 │                │
```

---

## 6. Datenbankzugriff

### 6.1 SQL-Queries

```csharp
// SqliteLeaderboardRepository.cs

public async Task<IEnumerable<LeaderboardEntry>> GetTopEntriesAsync(
    PlayerRole role,
    int count)
{
    const string sql = @"
        SELECT Id, Nickname, Role, ThinkingTimeMs, CreatedAt
        FROM LeaderboardEntries
        WHERE Role = @Role
        ORDER BY ThinkingTimeMs ASC
        LIMIT @Count";

    using var connection = GetConnection();
    return await connection.QueryAsync<LeaderboardEntry>(sql, new
    {
        Role = role.ToString().ToLower(),
        Count = count
    });
}

public async Task<int> AddEntryAsync(LeaderboardEntry entry)
{
    const string sql = @"
        INSERT INTO LeaderboardEntries (Nickname, Role, ThinkingTimeMs, CreatedAt)
        VALUES (@Nickname, @Role, @ThinkingTimeMs, @CreatedAt);
        SELECT last_insert_rowid();";

    using var connection = GetConnection();
    return await connection.ExecuteScalarAsync<int>(sql, new
    {
        entry.Nickname,
        Role = entry.Role.ToString().ToLower(),
        entry.ThinkingTimeMs,
        CreatedAt = DateTime.UtcNow.ToString("o")
    });
}

public async Task InitializeDatabaseAsync()
{
    const string sql = @"
        CREATE TABLE IF NOT EXISTS LeaderboardEntries (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Nickname TEXT NOT NULL CHECK(length(Nickname) <= 50),
            Role TEXT NOT NULL CHECK(Role IN ('rabbit', 'children')),
            ThinkingTimeMs INTEGER NOT NULL,
            CreatedAt TEXT NOT NULL
        );

        CREATE INDEX IF NOT EXISTS IX_Leaderboard_Role_Time
        ON LeaderboardEntries(Role, ThinkingTimeMs ASC);";

    using var connection = GetConnection();
    await connection.ExecuteAsync(sql);
}
```

---

## 7. Projektstruktur

### 7.1 Backend-Projektstruktur

```
backend/
├── CatchTheRabbit.Api/
│   ├── Controllers/
│   │   ├── GameController.cs
│   │   └── LeaderboardController.cs
│   ├── Hubs/
│   │   └── GameHub.cs
│   ├── Program.cs
│   └── appsettings.json
│
├── CatchTheRabbit.Core/
│   ├── Models/
│   │   ├── Position.cs
│   │   ├── Move.cs
│   │   ├── GameState.cs
│   │   ├── LeaderboardEntry.cs
│   │   └── Enums.cs
│   ├── Services/
│   │   ├── IGameService.cs
│   │   ├── GameService.cs
│   │   ├── IAIService.cs
│   │   ├── AIService.cs
│   │   ├── ILeaderboardService.cs
│   │   └── LeaderboardService.cs
│   └── Interfaces/
│       └── ILeaderboardRepository.cs
│
├── CatchTheRabbit.Infrastructure/
│   └── Repositories/
│       └── SqliteLeaderboardRepository.cs
│
├── CatchTheRabbit.Tests/
│   ├── GameServiceTests.cs
│   ├── AIServiceTests.cs
│   └── LeaderboardServiceTests.cs
│
├── Dockerfile
└── CatchTheRabbit.sln
```

### 7.2 Frontend-Projektstruktur

```
frontend/
├── src/
│   ├── assets/
│   │   ├── images/
│   │   │   ├── rabbit.svg
│   │   │   └── child.svg
│   │   └── sounds/
│   │       ├── move.mp3
│   │       ├── win.mp3
│   │       └── lose.mp3
│   │
│   ├── components/
│   │   ├── game/
│   │   │   ├── GameBoard.vue
│   │   │   ├── BoardCell.vue
│   │   │   ├── GamePiece.vue
│   │   │   ├── GameControls.vue
│   │   │   ├── Timer.vue
│   │   │   └── GameEndModal.vue
│   │   ├── leaderboard/
│   │   │   ├── LeaderboardList.vue
│   │   │   └── LeaderboardItem.vue
│   │   └── common/
│   │       ├── HeaderBar.vue
│   │       └── RoleSelect.vue
│   │
│   ├── views/
│   │   ├── HomeView.vue
│   │   ├── GameView.vue
│   │   └── LeaderboardView.vue
│   │
│   ├── stores/
│   │   ├── gameStore.ts
│   │   └── leaderboardStore.ts
│   │
│   ├── services/
│   │   ├── gameService.ts
│   │   ├── leaderboardService.ts
│   │   └── signalRService.ts
│   │
│   ├── types/
│   │   └── game.ts
│   │
│   ├── router/
│   │   └── index.ts
│   │
│   ├── App.vue
│   └── main.ts
│
├── public/
│   └── favicon.ico
│
├── Dockerfile
├── nginx.conf
├── package.json
├── vite.config.ts
└── tsconfig.json
```

---

## 8. Anhang

### 8.1 Konstanten

```csharp
// Constants.cs

public static class GameConstants
{
    public const int BoardSize = 10;
    public const int ChildrenCount = 4;

    public const int RabbitStartMinY = 7;
    public const int RabbitStartMaxY = 9;

    public const int ChildrenStartMinY = 0;
    public const int ChildrenStartMaxY = 2;

    public const int RabbitWinY = 0;
}

public static class AIConstants
{
    public const int DefaultMaxDepth = 8;
    public const int DefaultMaxTimeMs = 900;

    public const int WinScore = 10000;
    public const int ProgressWeight = 100;
    public const int MobilityWeight = 50;
    public const int EncirclementWeight = 30;
    public const int FormationWeight = 20;
}

public static class LeaderboardConstants
{
    public const int MaxEntries = 20;
    public const int MaxNicknameLength = 50;
}
```

### 8.2 Änderungshistorie

| Version | Datum | Autor | Änderung |
|---------|-------|-------|----------|
| 1.0 | 21.03.2025 | - | Initiale Version |
