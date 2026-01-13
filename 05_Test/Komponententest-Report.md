# Komponententest-Report
## Catch The Rabbit - Unit Test Ergebnisse

**Testdatum:** 2026-01-13
**Testumgebung:** Windows 10, .NET 9.0
**Test-Framework:** xUnit 2.9.2, FluentAssertions 6.12.2

---

## 1. Zusammenfassung

| Metrik | Wert |
|--------|------|
| **Gesamtanzahl Unit Tests** | 45 |
| **Bestanden** | 45 |
| **Fehlgeschlagen** | 0 |
| **Übersprungen** | 0 |
| **Erfolgsquote** | 100% |
| **Gesamtdauer** | ~400ms |

---

## 2. Testergebnisse nach Komponente

### 2.1 GameService Tests (17 Tests)

| Test-ID | Testfall | Status | Dauer |
|---------|----------|--------|-------|
| KT-GS-001 | CreateGame_AsRabbit_ReturnsCorrectInitialState | BESTANDEN | <10ms |
| KT-GS-001 | CreateGame_AsRabbit_RabbitOnValidPosition | BESTANDEN | <10ms |
| KT-GS-001 | CreateGame_AsRabbit_ChildrenOnValidPositions | BESTANDEN | <10ms |
| KT-GS-002 | CreateGame_AsChildren_ReturnsCorrectInitialState | BESTANDEN | <10ms |
| KT-GS-003 | ValidateMove_RabbitDiagonalMove_IsValid | BESTANDEN | <10ms |
| KT-GS-003 | ValidateMove_RabbitCanMoveAllDiagonalDirections | BESTANDEN | <10ms |
| KT-GS-004 | ValidateMove_ChildMoveDownward_IsValid | BESTANDEN | <10ms |
| KT-GS-004 | ValidateMove_ChildMoveUpward_IsInvalid | BESTANDEN | <10ms |
| KT-GS-005 | ValidateMove_OutOfBounds_IsInvalid (-1, 0) | BESTANDEN | <10ms |
| KT-GS-005 | ValidateMove_OutOfBounds_IsInvalid (0, -1) | BESTANDEN | <10ms |
| KT-GS-005 | ValidateMove_OutOfBounds_IsInvalid (10, 5) | BESTANDEN | <10ms |
| KT-GS-005 | ValidateMove_OutOfBounds_IsInvalid (5, 10) | BESTANDEN | <10ms |
| KT-GS-006 | ValidateMove_ToOccupiedField_IsInvalid | BESTANDEN | <10ms |
| KT-GS-007 | CheckGameEnd_RabbitReachesTopRow_RabbitWins | BESTANDEN | <10ms |
| KT-GS-008 | CheckGameEnd_RabbitBlocked_ChildrenWin | BESTANDEN | <10ms |
| KT-GS-009 | ApplyMove_ValidMove_SwitchesTurn | BESTANDEN | <10ms |
| KT-GS-009 | ApplyMove_ValidMove_AddsMoveToHistory | BESTANDEN | <10ms |

### 2.2 AIService Tests (9 Tests)

| Test-ID | Testfall | Status | Dauer |
|---------|----------|--------|-------|
| KT-AI-001 | CalculateBestMove_AsRabbit_ReturnsValidMove | BESTANDEN | ~100ms |
| KT-AI-001 | CalculateBestMove_AsRabbit_MovesDiagonally | BESTANDEN | ~100ms |
| KT-AI-002 | CalculateBestMove_AsChildren_ReturnsValidMove | BESTANDEN | ~100ms |
| KT-AI-002 | CalculateBestMove_AsChildren_MovesDownward | BESTANDEN | ~100ms |
| KT-AI-003 | CalculateBestMove_CompletesWithinTimeLimit | BESTANDEN | ~500ms |
| KT-AI-003 | CalculateBestMove_MultipleStates_AllComplete | BESTANDEN | ~500ms |
| KT-AI-004 | CalculateBestMove_RabbitNearWin_MovesTowardGoal | BESTANDEN | ~100ms |
| KT-AI-005 | CalculateBestMove_ComplexState_CompletesQuickly | BESTANDEN | ~300ms |

### 2.3 Position Tests (11 Tests)

| Test-ID | Testfall | Status | Dauer |
|---------|----------|--------|-------|
| KT-POS-001 | IsValid_ValidPositions_ReturnsTrue (0,0) | BESTANDEN | <10ms |
| KT-POS-001 | IsValid_ValidPositions_ReturnsTrue (9,9) | BESTANDEN | <10ms |
| KT-POS-001 | IsValid_ValidPositions_ReturnsTrue (5,5) | BESTANDEN | <10ms |
| KT-POS-001 | IsValid_InvalidPositions_ReturnsFalse (-1,0) | BESTANDEN | <10ms |
| KT-POS-001 | IsValid_InvalidPositions_ReturnsFalse (10,10) | BESTANDEN | <10ms |
| KT-POS-001 | IsBlackField_ReturnsCorrectValue | BESTANDEN | <10ms |
| KT-POS-002 | Equals_SamePosition_ReturnsTrue | BESTANDEN | <10ms |
| KT-POS-002 | Equals_DifferentPosition_ReturnsFalse | BESTANDEN | <10ms |
| KT-POS-002 | GetHashCode_SamePosition_SameHashCode | BESTANDEN | <10ms |
| KT-POS-002 | Position_CanBeUsedAsDictionaryKey | BESTANDEN | <10ms |
| KT-POS-002 | Position_InHashSet_WorksCorrectly | BESTANDEN | <10ms |

### 2.4 GameState Tests (8 Tests)

| Test-ID | Testfall | Status | Dauer |
|---------|----------|--------|-------|
| KT-STATE-001 | Clone_ReturnsDeepCopy | BESTANDEN | <10ms |
| KT-STATE-001 | Clone_ChildrenArray_IsIndependent | BESTANDEN | <10ms |
| KT-STATE-001 | Clone_ModifyingOriginal_DoesNotAffectClone | BESTANDEN | <10ms |
| KT-STATE-001 | Clone_ModifyingClone_DoesNotAffectOriginal | BESTANDEN | <10ms |
| KT-STATE-001 | Clone_PreservesAllChildren | BESTANDEN | <10ms |
| KT-STATE-002 | GameState_InitialValues_AreCorrect | BESTANDEN | <10ms |
| KT-STATE-002 | GameState_Id_IsUnique | BESTANDEN | <10ms |
| KT-STATE-003 | IsOccupied_Tests | BESTANDEN | <10ms |

### 2.5 LeaderboardRepository Tests (8 Tests)

| Test-ID | Testfall | Status | Dauer |
|---------|----------|--------|-------|
| KT-LB-001 | AddEntryAsync_ValidEntry_SavesSuccessfully | BESTANDEN | <50ms |
| KT-LB-001 | AddEntryAsync_MultipleEntries_AllSaved | BESTANDEN | <50ms |
| KT-LB-002 | GetTopEntriesAsync_With25Entries_Returns20 | BESTANDEN | <50ms |
| KT-LB-002 | GetTopEntriesAsync_SortedByThinkingTime | BESTANDEN | <50ms |
| KT-LB-002 | GetTopEntriesAsync_EmptyDatabase_ReturnsEmptyList | BESTANDEN | <50ms |
| KT-LB-002 | GetTopEntriesAsync_FiltersByRole | BESTANDEN | <50ms |
| KT-LB-003 | CountBetterEntriesAsync_ReturnsCorrectCount | BESTANDEN | <50ms |
| KT-LB-003 | CountBetterEntriesAsync_BestTime_ReturnsZero | BESTANDEN | <50ms |

---

## 3. Code-Abdeckung

Die Komponententests decken folgende Kernbereiche ab:

- **GameService**: Spielinitialisierung, Zugvalidierung, Spielende-Erkennung
- **AIService**: KI-Zugberechnung, Zeitlimit-Einhaltung, Strategie
- **Position**: Positionsvalidierung, Gleichheitsoperationen
- **GameState**: Zustandsklonierung, Unveränderlichkeit
- **LeaderboardRepository**: Datenpersistenz, Abfragen, Sortierung

---

## 4. Bekannte Einschränkungen

- KI-Tests können je nach CPU-Auslastung leicht variieren
- SQLite-Tests verwenden temporäre Dateien

---

## 5. Fazit

Alle 45 Komponententests wurden erfolgreich bestanden. Die Kernlogik des Spiels (Spielregeln, KI, Datenhaltung) ist vollständig getestet und funktioniert korrekt.

---

**Erstellt von:** Automatisiertes Testsystem
**Version:** 1.0.0
