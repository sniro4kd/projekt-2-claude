# Systemtest-Spezifikation - CatchTheRabbit

| Dokument      | Systemtest-Spezifikation          |
|---------------|-----------------------------------|
| Projekt       | CatchTheRabbit                    |
| Version       | 1.0                               |
| Datum         | 2025-03-21                        |
| Autor         | Entwicklungsteam                  |

---

## 1. Übersicht

### 1.1 Zweck
Diese Spezifikation definiert die Integrationstests für die API-Endpunkte und SignalR-Hub des CatchTheRabbit-Backends.

### 1.2 Testansatz
- Integration Tests mit WebApplicationFactory
- In-Memory SQLite Datenbank
- Vollständiger Request/Response-Zyklus

### 1.3 Testwerkzeuge
- xUnit als Test-Framework
- Microsoft.AspNetCore.Mvc.Testing
- SignalR Client für Hub-Tests
- In-Memory SQLite

---

## 2. API-Tests: Game Controller

### ST-API-001: Spiel starten als Hase

| Attribut | Wert |
|----------|------|
| **ID** | ST-API-001 |
| **Endpunkt** | POST /api/game/start |
| **Priorität** | Hoch |

**Request:**
```json
{
  "playerRole": "rabbit"
}
```

**Erwartete Response:**
- Status: 200 OK
- Body enthält:
  - `gameId`: Non-empty GUID
  - `gameState.playerRole`: "rabbit"
  - `gameState.isPlayerTurn`: true
  - `gameState.rabbitPosition`: { row: 0, col: 4 }
  - `gameState.childrenPositions`: 4 Positionen
  - `gameState.status`: "playing"

---

### ST-API-002: Spiel starten als Kinder

| Attribut | Wert |
|----------|------|
| **ID** | ST-API-002 |
| **Endpunkt** | POST /api/game/start |
| **Priorität** | Hoch |

**Request:**
```json
{
  "playerRole": "children"
}
```

**Erwartete Response:**
- Status: 200 OK
- Body enthält:
  - `gameState.playerRole`: "children"
  - `gameState.isPlayerTurn`: false (KI beginnt als Hase)

---

### ST-API-003: Spiel starten mit ungültiger Rolle

| Attribut | Wert |
|----------|------|
| **ID** | ST-API-003 |
| **Endpunkt** | POST /api/game/start |
| **Priorität** | Mittel |

**Request:**
```json
{
  "playerRole": "invalid"
}
```

**Erwartete Response:**
- Status: 400 Bad Request
- Fehlermeldung im Body

---

### ST-API-004: Gültiger Spielzug

| Attribut | Wert |
|----------|------|
| **ID** | ST-API-004 |
| **Endpunkt** | POST /api/game/{id}/move |
| **Priorität** | Hoch |

**Vorbedingung:** Spiel gestartet als Hase

**Request:**
```json
{
  "from": { "row": 0, "col": 4 },
  "to": { "row": 1, "col": 5 }
}
```

**Erwartete Response:**
- Status: 200 OK
- `gameState.rabbitPosition`: { row: 1, col: 5 }
- `gameState.moveCount`: erhöht
- KI-Zug wurde ausgeführt (Kind bewegt)

---

### ST-API-005: Ungültiger Spielzug (außerhalb)

| Attribut | Wert |
|----------|------|
| **ID** | ST-API-005 |
| **Endpunkt** | POST /api/game/{id}/move |
| **Priorität** | Hoch |

**Request:**
```json
{
  "from": { "row": 0, "col": 4 },
  "to": { "row": -1, "col": 5 }
}
```

**Erwartete Response:**
- Status: 400 Bad Request
- Fehlermeldung: Position außerhalb des Spielfelds

---

### ST-API-006: Ungültiger Spielzug (nicht am Zug)

| Attribut | Wert |
|----------|------|
| **ID** | ST-API-006 |
| **Endpunkt** | POST /api/game/{id}/move |
| **Priorität** | Hoch |

**Vorbedingung:** Spiel als Kinder gestartet (KI am Zug)

**Request:** Versuch einen Zug zu machen

**Erwartete Response:**
- Status: 400 Bad Request
- Fehlermeldung: Nicht am Zug

---

### ST-API-007: KI-Antwort nach Spielerzug

| Attribut | Wert |
|----------|------|
| **ID** | ST-API-007 |
| **Endpunkt** | POST /api/game/{id}/move |
| **Priorität** | Hoch |

**Aktion:** Gültigen Spielerzug ausführen

**Erwartete Response:**
- `aiMove`: Enthält KI-Zugdaten
- `aiThinkingTimeMs`: > 0
- `gameState.isPlayerTurn`: true (wieder Spieler am Zug)
- Spielfeld zeigt KI-Zug

---

### ST-API-008: Spielstatus nach Sieg

| Attribut | Wert |
|----------|------|
| **ID** | ST-API-008 |
| **Endpunkt** | POST /api/game/{id}/move |
| **Priorität** | Hoch |

**Vorbedingung:** Hase auf (8,4), Siegzug möglich

**Request:** Zug nach (9,5)

**Erwartete Response:**
- `gameState.status`: "rabbitWins"
- `isGameOver`: true

---

### ST-API-009: Spielzustand abrufen (gültige ID)

| Attribut | Wert |
|----------|------|
| **ID** | ST-API-009 |
| **Endpunkt** | GET /api/game/{id}/state |
| **Priorität** | Mittel |

**Vorbedingung:** Spiel existiert

**Erwartete Response:**
- Status: 200 OK
- Vollständiger GameState im Body

---

### ST-API-010: Spielzustand abrufen (ungültige ID)

| Attribut | Wert |
|----------|------|
| **ID** | ST-API-010 |
| **Endpunkt** | GET /api/game/{id}/state |
| **Priorität** | Mittel |

**Request:** Ungültige/nicht existierende Game-ID

**Erwartete Response:**
- Status: 404 Not Found

---

## 3. API-Tests: Leaderboard Controller

### ST-API-011: Leaderboard abrufen (leer)

| Attribut | Wert |
|----------|------|
| **ID** | ST-API-011 |
| **Endpunkt** | GET /api/leaderboard |
| **Priorität** | Mittel |

**Vorbedingung:** Keine Einträge in Datenbank

**Erwartete Response:**
- Status: 200 OK
- `rabbitLeaderboard`: []
- `childrenLeaderboard`: []

---

### ST-API-012: Leaderboard abrufen (mit Einträgen)

| Attribut | Wert |
|----------|------|
| **ID** | ST-API-012 |
| **Endpunkt** | GET /api/leaderboard |
| **Priorität** | Mittel |

**Vorbedingung:** Mehrere Einträge in Datenbank

**Erwartete Response:**
- Status: 200 OK
- Beide Listen korrekt gefüllt
- Sortierung nach KI-Denkzeit (aufsteigend)

---

### ST-API-013: Score eintragen (gültig)

| Attribut | Wert |
|----------|------|
| **ID** | ST-API-013 |
| **Endpunkt** | POST /api/leaderboard/submit |
| **Priorität** | Hoch |

**Vorbedingung:** Spiel mit Spielersieg beendet

**Request:**
```json
{
  "gameId": "<valid-game-id>",
  "nickname": "TestSpieler"
}
```

**Erwartete Response:**
- Status: 200 OK
- `success`: true
- `rank`: Numerischer Rang

---

### ST-API-014: Score eintragen (leerer Nickname)

| Attribut | Wert |
|----------|------|
| **ID** | ST-API-014 |
| **Endpunkt** | POST /api/leaderboard/submit |
| **Priorität** | Mittel |

**Request:**
```json
{
  "gameId": "<valid-game-id>",
  "nickname": ""
}
```

**Erwartete Response:**
- Status: 400 Bad Request
- Fehlermeldung: Nickname erforderlich

---

### ST-API-015: Score eintragen (ungültige Game-ID)

| Attribut | Wert |
|----------|------|
| **ID** | ST-API-015 |
| **Endpunkt** | POST /api/leaderboard/submit |
| **Priorität** | Mittel |

**Request:**
```json
{
  "gameId": "non-existent-id",
  "nickname": "TestSpieler"
}
```

**Erwartete Response:**
- Status: 404 Not Found oder 400 Bad Request
- Fehlermeldung: Spiel nicht gefunden

---

## 4. SignalR Hub Tests

### ST-HUB-001: Verbindung zum GameHub

| Attribut | Wert |
|----------|------|
| **ID** | ST-HUB-001 |
| **Hub** | /gamehub |
| **Priorität** | Hoch |

**Aktion:** SignalR-Verbindung aufbauen

**Erwartetes Ergebnis:**
- Verbindung erfolgreich
- ConnectionId wird zugewiesen

---

### ST-HUB-002: JoinGame Event

| Attribut | Wert |
|----------|------|
| **ID** | ST-HUB-002 |
| **Hub** | /gamehub |
| **Methode** | JoinGame(gameId) |
| **Priorität** | Hoch |

**Aktion:** JoinGame mit gültiger Game-ID aufrufen

**Erwartetes Ergebnis:**
- Client ist Gruppe beigetreten
- Kann GameState-Updates empfangen

---

### ST-HUB-003: MoveMade Event

| Attribut | Wert |
|----------|------|
| **ID** | ST-HUB-003 |
| **Hub** | /gamehub |
| **Event** | MoveMade |
| **Priorität** | Hoch |

**Aktion:**
1. Verbinden und JoinGame
2. Spielzug über API ausführen

**Erwartetes Ergebnis:**
- `MoveMade` Event wird empfangen
- Event enthält aktualisierten GameState

---

### ST-HUB-004: GameOver Event

| Attribut | Wert |
|----------|------|
| **ID** | ST-HUB-004 |
| **Hub** | /gamehub |
| **Event** | GameOver |
| **Priorität** | Hoch |

**Aktion:** Spiel zu Ende spielen

**Erwartetes Ergebnis:**
- `GameOver` Event wird empfangen
- Event enthält Sieger-Information

---

### ST-HUB-005: Verbindungsabbruch

| Attribut | Wert |
|----------|------|
| **ID** | ST-HUB-005 |
| **Hub** | /gamehub |
| **Priorität** | Mittel |

**Aktion:** Verbindung trennen und neu verbinden

**Erwartetes Ergebnis:**
- Reconnect möglich
- Spielzustand bleibt erhalten

---

## 5. Performance-Tests

### ST-PERF-001: API Response Time

| Attribut | Wert |
|----------|------|
| **ID** | ST-PERF-001 |
| **Ziel** | Response-Zeit unter 100ms |
| **Priorität** | Mittel |

**Endpunkte zu testen:**
- GET /api/leaderboard: < 50ms
- POST /api/game/start: < 100ms
- POST /api/game/{id}/move: < 1100ms (inkl. KI)

---

### ST-PERF-002: Concurrent Connections

| Attribut | Wert |
|----------|------|
| **ID** | ST-PERF-002 |
| **Ziel** | 10 gleichzeitige Spiele |
| **Priorität** | Niedrig |

**Aktion:** 10 parallele Spiele starten und spielen

**Erwartetes Ergebnis:**
- Alle Spiele funktionieren korrekt
- Keine Datenkorruption zwischen Spielen

---

## 6. Fehlerbehandlung

### ST-ERR-001: Malformed JSON

| Attribut | Wert |
|----------|------|
| **ID** | ST-ERR-001 |
| **Priorität** | Mittel |

**Request:** Ungültiges JSON an beliebigen Endpunkt

**Erwartete Response:**
- Status: 400 Bad Request
- Verständliche Fehlermeldung

---

### ST-ERR-002: Health Check

| Attribut | Wert |
|----------|------|
| **ID** | ST-ERR-002 |
| **Endpunkt** | GET /health |
| **Priorität** | Mittel |

**Erwartete Response:**
- Status: 200 OK
- `status`: "healthy"

---

## 7. Testdaten-Setup

### 7.1 Datenbank-Fixtures

```csharp
// Leaderboard mit 25 Einträgen
public static List<LeaderboardEntry> CreateTestLeaderboard()
{
    return Enumerable.Range(1, 25)
        .Select(i => new LeaderboardEntry
        {
            Nickname = $"Player{i}",
            PlayerRole = i % 2 == 0 ? PlayerRole.Rabbit : PlayerRole.Children,
            AiThinkingTimeMs = 100 + (i * 50),
            CreatedAt = DateTime.UtcNow.AddDays(-i)
        })
        .ToList();
}
```

### 7.2 Spiel-Fixtures

```csharp
// Spiel kurz vor Hase-Sieg
public static GameState CreateRabbitNearWinState()
{
    return new GameState
    {
        RabbitPosition = new Position(8, 4),
        ChildrenPositions = new List<Position>
        {
            new Position(7, 1),
            new Position(7, 7),
            new Position(6, 3),
            new Position(6, 5)
        },
        // ... weitere Properties
    };
}
```

---

## 8. Testmatrix

| Test-ID | Kategorie | Abhängigkeiten | Automatisiert |
|---------|-----------|----------------|---------------|
| ST-API-001 | Game | - | Ja |
| ST-API-002 | Game | - | Ja |
| ST-API-003 | Game | - | Ja |
| ST-API-004 | Game | ST-API-001 | Ja |
| ST-API-005 | Game | ST-API-001 | Ja |
| ST-API-006 | Game | ST-API-002 | Ja |
| ST-API-007 | Game | ST-API-001 | Ja |
| ST-API-008 | Game | ST-API-001 | Ja |
| ST-API-009 | Game | ST-API-001 | Ja |
| ST-API-010 | Game | - | Ja |
| ST-API-011 | Leaderboard | - | Ja |
| ST-API-012 | Leaderboard | Fixtures | Ja |
| ST-API-013 | Leaderboard | ST-API-008 | Ja |
| ST-API-014 | Leaderboard | ST-API-008 | Ja |
| ST-API-015 | Leaderboard | - | Ja |
| ST-HUB-001 | SignalR | - | Ja |
| ST-HUB-002 | SignalR | ST-API-001 | Ja |
| ST-HUB-003 | SignalR | ST-HUB-002 | Ja |
| ST-HUB-004 | SignalR | ST-HUB-002 | Ja |
| ST-HUB-005 | SignalR | ST-HUB-001 | Ja |

---

## 9. Änderungshistorie

| Version | Datum | Autor | Änderung |
|---------|-------|-------|----------|
| 1.0 | 2025-03-21 | Entwicklungsteam | Initiale Version |
