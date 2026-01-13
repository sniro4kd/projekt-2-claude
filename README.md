# CatchTheRabbit - Fang den Hasen!

Ein browserbasiertes Strategiespiel entwickelt für ERP_Champion GmbH.

## Projektbeschreibung

"Fang den Hasen" ist ein klassisches Strategiespiel auf einem 10x10 Spielfeld. Ein Spieler steuert entweder den Hasen oder fünf Kinder, während die KI den Gegenspieler übernimmt.

### Spielregeln

**Der Hase:**
- Startet unten auf dem Spielfeld
- Bewegt sich diagonal in alle 4 Richtungen
- Gewinnt, wenn er die obere Spielfeldseite erreicht

**Die Kinder:**
- Starten oben auf dem Spielfeld (5 Figuren)
- Bewegen sich nur diagonal nach unten
- Gewinnen, wenn sie den Hasen einkreisen (keine Zugmöglichkeiten mehr)

### Features

- Intelligente KI mit Minimax-Algorithmus und Alpha-Beta-Pruning
- Echtzeit-Updates via SignalR WebSocket
- Bestenliste mit Top 20 Spielern (sortiert nach KI-Denkzeit)
- Kindgerechtes, animiertes Design
- Responsive Desktop-Browser-Unterstützung

## Technologie-Stack

| Komponente | Technologie |
|------------|-------------|
| Frontend | Vue.js 3, TypeScript, Pinia, Vue Router |
| Backend | ASP.NET Core 8.0, SignalR, Dapper |
| Datenbank | SQLite |
| Container | Docker, docker-compose |

## Schnellstart mit Docker

### Voraussetzungen

- [Docker](https://www.docker.com/get-started) installiert
- [Docker Compose](https://docs.docker.com/compose/install/) installiert

### Anwendung starten

1. **Repository klonen:**
   ```bash
   git clone <repository-url>
   cd projekt-2-claude
   ```

2. **Container bauen und starten:**
   ```bash
   cd 04_Implementierung
   docker-compose up --build
   ```

3. **Anwendung öffnen:**

   Öffne im Browser: [http://localhost](http://localhost)

### Container stoppen

```bash
docker-compose down
```

### Daten löschen (inkl. Bestenliste)

```bash
docker-compose down -v
```

## Entwicklung

### Backend lokal starten

```bash
cd 04_Implementierung/backend
dotnet restore
dotnet run --project CatchTheRabbit.Api
```

Backend läuft auf: http://localhost:5000

### Frontend lokal starten

```bash
cd 04_Implementierung/frontend
npm install
npm run dev
```

Frontend läuft auf: http://localhost:5173

### Tests ausführen

```bash
cd 04_Implementierung/backend
dotnet test
```

## Projektstruktur (V-Modell)

```
projekt-2-claude/
├── 00_Projektmanagement/  # ADRs, Proposal, Protokolle
│   ├── entscheidungen/    # Architecture Decision Records
│   ├── Projektprotokoll.md
│   └── Proposal_CatchTheRabbit_V.1.0._250321.pdf
├── 01_Anforderungen/      # Lastenheft
├── 02_Spezifikation/      # Pflichtenheft
├── 03_Entwurf/            # High-Level & Low-Level Design
├── 04_Implementierung/    # Quellcode
│   ├── backend/           # ASP.NET Core API
│   ├── frontend/          # Vue.js Frontend
│   └── docker-compose.yml # Container-Orchestrierung
├── 05_Test/               # Testdokumentation
├── CHANGELOG.md           # Änderungshistorie
└── CLAUDE.md              # KI-Assistenz-Dokumentation
```

## API-Endpunkte

| Methode | Endpunkt | Beschreibung |
|---------|----------|--------------|
| POST | `/api/game/start` | Neues Spiel starten |
| POST | `/api/game/{id}/move` | Spielzug ausführen |
| GET | `/api/game/{id}/state` | Spielzustand abrufen |
| GET | `/api/leaderboard` | Bestenliste abrufen |
| POST | `/api/leaderboard/submit` | Score eintragen |
| - | `/gamehub` | SignalR Hub für Echtzeit |

## Lizenz

Proprietär - ERP_Champion GmbH

## Kontakt

Entwickelt im Rahmen eines Softwareprojekts nach V-Modell.
