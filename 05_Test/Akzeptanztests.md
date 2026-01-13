# Akzeptanztest-Spezifikation - CatchTheRabbit

| Dokument      | Akzeptanztest-Spezifikation       |
|---------------|-----------------------------------|
| Projekt       | CatchTheRabbit                    |
| Version       | 1.0                               |
| Datum         | 2025-03-21                        |
| Autor         | Entwicklungsteam                  |

---

## 1. Übersicht

### 1.1 Zweck
Diese Spezifikation definiert die Akzeptanztests zur Validierung der funktionalen und nicht-funktionalen Anforderungen aus dem Lastenheft.

### 1.2 Testansatz
- End-to-End Tests mit Playwright
- Simulation echter Benutzerinteraktionen
- Validierung gegen Anforderungen aus Lastenheft

### 1.3 Testwerkzeuge
- Playwright für Browser-Automatisierung
- xUnit als Test-Runner
- Screenshot-Capture für Dokumentation

---

## 2. Testfälle

### AT-001: Spielfeld-Anzeige

| Attribut | Wert |
|----------|------|
| **ID** | AT-001 |
| **Anforderung** | FA-101 |
| **Titel** | 10x10 Spielfeld wird korrekt angezeigt |
| **Priorität** | Hoch |
| **Vorbedingung** | Anwendung ist gestartet |

**Testschritte:**
1. Navigiere zur Startseite
2. Wähle Rolle "Kinder"
3. Klicke "Spiel starten"
4. Überprüfe Spielfeld-Anzeige

**Erwartetes Ergebnis:**
- Spielfeld mit 10x10 Feldern wird angezeigt
- Felder sind abwechselnd hell/dunkel (Schachbrettmuster)
- Spielfeld ist vollständig sichtbar

**Nachbedingung:** Spiel ist aktiv

---

### AT-002: Startpositionen

| Attribut | Wert |
|----------|------|
| **ID** | AT-002 |
| **Anforderung** | FA-102, FA-103 |
| **Titel** | Figuren starten auf korrekten Positionen |
| **Priorität** | Hoch |
| **Vorbedingung** | Neues Spiel gestartet |

**Testschritte:**
1. Starte neues Spiel
2. Überprüfe Position des Hasen
3. Überprüfe Positionen der Kinder

**Erwartetes Ergebnis:**
- Hase befindet sich auf Position (0, 4) - Mitte erste Reihe
- Kinder befinden sich auf Positionen (9, 1), (9, 3), (9, 5), (9, 7)

**Nachbedingung:** Spielfiguren sind sichtbar

---

### AT-003: Hase-Bewegung

| Attribut | Wert |
|----------|------|
| **ID** | AT-003 |
| **Anforderung** | FA-201 |
| **Titel** | Hase bewegt sich diagonal in alle Richtungen |
| **Priorität** | Hoch |
| **Vorbedingung** | Spiel als Hase gestartet, Spieler am Zug |

**Testschritte:**
1. Starte Spiel als Hase
2. Klicke auf den Hasen
3. Überprüfe angezeigte Zugmöglichkeiten
4. Führe Zug diagonal nach unten-rechts aus
5. Nach KI-Zug: Führe Zug diagonal nach unten-links aus

**Erwartetes Ergebnis:**
- 4 diagonale Zugmöglichkeiten werden angezeigt (sofern nicht blockiert)
- Hase bewegt sich korrekt in gewählte Richtung
- Spielfeld aktualisiert sich nach jedem Zug

**Nachbedingung:** Hase auf neuer Position

---

### AT-004: Kinder-Bewegung

| Attribut | Wert |
|----------|------|
| **ID** | AT-004 |
| **Anforderung** | FA-202 |
| **Titel** | Kinder bewegen sich diagonal nach oben |
| **Priorität** | Hoch |
| **Vorbedingung** | Spiel als Kinder gestartet, Spieler am Zug |

**Testschritte:**
1. Starte Spiel als Kinder
2. Klicke auf ein Kind
3. Überprüfe angezeigte Zugmöglichkeiten
4. Führe Zug diagonal nach oben aus

**Erwartetes Ergebnis:**
- Nur 2 diagonale Zugmöglichkeiten nach oben werden angezeigt
- Keine Bewegung nach unten möglich
- Kind bewegt sich korrekt

**Nachbedingung:** Kind auf neuer Position

---

### AT-005: Rollenwahl

| Attribut | Wert |
|----------|------|
| **ID** | AT-005 |
| **Anforderung** | FA-301 |
| **Titel** | Spieler wählt Rolle vor Spielbeginn |
| **Priorität** | Hoch |
| **Vorbedingung** | Anwendung auf Startseite |

**Testschritte:**
1. Navigiere zur Startseite
2. Überprüfe Rollenauswahl-Optionen
3. Wähle "Hase"
4. Klicke "Spiel starten"
5. Überprüfe Spieler-Rolle in Spielansicht

**Erwartetes Ergebnis:**
- Zwei Auswahlmöglichkeiten: "Hase" und "Kinder"
- Gewählte Rolle wird hervorgehoben
- Nach Spielstart: Spieler steuert gewählte Figur(en)

**Nachbedingung:** Spiel mit gewählter Rolle aktiv

---

### AT-006: KI-Reaktion

| Attribut | Wert |
|----------|------|
| **ID** | AT-006 |
| **Anforderung** | FA-302 |
| **Titel** | KI spielt automatisch nach Spielerzug |
| **Priorität** | Hoch |
| **Vorbedingung** | Spiel aktiv, Spieler am Zug |

**Testschritte:**
1. Führe gültigen Spielerzug aus
2. Warte auf KI-Reaktion
3. Überprüfe KI-Zug

**Erwartetes Ergebnis:**
- KI führt automatisch einen Zug aus
- Spielfeld aktualisiert sich
- Spieler ist wieder am Zug (außer Spielende)

**Nachbedingung:** Spieler wieder am Zug oder Spiel beendet

---

### AT-007: Siegbedingung Hase

| Attribut | Wert |
|----------|------|
| **ID** | AT-007 |
| **Anforderung** | FA-401 |
| **Titel** | Hase gewinnt bei Erreichen des unteren Rands |
| **Priorität** | Hoch |
| **Vorbedingung** | Spiel aktiv, Hase nahe unterem Rand |

**Testschritte:**
1. Spiele bis Hase Reihe 9 erreicht (oder simuliere Spielzustand)
2. Bewege Hase auf Reihe 9 (unteren Rand)
3. Überprüfe Spielende-Anzeige

**Erwartetes Ergebnis:**
- Spiel endet mit "Hase gewinnt"
- Game-Over-Modal erscheint
- Korrekter Sieger wird angezeigt

**Nachbedingung:** Spiel beendet

---

### AT-008: Siegbedingung Kinder

| Attribut | Wert |
|----------|------|
| **ID** | AT-008 |
| **Anforderung** | FA-402 |
| **Titel** | Kinder gewinnen wenn Hase blockiert |
| **Priorität** | Hoch |
| **Vorbedingung** | Spiel aktiv, Hase fast eingekreist |

**Testschritte:**
1. Spiele bis Hase keine Zugmöglichkeit mehr hat
2. Überprüfe Spielende-Erkennung
3. Überprüfe Spielende-Anzeige

**Erwartetes Ergebnis:**
- Spiel endet mit "Kinder gewinnen"
- Game-Over-Modal erscheint
- Korrekter Sieger wird angezeigt

**Nachbedingung:** Spiel beendet

---

### AT-009: Bestenliste Anzeige

| Attribut | Wert |
|----------|------|
| **ID** | AT-009 |
| **Anforderung** | FA-501 |
| **Titel** | Bestenliste zeigt Top 20 |
| **Priorität** | Mittel |
| **Vorbedingung** | Einträge in Bestenliste vorhanden |

**Testschritte:**
1. Navigiere zur Bestenliste
2. Überprüfe angezeigte Einträge
3. Überprüfe bei mehr als 20 Einträgen

**Erwartetes Ergebnis:**
- Maximal 20 Einträge pro Kategorie (Hase/Kinder)
- Alle Spalten sichtbar (Rang, Name, Zeit, Datum)
- Korrekte Formatierung

**Nachbedingung:** Bestenliste wird angezeigt

---

### AT-010: Bestenliste Sortierung

| Attribut | Wert |
|----------|------|
| **ID** | AT-010 |
| **Anforderung** | FA-502 |
| **Titel** | Sortierung nach KI-Denkzeit |
| **Priorität** | Mittel |
| **Vorbedingung** | Mehrere Einträge in Bestenliste |

**Testschritte:**
1. Navigiere zur Bestenliste
2. Überprüfe Sortierreihenfolge
3. Vergleiche KI-Denkzeiten

**Erwartetes Ergebnis:**
- Einträge aufsteigend nach KI-Denkzeit sortiert
- Kürzeste Zeit auf Platz 1
- Gleiche Zeiten: älterer Eintrag höher

**Nachbedingung:** -

---

### AT-011: Nickname-Eingabe

| Attribut | Wert |
|----------|------|
| **ID** | AT-011 |
| **Anforderung** | FA-503 |
| **Titel** | Nickname-Eingabe nach Sieg |
| **Priorität** | Mittel |
| **Vorbedingung** | Spiel mit Spielersieg beendet |

**Testschritte:**
1. Gewinne ein Spiel
2. Überprüfe Game-Over-Modal
3. Gib Nickname ein
4. Klicke "Eintragen"
5. Überprüfe Bestätigung

**Erwartetes Ergebnis:**
- Eingabefeld für Nickname erscheint
- Nickname kann eingegeben werden
- Nach Eintragung: Rang wird angezeigt
- Eintrag erscheint in Bestenliste

**Nachbedingung:** Eintrag in Bestenliste gespeichert

---

### AT-012: KI-Antwortzeit

| Attribut | Wert |
|----------|------|
| **ID** | AT-012 |
| **Anforderung** | NFA-201 |
| **Titel** | KI antwortet unter 1 Sekunde |
| **Priorität** | Hoch |
| **Vorbedingung** | Spiel aktiv |

**Testschritte:**
1. Führe 10 Spielerzüge aus
2. Messe KI-Antwortzeit für jeden Zug
3. Berechne Durchschnitt und Maximum

**Erwartetes Ergebnis:**
- Jeder KI-Zug unter 1000ms
- Durchschnitt unter 500ms
- Keine spürbaren Verzögerungen

**Nachbedingung:** -

---

## 3. Testdaten

### 3.1 Vordefinierte Spielzustände

| ID | Beschreibung | Verwendung in Tests |
|----|--------------|---------------------|
| TD-001 | Leeres Spiel, Standardstart | AT-001, AT-002 |
| TD-002 | Hase nahe unterem Rand | AT-007 |
| TD-003 | Hase blockiert (1 Zug vor Ende) | AT-008 |
| TD-004 | Bestenliste mit 25 Einträgen | AT-009, AT-010 |

### 3.2 Testbenutzer

| Nickname | Verwendung |
|----------|------------|
| TestSpielerin | AT-011 |
| Highscore_User | AT-010 |

---

## 4. Anforderungsverfolgungsmatrix

| Anforderung | Testfall | Status |
|-------------|----------|--------|
| FA-101 | AT-001 | Geplant |
| FA-102 | AT-002 | Geplant |
| FA-103 | AT-002 | Geplant |
| FA-201 | AT-003 | Geplant |
| FA-202 | AT-004 | Geplant |
| FA-301 | AT-005 | Geplant |
| FA-302 | AT-006 | Geplant |
| FA-401 | AT-007 | Geplant |
| FA-402 | AT-008 | Geplant |
| FA-501 | AT-009 | Geplant |
| FA-502 | AT-010 | Geplant |
| FA-503 | AT-011 | Geplant |
| NFA-201 | AT-012 | Geplant |

---

## 5. Änderungshistorie

| Version | Datum | Autor | Änderung |
|---------|-------|-------|----------|
| 1.0 | 2025-03-21 | Entwicklungsteam | Initiale Version |
