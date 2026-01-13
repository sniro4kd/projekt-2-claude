# ADR-005: KI-Algorithmus

| Attribut | Wert |
|----------|------|
| Status | Akzeptiert |
| Datum | 21.03.2025 |
| Entscheider | Entwicklungsteam |

## Kontext

Für den Computergegner in CatchTheRabbit wird ein Algorithmus benötigt, der:
- Innerhalb von maximal 1 Sekunde einen Zug berechnet
- Eine ausgeglichene Gewinnrate von ca. 50% erreicht (unabhängig von der Rolle)
- Deterministisch und nachvollziehbar ist (für Debugging und Balancing)

## Entscheidung

**Minimax mit Alpha-Beta-Pruning** wird als KI-Algorithmus verwendet.

Zusätzlich wird **Iterative Deepening** implementiert, um das Zeitlimit zu garantieren.

## Begründung

| Kriterium | Minimax + Alpha-Beta | MCTS | Einfache Heuristik |
|-----------|---------------------|------|-------------------|
| Spielstärke | Hoch | Hoch | Niedrig |
| Determinismus | Ja | Nein | Ja |
| Zeitkontrolle | Gut (mit Iterative Deepening) | Sehr gut | Trivial |
| Implementierungsaufwand | Mittel | Hoch | Niedrig |
| Debugging | Einfach | Schwierig | Einfach |

**Minimax + Alpha-Beta** bietet die beste Balance aus:
- Starker, aber schlagbarer Gegner (für ~50% Gewinnrate)
- Deterministische Ergebnisse (gleiche Position → gleicher Zug)
- Gut verständlicher und wartbarer Code
- Etablierter Algorithmus für Zwei-Spieler-Spiele

## Technische Details

### Suchtiefe
- Start: 6 Halbzüge
- Maximum: 10 Halbzüge (mit Iterative Deepening)
- Anpassbar für Balancing

### Bewertungsfunktion (Heuristik)
Faktoren für die Stellungsbewertung:
1. **Hasen-Position**: Fortschritt Richtung Zielreihe
2. **Mobilität**: Anzahl gültiger Züge des Hasen
3. **Einkreisung**: Position der Kinder relativ zum Hasen
4. **Formation**: Zusammenhalt der Kinder-Linie

### Zeitmanagement
- Zeitlimit: 900ms (Puffer für 1s-Anforderung)
- Iterative Deepening garantiert Antwort
- Fallback: Zufälliger gültiger Zug bei Timeout

## Konsequenzen

### Positiv
- Vorhersagbare KI-Stärke
- Einfaches Balancing durch Anpassung der Heuristik-Gewichte
- Gut testbar durch Determinismus

### Negativ
- Kann durch "Auswendiglernen" ausgetrickst werden (gleiche Züge in gleichen Situationen)
- Begrenzte Anpassungsfähigkeit an Spielerstil

## Referenzen

- Pflichtenheft Kapitel 4: KI-Spezifikation
- [Wikipedia: Alpha-Beta-Pruning](https://de.wikipedia.org/wiki/Alpha-Beta-Suche)
