# Code Coverage Report
## Catch The Rabbit - Testabdeckung

**Erstellungsdatum:** 2026-01-13
**Testframework:** xUnit 2.9.2 + Coverlet 6.0.2
**Report-Tool:** ReportGenerator 5.5.1

---

## 1. Zusammenfassung

| Metrik | Wert |
|--------|------|
| **Line Coverage** | 83.8% |
| **Branch Coverage** | 73.1% |
| **Method Coverage** | 89.6% |
| **Full Method Coverage** | 79.3% |

### Detaillierte Statistik

| Kategorie | Abgedeckt | Gesamt | Prozent |
|-----------|-----------|--------|---------|
| Lines | 586 | 699 | 83.8% |
| Branches | 139 | 190 | 73.1% |
| Methods | 113 | 126 | 89.6% |
| Fully Covered Methods | 100 | 126 | 79.3% |

---

## 2. Coverage nach Assembly

| Assembly | Line Coverage | Details |
|----------|---------------|---------|
| **CatchTheRabbit.Core** | 88.5% | Kernlogik |
| **CatchTheRabbit.Api** | 73.5% | REST API |
| **CatchTheRabbit.Infrastructure** | 100% | Datenzugriff |

---

## 3. Detaillierte Coverage nach Klasse

### 3.1 CatchTheRabbit.Core (88.5%)

| Klasse | Coverage | Status |
|--------|----------|--------|
| GameService | 95.4% | Sehr gut |
| AIService | 81.8% | Gut |
| GameState | 81.5% | Gut |
| LeaderboardService | 90.4% | Sehr gut |
| Position | 100% | Vollständig |
| Move | 100% | Vollständig |
| ValidationResult | 100% | Vollständig |
| LeaderboardEntry | 100% | Vollständig |

### 3.2 CatchTheRabbit.Api (73.5%)

| Klasse | Coverage | Status |
|--------|----------|--------|
| GameController | 72% | Gut |
| LeaderboardController | 78.7% | Gut |
| Program | 100% | Vollständig |
| GameStateResponse | 100% | Vollständig |
| CreateGameRequest | 100% | Vollständig |
| MakeMoveRequest | 100% | Vollständig |
| PositionDto | 100% | Vollständig |
| LeaderboardResponse | 100% | Vollständig |
| AddEntryResponse | 100% | Vollständig |
| AddLeaderboardEntryRequest | 100% | Vollständig |
| MoveResponse | 100% | Vollständig |
| LeaderboardEntryDto | 90% | Gut |
| MoveDto | 0% | Nicht getestet |
| GameHub | 0% | Nicht getestet |

### 3.3 CatchTheRabbit.Infrastructure (100%)

| Klasse | Coverage | Status |
|--------|----------|--------|
| SqliteLeaderboardRepository | 100% | Vollständig |

---

## 4. Nicht abgedeckte Bereiche

### 4.1 Klassen ohne Coverage

| Klasse | Grund |
|--------|-------|
| GameHub | SignalR Hub - erfordert Echtzeit-Tests |
| MoveDto | Nur in AI-Response verwendet |

### 4.2 Branches ohne Coverage

Die fehlenden Branches befinden sich hauptsächlich in:
- Exception-Handling Pfaden
- Edge Cases in der KI-Logik
- Timeout-Szenarien

---

## 5. Coverage-Trend

```
┌─────────────────────────────────────────────────┐
│  Line Coverage: 83.8%                           │
│  ██████████████████████████████████░░░░░░░░░░░  │
│                                                 │
│  Branch Coverage: 73.1%                         │
│  █████████████████████████████░░░░░░░░░░░░░░░░  │
│                                                 │
│  Method Coverage: 89.6%                         │
│  ████████████████████████████████████░░░░░░░░░  │
└─────────────────────────────────────────────────┘
```

---

## 6. Empfehlungen

### 6.1 Hohe Priorität
- [ ] SignalR GameHub Tests implementieren
- [ ] MoveDto Constructor testen

### 6.2 Mittlere Priorität
- [ ] Exception-Handling Pfade testen
- [ ] KI-Timeout Szenarien testen
- [ ] Edge Cases im AIService abdecken

### 6.3 Niedrige Priorität
- [ ] Alle DTO Konstruktoren vollständig testen
- [ ] Negative Testfälle erweitern

---

## 7. Qualitätsziele

| Ziel | Aktuell | Status |
|------|---------|--------|
| Line Coverage > 80% | 83.8% | Erreicht |
| Branch Coverage > 70% | 73.1% | Erreicht |
| Method Coverage > 85% | 89.6% | Erreicht |
| Core Logic > 90% | 88.5% | Fast erreicht |
| Infrastructure = 100% | 100% | Erreicht |

---

## 8. Fazit

Die Code Coverage des Projekts liegt mit **83.8% Line Coverage** und **73.1% Branch Coverage** im guten Bereich. Die kritischen Komponenten (Core Logic und Infrastructure) sind mit 88.5% bzw. 100% sehr gut abgedeckt.

Hauptsächlich fehlen Tests für:
- SignalR WebSocket-Kommunikation (GameHub)
- Einige Edge Cases in der KI-Logik

Diese Bereiche sind für die Grundfunktionalität nicht kritisch, sollten aber für Produktionsreife noch getestet werden.

---

**Report generiert mit:** Coverlet + ReportGenerator
**HTML-Report:** `05_Test/coverage/index.html`
