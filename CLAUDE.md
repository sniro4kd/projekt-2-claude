# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Projekt: CatchTheRabbit

Browserbasiertes Strategiespiel für ERP_Champion GmbH. Ein Spieler (Mensch) spielt gegen eine KI - entweder als Hase oder als die vier Kinder auf einem 10x10 Spielfeld.

## Technologie-Stack (vorgegeben, nicht ändern)

| Komponente | Technologie |
|------------|-------------|
| Frontend | Vue.js 3 |
| Backend | ASP.NET Core (C#) |
| API-Kommunikation | REST + SignalR |
| Datenbank | SQLite (durch Interface abstrahiert) |
| Containerisierung | Docker + docker-compose |

## Architektur

Das Backend folgt einer Schichtenarchitektur:
```
Controller → Business Logic → Data Access → Persistenz (SQLite)
```

**Wichtig:** Die Persistenzschicht muss durch ein Interface abstrahiert sein, sodass SQLite austauschbar bleibt.

## Docker-Setup

- Separate Container für Frontend, Backend und Datenbank
- Orchestrierung via docker-compose

## Projektstruktur

```
projekt-2-claude/
├── proposal/           # Ursprüngliches Project Proposal (PDF)
├── planung/            # Planungsdokumente
│   ├── Lastenheft.md
│   ├── CHANGELOG.md
│   ├── entscheidungen/ # ADRs (Architecture Decision Records)
│   └── protokolle/     # Gesprächsprotokolle
├── frontend/           # Vue.js Frontend (noch anzulegen)
└── backend/            # ASP.NET Backend (noch anzulegen)
```

## Dokumentationsregeln

- **ADRs**: `planung/entscheidungen/ADR-XXX-kurzbeschreibung.md`
- **Protokolle**: `planung/protokolle/YYYY-MM-DD-thema.md`
- **CHANGELOG**: Nach jeder Session aktualisieren
- **Commits**: Nach jedem abgeschlossenen Schritt committen und pushen

## Sprache

- Codekommentare: Englisch
- Dokumentation: Deutsch
- UI-Texte: Deutsch

## Wichtige Anforderungen

- KI-Zugberechnung: max. 1 Sekunde
- Bestenlisten: Top 20, öffentlich, sortiert nach Bedenkzeit
- Zielplattform: Desktop-Browser (Chrome, Firefox, Edge)
- Design: Kindgerecht mit ERP_Champion-Branding
