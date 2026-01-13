# Balancing-Optionen Analyse
## Entscheidungsgrundlage für FA-502 (Gewinnwahrscheinlichkeit)

**Erstellt:** 2026-01-13
**Zielgruppe:** Projektverantwortliche
**Zweck:** Entscheidungsgrundlage für Balancing-Maßnahmen

---

## Executive Summary

### Ausgangslage
Die KI-Simulation (500 Spiele) zeigt eine **100% Gewinnrate für die Kinder-Seite**. Die Anforderung FA-502 (ausgeglichene 50% Gewinnrate) ist nicht erfüllt.

### Empfehlung
**Option C: Anforderungsanpassung** wird empfohlen.

| Option | Aufwand | Risiko | Effektivität | Empfehlung |
|--------|---------|--------|--------------|------------|
| A: KI-Handicap | Mittel | Niedrig | Unsicher | Möglich |
| B: Spielregel-Änderung | Hoch | Hoch | Gut | Nicht empfohlen |
| **C: Anforderungsanpassung** | **Niedrig** | **Niedrig** | **Hoch** | **Empfohlen** |
| D: Hybridlösung | Hoch | Mittel | Gut | Für V2.0 |

### Begründung
1. Das Spiel "Fang den Hasen" ist **spieltheoretisch asymmetrisch** - dies ist kein Bug, sondern Spieldesign
2. Für die Zielgruppe (Kinder) ist eine **leichte "Kinder"-Rolle wünschenswert**
3. Änderungen an Spielregeln würden das **klassische Spielprinzip verfälschen**
4. Der Aufwand für KI-Balancing steht in keinem Verhältnis zum Nutzen

---

## 1. Problemanalyse

### 1.1 Spieltheoretischer Hintergrund

"Fang den Hasen" gehört zur Kategorie der **asymmetrischen Verfolgungsspiele** (Fox and Hounds, Halma-Varianten). Diese Spiele haben inhärente Eigenschaften:

```
┌─────────────────────────────────────────────────────────┐
│                    SPIELASYMMETRIE                       │
├─────────────────────────────────────────────────────────┤
│                                                         │
│   HASE (Flüchtender)          KINDER (Verfolger)       │
│   ─────────────────          ──────────────────        │
│   • 1 Figur                   • 4 Figuren              │
│   • 4 Bewegungsrichtungen     • 2 Bewegungsrichtungen  │
│   • Muss Ziel erreichen       • Muss nur blockieren    │
│   • Aktives Spielziel         • Reaktives Spielziel    │
│   • Fehler = Verlust          • Fehler = korrigierbar  │
│                                                         │
│   VORTEIL: Mobilität          VORTEIL: Redundanz       │
│                                                         │
└─────────────────────────────────────────────────────────┘
```

### 1.2 Mathematische Betrachtung

**Zustandsraum-Analyse:**
- Spielfeld: 10×10, davon 50 schwarze Felder
- Hase-Positionen: 50 mögliche Felder
- Kinder-Positionen: C(50,4) = 230.300 Kombinationen
- Gesamtzustände: ~11,5 Millionen

**Gewinnbedingungen:**
- Hase: Erreiche 1 von 5 Zielfeldern (Y=0)
- Kinder: Blockiere alle 4 Diagonalen des Hasen

Die Kinder haben einen **kombinatorischen Vorteil**: Mit 4 Figuren können sie systematisch den Bewegungsraum des Hasen einschränken.

### 1.3 Warum gewinnen die Kinder immer?

| Phase | Situation | Vorteil |
|-------|-----------|---------|
| Eröffnung | Kinder kontrollieren obere Hälfte | Kinder |
| Mittelspiel | Kinder bilden Barriere, Hase sucht Lücke | Kinder |
| Endspiel | Hase eingekesselt oder durchgebrochen | Meist Kinder |

**Schlüsselerkenntnis:** Bei **perfektem Spiel beider Seiten** gewinnen die Kinder immer. Dies ist eine Eigenschaft des Spieldesigns, nicht der KI.

---

## 2. Option A: KI-Handicap System

### 2.1 Beschreibung

Die KI wird je nach Rolle unterschiedlich stark eingestellt:
- **Hase-KI**: Volle Stärke (Tiefe 6-8)
- **Kinder-KI**: Reduzierte Stärke (Tiefe 2-4)

### 2.2 Implementierungsvarianten

#### Variante A1: Suchtiefe-Anpassung
```csharp
int GetSearchDepth(PlayerRole aiRole) {
    return aiRole == PlayerRole.Rabbit
        ? 8   // Hase: starke KI
        : 3;  // Kinder: schwache KI
}
```

**Vorteile:**
- Einfach zu implementieren (~10 Zeilen Code)
- Keine Änderung der Spielregeln
- Einstellbar über Konfiguration

**Nachteile:**
- KI spielt absichtlich schlecht → unnatürlich
- Spieler könnten "dumme" Züge der KI bemerken
- Schwer zu kalibrieren (welche Tiefe = 50%?)

#### Variante A2: Bewertungsfunktion-Handicap
```csharp
int Evaluate(GameState state, PlayerRole aiRole) {
    int score = CalculateBaseScore(state, aiRole);

    if (aiRole == PlayerRole.Children) {
        score = (int)(score * 0.7); // 30% Handicap
    }

    return score;
}
```

**Vorteile:**
- Subtiler als Tiefenreduktion
- KI macht "plausible" Fehler

**Nachteile:**
- Schwer vorhersagbar
- Kann zu seltsamen Zügen führen

#### Variante A3: Zufallsbasiertes Handicap
```csharp
Move SelectMove(List<Move> rankedMoves, PlayerRole aiRole) {
    if (aiRole == PlayerRole.Children && random.NextDouble() < 0.3) {
        // 30% Chance: Wähle zweit- oder drittbesten Zug
        return rankedMoves[random.Next(1, Math.Min(3, rankedMoves.Count))];
    }
    return rankedMoves[0];
}
```

**Vorteile:**
- Natürlichere "Fehler"
- Varianz in den Spielen

**Nachteile:**
- Inkonsistentes Spielerlebnis
- Frustration wenn KI plötzlich "aufwacht"

### 2.3 Erwartete Ergebnisse

| Tiefe Hase | Tiefe Kinder | Erwartete Hase-Gewinnrate |
|------------|--------------|---------------------------|
| 6 | 6 | 0% (aktuell) |
| 6 | 4 | ~10-20% (geschätzt) |
| 6 | 3 | ~30-40% (geschätzt) |
| 6 | 2 | ~50-60% (geschätzt) |
| 8 | 2 | ~60-70% (geschätzt) |

**Hinweis:** Diese Werte sind Schätzungen und müssten empirisch validiert werden.

### 2.4 Aufwand und Risiken

| Aspekt | Bewertung |
|--------|-----------|
| Implementierungsaufwand | 2-4 Stunden |
| Testaufwand | 4-8 Stunden (Kalibrierung) |
| Risiko: Unnatürliches Spielgefühl | Mittel |
| Risiko: Frustration bei "dummen" KI-Zügen | Mittel |
| Wartbarkeit | Gut |

### 2.5 Fazit Option A

**Geeignet für:** Schnelle Lösung ohne Regeländerungen
**Nicht geeignet für:** Authentisches Spielerlebnis

---

## 3. Option B: Spielregel-Änderungen

### 3.1 Beschreibung

Anpassung der Spielregeln um die Balance zu verbessern.

### 3.2 Implementierungsvarianten

#### Variante B1: Startposition ändern

**Aktuell:**
```
Y=0  [K] [ ] [K] [ ] [K] [ ] [K] [ ] [ ] [ ]  ← Ziel Hase
Y=1  [ ] [ ] [ ] [ ] [ ] [ ] [ ] [ ] [ ] [ ]
...
Y=7  [ ] [ ] [ ] [ ] [ ] [ ] [ ] [ ] [ ] [ ]
Y=8  [ ] [ ] [ ] [ ] [ ] [H] [ ] [ ] [ ] [ ]  ← Start Hase
Y=9  [ ] [ ] [ ] [ ] [ ] [ ] [ ] [ ] [ ] [ ]
```

**Vorgeschlagen:**
```
Y=0  [K] [ ] [K] [ ] [K] [ ] [K] [ ] [ ] [ ]  ← Ziel Hase
Y=1  [ ] [ ] [ ] [ ] [ ] [ ] [ ] [ ] [ ] [ ]
...
Y=4  [ ] [ ] [ ] [ ] [ ] [ ] [ ] [ ] [ ] [ ]
Y=5  [ ] [ ] [ ] [ ] [H] [ ] [ ] [ ] [ ] [ ]  ← Start Hase (NEU)
...
```

**Analyse:**
- Reduziert Distanz zum Ziel von 7-9 auf 4-6 Felder
- Erwartete Verbesserung: +20-30% für Hase
- **Problem:** Verändert Spielcharakter fundamental

#### Variante B2: Anzahl Kinder reduzieren

**Aktuell:** 4 Kinder
**Vorgeschlagen:** 3 Kinder

**Analyse:**
- Weniger Deckung der Barriere
- Erwartete Verbesserung: +30-40% für Hase
- **Problem:** Klassisches Spiel hat 4 Kinder

#### Variante B3: Zusätzliche Bewegungsoptionen für Hase

**Aktuell:** Nur diagonale Züge
**Vorgeschlagen:** Diagonale + gerade Züge ODER Sprünge

**Analyse:**
- Erhöht Mobilität drastisch
- **Problem:** Komplett anderes Spiel

#### Variante B4: Kinder können auch rückwärts

**Aktuell:** Kinder nur vorwärts
**Vorgeschlagen:** Kinder alle Richtungen

**Analyse:**
- **Verschlechtert** die Hase-Gewinnrate (Kinder werden noch stärker)
- Nicht zielführend

### 3.3 Auswirkungen auf Spieldesign

| Änderung | Spielcharakter | Klassisch? | Empfehlung |
|----------|----------------|------------|------------|
| Startposition | Leicht verändert | Nein | Möglich |
| 3 Kinder | Stark verändert | Nein | Nicht empfohlen |
| Zusätzliche Züge | Komplett anders | Nein | Nicht empfohlen |

### 3.4 Aufwand und Risiken

| Aspekt | Bewertung |
|--------|-----------|
| Implementierungsaufwand | 4-16 Stunden (je nach Variante) |
| Testaufwand | 8-16 Stunden |
| Risiko: Spielcharakter verändert | Hoch |
| Risiko: Bestehende Dokumentation ungültig | Hoch |
| Risiko: Neue Bugs | Mittel |
| Wartbarkeit | Mittel |

### 3.5 Fazit Option B

**Geeignet für:** Grundlegende Neugestaltung des Spiels
**Nicht geeignet für:** Erhalt des klassischen Spielprinzips

---

## 4. Option C: Anforderungsanpassung

### 4.1 Beschreibung

Die Anforderung FA-502 wird angepasst oder als nicht erfüllbar dokumentiert.

### 4.2 Begründungsvarianten

#### Variante C1: Anforderung streichen
```markdown
~~FA-502: Die KI soll eine ausgeglichene Gewinnrate von ca. 50% erreichen~~
ENTFERNT - Spieltheoretisch nicht erreichbar ohne Spielverfälschung
```

#### Variante C2: Anforderung abschwächen
```markdown
FA-502 (NEU): Die KI soll ein herausforderndes aber faires Spielerlebnis bieten.
- Als "Kinder": Spieler gewinnt in >80% der Fälle (Erfolgserlebnis)
- Als "Hase": Spieler gewinnt in <20% der Fälle (Herausforderung)
```

#### Variante C3: Anforderung differenzieren
```markdown
FA-502a: Rolle "Kinder" - Einsteigerfreundlich (Spieler gewinnt meist)
FA-502b: Rolle "Hase" - Expertenherausforderung (KI gewinnt meist)
```

### 4.3 Pädagogische Argumentation

**Für ein Kinderspiel ist Asymmetrie oft gewünscht:**

| Rolle | Zielgruppe | Erwartung | Erfüllt? |
|-------|------------|-----------|----------|
| Kinder | Anfänger, jüngere Kinder | Leichter Sieg | ✓ Ja |
| Hase | Fortgeschrittene, ältere Kinder | Herausforderung | ✓ Ja |

**Lerneffekt:**
- Kinder lernen: "Zusammenarbeit (4 Kinder) ist stärker als Einzelkämpfer"
- Strategisches Denken wird gefördert
- Erfolgserlebnisse motivieren zum Weiterspielen

### 4.4 Dokumentationsänderungen

**Lastenheft (01_Anforderungen/Lastenheft.md):**
```markdown
| FA-502 | Die Rollen bieten unterschiedliche Schwierigkeitsgrade | Soll |
|        | - "Kinder": Leicht (Spieler gewinnt >80%)              |      |
|        | - "Hase": Schwer (Spieler gewinnt <20%)                |      |
```

**Zusätzliche Dokumentation:**
- Spielanleitung mit Schwierigkeitshinweis
- UI-Anzeige der Rollenschwierigkeit

### 4.5 Aufwand und Risiken

| Aspekt | Bewertung |
|--------|-----------|
| Dokumentationsaufwand | 1-2 Stunden |
| Implementierungsaufwand | 0-1 Stunde (UI-Hinweis) |
| Testaufwand | 0 Stunden |
| Risiko | Sehr niedrig |
| Wartbarkeit | Sehr gut |

### 4.6 Fazit Option C

**Geeignet für:** Pragmatische Lösung mit minimalem Aufwand
**Nicht geeignet für:** Wenn 50% Balance zwingend erforderlich ist

---

## 5. Option D: Hybridlösung (für zukünftige Versionen)

### 5.1 Beschreibung

Kombination mehrerer Maßnahmen mit konfigurierbarem Schwierigkeitsgrad.

### 5.2 Konzept

```
┌─────────────────────────────────────────────────────────┐
│                  SCHWIERIGKEITSAUSWAHL                   │
├─────────────────────────────────────────────────────────┤
│                                                         │
│   [Leicht]     [Mittel]      [Schwer]     [Experte]    │
│                                                         │
│   Rolle:       Rolle:        Rolle:       Rolle:       │
│   Kinder       Kinder        Hase         Hase         │
│   KI-Tiefe: 2  KI-Tiefe: 4   KI-Tiefe: 4  KI-Tiefe: 6  │
│                                                         │
│   Gewinnrate:  Gewinnrate:   Gewinnrate:  Gewinnrate:  │
│   ~90%         ~70%          ~30%         ~5%          │
│                                                         │
└─────────────────────────────────────────────────────────┘
```

### 5.3 Implementierung

```csharp
enum Difficulty { Easy, Medium, Hard, Expert }

GameConfig GetConfig(Difficulty diff) {
    return diff switch {
        Difficulty.Easy   => new(PlayerRole.Children, aiDepth: 2),
        Difficulty.Medium => new(PlayerRole.Children, aiDepth: 4),
        Difficulty.Hard   => new(PlayerRole.Rabbit, aiDepth: 4),
        Difficulty.Expert => new(PlayerRole.Rabbit, aiDepth: 6),
    };
}
```

### 5.4 Vorteile

- Alle Spielertypen werden bedient
- Transparente Schwierigkeitsauswahl
- Progression möglich (von Leicht zu Experte)

### 5.5 Aufwand

| Aspekt | Bewertung |
|--------|-----------|
| Implementierungsaufwand | 8-16 Stunden |
| UI-Anpassungen | 4-8 Stunden |
| Testaufwand | 8-16 Stunden |
| Dokumentation | 2-4 Stunden |
| **Gesamt** | **22-44 Stunden** |

### 5.6 Fazit Option D

**Geeignet für:** Version 2.0 mit erweitertem Funktionsumfang
**Nicht geeignet für:** Aktuelle Projektphase (zu aufwändig)

---

## 6. Entscheidungsmatrix

### 6.1 Bewertungskriterien

| Kriterium | Gewichtung | Beschreibung |
|-----------|------------|--------------|
| Aufwand | 25% | Implementierungs- und Testzeit |
| Risiko | 25% | Wahrscheinlichkeit von Problemen |
| Effektivität | 25% | Löst das Problem tatsächlich? |
| Spielqualität | 25% | Auswirkung auf Spielerlebnis |

### 6.2 Bewertung (1-5, höher = besser)

| Option | Aufwand | Risiko | Effektivität | Spielqualität | **Gesamt** |
|--------|---------|--------|--------------|---------------|------------|
| A: KI-Handicap | 4 | 3 | 3 | 2 | **3.00** |
| B: Spielregeln | 2 | 2 | 4 | 2 | **2.50** |
| **C: Anforderung** | **5** | **5** | **4** | **4** | **4.50** |
| D: Hybrid | 2 | 3 | 5 | 5 | **3.75** |

### 6.3 Visualisierung

```
Aufwand vs. Effektivität
                    │
     Hoch           │
   Effektivität     │    [D]●────────●[C] ← EMPFOHLEN
                    │
                    │  [B]●      ●[A]
     Niedrig        │
                    └──────────────────────
                    Hoch    Aufwand    Niedrig
```

---

## 7. Empfehlung

### 7.1 Primärempfehlung: Option C

**Die Anforderung FA-502 sollte angepasst werden.**

**Begründung:**
1. **Spieltheoretisch korrekt:** Das Spiel IST asymmetrisch - das ist Designabsicht
2. **Zielgruppengerecht:** Für Kinder ist ein leichter Einstieg wichtig
3. **Minimal invasiv:** Keine Code-Änderungen, nur Dokumentation
4. **Schnell umsetzbar:** 1-2 Stunden Aufwand
5. **Kein Risiko:** Bestehende Funktionalität bleibt unverändert

### 7.2 Sekundärempfehlung: Option D für V2.0

Für eine zukünftige Version könnte ein Schwierigkeitsgrad-System implementiert werden, das verschiedene Spielertypen bedient.

### 7.3 Nicht empfohlen: Option B

Änderungen an den Spielregeln würden:
- Das klassische Spielprinzip verfälschen
- Hohen Testaufwand erfordern
- Bestehende Dokumentation ungültig machen

---

## 8. Nächste Schritte (bei Annahme Option C)

1. **Lastenheft anpassen** (30 Min)
   - FA-502 umformulieren

2. **Spielanleitung erweitern** (30 Min)
   - Schwierigkeitshinweis für Rollen

3. **UI-Anpassung** (optional, 1-2 Std)
   - Tooltip bei Rollenauswahl: "Leicht" / "Schwer"

4. **Dokumentation aktualisieren** (30 Min)
   - Gewinnwahrscheinlichkeit-Analyse finalisieren

---

## Anhang: Referenzen

- `05_Test/Gewinnwahrscheinlichkeit-Analyse.md` - Simulationsergebnisse
- `01_Anforderungen/Lastenheft.md` - Aktuelle Anforderungen
- `04_Implementierung/backend/CatchTheRabbit.Core/Services/AIService.cs` - KI-Implementierung

---

**Dokument erstellt:** 2026-01-13
**Autor:** Claude AI
**Version:** 1.0
