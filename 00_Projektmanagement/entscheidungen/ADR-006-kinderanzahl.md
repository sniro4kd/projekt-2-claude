# ADR-006: Änderung der Kinderanzahl von 4 auf 5

## Status
**Akzeptiert** - 2026-01-13

## Kontext

Bei der Gewinnwahrscheinlichkeits-Analyse (FA-502) wurde festgestellt, dass die ursprüngliche Konfiguration mit 4 Kindern zu einem stark unausgeglichenen Spiel führt:

- **Mit 4 Kindern:** Der Hase gewinnt ~95% der Spiele
- **Ursache:** Pro Reihe gibt es 5 schwarze (bespielbare) Felder, aber nur 4 Kinder können diese blockieren
- **Ergebnis:** Es existiert immer mindestens eine Lücke, durch die der Hase entkommen kann

Die Anforderung FA-502 verlangt eine ausgeglichene Gewinnrate von ca. 50%.

## Entscheidung

Die Anzahl der Kinder wird von **4 auf 5** erhöht.

### Begründung

1. **Mathematische Notwendigkeit:** 5 schwarze Felder pro Reihe erfordern 5 Kinder für eine lückenlose Barriere
2. **Spielbalance:** Mit 5 Kindern ist eine vollständige Blockade möglich, was die Gewinnchancen der Kinder verbessert
3. **Minimale Regeländerung:** Die Spielmechanik bleibt identisch, nur die Figurenanzahl ändert sich

## Konsequenzen

### Positive Auswirkungen
- KI vs KI Simulation zeigt nun ~45%/55% (ausgeglichen)
- Kinder haben realistische Chance, den Hasen zu fangen
- Spielprinzip (Barriere bilden) funktioniert wie beabsichtigt

### Negative Auswirkungen / Neue Erkenntnisse
- **FA-502 bleibt unerfüllt:** Mensch vs KI ist weiterhin unausgeglichen
  - Mensch als Kinder: gewinnt ~80-90% (zu leicht)
  - Mensch als Hase: gewinnt ~30-50% (ausgeglichen)
- **Ursache:** Menschen setzen die Barriere-Taktik intuitiv besser um als die KI

### Betroffene Dokumente
- `01_Anforderungen/Lastenheft.md` (FA-102, FA-202)
- `02_Spezifikation/Pflichtenheft.md`
- `03_Entwurf/Low-Level-Design.md`
- `04_Implementierung/backend/CatchTheRabbit.Core/Constants.cs`
- `04_Implementierung/frontend/src/views/HomeView.vue`
- `04_Implementierung/frontend/src/components/GamePiece.vue`
- Diverse Unit Tests
- `05_Test/Gewinnwahrscheinlichkeit-Analyse.md`
- `05_Test/Balancing-Optionen-Analyse.md`

## Alternativen

| Alternative | Beschreibung | Grund für Ablehnung |
|-------------|--------------|---------------------|
| 4 Kinder beibehalten | Ursprüngliche Konfiguration | Hase gewinnt 95% - zu unausgeglichen |
| 3 Kinder | Weniger Kinder | Würde Problem verschärfen |
| 6 Kinder | Mehr Kinder | Übermächtig, verändert Spielcharakter |
| KI-Handicap | Unterschiedliche KI-Stärken | Für V1.0 zu aufwändig, als Option D dokumentiert |

## Offene Punkte

Die Anforderung FA-502 (50% Gewinnrate) ist mit dieser Änderung allein nicht erfüllbar. Weitere Optionen sind in `05_Test/Balancing-Optionen-Analyse.md` dokumentiert:

- **Option A:** KI-Handicap (rollenbasierte Stärke)
- **Option C:** Anforderung anpassen (Asymmetrie akzeptieren)
- **Option D:** Schwierigkeitsgrade (für V2.0)

Eine finale Entscheidung zur Handhabung von FA-502 steht noch aus.

---

**Entscheider:** Projektteam
**Datum:** 2026-01-13
