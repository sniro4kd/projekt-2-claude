# High-Level-Design: CatchTheRabbit

| Attribut | Wert |
|----------|------|
| Projekt | CatchTheRabbit |
| Version | 1.0 |
| Datum | 21.03.2025 |
| Status | Entwurf |
| Basis | Pflichtenheft v1.0 |

---

## 1. Einleitung

### 1.1 Zweck

Dieses Dokument beschreibt die Systemarchitektur von CatchTheRabbit auf hoher Abstraktionsebene. Es definiert die Hauptkomponenten, deren Verantwortlichkeiten und die Interaktionen zwischen ihnen.

### 1.2 Scope

- Systemkontext und externe Schnittstellen
- Container-Architektur (Deployment)
- Komponenten-Architektur (Logische Struktur)
- Datenfluss und Kommunikationsmuster
- Querschnittsthemen (Logging, Fehlerbehandlung)

### 1.3 Referenzen

| Dokument | Pfad |
|----------|------|
| Pflichtenheft | `02_Spezifikation/Pflichtenheft.md` |
| ADR-001 Technologie-Stack | `00_Projektmanagement/entscheidungen/ADR-001-technologie-stack.md` |

---

## 2. Architektur-Übersicht

### 2.1 Architektur-Prinzipien

| Prinzip | Beschreibung |
|---------|--------------|
| **Separation of Concerns** | Klare Trennung zwischen Präsentation, Geschäftslogik und Datenzugriff |
| **Dependency Inversion** | Abhängigkeiten zeigen nach innen; Interfaces abstrahieren Implementierungen |
| **Single Responsibility** | Jede Komponente hat genau eine Verantwortlichkeit |
| **Stateless Backend** | Spielzustand wird pro Request übergeben, kein Server-Side Session State |

### 2.2 Architektur-Stil

**Client-Server mit RESTful API + WebSocket**

```
┌──────────────────────────────────────────────────────────────────┐
│                                                                  │
│                        ┌─────────────┐                          │
│                        │   Browser   │                          │
│                        │   (Client)  │                          │
│                        └──────┬──────┘                          │
│                               │                                  │
│              ┌────────────────┼────────────────┐                │
│              │                │                │                │
│              ▼                ▼                ▼                │
│         HTTP/REST        WebSocket         Static              │
│         (Sync)           (Async)           Assets              │
│              │                │                │                │
│              └────────────────┼────────────────┘                │
│                               │                                  │
│                        ┌──────┴──────┐                          │
│                        │   Server    │                          │
│                        │  (Backend)  │                          │
│                        └─────────────┘                          │
│                                                                  │
└──────────────────────────────────────────────────────────────────┘
```

---

## 3. Systemkontext

### 3.1 Kontextdiagramm

```
                              ┌─────────────────────────────────────┐
                              │                                     │
    ┌────────────┐            │         CatchTheRabbit              │
    │            │  Browser   │           System                    │
    │  Spieler   │◄──────────►│                                     │
    │            │  (HTTPS)   │  ┌─────────┐      ┌─────────────┐  │
    └────────────┘            │  │Frontend │◄────►│   Backend   │  │
                              │  └─────────┘      └──────┬──────┘  │
    ┌────────────┐            │                          │         │
    │            │  Browser   │                          ▼         │
    │  Besucher  │◄──────────►│                   ┌───────────┐    │
    │            │  (HTTPS)   │                   │  SQLite   │    │
    └────────────┘            │                   │    DB     │    │
                              │                   └───────────┘    │
                              │                                     │
                              └─────────────────────────────────────┘
```

### 3.2 Externe Akteure

| Akteur | Beschreibung | Interaktion |
|--------|--------------|-------------|
| Spieler | Spielt aktiv gegen die KI | Spielzüge, Bestenlisten-Eintrag |
| Besucher | Schaut sich Bestenlisten an | Nur lesender Zugriff |

### 3.3 Externe Systeme

Keine externen Systemabhängigkeiten. Das System ist vollständig self-contained.

---

## 4. Container-Architektur (Deployment)

### 4.1 Container-Diagramm

```
┌─────────────────────────────────────────────────────────────────────────┐
│                            Docker Host                                   │
│                                                                          │
│  ┌─────────────────────────────────────────────────────────────────┐    │
│  │                      Docker Compose Network                      │    │
│  │                                                                  │    │
│  │   ┌─────────────────────────┐    ┌─────────────────────────┐   │    │
│  │   │                         │    │                         │   │    │
│  │   │    frontend             │    │    backend              │   │    │
│  │   │    (nginx:alpine)       │    │    (ASP.NET Core)       │   │    │
│  │   │                         │    │                         │   │    │
│  │   │  ┌───────────────────┐  │    │  ┌───────────────────┐  │   │    │
│  │   │  │   Vue.js SPA      │  │    │  │   Web API         │  │   │    │
│  │   │  │   (Static Files)  │  │    │  │   + SignalR Hub   │  │   │    │
│  │   │  └───────────────────┘  │    │  └───────────────────┘  │   │    │
│  │   │                         │    │           │             │   │    │
│  │   │  Port: 80 (extern)      │    │           ▼             │   │    │
│  │   │                         │    │  ┌───────────────────┐  │   │    │
│  │   │  nginx.conf:            │    │  │   SQLite DB       │  │   │    │
│  │   │  - / → Vue SPA          │    │  │   (Volume Mount)  │  │   │    │
│  │   │  - /api → backend:5000  │    │  └───────────────────┘  │   │    │
│  │   │  - /gamehub → backend   │    │                         │   │    │
│  │   │                         │    │  Port: 5000 (intern)    │   │    │
│  │   └────────────┬────────────┘    └─────────────────────────┘   │    │
│  │                │                              ▲                 │    │
│  │                └──────────────────────────────┘                 │    │
│  │                     HTTP Reverse Proxy                          │    │
│  └─────────────────────────────────────────────────────────────────┘    │
│                                                                          │
│  Volumes:                                                                │
│  - catchtherabbit_data:/app/data  (SQLite Persistenz)                   │
│                                                                          │
└─────────────────────────────────────────────────────────────────────────┘
```

### 4.2 Container-Spezifikation

| Container | Base Image | Ports | Volumes | Zweck |
|-----------|------------|-------|---------|-------|
| frontend | nginx:alpine | 80:80 | - | Statische Assets + Reverse Proxy |
| backend | mcr.microsoft.com/dotnet/aspnet:8.0 | 5000 (intern) | /app/data | API + Spiellogik + KI |

### 4.3 Docker Compose Struktur

```yaml
# docker-compose.yml (Konzept)
version: '3.8'

services:
  frontend:
    build: ./frontend
    ports:
      - "80:80"
    depends_on:
      - backend

  backend:
    build: ./backend
    expose:
      - "5000"
    volumes:
      - catchtherabbit_data:/app/data
    environment:
      - ASPNETCORE_ENVIRONMENT=Production

volumes:
  catchtherabbit_data:
```

---

## 5. Komponenten-Architektur

### 5.1 Frontend-Komponenten

```
┌─────────────────────────────────────────────────────────────────────┐
│                         Vue.js Frontend                              │
│                                                                      │
│  ┌─────────────────────────────────────────────────────────────┐    │
│  │                        Views (Pages)                         │    │
│  │  ┌─────────────┐  ┌─────────────┐  ┌─────────────────────┐  │    │
│  │  │  HomeView   │  │  GameView   │  │  LeaderboardView    │  │    │
│  │  └─────────────┘  └─────────────┘  └─────────────────────┘  │    │
│  └─────────────────────────────────────────────────────────────┘    │
│                                │                                     │
│                                ▼                                     │
│  ┌─────────────────────────────────────────────────────────────┐    │
│  │                      Components                              │    │
│  │  ┌──────────────┐  ┌──────────────┐  ┌──────────────────┐   │    │
│  │  │  GameBoard   │  │ GameControls │  │  LeaderboardList │   │    │
│  │  ├──────────────┤  ├──────────────┤  ├──────────────────┤   │    │
│  │  │  BoardCell   │  │  RoleSelect  │  │  LeaderboardItem │   │    │
│  │  │  GamePiece   │  │  Timer       │  │                  │   │    │
│  │  │              │  │  SoundToggle │  │                  │   │    │
│  │  └──────────────┘  └──────────────┘  └──────────────────┘   │    │
│  │                                                              │    │
│  │  ┌──────────────┐  ┌──────────────┐                         │    │
│  │  │ GameEndModal │  │ HeaderBar    │                         │    │
│  │  └──────────────┘  └──────────────┘                         │    │
│  └─────────────────────────────────────────────────────────────┘    │
│                                │                                     │
│                                ▼                                     │
│  ┌─────────────────────────────────────────────────────────────┐    │
│  │                    State Management (Pinia)                  │    │
│  │  ┌──────────────────────┐  ┌─────────────────────────────┐  │    │
│  │  │     gameStore        │  │    leaderboardStore         │  │    │
│  │  ├──────────────────────┤  ├─────────────────────────────┤  │    │
│  │  │ - gameState          │  │ - rabbitLeaderboard         │  │    │
│  │  │ - selectedPiece      │  │ - childrenLeaderboard       │  │    │
│  │  │ - validMoves         │  │ - isLoading                 │  │    │
│  │  │ - playerThinkingTime │  │                             │  │    │
│  │  └──────────────────────┘  └─────────────────────────────┘  │    │
│  └─────────────────────────────────────────────────────────────┘    │
│                                │                                     │
│                                ▼                                     │
│  ┌─────────────────────────────────────────────────────────────┐    │
│  │                       Services                               │    │
│  │  ┌──────────────────────┐  ┌─────────────────────────────┐  │    │
│  │  │    gameService       │  │    signalRService           │  │    │
│  │  ├──────────────────────┤  ├─────────────────────────────┤  │    │
│  │  │ - createGame()       │  │ - connect()                 │  │    │
│  │  │ - makeMove()         │  │ - onGameStateUpdated()      │  │    │
│  │  │ - getLeaderboard()   │  │ - onAiMoveCompleted()       │  │    │
│  │  │ - submitScore()      │  │ - disconnect()              │  │    │
│  │  └──────────────────────┘  └─────────────────────────────┘  │    │
│  └─────────────────────────────────────────────────────────────┘    │
│                                                                      │
└─────────────────────────────────────────────────────────────────────┘
```

### 5.2 Backend-Komponenten

```
┌─────────────────────────────────────────────────────────────────────┐
│                        ASP.NET Core Backend                          │
│                                                                      │
│  ┌─────────────────────────────────────────────────────────────┐    │
│  │                    Presentation Layer                        │    │
│  │                                                              │    │
│  │  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────┐  │    │
│  │  │ GameController  │  │LeaderboardCtrl  │  │  GameHub    │  │    │
│  │  │     [REST]      │  │    [REST]       │  │  [SignalR]  │  │    │
│  │  ├─────────────────┤  ├─────────────────┤  ├─────────────┤  │    │
│  │  │ POST /game/new  │  │ GET /leaderboard│  │ JoinGame()  │  │    │
│  │  │ POST /game/move │  │ POST /leaderboard│ │ LeaveGame() │  │    │
│  │  └────────┬────────┘  └────────┬────────┘  └──────┬──────┘  │    │
│  └───────────┼────────────────────┼──────────────────┼──────────┘    │
│              │                    │                  │                │
│              ▼                    ▼                  ▼                │
│  ┌─────────────────────────────────────────────────────────────┐    │
│  │                    Business Logic Layer                      │    │
│  │                                                              │    │
│  │  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────┐  │    │
│  │  │   GameService   │  │   AIService     │  │Leaderboard  │  │    │
│  │  │                 │  │                 │  │  Service    │  │    │
│  │  ├─────────────────┤  ├─────────────────┤  ├─────────────┤  │    │
│  │  │ CreateGame()    │  │ CalculateMove() │  │ GetTop20()  │  │    │
│  │  │ ValidateMove()  │  │ Minimax()       │  │ AddEntry()  │  │    │
│  │  │ ApplyMove()     │  │ Evaluate()      │  │             │  │    │
│  │  │ CheckWin()      │  │                 │  │             │  │    │
│  │  └────────┬────────┘  └─────────────────┘  └──────┬──────┘  │    │
│  └───────────┼───────────────────────────────────────┼──────────┘    │
│              │                                       │                │
│              ▼                                       ▼                │
│  ┌─────────────────────────────────────────────────────────────┐    │
│  │                    Data Access Layer                         │    │
│  │                                                              │    │
│  │         ┌─────────────────────────────────────┐              │    │
│  │         │      ILeaderboardRepository        │◄─────────┐   │    │
│  │         │            «interface»              │          │   │    │
│  │         ├─────────────────────────────────────┤          │   │    │
│  │         │ + GetTopEntriesAsync(role, count)  │          │   │    │
│  │         │ + AddEntryAsync(entry)             │          │   │    │
│  │         └─────────────────────────────────────┘          │   │    │
│  │                          ▲                               │   │    │
│  │                          │ implements                    │   │    │
│  │                          │                               │   │    │
│  │         ┌────────────────┴────────────────────┐          │   │    │
│  │         │   SqliteLeaderboardRepository      │          │   │    │
│  │         └─────────────────────────────────────┘          │   │    │
│  └─────────────────────────────────────────────────────────────┘    │
│                                │                                     │
│                                ▼                                     │
│                    ┌─────────────────────┐                          │
│                    │     SQLite DB       │                          │
│                    │   (game.db)         │                          │
│                    └─────────────────────┘                          │
│                                                                      │
└─────────────────────────────────────────────────────────────────────┘
```

### 5.3 Komponentenverantwortlichkeiten

#### Frontend

| Komponente | Verantwortlichkeit |
|------------|-------------------|
| **Views** | Routing-Ziele, Seitenstruktur |
| **GameBoard** | Rendering des Spielfelds, Klick-Handling |
| **GameControls** | Spielsteuerung, Timer-Anzeige, Sound-Toggle |
| **gameStore** | Spielzustand, Zugvalidierung (Client-Side) |
| **gameService** | HTTP-Kommunikation mit Backend |
| **signalRService** | WebSocket-Verbindung für Echtzeit-Updates |

#### Backend

| Komponente | Verantwortlichkeit |
|------------|-------------------|
| **GameController** | REST-Endpoints für Spieloperationen |
| **LeaderboardController** | REST-Endpoints für Bestenliste |
| **GameHub** | SignalR-Hub für Echtzeit-Kommunikation |
| **GameService** | Spiellogik, Zugvalidierung, Spielzustand |
| **AIService** | KI-Berechnung (Minimax + Alpha-Beta) |
| **LeaderboardService** | Bestenlisten-Logik |
| **ILeaderboardRepository** | Datenzugriffs-Abstraktion |
| **SqliteLeaderboardRepository** | SQLite-Implementierung |

---

## 6. Datenfluss

### 6.1 Spielzug-Sequenz (Übersicht)

```
┌────────┐          ┌────────┐          ┌────────┐          ┌────────┐
│ Browser│          │Frontend│          │Backend │          │   KI   │
└───┬────┘          └───┬────┘          └───┬────┘          └───┬────┘
    │                   │                   │                   │
    │  Klick auf Figur  │                   │                   │
    │──────────────────►│                   │                   │
    │                   │                   │                   │
    │                   │ Zeige gültige Züge│                   │
    │◄──────────────────│ (lokal berechnet) │                   │
    │                   │                   │                   │
    │  Klick auf Ziel   │                   │                   │
    │──────────────────►│                   │                   │
    │                   │                   │                   │
    │                   │ POST /api/game/move                   │
    │                   │──────────────────►│                   │
    │                   │                   │                   │
    │                   │                   │ Validiere Zug     │
    │                   │                   │──────────────────►│
    │                   │                   │                   │
    │                   │                   │ Berechne KI-Zug   │
    │                   │                   │──────────────────►│
    │                   │                   │                   │
    │                   │                   │◄──────────────────│
    │                   │                   │   KI-Zug          │
    │                   │                   │                   │
    │                   │◄──────────────────│                   │
    │                   │ Response mit      │                   │
    │                   │ neuem GameState   │                   │
    │                   │ + KI-Zug          │                   │
    │                   │                   │                   │
    │◄──────────────────│                   │                   │
    │ Update UI         │                   │                   │
    │ (Animation)       │                   │                   │
    │                   │                   │                   │
```

### 6.2 Kommunikationsmuster

| Pattern | Verwendung | Technologie |
|---------|------------|-------------|
| Request-Response | Spielzüge, Bestenliste laden | REST (HTTP) |
| Push Notification | KI-Zug-Status, Spielende | SignalR (WebSocket) |
| Client-Side State | Zugvorschau, Timer | Pinia Store |

---

## 7. Querschnittsthemen

### 7.1 Fehlerbehandlung

```
┌─────────────────────────────────────────────────────────────────┐
│                    Fehlerbehandlungs-Strategie                   │
│                                                                  │
│  Frontend:                                                       │
│  ┌─────────────────────────────────────────────────────────┐    │
│  │  API-Fehler → Toast-Nachricht → Retry-Option            │    │
│  │  Netzwerk-Fehler → Reconnect-Dialog                     │    │
│  │  Validierungsfehler → Inline-Feedback                   │    │
│  └─────────────────────────────────────────────────────────┘    │
│                                                                  │
│  Backend:                                                        │
│  ┌─────────────────────────────────────────────────────────┐    │
│  │  Controller → try/catch → ProblemDetails Response       │    │
│  │  Service → Exception → Logging + Rethrow               │    │
│  │  Repository → DbException → Wrap in ServiceException   │    │
│  └─────────────────────────────────────────────────────────┘    │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

### 7.2 Logging

| Schicht | Log-Level | Inhalt |
|---------|-----------|--------|
| Controller | Information | API-Aufrufe, Request-IDs |
| Service | Information/Warning | Geschäftslogik-Events, Validierungsfehler |
| Repository | Debug | SQL-Queries (nur Development) |
| KI | Debug | Suchtiefe, Evaluierungs-Scores |

### 7.3 Konfiguration

```
┌─────────────────────────────────────────────────────────────────┐
│                    Konfigurationsquellen                         │
│                                                                  │
│  Backend (appsettings.json):                                    │
│  ├── ConnectionStrings:SqliteConnection                         │
│  ├── AI:MaxSearchDepth                                          │
│  ├── AI:MaxThinkingTimeMs                                       │
│  ├── Leaderboard:MaxEntries                                     │
│  └── Logging:LogLevel                                           │
│                                                                  │
│  Frontend (.env):                                                │
│  ├── VITE_API_BASE_URL                                          │
│  └── VITE_SIGNALR_HUB_URL                                       │
│                                                                  │
│  Docker (docker-compose.yml):                                   │
│  ├── Environment Variables                                      │
│  └── Volume Mounts                                              │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

### 7.4 Security

| Aspekt | Maßnahme |
|--------|----------|
| Input Validation | Server-seitige Validierung aller Eingaben |
| XSS | Vue.js automatisches Escaping + CSP Header |
| CORS | Konfigurierte Origins (Produktiv: nur eigene Domain) |
| Rate Limiting | Optional: Middleware für API-Endpoints |

---

## 8. Technologie-Stack (Detail)

### 8.1 Frontend

| Kategorie | Technologie | Version | Zweck |
|-----------|-------------|---------|-------|
| Framework | Vue.js | 3.x | SPA-Framework |
| Build Tool | Vite | 5.x | Dev-Server + Build |
| State | Pinia | 2.x | State Management |
| Routing | Vue Router | 4.x | Client-Side Routing |
| HTTP Client | Axios | 1.x | REST-API Calls |
| SignalR | @microsoft/signalr | 8.x | WebSocket Client |
| Styling | CSS/SCSS | - | Styling |

### 8.2 Backend

| Kategorie | Technologie | Version | Zweck |
|-----------|-------------|---------|-------|
| Framework | ASP.NET Core | 8.0 | Web API Framework |
| SignalR | Microsoft.AspNetCore.SignalR | 8.0 | WebSocket Server |
| ORM | Dapper | 2.x | Leichtgewichtiges ORM |
| Database | SQLite | 3.x | Embedded Database |
| Logging | Serilog | 3.x | Strukturiertes Logging |
| Validation | FluentValidation | 11.x | Request-Validierung |

### 8.3 DevOps

| Kategorie | Technologie | Version | Zweck |
|-----------|-------------|---------|-------|
| Container | Docker | 24.x | Containerisierung |
| Orchestrierung | Docker Compose | 2.x | Multi-Container |
| Web Server | nginx | 1.25 | Reverse Proxy |

---

## 9. Deployment-Strategie

### 9.1 Build-Prozess

```
┌─────────────────────────────────────────────────────────────────┐
│                        Build Pipeline                            │
│                                                                  │
│  1. Frontend Build                                               │
│     npm install → npm run build → dist/                         │
│                                                                  │
│  2. Backend Build                                                │
│     dotnet restore → dotnet publish → publish/                  │
│                                                                  │
│  3. Docker Build                                                 │
│     docker-compose build                                         │
│                                                                  │
│  4. Docker Push (optional)                                       │
│     docker-compose push                                          │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

### 9.2 Deployment-Optionen

| Option | Beschreibung | Zielgruppe |
|--------|--------------|------------|
| Docker Compose | `docker-compose up -d` | ERP_Champion Server |
| Self-Hosting | Kunde lädt Images, startet Compose | Kunden |

---

## 10. Anhang

### 10.1 Glossar Architektur-Begriffe

| Begriff | Definition |
|---------|------------|
| Container | Docker-Container als Deployment-Einheit |
| Layer | Logische Schicht der Anwendung |
| Service | Klasse mit Geschäftslogik |
| Repository | Datenzugriffs-Abstraktion |
| Hub | SignalR-Endpunkt für bidirektionale Kommunikation |

### 10.2 Entscheidungslog

| Entscheidung | Begründung |
|--------------|------------|
| nginx als Reverse Proxy | Effizientes Static File Serving, einfache Konfiguration |
| Dapper statt EF Core | Leichtgewichtig, volle SQL-Kontrolle für einfaches Schema |
| Pinia statt Vuex | Moderner, TypeScript-freundlicher, offiziell empfohlen |
| Stateless Backend | Einfachere Skalierung, kein Session-Management nötig |
