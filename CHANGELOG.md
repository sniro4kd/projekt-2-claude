# Changelog - CatchTheRabbit Projektplanung

Dieses Dokument gibt einen Überblick über den Fortschritt der Projektplanung.

---

## 2026-01-13 - Testphase abgeschlossen

### Anforderungsverifikation
- **Gesamt:** 44/50 Anforderungen erfüllt (88%)
- **Muss-Anforderungen:** 37/39 erfüllt (94.9%)
- **Abnahmekriterien:** 7/7 erfüllt (100%)
- **Nicht erfüllt (Soll):** FA-502 (Gewinnrate), FA-800/801 (Audio)
- **Status:** Abnahmebereit

### Testergebnisse
- **Komponententests**: 45/45 bestanden (100%)
- **Systemtests**: 12/12 bestanden (100%)
- **Akzeptanztests**: 8/8 bestanden (100%)
- **Simulationstests**: 3/3 bestanden (Gewinnwahrscheinlichkeit dokumentiert)

### Akzeptanztests - E2E Tests mit Playwright
- **Testframework:** Playwright 1.40.0
- **Browser:** Chromium (headless)
- **Ausführungszeit:** ~10 Sekunden
- **Ergebnis:** Alle Tests bestanden

### Während der Tests gefundene und behobene Bugs
1. **Backend:** Spielende-Erkennung - `GetValidMoves()` prüfte fälschlicherweise Zugwechsel
2. **Frontend:** Eigenschaftsnamen - `rabbitPosition` statt `rabbit`
3. **Frontend:** Koordinaten-Mapping - `row/col` statt `x/y`
4. **Frontend:** Store-Getter - `gameState?.isPlayerTurn` statt `gameStore.isPlayerTurn`
5. **Frontend:** Funktion - `startNewGame()` statt `createGame()`

### Code Coverage
- **Line Coverage**: 83.8%
- **Branch Coverage**: 73.1%
- **Method Coverage**: 89.6%

### Gewinnwahrscheinlichkeit (FA-502)
- **Hase gewinnt**: 0% (KI vs KI)
- **Kinder gewinnen**: 100% (KI vs KI)
- **Status**: FA-502 NICHT ERFÜLLT - Spiel favorisiert strukturell die Kinder
- **Analyse**: `05_Test/Gewinnwahrscheinlichkeit-Analyse.md`
- **Balancing-Optionen**: `05_Test/Balancing-Optionen-Analyse.md`
- **Empfehlung**: Anforderungsanpassung (Option C)

### Hinzugefügt
- **Test-Reports** (`05_Test/`)
  - `Komponententest-Report.md` - Detaillierte Unit Test Ergebnisse
  - `Systemtest-Report.md` - API Integration Test Ergebnisse
  - `Akzeptanztest-Report.md` - E2E Test Status und Anleitung
  - `Code-Coverage-Report.md` - Testabdeckung nach Assembly/Klasse
  - `Gewinnwahrscheinlichkeit-Analyse.md` - FA-502 Validierung

- **Simulationstests** (`CatchTheRabbit.Tests/Simulation/`)
  - `WinRateSimulationTests.cs` - KI vs KI Simulation (500 Spiele)

### Korrigiert
- Testpositionen auf schwarze Felder (X+Y ungerade) angepasst
- API-Routen in Integration Tests korrigiert
- Assertion-Nachrichten an tatsächliche Implementierung angepasst

---

## 2026-01-13 - Testphase gestartet

### Hinzugefügt
- **Mastertestplan v1.0** (`05_Test/Mastertestplan.md`)
  - Teststrategie und Testebenen
  - Testumfang für Komponenten-, System- und Akzeptanztests
  - Testumgebung und Werkzeuge
  - Risiken und Maßnahmen
  - Metriken und Berichterstattung

- **Testspezifikationen**
  - `05_Test/Komponententests.md` - Unit Test Spezifikation
  - `05_Test/Systemtests.md` - Integration Test Spezifikation
  - `05_Test/Akzeptanztests.md` - E2E Test Spezifikation

- **Test-Implementierung** (`CatchTheRabbit.Tests/`)
  - GameServiceTests (17 Testfälle)
  - AIServiceTests (9 Testfälle)
  - PositionTests (11 Testfälle)
  - GameStateTests (8 Testfälle)
  - LeaderboardRepositoryTests (8 Testfälle)
  - GameControllerTests (9 Integration Tests)
  - LeaderboardControllerTests (3 Integration Tests)

---

## 2026-01-13 - Implementierung abgeschlossen

### Hinzugefügt
- **Backend-Implementierung** (`04_Implementierung/backend/`)
  - `CatchTheRabbit.Core/` - Domain-Modelle und Business Logic
    - Models: GameState, Position, Move, Enums
    - Services: GameService, AIService, LeaderboardService
    - Interfaces: IGameService, IAIService, ILeaderboardService
  - `CatchTheRabbit.Infrastructure/` - Datenzugriffsschicht
    - SqliteLeaderboardRepository mit Dapper ORM
  - `CatchTheRabbit.Api/` - Web API Layer
    - GameController (REST-Endpunkte)
    - LeaderboardController (Bestenliste)
    - GameHub (SignalR für Echtzeit)
    - DTOs für Request/Response

- **Frontend-Implementierung** (`04_Implementierung/frontend/`)
  - Vue.js 3 mit TypeScript
  - Pinia Stores (gameStore, leaderboardStore)
  - Vue Router mit 3 Routen
  - Komponenten:
    - HeaderBar (Navigation)
    - GameBoard, BoardCell, GamePiece (Spiellogik)
    - GameInfo (Spielstatus)
    - GameOverModal (Spielende)
  - Views: HomeView, GameView, LeaderboardView
  - Kindgerechtes Design mit Animationen

- **Docker-Konfiguration**
  - `backend/Dockerfile` - Multi-Stage Build für .NET
  - `frontend/Dockerfile` - Multi-Stage Build mit nginx
  - `docker-compose.yml` - Orchestrierung beider Container
  - `frontend/nginx.conf` - Reverse Proxy für API

### Technische Details
- KI-Algorithmus: Minimax mit Alpha-Beta Pruning (Tiefe 6)
- Echtzeit-Updates über SignalR WebSocket
- SQLite-Datenbank für Bestenliste (Top 20)

---

## 2026-01-13 - Systementwurf erstellt

### Hinzugefügt
- **High-Level-Design v1.0** (`03_Entwurf/High-Level-Design.md`)
  - Architektur-Prinzipien und -Stil
  - Systemkontext-Diagramm
  - Container-Architektur (Docker Compose)
  - Frontend- und Backend-Komponenten-Übersicht
  - Datenfluss und Kommunikationsmuster
  - Querschnittsthemen (Logging, Fehlerbehandlung, Security)
  - Technologie-Stack Details
  - Deployment-Strategie

- **Low-Level-Design v1.0** (`03_Entwurf/Low-Level-Design.md`)
  - Domain-Modell (Klassendiagramme)
  - Service-Klassen mit Methodensignaturen
  - Repository-Pattern Implementation
  - Controller und DTOs
  - TypeScript-Interfaces (Frontend)
  - Pinia Store Design
  - Vue-Komponenten-Hierarchie
  - Algorithmus-Details (Zugvalidierung, KI-Bewertung, Minimax)
  - Sequenzdiagramme (Spielstart, Spielzug, Bestenliste)
  - SQL-Queries
  - Projektstruktur (Backend + Frontend)

---

## 2026-01-13 - Pflichtenheft erstellt

### Hinzugefügt
- **Pflichtenheft v1.0** erstellt (`02_Spezifikation/Pflichtenheft.md`)
  - Systemarchitektur (Frontend, Backend, Deployment)
  - Spiellogik-Spezifikation (Spielfeld, Zugvalidierung, Siegbedingungen)
  - KI-Spezifikation (Minimax mit Alpha-Beta-Pruning)
  - Use Cases (UC-01 bis UC-05)
  - Datenmodell und SQLite-Schema
  - REST API und SignalR Hub Spezifikation
  - UI-Spezifikation mit Wireframes
  - Fehlerbehandlung und Logging
  - Testabdeckung und Testfälle

### Entscheidungen
- [ADR-005](00_Projektmanagement/entscheidungen/ADR-005-ki-algorithmus.md): Minimax + Alpha-Beta als KI-Algorithmus

---

## 2026-01-13 - V-Modell Einführung

### Geändert
- **Projektstruktur** auf V-Modell Phasen umgestellt
  - `01_Anforderungen/` - Lastenheft
  - `02_Spezifikation/` - Pflichtenheft
  - `03_Entwurf/` - Architektur
  - `04_Implementierung/` - Quellcode
  - `05_Test/` - Testdokumentation
- **Docker-Setup** angepasst: Kein separater DB-Container für SQLite nötig

### Entscheidungen
- [ADR-004](00_Projektmanagement/entscheidungen/ADR-004-vorgehensmodell.md): V-Modell als Vorgehensmodell

---

## 2026-01-13 - Projektstart

### Hinzugefügt
- **Lastenheft v1.0** erstellt (`01_Anforderungen/Lastenheft.md`)
  - Funktionale Anforderungen (FA-100 bis FA-801)
  - Nichtfunktionale Anforderungen (NFA-100 bis NFA-502)
  - Systemarchitektur-Überblick
  - Abnahmekriterien

### Entscheidungen
- [ADR-001](00_Projektmanagement/entscheidungen/ADR-001-technologie-stack.md): Technologie-Stack bestätigt
- [ADR-002](00_Projektmanagement/entscheidungen/ADR-002-datenbank.md): SQLite als Datenbank gewählt
- [ADR-003](00_Projektmanagement/entscheidungen/ADR-003-projektdokumentation.md): Dokumentationsstruktur festgelegt

### Protokoll
- [2026-01-13](00_Projektmanagement/Projektprotokoll.md): Gesamtes Projekt durchgeführt
