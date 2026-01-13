# Balancing-Optionen Analyse
## Entscheidungsgrundlage für FA-502 (Gewinnwahrscheinlichkeit)

**Erstellt:** 2026-01-13
**Version:** 2.0
**Status:** Entscheidung ausstehend

---

## Executive Summary

### Ausgangslage

| Anforderung | Status |
|-------------|--------|
| **FA-502:** KI soll ~50% Gewinnrate erreichen | **NICHT ERFÜLLT** |

### Aktueller Stand (5 Kinder)

| Mensch spielt als | KI-Gewinnrate | Problem |
|-------------------|---------------|---------|
| Kinder | ~10-20% | KI-Hase verliert fast immer |
| Hase | ~50-70% | Ausgeglichen |

### Das Dilemma

| Konfiguration | Ergebnis |
|---------------|----------|
| 4 Kinder | Hase gewinnt ~95% (Lücke in Barriere) |
| 5 Kinder | Kinder gewinnen ~85% (perfekte Barriere möglich) |

**Kernproblem:** Es gibt keine Kinderanzahl, die automatisch 50/50 ergibt.

### Optionen-Übersicht

| Option | Beschreibung | Aufwand | Empfehlung |
|--------|--------------|---------|------------|
| **A** | KI-Handicap (rollenbasierte Stärke) | Mittel | Technisch möglich |
| **B** | Spielregel-Änderung | Hoch | Bereits teilweise umgesetzt |
| **C** | Anforderung anpassen/akzeptieren | Niedrig | Pragmatisch |
| **D** | Schwierigkeitsgrade einführen | Mittel-Hoch | Für V2.0 |

---

## 1. Problemanalyse

### 1.1 Warum ist das Spiel unausgeglichen?

```
┌─────────────────────────────────────────────────────────┐
│                    SPIELASYMMETRIE                       │
├─────────────────────────────────────────────────────────┤
│                                                         │
│   HASE (Flüchtender)          KINDER (Verfolger)       │
│   ─────────────────          ──────────────────        │
│   • 1 Figur                   • 5 Figuren              │
│   • 4 Bewegungsrichtungen     • 2 Bewegungsrichtungen  │
│   • Muss Ziel erreichen       • Muss nur blockieren    │
│   • Jeder Fehler = Verlust    • Fehler korrigierbar    │
│                                                         │
│   Schwarze Felder pro Reihe: 5                         │
│   → 5 Kinder = perfekte Blockade möglich               │
│                                                         │
└─────────────────────────────────────────────────────────┘
```

### 1.2 Mensch vs KI Unterschied

| Spieler-Typ | Kinder-Strategie | Ergebnis |
|-------------|------------------|----------|
| **KI** | Suboptimale Barriere (begrenzte Suchtiefe) | Hase findet Lücken |
| **Mensch** | Intuitive perfekte Barriere | Hase hat keine Chance |

**Schlussfolgerung:** Die KI-Kinder sind "dümmer" als menschliche Kinder-Spieler. Daher ist KI vs KI ausgeglichen (45/55), aber Mensch vs KI nicht.

### 1.3 Mathematische Betrachtung

- Spielfeld: 10×10, davon 50 schwarze Felder
- Schwarze Felder pro Reihe: **5**
- Kinder: **5**
- → Kinder können **jede Reihe vollständig blockieren**

Bei optimaler Kinder-Strategie ist der Hase **mathematisch chancenlos**.

---

## 2. Option A: KI-Handicap System

### 2.1 Beschreibung

Die KI-Stärke wird je nach Rolle angepasst:

| Wenn Mensch spielt als | KI spielt als | KI-Anpassung |
|------------------------|---------------|--------------|
| Kinder | Hase | **Stärker** (Tiefe 8-10) |
| Hase | Kinder | **Schwächer** (Tiefe 3-4) |

### 2.2 Implementierung

```csharp
int GetSearchDepth(PlayerRole humanRole)
{
    return humanRole switch
    {
        PlayerRole.Children => 10,  // KI-Hase stärker
        PlayerRole.Rabbit => 4,     // KI-Kinder schwächer
        _ => 6
    };
}
```

### 2.3 Vor- und Nachteile

| Vorteile | Nachteile |
|----------|-----------|
| Keine Regeländerung | KI spielt "künstlich" |
| Schnell implementierbar (~2-4 Std) | Schwer zu kalibrieren |
| Einstellbar | Spieler bemerken evtl. "dumme" KI-Züge |

### 2.4 Erwartete Ergebnisse

| Konfiguration | Erwartete KI-Gewinnrate |
|---------------|-------------------------|
| KI-Hase Tiefe 6 (aktuell) | ~10-20% |
| KI-Hase Tiefe 8 | ~25-35% (geschätzt) |
| KI-Hase Tiefe 10 | ~35-45% (geschätzt) |
| KI-Hase Tiefe 12+ | ~45-55% (geschätzt) |

**Hinweis:** Höhere Suchtiefe = längere Denkzeit. Muss getestet werden.

### 2.5 Bewertung

| Kriterium | Bewertung |
|-----------|-----------|
| Aufwand | Mittel (2-4 Std + Kalibrierung) |
| Risiko | Mittel (unnatürliches Spielgefühl) |
| Effektivität | Unsicher (muss empirisch getestet werden) |

---

## 3. Option B: Spielregel-Änderungen

### 3.1 Bereits umgesetzt: 5 Kinder

| Vorher | Nachher |
|--------|---------|
| 4 Kinder | 5 Kinder |
| Hase gewinnt ~95% | Kinder gewinnen ~85% |

**Ergebnis:** Problem wurde nicht gelöst, nur umgekehrt.

### 3.2 Weitere mögliche Regeländerungen

| Änderung | Effekt | Bewertung |
|----------|--------|-----------|
| Hase startet weiter oben (Y=5) | Kürzerer Weg zum Ziel | Verändert Spielcharakter |
| Hase kann auch gerade ziehen | Mehr Mobilität | Komplett anderes Spiel |
| Kinder können rückwärts | Stärkt Kinder noch mehr | Kontraproduktiv |
| Zurück auf 4 Kinder | Hase gewinnt wieder ~95% | Löst Problem nicht |

### 3.3 Bewertung

| Kriterium | Bewertung |
|-----------|-----------|
| Aufwand | Hoch (Code + Doku + Tests) |
| Risiko | Hoch (Spielcharakter verändert) |
| Effektivität | Ungewiss |

**Empfehlung:** Keine weiteren Regeländerungen.

---

## 4. Option C: Anforderung anpassen

### 4.1 Beschreibung

FA-502 wird als "nicht erfüllbar" dokumentiert oder umformuliert.

### 4.2 Varianten

#### C1: Anforderung streichen
```markdown
FA-502: ENTFERNT
Begründung: Spieltheoretisch nicht erreichbar ohne Spielverfälschung
```

#### C2: Anforderung abschwächen
```markdown
FA-502 (NEU): Die Rollen bieten unterschiedliche Schwierigkeitsgrade:
- Rolle "Kinder": Leicht (Spieler gewinnt >80%)
- Rolle "Hase": Schwer (Spieler gewinnt ~40%)
```

#### C3: Asymmetrie als Feature dokumentieren
```markdown
FA-502 (NEU): Das Spiel bietet zwei Erlebnismodi:
- "Kinder" für Anfänger und jüngere Spieler (leichter Sieg)
- "Hase" für Fortgeschrittene (Herausforderung)
```

### 4.3 Pädagogische Argumentation

Für ein **Kinderspiel** ist die Asymmetrie möglicherweise **gewünscht**:

| Rolle | Zielgruppe | Spielerlebnis |
|-------|------------|---------------|
| Kinder | Anfänger, jüngere Kinder | Erfolgserlebnis, Motivation |
| Hase | Fortgeschrittene, Erwachsene | Herausforderung |

**Lerneffekt:** "Zusammenarbeit (5 Kinder) ist stärker als Einzelkämpfer (Hase)"

### 4.4 Bewertung

| Kriterium | Bewertung |
|-----------|-----------|
| Aufwand | Niedrig (nur Dokumentation) |
| Risiko | Niedrig |
| Effektivität | Hoch (Problem wird akzeptiert statt gelöst) |

---

## 5. Option D: Schwierigkeitsgrade (V2.0)

### 5.1 Beschreibung

Spieler wählt vor Spielbeginn einen Schwierigkeitsgrad.

### 5.2 Konzept

```
┌─────────────────────────────────────────────────────────┐
│              SCHWIERIGKEITSAUSWAHL                       │
├─────────────────────────────────────────────────────────┤
│                                                         │
│   [Leicht]        [Mittel]        [Schwer]             │
│                                                         │
│   Rolle: Kinder   Rolle: Kinder   Rolle: Hase          │
│   KI-Hase: Tiefe 4 KI-Hase: Tiefe 8 KI-Kinder: Tiefe 6 │
│                                                         │
│   Gewinnrate:     Gewinnrate:     Gewinnrate:          │
│   ~90%            ~50%            ~40%                  │
│                                                         │
└─────────────────────────────────────────────────────────┘
```

### 5.3 Implementierung

```csharp
record DifficultyConfig(PlayerRole Role, int AiDepth);

DifficultyConfig GetConfig(Difficulty diff) => diff switch
{
    Difficulty.Easy   => new(PlayerRole.Children, aiDepth: 4),
    Difficulty.Medium => new(PlayerRole.Children, aiDepth: 8),
    Difficulty.Hard   => new(PlayerRole.Rabbit, aiDepth: 6),
    _ => throw new ArgumentException()
};
```

### 5.4 Vor- und Nachteile

| Vorteile | Nachteile |
|----------|-----------|
| Alle Spielertypen bedient | Höherer Implementierungsaufwand |
| Transparente Schwierigkeitsauswahl | UI-Anpassungen nötig |
| Progression möglich | Für V1.0 zu aufwändig |

### 5.5 Bewertung

| Kriterium | Bewertung |
|-----------|-----------|
| Aufwand | Mittel-Hoch (8-16 Std) |
| Risiko | Niedrig |
| Effektivität | Hoch |

**Empfehlung:** Für Version 2.0 vormerken.

---

## 6. Entscheidungsmatrix

### 6.1 Bewertungskriterien

| Kriterium | Gewichtung |
|-----------|------------|
| Aufwand | 25% |
| Risiko | 25% |
| Effektivität | 25% |
| Spielqualität | 25% |

### 6.2 Bewertung (1-5, höher = besser)

| Option | Aufwand | Risiko | Effektivität | Spielqualität | **Gesamt** |
|--------|---------|--------|--------------|---------------|------------|
| A: KI-Handicap | 3 | 3 | 3 | 3 | **3.00** |
| B: Spielregeln | 2 | 2 | 2 | 2 | **2.00** |
| **C: Anforderung** | **5** | **5** | **4** | **4** | **4.50** |
| D: Schwierigkeitsgrade | 2 | 4 | 5 | 5 | **4.00** |

### 6.3 Visualisierung

```
                    Effektivität
                         ▲
                    Hoch │    [D]        [C] ← Höchste Bewertung
                         │
                         │         [A]
                         │
                  Niedrig│    [B]
                         └──────────────────► Aufwand
                              Hoch    Niedrig
```

---

## 7. Empfehlungen

### 7.1 Für V1.0: Option C (Anforderung anpassen)

**Begründung:**
1. Geringster Aufwand
2. Kein Risiko für bestehende Funktionalität
3. Asymmetrie ist für Kinderspiel akzeptabel/wünschenswert
4. Ehrliche Dokumentation der Spieleigenschaften

**Umsetzung:**
- FA-502 im Lastenheft umformulieren
- Rollenauswahl mit Schwierigkeitshinweis versehen
- Spielanleitung entsprechend anpassen

### 7.2 Für V2.0: Option D (Schwierigkeitsgrade)

**Begründung:**
1. Beste langfristige Lösung
2. Bedient alle Spielertypen
3. Kann schrittweise implementiert werden

### 7.3 Optional: Option A (KI-Handicap)

Falls 50/50 zwingend erforderlich ist:
- KI-Hase Suchtiefe erhöhen (10-12)
- Empirisch testen und kalibrieren
- Denkzeit-Limit beachten (max 1 Sekunde)

---

## 8. Nächste Schritte

### Bei Entscheidung für Option C:

1. **Lastenheft anpassen** (30 Min)
   - FA-502 umformulieren zu: "Rollen bieten unterschiedliche Schwierigkeitsgrade"

2. **UI-Hinweis hinzufügen** (optional, 1-2 Std)
   - Bei Rollenauswahl: "Leicht" / "Herausfordernd" anzeigen

3. **Dokumentation aktualisieren** (30 Min)
   - Diese Analyse als Entscheidungsgrundlage referenzieren

### Bei Entscheidung für Option A:

1. **AIService anpassen** (2 Std)
   - Rollenbasierte Suchtiefe implementieren

2. **Kalibrierung** (4-8 Std)
   - Verschiedene Tiefen testen
   - Optimale Werte ermitteln

3. **Tests anpassen** (2 Std)
   - Unit Tests für neue Logik

---

## Anhang

### A.1 Historische Entwicklung

| Datum | Änderung | Ergebnis |
|-------|----------|----------|
| Initial | 4 Kinder | Hase gewinnt ~95% |
| 13.01.2026 | 5 Kinder | Kinder gewinnen ~85% |
| Aktuell | - | FA-502 nicht erfüllt |

### A.2 Referenzen

- `05_Test/Gewinnwahrscheinlichkeit-Analyse.md` - Detaillierte Testergebnisse
- `01_Anforderungen/Lastenheft.md` - Aktuelle Anforderungen
- `04_Implementierung/backend/CatchTheRabbit.Core/Services/AIService.cs` - KI-Implementierung

---

**Dokument:** Balancing-Optionen-Analyse
**Version:** 2.0
**Status:** Entscheidung ausstehend
**Erstellt:** 2026-01-13
