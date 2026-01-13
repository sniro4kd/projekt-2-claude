# Protokoll: Pflichtenheft
**Datum:** 2026-01-10
**Phase:** Systemspezifikation

---

## Teilnehmer
- Projektteam

## Agenda
1. Pflichtenheft erstellen
2. Use Cases definieren
3. API-Spezifikation
4. KI-Algorithmus festlegen

## Beschlüsse

### 1. Pflichtenheft v1.0
Umfassendes Pflichtenheft erstellt mit:
- Systemarchitektur (3-Tier)
- Spiellogik-Spezifikation
- Datenmodell
- UI-Wireframes

### 2. Use Cases
5 Use Cases definiert:
- UC-01: Spiel starten
- UC-02: Spielzug ausführen
- UC-03: Spielende erkennen
- UC-04: Bestenliste anzeigen
- UC-05: Eintrag hinzufügen

### 3. API-Spezifikation
REST API Endpunkte definiert:
- POST /api/game/new
- GET /api/game/{id}
- POST /api/game/{id}/move
- GET /api/leaderboard
- POST /api/leaderboard

### 4. KI-Algorithmus (ADR-005)
Minimax mit Alpha-Beta Pruning gewählt:
- Suchtiefe: 6
- Zeitlimit: 1 Sekunde
- Bewertungsfunktion basierend auf Position

## Nächste Schritte
- [ ] High-Level-Design erstellen
- [ ] Low-Level-Design erstellen

---

**Protokollant:** Claude AI
