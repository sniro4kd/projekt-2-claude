# Protokoll: Projektstart

| Attribut | Wert |
|----------|------|
| Datum | 2025-03-21 |
| Teilnehmer | Entwicklungsteam, Claude (AI-Assistent) |
| Thema | Initiale Projektplanung |

---

## Agenda

1. Analyse des Project Proposals
2. Klärung offener Fragen
3. Erstellung des Lastenhefts
4. Einrichtung der Projektstruktur
5. Einführung V-Modell

---

## 1. Analyse des Project Proposals

Das Project Proposal "CatchTheRabbit" (Version 1.0, 21.03.2025) wurde analysiert.

**Kernpunkte:**
- Browserbasiertes Strategiespiel für Kinder
- 10x10 Spielfeld, 4 Kinder vs. 1 Hase
- KI-Gegner mit max. 1 Sekunde Rechenzeit
- Zwei Bestenlisten (sortiert nach Bedenkzeit)
- Vorgegebener Tech-Stack: Vue.js + ASP.NET + SignalR

---

## 2. Klärung offener Fragen

Folgende Punkte wurden im Gespräch geklärt:

| Frage | Entscheidung |
|-------|--------------|
| Datenbank | SQLite (leichtgewichtig), aber durch Interface abstrahiert |
| Spieler-Identifikation | Freie Nickname-Eingabe (da Kinderspiel) |
| Mehrsprachigkeit | Deutsch reicht aus |
| Plattform | Desktop-Browser (keine Mobile-Unterstützung erforderlich) |
| Bestenlisten-Umfang | Top 20, öffentlich für alle sichtbar |
| Sound/Animationen | Ja, gewünscht |
| Docker-Setup | Separate Container für Frontend und Backend, docker-compose |
| Branding/Design | Freie Wahl, kindgerechtes Aussehen wichtig |
| Vorgehensmodell | V-Modell |

---

## 3. V-Modell Einführung

Das Projekt wird nach dem V-Modell strukturiert:

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

---

## 4. Ergebnisse

### Erstellte Dokumente (finale Struktur)

| Dokument | Beschreibung |
|----------|--------------|
| `01_Anforderungen/Lastenheft.md` | Vollständiges Lastenheft mit FA/NFA |
| `.gitignore` | Für Vue.js + ASP.NET + SQLite + Docker |
| `CHANGELOG.md` | Projekt-Changelog |
| `CLAUDE.md` | Kontext für AI-Assistenten |
| `entscheidungen/ADR-001-technologie-stack.md` | Tech-Stack Entscheidung |
| `entscheidungen/ADR-002-datenbank.md` | Datenbank Entscheidung |
| `entscheidungen/ADR-003-projektdokumentation.md` | Dokumentationsstruktur |
| `entscheidungen/ADR-004-vorgehensmodell.md` | V-Modell Entscheidung |

### Projektstruktur (V-Modell)

```
projekt-2-claude/
├── 01_Anforderungen/       # Lastenheft
├── 02_Spezifikation/       # Pflichtenheft (noch leer)
├── 03_Entwurf/             # Architektur (noch leer)
├── 04_Implementierung/     # Quellcode (noch leer)
│   ├── frontend/
│   └── backend/
├── 05_Test/                # Testdokumentation (noch leer)
├── entscheidungen/         # ADRs
├── protokolle/             # Gesprächsprotokolle
└── proposal/               # Ursprüngliches Proposal
```

### Offene Punkte für nächste Sessions

- [x] Review des Lastenhefts
- [x] KI-Algorithmus-Auswahl → Minimax + Alpha-Beta (ADR-005)
- [x] Pflichtenheft erstellen (02_Spezifikation)
- [x] Architektur-Entwurf (03_Entwurf) - High-Level + Low-Level Design
- [ ] Design-Mockups erstellen (Grafiken, Assets)
- [ ] Implementierung beginnen

---

## 5. Nächste Schritte

1. ~~Pflichtenheft erstellen (Phase: Systemspezifikation)~~ ✓
2. ~~Architektur-Entwurf (Phase: Systementwurf)~~ ✓
3. Implementierung beginnen (Phase: Implementierung)
