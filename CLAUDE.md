# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Projekt: CatchTheRabbit

Browserbasiertes Strategiespiel für ERP_Champion GmbH. Ein Spieler (Mensch) spielt gegen eine KI - entweder als Hase oder als die fünf Kinder auf einem 10x10 Spielfeld.

## Vorgehensmodell: V-Modell

```
    Anforderungsanalyse ─────────────────────────── Abnahmetest
           │                                            │
           ▼                                            ▲
    Systemspezifikation ─────────────────────── Systemtest
           │                                            │
           ▼                                            ▲
    Systementwurf ───────────────────────── Integrationstest
           │                                            │
           ▼                                            ▲
    Komponentenentwurf ────────────────── Komponententest
           │                                            │
           ▼                                            ▲
           └──────────► Implementierung ◄───────────────┘
```

## Projektstruktur (V-Modell Phasen)

```
projekt-2-claude/
├── 00_Projektmanagement/   # ADRs, Proposal, Protokolle
│   ├── entscheidungen/     # Architecture Decision Records
│   ├── Projektprotokoll.md
│   └── Proposal_*.pdf
├── 01_Anforderungen/       # Lastenheft
├── 02_Spezifikation/       # Pflichtenheft, Use Cases
├── 03_Entwurf/             # Architektur, API-Spec, Datenmodell
├── 04_Implementierung/     # Quellcode
│   ├── frontend/           # Vue.js
│   └── backend/            # ASP.NET Core
├── 05_Test/                # Testkonzept, Testfälle, Testberichte
└── CHANGELOG.md            # Projektfortschritt
```

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

- Separate Container für Frontend und Backend
- SQLite als Datei im Backend-Container (kein separater DB-Container nötig)
- Orchestrierung via docker-compose

## Dokumentationsregeln

- **ADRs**: `00_Projektmanagement/entscheidungen/ADR-XXX-kurzbeschreibung.md`
- **Protokolle**: `00_Projektmanagement/Projektprotokoll.md`
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
