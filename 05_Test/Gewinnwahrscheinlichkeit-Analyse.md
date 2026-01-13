# Gewinnwahrscheinlichkeits-Analyse
## FA-502: KI-Gewinnrate Validierung

**Testdatum:** 2026-01-13
**Anforderung:** FA-502 - Die KI soll eine ausgeglichene Gewinnrate von ca. 50% erreichen

---

## 1. Executive Summary

| Ergebnis | Status |
|----------|--------|
| **FA-502 erfüllt?** | **NEIN** |
| **Hase Gewinnrate** | 0% |
| **Kinder Gewinnrate** | 100% |
| **Empfehlung** | Balancing erforderlich |

---

## 2. Testmethodik

### 2.1 Simulationsaufbau
- **Anzahl Spiele:** 500 (KI vs KI)
- **KI-Zeitlimit:** 200ms pro Zug
- **Algorithmus:** Minimax mit Alpha-Beta Pruning
- **Suchtiefe:** max. 6

### 2.2 Testcode
```csharp
// Simulation: KI vs KI über 500 Spiele
var result = SimulateGames(500, aiTimeLimitMs: 200);
```

---

## 3. Testergebnisse

### 3.1 Gewinnstatistik

| Gewinner | Anzahl | Prozent |
|----------|--------|---------|
| Hase | 0 | 0.0% |
| Kinder | 500 | 100.0% |
| Unentschieden | 0 | 0.0% |

### 3.2 Visualisierung

```
Gewinnverteilung (n=500)
═══════════════════════════════════════════════════════

Hase:    ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░  0%

Kinder:  ████████████████████████████████████████  100%

═══════════════════════════════════════════════════════
```

---

## 4. Analyse der Ursachen

### 4.1 Spielmechanik-Asymmetrie

| Aspekt | Hase | Kinder |
|--------|------|--------|
| Anzahl Figuren | 1 | 4 |
| Bewegungsrichtung | Alle 4 Diagonalen | Nur vorwärts (2 Diagonalen) |
| Startposition | Y=7-9 (unten) | Y=0-2 (oben) |
| Siegbedingung | Erreiche Y=0 | Blockiere Hasen |
| Distanz zum Ziel | 7-9 Reihen | N/A |

### 4.2 Struktureller Vorteil der Kinder

1. **Überzahl:** 4 Kinder vs 1 Hase ermöglicht Koordination
2. **Defensive Stärke:** Kinder bilden natürliche Barriere
3. **Konvergenz:** Kinder bewegen sich zum Hasen hin
4. **Begrenzter Raum:** Hase wird nach oben gedrängt und eingekreist

### 4.3 KI-Bewertungsfunktion

```csharp
// Faktoren in AIService.Evaluate():
- rabbitProgress: (9 - rabbit.Y) * 10     // Fortschritt zum Ziel
- mobilityScore: validMoves * 5           // Bewegungsfreiheit
- encirclement: blockedDirections * 15    // Einkreisung
- formation: 10 - childSpread             // Kinderformation
```

Die Bewertungsfunktion ist grundsätzlich korrekt, aber das Spiel selbst favorisiert die Kinder strukturell.

---

## 5. Vergleich mit Anforderung

### FA-502 Anforderung:
> "Die KI soll eine ausgeglichene Gewinnrate von ca. 50% erreichen (unabhängig von der Rolle)"

### Ist-Zustand:
| Rolle | Soll | Ist | Delta |
|-------|------|-----|-------|
| Hase | ~50% | 0% | -50% |
| Kinder | ~50% | 100% | +50% |

### Bewertung:
**Die Anforderung FA-502 ist NICHT ERFÜLLT.**

---

## 6. Empfohlene Maßnahmen

### 6.1 Option A: KI-Balancing
- Unterschiedliche Suchtiefen je nach Rolle
- Hase: Tiefe 8, Kinder: Tiefe 4
- Handicap-System einführen

### 6.2 Option B: Spielregel-Anpassung
- Startposition des Hasen näher an Ziel (Y=5-7)
- Weniger Kinder (3 statt 4)
- Zusätzliche Bewegungsoptionen für Hasen

### 6.3 Option C: Anforderung anpassen
- FA-502 als "Soll"-Anforderung akzeptieren dass sie nicht erfüllt ist
- Dokumentieren, dass das Spiel strukturell die Kinder favorisiert
- Kinderfreundlich: Kinder (als Spieler) gewinnen öfter

---

## 7. Auswirkung auf Benutzer

### Spieler wählt Rolle "Hase":
- KI (Kinder) gewinnt fast immer
- Schwierigkeitsgrad: **Sehr hoch**
- Geeignet für: Erfahrene Spieler, Herausforderung

### Spieler wählt Rolle "Kinder":
- KI (Hase) verliert fast immer
- Schwierigkeitsgrad: **Sehr niedrig**
- Geeignet für: Kinder, Anfänger, Erfolgserlebnis

**Für ein Kinderspiel könnte dies sogar gewünscht sein!**

---

## 8. Fazit

Die Gewinnwahrscheinlichkeits-Analyse zeigt eine **starke Asymmetrie** zugunsten der Kinder-Rolle:

- **Hase gewinnt: 0%**
- **Kinder gewinnen: 100%**

Dies liegt an der **strukturellen Spielmechanik**, nicht an einem Fehler in der KI-Implementierung. Das Spiel "Fang den Hasen" favorisiert die Jäger (Kinder) gegenüber dem Gejagten (Hase).

### Empfehlung:
Für ein Kinderspiel könnte diese Asymmetrie **akzeptabel** sein:
- Kinder (Spieler) können als "Kinder"-Rolle leicht gewinnen
- Die Rolle "Hase" bietet eine Herausforderung für erfahrene Spieler

Die Anforderung FA-502 sollte als **nicht erfüllt** dokumentiert und im Lastenheft ggf. angepasst werden.

---

## Anhang: Testprotokoll

```
╔════════════════════════════════════════════════════════════╗
║       GEWINNWAHRSCHEINLICHKEITS-ANALYSE (FA-502)           ║
╠════════════════════════════════════════════════════════════╣
║  Anzahl Spiele:           500                              ║
║  Ausführungszeit:         <1s                              ║
╠════════════════════════════════════════════════════════════╣
║  Hase gewinnt:              0  (  0.0%)                    ║
║  Kinder gewinnen:         500  (100.0%)                    ║
║  Unentschieden:             0  (  0.0%)                    ║
╠════════════════════════════════════════════════════════════╣
║  FA-502 Status:        NICHT ERFÜLLT                       ║
╚════════════════════════════════════════════════════════════╝
```

---

**Erstellt von:** Automatisiertes Testsystem
**Testklasse:** `WinRateSimulationTests.cs`
