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
| Docker-Setup | Separate Container, docker-compose |
| Branding/Design | Freie Wahl, kindgerechtes Aussehen wichtig |

---

## 3. Ergebnisse

### Erstellte Dokumente

| Dokument | Beschreibung |
|----------|--------------|
| `planung/Lastenheft.md` | Vollständiges Lastenheft mit FA/NFA |
| `.gitignore` | Für Vue.js + ASP.NET + SQLite + Docker |
| `planung/CHANGELOG.md` | Projekt-Changelog |
| `planung/entscheidungen/ADR-001-technologie-stack.md` | Tech-Stack Entscheidung |
| `planung/entscheidungen/ADR-002-datenbank.md` | Datenbank Entscheidung |
| `planung/entscheidungen/ADR-003-projektdokumentation.md` | Dokumentationsstruktur |

### Offene Punkte für nächste Sessions

- [ ] Review des Lastenhefts
- [ ] KI-Algorithmus-Auswahl (Minimax, Alpha-Beta, MCTS)
- [ ] Design-Mockups erstellen
- [ ] Pflichtenheft erstellen
- [ ] Projektstruktur (Ordner für Frontend/Backend) anlegen

---

## 4. Nächste Schritte

1. Review des Lastenhefts durch Auftraggeber
2. Bei Freigabe: Erstellung des Pflichtenhefts
3. Technische Detailplanung (Architektur, API-Design)
