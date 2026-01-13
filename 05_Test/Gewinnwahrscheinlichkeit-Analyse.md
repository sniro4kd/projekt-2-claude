# Gewinnwahrscheinlichkeits-Analyse
## FA-502: KI-Gewinnrate Validierung

**Testdatum:** 2026-01-13
**Anforderung:** FA-502 - Die KI soll eine ausgeglichene Gewinnrate von ca. 50% erreichen
**Aktuelle Konfiguration:** 5 Kinder

---

## 1. Executive Summary

| Ergebnis | Status |
|----------|--------|
| **FA-502 erfüllt?** | **NEIN** |
| **KI vs KI Simulation** | Hase 45% / Kinder 55% |
| **Mensch vs KI (Realität)** | Stark unausgeglichen |
| **Handlungsbedarf** | Ja - siehe Optionen |

### Kernproblem

Die Anforderung FA-502 bezieht sich auf das Spielerlebnis **Mensch vs KI**, nicht auf KI vs KI. In der Praxis:

| Mensch spielt als | KI spielt als | KI-Gewinnrate | FA-502 (Soll: ~50%) |
|-------------------|---------------|---------------|---------------------|
| **Kinder** | Hase | ~10-20% | ❌ Nicht erfüllt |
| **Hase** | Kinder | ~50-70% | ✅ Erfüllt |

**Ursache:** Menschen setzen die Kinder-Barriere-Taktik intuitiv besser um als die KI. Die KI-Hase kann gegen einen menschlichen Kinder-Spieler fast nie gewinnen.

---

## 2. Testmethodik

### 2.1 KI vs KI Simulation
- **Anzahl Spiele:** 20
- **KI-Zeitlimit:** 200ms pro Zug
- **Algorithmus:** Minimax mit Alpha-Beta Pruning
- **Suchtiefe:** max. 6

### 2.2 Mensch vs KI Beobachtung
- **Methode:** Manuelle Spieltests
- **Beobachtung:** Menschliche Kinder-Spieler gewinnen fast immer

---

## 3. Testergebnisse

### 3.1 KI vs KI Simulation (5 Kinder)

| Gewinner | Anzahl | Prozent |
|----------|--------|---------|
| Hase | 9 | 45.0% |
| Kinder | 11 | 55.0% |

```
KI vs KI Gewinnverteilung (n=20)
═══════════════════════════════════════════════════════

Hase:    ██████████████████░░░░░░░░░░░░░░░░░░░░░░  45%

Kinder:  ██████████████████████░░░░░░░░░░░░░░░░░░  55%

═══════════════════════════════════════════════════════
```

### 3.2 Mensch vs KI (geschätzt)

| Mensch als | Mensch-Gewinnrate | KI-Gewinnrate |
|------------|-------------------|---------------|
| Kinder | ~80-90% | ~10-20% |
| Hase | ~30-50% | ~50-70% |

```
Mensch vs KI Gewinnverteilung (geschätzt)
═══════════════════════════════════════════════════════

Mensch als Kinder:  ████████████████████████████████░░░░  ~85%
KI als Hase:        ██████░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░  ~15%

Mensch als Hase:    ████████████████░░░░░░░░░░░░░░░░░░░░  ~40%
KI als Kinder:      ████████████████████████░░░░░░░░░░░░  ~60%

═══════════════════════════════════════════════════════
```

---

## 4. Analyse

### 4.1 Warum KI vs KI ≠ Mensch vs KI?

| Aspekt | KI-Kinder | Mensch-Kinder |
|--------|-----------|---------------|
| Barriere-Bildung | Suboptimal (begrenzte Suchtiefe) | Nahezu perfekt (intuitive Strategie) |
| Vorausplanung | 6 Züge (Suchtiefe) | Unbegrenzt (strategisches Denken) |
| Fehler | Gelegentlich Lücken | Sehr selten |

**Ergebnis:** Die KI-Hase kann gegen KI-Kinder gewinnen (45%), weil die KI-Kinder Fehler machen. Gegen menschliche Kinder-Spieler hat die KI-Hase fast keine Chance.

### 4.2 Das Kinder-Anzahl-Dilemma

| Konfiguration | Problem |
|---------------|---------|
| **4 Kinder** | Strukturelle Lücke: 4 Kinder können nie alle 5 schwarzen Felder pro Reihe blockieren → Hase gewinnt ~95% |
| **5 Kinder** | Perfekte Barriere möglich: Menschen nutzen dies aus → Kinder gewinnen ~85% |

Es gibt keine Kinderanzahl, die automatisch 50/50 ergibt.

### 4.3 Spielmechanik-Analyse

| Aspekt | Hase | Kinder (5) |
|--------|------|------------|
| Anzahl Figuren | 1 | 5 |
| Bewegungsrichtung | Alle 4 Diagonalen | Nur 2 (nach unten) |
| Schwarze Felder pro Reihe | - | 5 (= genau deckbar) |
| Siegbedingung | Erreiche obere Reihe | Blockiere alle Hasen-Züge |

---

## 5. Vergleich mit Anforderung

### FA-502 Anforderung:
> "Die KI soll eine ausgeglichene Gewinnrate von ca. 50% erreichen (unabhängig von der Rolle)"

### Interpretation:
Die KI soll gegen menschliche Spieler ~50% Gewinnrate haben, egal ob der Mensch Hase oder Kinder spielt.

### Ist-Zustand:

| Szenario | KI-Gewinnrate | Abweichung von 50% |
|----------|---------------|-------------------|
| KI als Hase | ~10-20% | **-30 bis -40%** |
| KI als Kinder | ~50-70% | 0 bis +20% |

### Bewertung:
**FA-502 ist NICHT ERFÜLLT.**

Die KI als Hase verliert fast immer gegen menschliche Kinder-Spieler.

---

## 6. Handlungsoptionen

Siehe **Balancing-Optionen-Analyse.md** für detaillierte Optionen:

| Option | Kurzbeschreibung | Empfehlung |
|--------|------------------|------------|
| A | KI-Handicap (unterschiedliche Stärken) | Möglich |
| B | Spielregel-Änderung | Bereits umgesetzt (5 Kinder) |
| C | Anforderung anpassen | Pragmatisch |
| D | Schwierigkeitsgrade | Für V2.0 |

---

## 7. Fazit

Die Gewinnwahrscheinlichkeits-Analyse zeigt:

1. **KI vs KI ist ausgeglichen** (45%/55%) - aber das ist nicht das Ziel von FA-502
2. **Mensch vs KI ist unausgeglichen** - KI-Hase verliert fast immer
3. **FA-502 ist NICHT erfüllt** nach der korrekten Interpretation

### Das fundamentale Problem:
- Mit 4 Kindern: Hase zu stark (strukturelle Lücke)
- Mit 5 Kindern: Kinder zu stark (perfekte Barriere möglich)
- Keine Kinderanzahl löst das Problem automatisch

### Nächste Schritte:
Eine Entscheidung über die Handlungsoptionen (A, C oder D) ist erforderlich.

---

**Erstellt von:** Testsystem
**Dokument:** Gewinnwahrscheinlichkeit-Analyse
**Version:** 2.0 (korrigiert)
