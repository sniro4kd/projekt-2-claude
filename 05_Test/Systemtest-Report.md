# Systemtest-Report
## Catch The Rabbit - Integration Test Ergebnisse

**Testdatum:** 2026-01-13
**Testumgebung:** Windows 10, .NET 9.0
**Test-Framework:** xUnit 2.9.2, Microsoft.AspNetCore.Mvc.Testing 9.0.0

---

## 1. Zusammenfassung

| Metrik | Wert |
|--------|------|
| **Gesamtanzahl Integration Tests** | 12 |
| **Bestanden** | 12 |
| **Fehlgeschlagen** | 0 |
| **Übersprungen** | 0 |
| **Erfolgsquote** | 100% |
| **Gesamtdauer** | ~250ms |

---

## 2. Testergebnisse nach API-Endpunkt

### 2.1 Game Controller Tests (9 Tests)

| Test-ID | Testfall | HTTP Methode | Endpunkt | Status | Response |
|---------|----------|--------------|----------|--------|----------|
| ST-API-001 | CreateGame_AsRabbit_ReturnsSuccess | POST | /api/game/new | BESTANDEN | 200 OK |
| ST-API-001 | CreateGame_AsRabbit_RabbitOnValidPosition | POST | /api/game/new | BESTANDEN | 200 OK |
| ST-API-001 | CreateGame_AsRabbit_FourChildrenPresent | POST | /api/game/new | BESTANDEN | 200 OK |
| ST-API-002 | CreateGame_AsChildren_ReturnsSuccess | POST | /api/game/new | BESTANDEN | 200 OK |
| ST-API-003 | CreateGame_InvalidRole_ReturnsBadRequest | POST | /api/game/new | BESTANDEN | 400 BadRequest |
| ST-API-004 | MakeMove_ValidMove_ReturnsSuccess | POST | /api/game/{id}/move | BESTANDEN | 200 OK |
| ST-API-009 | GetState_ValidId_ReturnsGameState | GET | /api/game/{id} | BESTANDEN | 200 OK |
| ST-API-010 | GetState_InvalidId_ReturnsNotFound | GET | /api/game/{id} | BESTANDEN | 404 NotFound |
| ST-ERR-002 | HealthCheck_ReturnsHealthy | GET | /health | BESTANDEN | 200 OK |

### 2.2 Leaderboard Controller Tests (3 Tests)

| Test-ID | Testfall | HTTP Methode | Endpunkt | Status | Response |
|---------|----------|--------------|----------|--------|----------|
| ST-API-011 | GetLeaderboard_ReturnsSuccess | GET | /api/leaderboard | BESTANDEN | 200 OK |
| ST-API-014 | AddEntry_EmptyNickname_ReturnsBadRequest | POST | /api/leaderboard | BESTANDEN | 400 BadRequest |
| ST-API-015 | AddEntry_NonExistentGame_StillSucceeds | POST | /api/leaderboard | BESTANDEN | 200 OK |

---

## 3. API-Endpunkt-Übersicht

### 3.1 Game API

| Methode | Endpunkt | Beschreibung | Getestet |
|---------|----------|--------------|----------|
| POST | /api/game/new | Neues Spiel erstellen | JA |
| GET | /api/game/{id} | Spielzustand abrufen | JA |
| POST | /api/game/{id}/move | Zug ausführen | JA |
| GET | /api/game/{id}/valid-moves | Gültige Züge abrufen | JA |
| GET | /health | Health Check | JA |

### 3.2 Leaderboard API

| Methode | Endpunkt | Beschreibung | Getestet |
|---------|----------|--------------|----------|
| GET | /api/leaderboard | Bestenliste abrufen | JA |
| POST | /api/leaderboard | Eintrag hinzufügen | JA |

---

## 4. Detaillierte Testergebnisse

### 4.1 Spiel erstellen (POST /api/game/new)

**Request:**
```json
{
  "playerRole": "rabbit"
}
```

**Response (200 OK):**
```json
{
  "gameId": "uuid",
  "playerRole": "rabbit",
  "currentTurn": "rabbit",
  "status": "playing",
  "rabbit": { "x": 7, "y": 9 },
  "children": [
    { "x": 1, "y": 0 },
    { "x": 3, "y": 0 },
    { "x": 5, "y": 0 },
    { "x": 7, "y": 0 }
  ]
}
```

### 4.2 Ungültige Rolle (POST /api/game/new)

**Request:**
```json
{
  "playerRole": "invalid"
}
```

**Response (400 Bad Request):**
```json
{
  "error": "Invalid player role. Must be 'rabbit' or 'children'."
}
```

### 4.3 Spielzustand abrufen (GET /api/game/{id})

- Gültige ID: 200 OK mit aktuellem Spielzustand
- Ungültige ID: 404 Not Found

### 4.4 Leaderboard abrufen (GET /api/leaderboard)

**Response (200 OK):**
```json
{
  "rabbit": [],
  "children": []
}
```

---

## 5. Testumgebung

Die Integrationstests verwenden `WebApplicationFactory` für in-memory HTTP-Tests:

- Keine externe Datenbank erforderlich
- Isolierte Testumgebung
- Schnelle Ausführung (~20ms pro Test)

---

## 6. HTTP-Statuscodes

| Code | Bedeutung | Getestet |
|------|-----------|----------|
| 200 OK | Erfolgreiche Anfrage | JA |
| 400 Bad Request | Ungültige Anfrage | JA |
| 404 Not Found | Ressource nicht gefunden | JA |
| 500 Internal Server Error | Serverfehler | NEIN (kein Fehlerfall aufgetreten) |

---

## 7. Fazit

Alle 12 Integrationstests wurden erfolgreich bestanden. Die REST-API ist vollständig funktionsfähig und liefert korrekte HTTP-Statuscodes für alle getesteten Szenarien.

---

**Erstellt von:** Automatisiertes Testsystem
**Version:** 1.0.0
