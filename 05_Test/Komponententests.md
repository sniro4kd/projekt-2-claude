# Komponententest-Spezifikation - CatchTheRabbit

| Dokument      | Komponententest-Spezifikation     |
|---------------|-----------------------------------|
| Projekt       | CatchTheRabbit                    |
| Version       | 1.0                               |
| Datum         | 2025-03-21                        |
| Autor         | Entwicklungsteam                  |

---

## 1. Übersicht

### 1.1 Zweck
Diese Spezifikation definiert die Unit Tests für die einzelnen Komponenten des CatchTheRabbit-Backends.

### 1.2 Testansatz
- Isolierte Tests einzelner Klassen/Methoden
- Mocking von Dependencies
- Fokus auf Geschäftslogik

### 1.3 Testwerkzeuge
- xUnit als Test-Framework
- Moq für Mocking
- FluentAssertions für lesbare Assertions
- Coverlet für Code Coverage

---

## 2. Testfälle GameService

### KT-GS-001: Neues Spiel initialisieren (Hase)

| Attribut | Wert |
|----------|------|
| **ID** | KT-GS-001 |
| **Klasse** | GameService |
| **Methode** | CreateNewGame(PlayerRole) |
| **Priorität** | Hoch |

**Eingabe:** `PlayerRole.Rabbit`

**Erwartetes Ergebnis:**
- Neuer GameState wird erstellt
- `PlayerRole` = Rabbit
- `IsPlayerTurn` = true (Hase beginnt)
- Hase auf Position (0, 4)
- 4 Kinder auf Positionen (9, 1), (9, 3), (9, 5), (9, 7)
- `Status` = Playing
- `MoveCount` = 0

---

### KT-GS-002: Neues Spiel initialisieren (Kinder)

| Attribut | Wert |
|----------|------|
| **ID** | KT-GS-002 |
| **Klasse** | GameService |
| **Methode** | CreateNewGame(PlayerRole) |
| **Priorität** | Hoch |

**Eingabe:** `PlayerRole.Children`

**Erwartetes Ergebnis:**
- Neuer GameState wird erstellt
- `PlayerRole` = Children
- `IsPlayerTurn` = false (Hase/KI beginnt)
- Startpositionen wie KT-GS-001

---

### KT-GS-003: Gültige Hase-Bewegung (alle Richtungen)

| Attribut | Wert |
|----------|------|
| **ID** | KT-GS-003 |
| **Klasse** | GameService |
| **Methode** | ValidateMove(GameState, Move) |
| **Priorität** | Hoch |

**Testfälle:**

| Subtest | Von | Nach | Erwartet |
|---------|-----|------|----------|
| a) Unten-Rechts | (0,4) | (1,5) | Gültig |
| b) Unten-Links | (0,4) | (1,3) | Gültig |
| c) Oben-Rechts | (5,4) | (4,5) | Gültig |
| d) Oben-Links | (5,4) | (4,3) | Gültig |

---

### KT-GS-004: Gültige Kind-Bewegung

| Attribut | Wert |
|----------|------|
| **ID** | KT-GS-004 |
| **Klasse** | GameService |
| **Methode** | ValidateMove(GameState, Move) |
| **Priorität** | Hoch |

**Testfälle:**

| Subtest | Von | Nach | Erwartet |
|---------|-----|------|----------|
| a) Oben-Rechts | (9,1) | (8,2) | Gültig |
| b) Oben-Links | (9,3) | (8,2) | Gültig |
| c) Unten (verboten) | (8,2) | (9,3) | Ungültig |

---

### KT-GS-005: Bewegung außerhalb Spielfeld

| Attribut | Wert |
|----------|------|
| **ID** | KT-GS-005 |
| **Klasse** | GameService |
| **Methode** | ValidateMove(GameState, Move) |
| **Priorität** | Hoch |

**Testfälle:**

| Subtest | Von | Nach | Erwartet |
|---------|-----|------|----------|
| a) Über oberen Rand | (0,4) | (-1,5) | Ungültig |
| b) Unter unteren Rand | (9,4) | (10,5) | Ungültig |
| c) Links außerhalb | (5,0) | (4,-1) | Ungültig |
| d) Rechts außerhalb | (5,9) | (4,10) | Ungültig |

---

### KT-GS-006: Bewegung auf besetztes Feld

| Attribut | Wert |
|----------|------|
| **ID** | KT-GS-006 |
| **Klasse** | GameService |
| **Methode** | ValidateMove(GameState, Move) |
| **Priorität** | Hoch |

**Setup:** Kind auf Position (8,2)

**Eingabe:** Hase von (7,1) nach (8,2)

**Erwartetes Ergebnis:** Move ist ungültig, Fehlermeldung "Feld ist besetzt"

---

### KT-GS-007: Siegbedingung Hase

| Attribut | Wert |
|----------|------|
| **ID** | KT-GS-007 |
| **Klasse** | GameService |
| **Methode** | CheckGameStatus(GameState) |
| **Priorität** | Hoch |

**Setup:** Hase auf Position (8,4)

**Aktion:** Bewege Hase nach (9,5)

**Erwartetes Ergebnis:**
- `Status` = RabbitWins
- Spielzustand wird als beendet markiert

---

### KT-GS-008: Siegbedingung Kinder

| Attribut | Wert |
|----------|------|
| **ID** | KT-GS-008 |
| **Klasse** | GameService |
| **Methode** | CheckGameStatus(GameState) |
| **Priorität** | Hoch |

**Setup:**
- Hase auf (5,4)
- Kinder blockieren alle 4 diagonalen Felder

**Erwartetes Ergebnis:**
- `Status` = ChildrenWin
- Spielzustand wird als beendet markiert

---

### KT-GS-009: Zugwechsel

| Attribut | Wert |
|----------|------|
| **ID** | KT-GS-009 |
| **Klasse** | GameService |
| **Methode** | ApplyMove(GameState, Move) |
| **Priorität** | Mittel |

**Setup:** `IsPlayerTurn` = true

**Aktion:** Gültiger Zug ausführen

**Erwartetes Ergebnis:**
- `IsPlayerTurn` wechselt zu false
- `MoveCount` erhöht sich um 1

---

## 3. Testfälle AIService

### KT-AI-001: KI berechnet Zug als Hase

| Attribut | Wert |
|----------|------|
| **ID** | KT-AI-001 |
| **Klasse** | AIService |
| **Methode** | CalculateMove(GameState) |
| **Priorität** | Hoch |

**Setup:** KI spielt als Hase, KI am Zug

**Erwartetes Ergebnis:**
- Gültiger Move wird zurückgegeben
- Move bewegt den Hasen diagonal
- Move führt nicht auf besetztes Feld

---

### KT-AI-002: KI berechnet Zug als Kinder

| Attribut | Wert |
|----------|------|
| **ID** | KT-AI-002 |
| **Klasse** | AIService |
| **Methode** | CalculateMove(GameState) |
| **Priorität** | Hoch |

**Setup:** KI spielt als Kinder, KI am Zug

**Erwartetes Ergebnis:**
- Gültiger Move wird zurückgegeben
- Move bewegt ein Kind diagonal nach oben
- Move führt nicht auf besetztes Feld

---

### KT-AI-003: KI-Berechnung Zeitlimit

| Attribut | Wert |
|----------|------|
| **ID** | KT-AI-003 |
| **Klasse** | AIService |
| **Methode** | CalculateMove(GameState) |
| **Priorität** | Hoch |

**Setup:** Verschiedene Spielzustände

**Aktion:** Messe Berechnungszeit für 20 verschiedene Zustände

**Erwartetes Ergebnis:**
- Jede Berechnung < 1000ms
- Durchschnitt < 500ms

---

### KT-AI-004: KI wählt Siegzug

| Attribut | Wert |
|----------|------|
| **ID** | KT-AI-004 |
| **Klasse** | AIService |
| **Methode** | CalculateMove(GameState) |
| **Priorität** | Hoch |

**Setup (Hase):** Hase auf (8,4), Weg zu (9,5) frei

**Erwartetes Ergebnis:** KI wählt Zug nach (9,5) (Siegzug)

**Setup (Kinder):** Hase auf (5,4) mit nur einem freien Feld

**Erwartetes Ergebnis:** KI blockiert das letzte freie Feld

---

### KT-AI-005: Alpha-Beta Pruning Effizienz

| Attribut | Wert |
|----------|------|
| **ID** | KT-AI-005 |
| **Klasse** | AIService |
| **Methode** | Minimax mit Alpha-Beta |
| **Priorität** | Mittel |

**Setup:** Komplexer Spielzustand in Spielmitte

**Messung:**
- Anzahl evaluierter Knoten mit Alpha-Beta
- Anzahl evaluierter Knoten ohne Alpha-Beta (Referenz)

**Erwartetes Ergebnis:**
- Alpha-Beta evaluiert < 50% der Knoten im Vergleich zu reinem Minimax

---

## 4. Testfälle Position

### KT-POS-001: Position Validierung

| Attribut | Wert |
|----------|------|
| **ID** | KT-POS-001 |
| **Klasse** | Position |
| **Methode** | IsValid() |
| **Priorität** | Mittel |

**Testfälle:**

| Eingabe | Erwartet |
|---------|----------|
| (0, 0) | true |
| (9, 9) | true |
| (5, 5) | true |
| (-1, 0) | false |
| (0, -1) | false |
| (10, 0) | false |
| (0, 10) | false |

---

### KT-POS-002: Position Gleichheit

| Attribut | Wert |
|----------|------|
| **ID** | KT-POS-002 |
| **Klasse** | Position |
| **Methode** | Equals(), GetHashCode() |
| **Priorität** | Mittel |

**Testfälle:**
- (5,5) == (5,5) → true
- (5,5) == (5,6) → false
- (5,5).GetHashCode() == (5,5).GetHashCode() → true

---

## 5. Testfälle GameState

### KT-STATE-001: GameState Clone

| Attribut | Wert |
|----------|------|
| **ID** | KT-STATE-001 |
| **Klasse** | GameState |
| **Methode** | Clone() |
| **Priorität** | Mittel |

**Aktion:**
1. Erstelle GameState
2. Clone erstellen
3. Original modifizieren

**Erwartetes Ergebnis:**
- Clone bleibt unverändert
- Tiefe Kopie aller Properties
- ChildrenPositions-Liste ist unabhängig

---

## 6. Testfälle LeaderboardRepository

### KT-LB-001: Eintrag speichern

| Attribut | Wert |
|----------|------|
| **ID** | KT-LB-001 |
| **Klasse** | SqliteLeaderboardRepository |
| **Methode** | AddEntryAsync(LeaderboardEntry) |
| **Priorität** | Hoch |

**Eingabe:** Gültiger LeaderboardEntry

**Erwartetes Ergebnis:**
- Eintrag wird in Datenbank gespeichert
- ID wird zurückgegeben

---

### KT-LB-002: Top 20 abrufen

| Attribut | Wert |
|----------|------|
| **ID** | KT-LB-002 |
| **Klasse** | SqliteLeaderboardRepository |
| **Methode** | GetTopEntriesAsync(PlayerRole, int) |
| **Priorität** | Hoch |

**Setup:** 25 Einträge in Datenbank

**Erwartetes Ergebnis:**
- Genau 20 Einträge zurückgegeben
- Sortiert nach AiThinkingTimeMs aufsteigend

---

### KT-LB-003: Rang berechnen

| Attribut | Wert |
|----------|------|
| **ID** | KT-LB-003 |
| **Klasse** | SqliteLeaderboardRepository |
| **Methode** | GetRankAsync(string, PlayerRole) |
| **Priorität** | Mittel |

**Setup:** 10 Einträge mit bekannten Zeiten

**Erwartetes Ergebnis:** Korrekter Rang basierend auf Zeit

---

## 7. Code Coverage Ziele

| Modul | Ziel | Minimum |
|-------|------|---------|
| GameService | 90% | 85% |
| AIService | 85% | 80% |
| Position | 100% | 95% |
| GameState | 80% | 75% |
| SqliteLeaderboardRepository | 80% | 75% |
| **Gesamt** | **85%** | **80%** |

---

## 8. Änderungshistorie

| Version | Datum | Autor | Änderung |
|---------|-------|-------|----------|
| 1.0 | 2025-03-21 | Entwicklungsteam | Initiale Version |
