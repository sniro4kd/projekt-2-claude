# Mastertestplan - CatchTheRabbit

| Dokument      | Mastertestplan                    |
|---------------|-----------------------------------|
| Projekt       | CatchTheRabbit                    |
| Version       | 1.0                               |
| Datum         | 2025-03-21                        |
| Autor         | Entwicklungsteam                  |
| Status        | In Bearbeitung                    |

---

## 1. Einleitung

### 1.1 Zweck
Dieser Mastertestplan definiert die übergreifende Teststrategie für das Projekt CatchTheRabbit. Er beschreibt die Testebenen, Testziele, Ressourcen und den Zeitplan für alle Testaktivitäten.

### 1.2 Geltungsbereich
Der Plan gilt für alle Testaktivitäten des CatchTheRabbit-Projekts, einschließlich:
- Komponententests (Unit Tests)
- Systemtests (Integration Tests)
- Akzeptanztests (End-to-End Tests)

### 1.3 Referenzen
- Lastenheft v1.0
- Pflichtenheft v1.0
- High-Level-Design v1.0
- Low-Level-Design v1.0

---

## 2. Teststrategie

### 2.1 Testebenen

```
┌─────────────────────────────────────────────────────────────┐
│                    AKZEPTANZTESTS                           │
│         (Validierung gegen Anforderungen)                   │
│    ┌───────────────────────────────────────────────────┐   │
│    │              SYSTEMTESTS                          │   │
│    │        (Integration & API-Tests)                  │   │
│    │    ┌─────────────────────────────────────────┐   │   │
│    │    │         KOMPONENTENTESTS                │   │   │
│    │    │        (Unit Tests)                     │   │   │
│    │    └─────────────────────────────────────────┘   │   │
│    └───────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
```

### 2.2 Testarten im Detail

| Testart | Ziel | Werkzeuge | Verantwortung |
|---------|------|-----------|---------------|
| Komponententest | Einzelne Klassen/Methoden isoliert testen | xUnit, Moq | Entwickler |
| Systemtest | Integration der Komponenten, API-Endpunkte | xUnit, WebApplicationFactory | Entwickler |
| Akzeptanztest | Validierung der Anforderungen aus Lastenheft | Playwright, xUnit | QA/Entwickler |

---

## 3. Testumfang

### 3.1 Komponententests

**Zu testende Module:**

| Modul | Klasse | Priorität | Abdeckungsziel |
|-------|--------|-----------|----------------|
| Core | GameService | Hoch | 90% |
| Core | AIService | Hoch | 85% |
| Core | Position | Mittel | 100% |
| Core | GameState | Mittel | 80% |
| Infrastructure | SqliteLeaderboardRepository | Hoch | 80% |

**Testfälle GameService:**
- KT-GS-001: Initialisierung neues Spiel (Hase-Rolle)
- KT-GS-002: Initialisierung neues Spiel (Kinder-Rolle)
- KT-GS-003: Gültige Bewegung Hase (alle 4 Richtungen)
- KT-GS-004: Gültige Bewegung Kind (2 Richtungen)
- KT-GS-005: Ungültige Bewegung außerhalb Spielfeld
- KT-GS-006: Ungültige Bewegung auf besetztes Feld
- KT-GS-007: Siegbedingung Hase erreicht
- KT-GS-008: Siegbedingung Kinder (Hase blockiert)
- KT-GS-009: Zugwechsel nach gültigem Zug

**Testfälle AIService:**
- KT-AI-001: KI berechnet gültigen Zug als Hase
- KT-AI-002: KI berechnet gültigen Zug als Kinder
- KT-AI-003: KI-Berechnung unter 1 Sekunde
- KT-AI-004: KI wählt optimalen Zug (Siegzug wenn möglich)
- KT-AI-005: Alpha-Beta Pruning reduziert Suchraum

### 3.2 Systemtests

**API-Endpunkte:**

| Endpunkt | Methode | Testfälle |
|----------|---------|-----------|
| /api/game/start | POST | ST-API-001 bis ST-API-003 |
| /api/game/{id}/move | POST | ST-API-004 bis ST-API-008 |
| /api/game/{id}/state | GET | ST-API-009, ST-API-010 |
| /api/leaderboard | GET | ST-API-011, ST-API-012 |
| /api/leaderboard/submit | POST | ST-API-013 bis ST-API-015 |
| /gamehub | SignalR | ST-HUB-001 bis ST-HUB-005 |

**Testfälle API:**
- ST-API-001: Spiel starten als Hase
- ST-API-002: Spiel starten als Kinder
- ST-API-003: Fehler bei ungültiger Rolle
- ST-API-004: Gültiger Spielzug
- ST-API-005: Ungültiger Spielzug (außerhalb)
- ST-API-006: Ungültiger Spielzug (nicht am Zug)
- ST-API-007: KI-Antwort nach Spielerzug
- ST-API-008: Spielstatus nach Sieg
- ST-API-009: Spielzustand abrufen (gültige ID)
- ST-API-010: Spielzustand abrufen (ungültige ID)
- ST-API-011: Leaderboard abrufen (leer)
- ST-API-012: Leaderboard abrufen (mit Einträgen)
- ST-API-013: Score eintragen (gültig)
- ST-API-014: Score eintragen (leerer Nickname)
- ST-API-015: Score eintragen (ungültige Game-ID)

**Testfälle SignalR:**
- ST-HUB-001: Verbindung zum GameHub
- ST-HUB-002: JoinGame Event
- ST-HUB-003: MoveMade Event nach Spielzug
- ST-HUB-004: GameOver Event
- ST-HUB-005: Verbindungsabbruch Handling

### 3.3 Akzeptanztests

**Anforderungsverfolgung:**

| Anforderung | Akzeptanztest | Beschreibung |
|-------------|---------------|--------------|
| FA-101 | AT-001 | 10x10 Spielfeld wird korrekt angezeigt |
| FA-102 | AT-002 | Hase startet in Mitte erste Reihe |
| FA-103 | AT-002 | Kinder starten in letzter Reihe |
| FA-201 | AT-003 | Hase bewegt sich diagonal (4 Richtungen) |
| FA-202 | AT-004 | Kinder bewegen sich diagonal nach oben |
| FA-301 | AT-005 | Spieler wählt Rolle vor Spielbeginn |
| FA-302 | AT-006 | KI spielt automatisch nach Spielerzug |
| FA-401 | AT-007 | Hase gewinnt bei Erreichen unterer Rand |
| FA-402 | AT-008 | Kinder gewinnen wenn Hase blockiert |
| FA-501 | AT-009 | Bestenliste zeigt Top 20 |
| FA-502 | AT-010 | Sortierung nach KI-Denkzeit |
| FA-503 | AT-011 | Nickname-Eingabe nach Sieg |
| NFA-201 | AT-012 | KI antwortet unter 1 Sekunde |

---

## 4. Testumgebung

### 4.1 Hardware-Anforderungen
- Entwicklungsrechner mit mindestens 8 GB RAM
- Stabile Internetverbindung für CI/CD

### 4.2 Software-Anforderungen

| Komponente | Version | Zweck |
|------------|---------|-------|
| .NET SDK | 8.0 | Backend-Tests |
| Node.js | 20.x | Frontend-Tests |
| xUnit | 2.6+ | Test-Framework Backend |
| Playwright | 1.40+ | E2E-Tests |
| SQLite | 3.x | Testdatenbank |

### 4.3 Testdaten
- Vordefinierte Spielzustände für Randfälle
- Leere Datenbank für Leaderboard-Tests
- Seed-Daten für Bestenlisten-Tests

---

## 5. Testdurchführung

### 5.1 Testreihenfolge

```
1. Komponententests
   └── Müssen zu 100% bestehen vor Systemtests

2. Systemtests
   └── Müssen zu 100% bestehen vor Akzeptanztests

3. Akzeptanztests
   └── Validierung der Anforderungsabdeckung
```

### 5.2 Eingangs- und Ausgangskriterien

**Eingangskriterien:**
- Code kompiliert fehlerfrei
- Alle Dependencies installiert
- Testumgebung konfiguriert

**Ausgangskriterien Komponententests:**
- Alle Tests bestanden
- Code Coverage ≥ 80%
- Keine kritischen Defekte

**Ausgangskriterien Systemtests:**
- Alle API-Tests bestanden
- Response-Zeiten im akzeptablen Bereich
- Keine Integrationsprobleme

**Ausgangskriterien Akzeptanztests:**
- Alle Anforderungen validiert
- Keine offenen kritischen Defekte
- Abnahmeprotokoll unterschrieben

---

## 6. Risiken und Maßnahmen

| Risiko | Wahrscheinlichkeit | Auswirkung | Maßnahme |
|--------|-------------------|------------|----------|
| KI-Algorithmus zu langsam | Mittel | Hoch | Performance-Profiling, Optimierung |
| SignalR-Verbindungsprobleme | Niedrig | Mittel | Fallback auf Polling |
| Flaky Tests durch Timing | Mittel | Mittel | Retry-Mechanismen, Timeouts |
| Browser-Kompatibilität | Niedrig | Niedrig | Cross-Browser-Tests |

---

## 7. Metriken und Berichterstattung

### 7.1 Zu erfassende Metriken
- Anzahl Testfälle (geplant/ausgeführt/bestanden/fehlgeschlagen)
- Code Coverage (Zeilen, Branches)
- Defekt-Density
- Testausführungszeit

### 7.2 Berichtsformat
- **Komponententest-Report**: Detaillierte Testergebnisse mit Coverage
- **Systemtest-Report**: API-Testergebnisse mit Response-Zeiten
- **Akzeptanztest-Report**: Anforderungsverfolgungsmatrix mit Screenshots

### 7.3 Berichtsfrequenz
- Nach jeder Testausführung
- Zusammenfassung nach Abschluss aller Tests

---

## 8. Freigabe

| Rolle | Name | Datum | Unterschrift |
|-------|------|-------|--------------|
| Projektleiter | | | |
| QA-Verantwortlicher | | | |
| Entwicklungsleiter | | | |

---

## Änderungshistorie

| Version | Datum | Autor | Änderung |
|---------|-------|-------|----------|
| 1.0 | 2025-03-21 | Entwicklungsteam | Initiale Version |
