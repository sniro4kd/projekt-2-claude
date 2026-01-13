# Pflichtenheft: CatchTheRabbit

| Attribut | Wert |
|----------|------|
| Projekt | CatchTheRabbit |
| Auftraggeber | ERP_Champion GmbH |
| Version | 1.0 |
| Datum | 21.03.2025 |
| Status | Entwurf |
| Basis | Lastenheft v1.0 |

---

## 1. Einleitung

### 1.1 Zweck des Dokuments

Dieses Pflichtenheft beschreibt die technische Umsetzung der im Lastenheft definierten Anforderungen. Es dient als verbindliche Grundlage für die Implementierung und bildet die Basis für den Systemtest.

### 1.2 Geltungsbereich

Das Dokument spezifiziert die technische Realisierung aller funktionalen und nichtfunktionalen Anforderungen aus dem Lastenheft (FA-100 bis FA-801, NFA-100 bis NFA-502).

### 1.3 Referenzen

| Dokument | Version | Pfad |
|----------|---------|------|
| Lastenheft | 1.0 | `01_Anforderungen/Lastenheft.md` |
| ADR-001 Technologie-Stack | - | `00_Projektmanagement/entscheidungen/ADR-001-technologie-stack.md` |
| ADR-002 Datenbank | - | `00_Projektmanagement/entscheidungen/ADR-002-datenbank.md` |
| ADR-004 Vorgehensmodell | - | `00_Projektmanagement/entscheidungen/ADR-004-vorgehensmodell.md` |

### 1.4 Glossar

| Begriff | Definition |
|---------|------------|
| Frontend | Vue.js Single-Page-Application im Browser |
| Backend | ASP.NET Core Web API Server |
| SignalR | Bibliothek für bidirektionale Echtzeit-Kommunikation |
| Minimax | Spielbaum-Suchalgorithmus für Zwei-Spieler-Spiele |
| Alpha-Beta-Pruning | Optimierung des Minimax-Algorithmus durch Abschneiden irrelevanter Äste |

---

## 2. Systemübersicht

### 2.1 Systemarchitektur

```
┌─────────────────────────────────────────────────────────────────┐
│                          Browser                                 │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │                    Vue.js Frontend                         │  │
│  │  ┌─────────────┐ ┌─────────────┐ ┌─────────────────────┐  │  │
│  │  │ GameBoard   │ │ GameControls│ │   Leaderboard       │  │  │
│  │  │ Component   │ │ Component   │ │   Component         │  │  │
│  │  └─────────────┘ └─────────────┘ └─────────────────────┘  │  │
│  │                         │                                  │  │
│  │  ┌─────────────────────────────────────────────────────┐  │  │
│  │  │              Game State Store (Pinia)               │  │  │
│  │  └─────────────────────────────────────────────────────┘  │  │
│  └───────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
                    │ HTTP/REST          │ WebSocket/SignalR
                    ▼                    ▼
┌─────────────────────────────────────────────────────────────────┐
│                      ASP.NET Core Backend                        │
│  ┌─────────────────────────────────────────────────────────┐    │
│  │                     Controller Layer                     │    │
│  │  ┌───────────────┐  ┌───────────────┐  ┌─────────────┐  │    │
│  │  │ GameController│  │LeaderboardCtrl│  │  GameHub    │  │    │
│  │  │   (REST)      │  │   (REST)      │  │  (SignalR)  │  │    │
│  │  └───────────────┘  └───────────────┘  └─────────────┘  │    │
│  └─────────────────────────────────────────────────────────┘    │
│                              │                                   │
│  ┌─────────────────────────────────────────────────────────┐    │
│  │                   Business Logic Layer                   │    │
│  │  ┌───────────────┐  ┌───────────────┐  ┌─────────────┐  │    │
│  │  │  GameService  │  │  AIService    │  │Leaderboard  │  │    │
│  │  │               │  │  (Minimax)    │  │  Service    │  │    │
│  │  └───────────────┘  └───────────────┘  └─────────────┘  │    │
│  └─────────────────────────────────────────────────────────┘    │
│                              │                                   │
│  ┌─────────────────────────────────────────────────────────┐    │
│  │                    Data Access Layer                     │    │
│  │  ┌─────────────────────────────────────────────────────┐│    │
│  │  │           ILeaderboardRepository                    ││    │
│  │  │                     ▲                               ││    │
│  │  │                     │                               ││    │
│  │  │    ┌────────────────┴────────────────┐              ││    │
│  │  │    │  SqliteLeaderboardRepository    │              ││    │
│  │  └────┴─────────────────────────────────┴──────────────┘│    │
│  └─────────────────────────────────────────────────────────┘    │
│                              │                                   │
│                              ▼                                   │
│                    ┌─────────────────┐                           │
│                    │  SQLite (Datei) │                           │
│                    └─────────────────┘                           │
└─────────────────────────────────────────────────────────────────┘
```

### 2.2 Deployment-Architektur

```
┌─────────────────────────────────────────────────────────────┐
│                      Docker Compose                          │
│                                                              │
│  ┌────────────────────┐      ┌────────────────────────────┐ │
│  │ frontend (nginx)   │      │ backend (ASP.NET Core)     │ │
│  │                    │      │                            │ │
│  │ Port: 80           │ ───► │ Port: 5000                 │ │
│  │                    │      │                            │ │
│  │ /app/* → Vue SPA   │      │ /api/* → REST Endpoints    │ │
│  │                    │      │ /gamehub → SignalR         │ │
│  │                    │      │                            │ │
│  │                    │      │ Volume: /data/game.db      │ │
│  └────────────────────┘      └────────────────────────────┘ │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

---

## 3. Spiellogik-Spezifikation

### 3.1 Spielfeld-Modell

Das Spielfeld wird als 10x10 Matrix modelliert. Nur die "schwarzen" Felder sind spielbar.

**Koordinatensystem:**
- X-Achse: 0-9 (links nach rechts)
- Y-Achse: 0-9 (oben nach unten, 0 = oberste Reihe)

**Schwarze Felder (spielbar):**
Ein Feld (x, y) ist schwarz, wenn `(x + y) % 2 == 1`

```
    0   1   2   3   4   5   6   7   8   9
  ┌───┬───┬───┬───┬───┬───┬───┬───┬───┬───┐
0 │   │ ■ │   │ ■ │   │ ■ │   │ ■ │   │ ■ │  ← Zielreihe Hase (5 schwarze Felder)
  ├───┼───┼───┼───┼───┼───┼───┼───┼───┼───┤
1 │ ■ │   │ ■ │   │ ■ │   │ ■ │   │ ■ │   │
  ├───┼───┼───┼───┼───┼───┼───┼───┼───┼───┤
2 │   │ ■ │   │ ■ │   │ ■ │   │ ■ │   │ ■ │  ← Kinder Startbereich (Reihe 0-2)
  ├───┼───┼───┼───┼───┼───┼───┼───┼───┼───┤
3 │ ■ │   │ ■ │   │ ■ │   │ ■ │   │ ■ │   │
  ├───┼───┼───┼───┼───┼───┼───┼───┼───┼───┤
4 │   │ ■ │   │ ■ │   │ ■ │   │ ■ │   │ ■ │
  ├───┼───┼───┼───┼───┼───┼───┼───┼───┼───┤
5 │ ■ │   │ ■ │   │ ■ │   │ ■ │   │ ■ │   │
  ├───┼───┼───┼───┼───┼───┼───┼───┼───┼───┤
6 │   │ ■ │   │ ■ │   │ ■ │   │ ■ │   │ ■ │
  ├───┼───┼───┼───┼───┼───┼───┼───┼───┼───┤
7 │ ■ │   │ ■ │   │ ■ │   │ ■ │   │ ■ │   │  ← Hase Startbereich (Reihe 7-9)
  ├───┼───┼───┼───┼───┼───┼───┼───┼───┼───┤
8 │   │ ■ │   │ ■ │   │ ■ │   │ ■ │   │ ■ │
  ├───┼───┼───┼───┼───┼───┼───┼───┼───┼───┤
9 │ ■ │   │ ■ │   │ ■ │   │ ■ │   │ ■ │   │
  └───┴───┴───┴───┴───┴───┴───┴───┴───┴───┘
```

### 3.2 Spielfiguren

| Figur | Anzahl | Bewegungsrichtungen | Startbereich |
|-------|--------|---------------------|--------------|
| Hase | 1 | Diagonal alle 4 Richtungen (↖ ↗ ↙ ↘) | Reihe 7, 8 oder 9 |
| Kind | 4 | Diagonal nur nach unten (↙ ↘) | Reihe 0, 1 oder 2 |

### 3.3 Zugvalidierung

Ein Zug ist gültig, wenn:
1. Die Figur dem aktiven Spieler gehört
2. Das Zielfeld ein schwarzes Feld ist
3. Das Zielfeld nicht besetzt ist
4. Das Zielfeld innerhalb des Spielfelds liegt (0-9)
5. Die Bewegungsrichtung für die Figur erlaubt ist

**Pseudocode Zugvalidierung:**
```
function isValidMove(piece, fromX, fromY, toX, toY, gameState):
    // Prüfe Feldgrenzen
    if toX < 0 or toX > 9 or toY < 0 or toY > 9:
        return false

    // Prüfe schwarzes Feld
    if (toX + toY) % 2 != 1:
        return false

    // Prüfe Zielfeld nicht besetzt
    if gameState.isOccupied(toX, toY):
        return false

    // Prüfe diagonale Bewegung (1 Feld)
    deltaX = abs(toX - fromX)
    deltaY = abs(toY - fromY)
    if deltaX != 1 or deltaY != 1:
        return false

    // Prüfe Bewegungsrichtung
    if piece.type == "child":
        if toY <= fromY:  // Kind darf nur nach unten
            return false

    return true
```

### 3.4 Siegbedingungen

**Hase gewinnt:**
```
function checkRabbitWins(rabbitY):
    return rabbitY == 0  // Hase erreicht oberste Reihe
```

**Kinder gewinnen:**
```
function checkChildrenWin(gameState):
    rabbitMoves = getValidMoves(gameState.rabbit, gameState)
    return rabbitMoves.length == 0  // Hase kann nicht mehr ziehen
```

### 3.5 Spielablauf-Zustandsdiagramm

```
                    ┌─────────────────┐
                    │   START_SCREEN  │
                    └────────┬────────┘
                             │ Spieler wählt Rolle
                             ▼
                    ┌─────────────────┐
                    │  INITIALIZING   │
                    │ (Positionen     │
                    │  generieren)    │
                    └────────┬────────┘
                             │
                             ▼
                    ┌─────────────────┐
              ┌────►│  RABBIT_TURN    │◄────┐
              │     └────────┬────────┘     │
              │              │ Hase zieht   │
              │              ▼              │
              │     ┌─────────────────┐     │
              │     │ CHECK_RABBIT_WIN│     │
              │     └────────┬────────┘     │
              │              │              │
              │    ┌─────────┴─────────┐    │
              │    ▼                   ▼    │
              │  [Ja]               [Nein]  │
              │    │                   │    │
              │    ▼                   ▼    │
              │ ┌──────┐    ┌─────────────┐ │
              │ │RABBIT│    │CHILDREN_TURN│ │
              │ │ WINS │    └──────┬──────┘ │
              │ └──────┘           │        │
              │                    │ Kind zieht
              │                    ▼        │
              │         ┌─────────────────┐ │
              │         │CHECK_CHILDREN   │ │
              │         │     WIN         │ │
              │         └────────┬────────┘ │
              │                  │          │
              │        ┌─────────┴─────────┐│
              │        ▼                   ▼│
              │      [Ja]               [Nein]
              │        │                   │
              │        ▼                   │
              │   ┌────────┐               │
              │   │CHILDREN│               │
              │   │  WIN   │               │
              │   └────────┘               │
              │                            │
              └────────────────────────────┘
```

---

## 4. KI-Spezifikation (Minimax mit Alpha-Beta)

### 4.1 Algorithmus-Überblick

Der Minimax-Algorithmus mit Alpha-Beta-Pruning durchsucht den Spielbaum und bewertet Spielzustände. Alpha-Beta-Pruning reduziert die Anzahl der zu evaluierenden Knoten.

### 4.2 Bewertungsfunktion (Heuristik)

```
function evaluate(gameState, aiRole):
    // Sofortige Endszustände
    if rabbitWins(gameState):
        return aiRole == "rabbit" ? +10000 : -10000
    if childrenWin(gameState):
        return aiRole == "children" ? +10000 : -10000

    score = 0
    rabbit = gameState.rabbit
    children = gameState.children

    // Faktor 1: Hasen-Position (je weiter oben, desto besser für Hase)
    rabbitProgress = (9 - rabbit.y) * 100

    // Faktor 2: Mobilität des Hasen
    rabbitMoves = countValidMoves(rabbit, gameState)
    mobilityScore = rabbitMoves * 50

    // Faktor 3: Einkreisung (Kinder um den Hasen)
    encirclementScore = calculateEncirclement(rabbit, children) * 30

    // Faktor 4: Kinder-Formation (Linie halten)
    formationScore = calculateFormation(children) * 20

    if aiRole == "rabbit":
        return rabbitProgress + mobilityScore - encirclementScore
    else:
        return encirclementScore + formationScore - rabbitProgress - mobilityScore
```

### 4.3 Minimax mit Alpha-Beta

```
function minimax(gameState, depth, alpha, beta, maximizing, aiRole):
    if depth == 0 or isGameOver(gameState):
        return evaluate(gameState, aiRole)

    if maximizing:
        maxEval = -INFINITY
        for move in getAllValidMoves(gameState, currentPlayer):
            newState = applyMove(gameState, move)
            eval = minimax(newState, depth - 1, alpha, beta, false, aiRole)
            maxEval = max(maxEval, eval)
            alpha = max(alpha, eval)
            if beta <= alpha:
                break  // Beta-Cutoff
        return maxEval
    else:
        minEval = +INFINITY
        for move in getAllValidMoves(gameState, opponent):
            newState = applyMove(gameState, move)
            eval = minimax(newState, depth - 1, alpha, beta, true, aiRole)
            minEval = min(minEval, eval)
            beta = min(beta, eval)
            if beta <= alpha:
                break  // Alpha-Cutoff
        return minEval
```

### 4.4 Suchtiefe und Zeitlimit

| Parameter | Wert | Begründung |
|-----------|------|------------|
| Maximale Suchtiefe | 8-10 Halbzüge | Balance zwischen Spielstärke und Performance |
| Zeitlimit | 900ms | Puffer für 1-Sekunden-Anforderung |
| Iterative Deepening | Ja | Garantiert Antwort innerhalb Zeitlimit |

**Iterative Deepening:**
```
function findBestMove(gameState, aiRole, timeLimit):
    startTime = now()
    bestMove = null

    for depth in 1 to MAX_DEPTH:
        if now() - startTime > timeLimit * 0.9:
            break

        currentBest = minimaxRoot(gameState, depth, aiRole)
        bestMove = currentBest

    return bestMove
```

---

## 5. Use Cases

### 5.1 UC-01: Neues Spiel starten

| Attribut | Beschreibung |
|----------|--------------|
| Akteur | Spieler |
| Vorbedingung | Startbildschirm wird angezeigt |
| Nachbedingung | Spiel ist initialisiert, Spielfeld wird angezeigt |

**Ablauf:**
1. Spieler öffnet die Anwendung
2. System zeigt Startbildschirm mit Rollenauswahl
3. Spieler wählt Rolle (Hase oder Kinder)
4. System generiert zufällige Startpositionen
5. System zeigt Spielfeld mit Figuren
6. System startet Bedenkzeit-Timer
7. (Wenn Spieler = Kinder) System führt ersten KI-Zug aus

### 5.2 UC-02: Zug ausführen (Spieler)

| Attribut | Beschreibung |
|----------|--------------|
| Akteur | Spieler |
| Vorbedingung | Spieler ist am Zug |
| Nachbedingung | Zug wurde ausgeführt, Gegner ist am Zug |

**Ablauf:**
1. System hebt aktive Spielfigur(en) hervor
2. Spieler klickt auf eine Figur
3. System zeigt gültige Zugmöglichkeiten
4. Spieler klickt auf Zielfeld
5. System validiert Zug
6. System führt Zug aus (Animation)
7. System prüft Siegbedingung
8. System wechselt zum KI-Zug

**Alternativ 5a: Ungültiger Zug**
- System zeigt Fehlermeldung
- Zurück zu Schritt 2

### 5.3 UC-03: KI-Zug

| Attribut | Beschreibung |
|----------|--------------|
| Akteur | System (KI) |
| Vorbedingung | KI ist am Zug |
| Nachbedingung | KI-Zug wurde ausgeführt |

**Ablauf:**
1. System berechnet besten Zug (Minimax)
2. System wartet kurze Verzögerung (mind. 500ms für UX)
3. System führt Zug aus (Animation)
4. System prüft Siegbedingung
5. System wechselt zum Spieler-Zug

### 5.4 UC-04: Spielende und Bestenliste

| Attribut | Beschreibung |
|----------|--------------|
| Akteur | Spieler |
| Vorbedingung | Siegbedingung erfüllt |
| Nachbedingung | Ergebnis angezeigt, ggf. Bestenliste aktualisiert |

**Ablauf:**
1. System erkennt Siegbedingung
2. System stoppt Bedenkzeit-Timer
3. System zeigt Ergebnis-Dialog
4. (Bei Spieler-Sieg) System fordert Nickname-Eingabe
5. Spieler gibt Nickname ein
6. System speichert Eintrag in Bestenliste
7. System zeigt aktualisierte Bestenliste
8. Spieler kann neues Spiel starten

### 5.5 UC-05: Bestenliste anzeigen

| Attribut | Beschreibung |
|----------|--------------|
| Akteur | Besucher |
| Vorbedingung | Anwendung ist geöffnet |
| Nachbedingung | Bestenlisten werden angezeigt |

**Ablauf:**
1. Besucher navigiert zu Bestenliste
2. System lädt Top 20 für beide Listen
3. System zeigt Bestenlisten (Rang, Name, Zeit)

---

## 6. Datenmodell

### 6.1 Entitäten

```
┌─────────────────────────────────────────┐
│            LeaderboardEntry             │
├─────────────────────────────────────────┤
│ + Id: int (PK)                          │
│ + Nickname: string (max 50)             │
│ + Role: string ("rabbit" | "children")  │
│ + ThinkingTimeMs: long                  │
│ + CreatedAt: datetime                   │
├─────────────────────────────────────────┤
│ Index: (Role, ThinkingTimeMs ASC)       │
└─────────────────────────────────────────┘
```

### 6.2 SQLite Schema

```sql
CREATE TABLE LeaderboardEntries (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Nickname TEXT NOT NULL CHECK(length(Nickname) <= 50),
    Role TEXT NOT NULL CHECK(Role IN ('rabbit', 'children')),
    ThinkingTimeMs INTEGER NOT NULL,
    CreatedAt TEXT NOT NULL DEFAULT (datetime('now'))
);

CREATE INDEX IX_Leaderboard_Role_Time
ON LeaderboardEntries(Role, ThinkingTimeMs ASC);
```

### 6.3 Spielzustand (In-Memory, nicht persistent)

```
GameState {
    gameId: string (GUID)
    playerRole: "rabbit" | "children"
    currentTurn: "rabbit" | "children"
    rabbit: Position { x: int, y: int }
    children: Position[] (4 Elemente)
    playerThinkingTimeMs: long
    gameStatus: "playing" | "rabbit_wins" | "children_win"
    moveHistory: Move[]
}

Position {
    x: int (0-9)
    y: int (0-9)
}

Move {
    pieceType: "rabbit" | "child"
    pieceIndex: int? (0-3 für Kinder)
    from: Position
    to: Position
    timestamp: datetime
}
```

---

## 7. API-Spezifikation

### 7.1 REST Endpoints

#### POST /api/game/new
Startet ein neues Spiel.

**Request:**
```json
{
    "playerRole": "rabbit" | "children"
}
```

**Response:**
```json
{
    "gameId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "playerRole": "rabbit",
    "currentTurn": "rabbit",
    "rabbit": { "x": 5, "y": 8 },
    "children": [
        { "x": 1, "y": 0 },
        { "x": 3, "y": 1 },
        { "x": 5, "y": 0 },
        { "x": 7, "y": 2 }
    ],
    "gameStatus": "playing"
}
```

#### POST /api/game/{gameId}/move
Führt einen Spielerzug aus.

**Request:**
```json
{
    "pieceType": "rabbit" | "child",
    "pieceIndex": 0,  // nur bei "child"
    "to": { "x": 4, "y": 7 }
}
```

**Response:**
```json
{
    "success": true,
    "gameState": { /* aktueller GameState */ },
    "aiMove": {  // null wenn Spiel vorbei
        "pieceType": "child",
        "pieceIndex": 2,
        "from": { "x": 5, "y": 0 },
        "to": { "x": 4, "y": 1 }
    }
}
```

**Fehler:**
```json
{
    "success": false,
    "error": "Invalid move: target field is occupied"
}
```

#### GET /api/leaderboard
Lädt die Bestenlisten.

**Response:**
```json
{
    "rabbit": [
        { "rank": 1, "nickname": "SpeedyHase", "thinkingTimeMs": 15420 },
        { "rank": 2, "nickname": "Bunny123", "thinkingTimeMs": 18200 }
    ],
    "children": [
        { "rank": 1, "nickname": "KidsCatcher", "thinkingTimeMs": 22100 }
    ]
}
```

#### POST /api/leaderboard
Fügt einen Bestenlisten-Eintrag hinzu.

**Request:**
```json
{
    "gameId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "nickname": "Winner2025"
}
```

**Response:**
```json
{
    "success": true,
    "rank": 5,
    "entry": {
        "nickname": "Winner2025",
        "role": "rabbit",
        "thinkingTimeMs": 25000
    }
}
```

### 7.2 SignalR Hub

**Hub-Endpunkt:** `/gamehub`

**Client → Server Methoden:**
| Methode | Parameter | Beschreibung |
|---------|-----------|--------------|
| `JoinGame` | `gameId: string` | Verbindet Client mit Spielsession |
| `LeaveGame` | `gameId: string` | Trennt Client von Spielsession |

**Server → Client Events:**
| Event | Payload | Beschreibung |
|-------|---------|--------------|
| `GameStateUpdated` | `GameState` | Spielzustand hat sich geändert |
| `AiMoveStarted` | `{}` | KI beginnt Zugberechnung |
| `AiMoveCompleted` | `Move` | KI-Zug wurde berechnet |
| `GameEnded` | `{ winner: string, reason: string }` | Spiel ist beendet |

---

## 8. UI-Spezifikation

### 8.1 Screens

#### 8.1.1 Startbildschirm

```
┌─────────────────────────────────────────────────────────┐
│                                                         │
│                    🐰 CatchTheRabbit                    │
│                                                         │
│              ┌─────────────────────────┐                │
│              │  ○ Als Hase spielen     │                │
│              │  ○ Als Kinder spielen   │                │
│              └─────────────────────────┘                │
│                                                         │
│                  [ Spiel starten ]                      │
│                                                         │
│               ┌─────────────────────┐                   │
│               │   Bestenliste →     │                   │
│               └─────────────────────┘                   │
│                                                         │
│                         ─────                           │
│                    ERP_Champion                         │
└─────────────────────────────────────────────────────────┘
```

#### 8.1.2 Spielbildschirm

```
┌─────────────────────────────────────────────────────────┐
│  🐰 CatchTheRabbit          ⏱ 00:42    🔊 [Stumm]      │
├─────────────────────────────────────────────────────────┤
│                                                         │
│      ┌───┬───┬───┬───┬───┬───┬───┬───┬───┬───┐        │
│      │   │👧│   │   │   │👦│   │👧│   │👦│        │
│      ├───┼───┼───┼───┼───┼───┼───┼───┼───┼───┤        │
│      │   │   │   │   │   │   │   │   │   │   │        │
│      ├───┼───┼───┼───┼───┼───┼───┼───┼───┼───┤        │
│      │   │   │   │   │   │   │   │   │   │   │        │
│      ├───┼───┼───┼───┼───┼───┼───┼───┼───┼───┤        │
│      │   │   │   │ ● │   │   │   │   │   │   │  ● = möglicher Zug
│      ├───┼───┼───┼───┼───┼───┼───┼───┼───┼───┤        │
│      │   │   │   │   │🐰│   │   │   │   │   │  🐰 = ausgewählt
│      ├───┼───┼───┼───┼───┼───┼───┼───┼───┼───┤        │
│      │   │   │   │ ● │   │ ● │   │   │   │   │        │
│      └───┴───┴───┴───┴───┴───┴───┴───┴───┴───┘        │
│                                                         │
│      Status: Du bist am Zug (Hase)                     │
│                                                         │
├─────────────────────────────────────────────────────────┤
│                    ERP_Champion                         │
└─────────────────────────────────────────────────────────┘
```

#### 8.1.3 Spielende-Dialog

```
┌─────────────────────────────────────────┐
│                                         │
│          🎉 Gewonnen! 🎉               │
│                                         │
│     Du hast als Hase gewonnen!         │
│     Deine Zeit: 01:23                   │
│                                         │
│     Nickname für Bestenliste:          │
│     ┌─────────────────────────────┐    │
│     │ ___________________________ │    │
│     └─────────────────────────────┘    │
│                                         │
│     [ Eintragen ]  [ Überspringen ]    │
│                                         │
│            [ Neues Spiel ]             │
│                                         │
└─────────────────────────────────────────┘
```

#### 8.1.4 Bestenliste

```
┌─────────────────────────────────────────────────────────┐
│  🐰 CatchTheRabbit                     [ ← Zurück ]    │
├─────────────────────────────────────────────────────────┤
│                                                         │
│   ┌─────────────────────┐ ┌─────────────────────┐      │
│   │   🐰 Hase Top 20    │ │  👧 Kinder Top 20   │      │
│   ├─────────────────────┤ ├─────────────────────┤      │
│   │ 1. SpeedyHase 0:15  │ │ 1. KidsCatcher 0:22│      │
│   │ 2. Bunny123   0:18  │ │ 2. TeamKids    0:28│      │
│   │ 3. HopMaster  0:21  │ │ 3. Fänger42    0:31│      │
│   │ ...                 │ │ ...                 │      │
│   │ 20. LastOne   1:45  │ │ 20. Player99   2:10│      │
│   └─────────────────────┘ └─────────────────────┘      │
│                                                         │
├─────────────────────────────────────────────────────────┤
│                    ERP_Champion                         │
└─────────────────────────────────────────────────────────┘
```

### 8.2 Farbschema (kindgerecht)

| Element | Farbe | Hex |
|---------|-------|-----|
| Hintergrund | Hellblau | #E3F2FD |
| Spielfeld hell | Beige | #FFF8E1 |
| Spielfeld dunkel | Grün | #A5D6A7 |
| Hase | Orange | #FF9800 |
| Kinder | Blau | #42A5F5 |
| Akzent/Highlight | Gelb | #FFEB3B |
| ERP-Branding | Grau | #607D8B |

### 8.3 Soundeffekte

| Ereignis | Sound | Datei |
|----------|-------|-------|
| Zug ausgeführt | Kurzes "Pop" | `move.mp3` |
| Sieg | Fröhliche Fanfare | `win.mp3` |
| Niederlage | Sanftes "Aww" | `lose.mp3` |
| Spielstart | Kurzes Jingle | `start.mp3` |
| Ungültiger Zug | Kurzes "Bonk" | `invalid.mp3` |

---

## 9. Fehlerbehandlung

### 9.1 Frontend-Fehler

| Fehlertyp | Behandlung |
|-----------|------------|
| Netzwerkfehler | Toast-Nachricht "Verbindung unterbrochen", automatischer Reconnect |
| Ungültiger Zug | Visuelle Hervorhebung, Sound, Zug wird nicht ausgeführt |
| Session abgelaufen | Dialog mit Option "Neues Spiel starten" |

### 9.2 Backend-Fehler

| Fehlertyp | HTTP-Code | Behandlung |
|-----------|-----------|------------|
| Ungültiger Zug | 400 | Fehlermeldung im Response |
| Spiel nicht gefunden | 404 | Fehlermeldung, Redirect zu Start |
| Server-Fehler | 500 | Generische Fehlermeldung, Logging |
| KI-Timeout | 500 | Fallback auf zufälligen gültigen Zug |

### 9.3 Logging

- Backend: Strukturiertes Logging mit Serilog
- Log-Level: Information für API-Aufrufe, Warning für Fehler, Error für Exceptions
- Keine sensiblen Daten im Log (nur Nicknames, keine IPs)

---

## 10. Qualitätssicherung

### 10.1 Testabdeckung

| Komponente | Testart | Mindestabdeckung |
|------------|---------|------------------|
| Spiellogik (GameService) | Unit-Tests | 90% |
| KI (AIService) | Unit-Tests | 80% |
| Zugvalidierung | Unit-Tests | 100% |
| API-Endpoints | Integrationstests | 80% |
| Frontend-Komponenten | Component-Tests | 70% |

### 10.2 Testfälle (Auszug)

| ID | Testfall | Erwartetes Ergebnis |
|----|----------|---------------------|
| T-001 | Hase zieht auf oberste Reihe | Spiel endet, Hase gewinnt |
| T-002 | Hase hat keine gültigen Züge | Spiel endet, Kinder gewinnen |
| T-003 | Zug auf besetztes Feld | Zug wird abgelehnt |
| T-004 | Kind zieht nach oben | Zug wird abgelehnt |
| T-005 | KI-Berechnung > 1s | Fallback-Zug wird ausgeführt |
| T-006 | Nickname > 50 Zeichen | Eingabe wird abgeschnitten |
| T-007 | Bestenliste mit 100 Einträgen | Nur Top 20 werden angezeigt |

---

## 11. Anforderungszuordnung

### 11.1 Funktionale Anforderungen

| Lastenheft-ID | Pflichtenheft-Kapitel | Status |
|---------------|----------------------|--------|
| FA-100 bis FA-104 | 3.1, 3.2 | Spezifiziert |
| FA-200 bis FA-203 | 3.1, 5.1 | Spezifiziert |
| FA-300 bis FA-306 | 3.3 | Spezifiziert |
| FA-400 bis FA-403 | 3.4, 3.5 | Spezifiziert |
| FA-500 bis FA-503 | 4.1 bis 4.4 | Spezifiziert |
| FA-600 bis FA-606 | 6.1, 6.2, 7.1 | Spezifiziert |
| FA-700 bis FA-705 | 8.1, 8.2 | Spezifiziert |
| FA-800 bis FA-801 | 8.3 | Spezifiziert |

### 11.2 Nichtfunktionale Anforderungen

| Lastenheft-ID | Pflichtenheft-Kapitel | Status |
|---------------|----------------------|--------|
| NFA-100 | 4.4 | Spezifiziert |
| NFA-101 | 8.1 | Spezifiziert |
| NFA-102 | 2.2 | Spezifiziert |
| NFA-200, NFA-201 | 6.2, 6.3 | Spezifiziert |
| NFA-300 bis NFA-303 | 2.1, 10.1 | Spezifiziert |
| NFA-400 bis NFA-403 | 2.2 | Spezifiziert |
| NFA-500 bis NFA-502 | 8.1, 8.2 | Spezifiziert |

---

## 12. Anhang

### 12.1 Offene Punkte

| ID | Thema | Verantwortlich | Status |
|----|-------|----------------|--------|
| OP-PF-01 | Finale Grafiken für Spielfiguren | Design | Offen |
| OP-PF-02 | Sound-Assets erstellen/beschaffen | Design | Offen |
| OP-PF-03 | KI-Balancing testen und justieren | Entwicklung | Offen |
| OP-PF-04 | ERP_Champion Logo-Integration | Design | Offen |

### 12.2 Änderungshistorie

| Version | Datum | Autor | Änderung |
|---------|-------|-------|----------|
| 1.0 | 21.03.2025 | - | Initiale Version |
