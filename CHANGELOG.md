# Changelog - CatchTheRabbit Projektplanung

Dieses Dokument gibt einen Überblick über den Fortschritt der Projektplanung.

---

## 2025-03-21 - Pflichtenheft erstellt

### Hinzugefügt
- **Pflichtenheft v1.0** erstellt (`02_Spezifikation/Pflichtenheft.md`)
  - Systemarchitektur (Frontend, Backend, Deployment)
  - Spiellogik-Spezifikation (Spielfeld, Zugvalidierung, Siegbedingungen)
  - KI-Spezifikation (Minimax mit Alpha-Beta-Pruning)
  - Use Cases (UC-01 bis UC-05)
  - Datenmodell und SQLite-Schema
  - REST API und SignalR Hub Spezifikation
  - UI-Spezifikation mit Wireframes
  - Fehlerbehandlung und Logging
  - Testabdeckung und Testfälle

### Entscheidungen
- [ADR-005](entscheidungen/ADR-005-ki-algorithmus.md): Minimax + Alpha-Beta als KI-Algorithmus

---

## 2025-03-21 - V-Modell Einführung

### Geändert
- **Projektstruktur** auf V-Modell Phasen umgestellt
  - `01_Anforderungen/` - Lastenheft
  - `02_Spezifikation/` - Pflichtenheft (noch zu erstellen)
  - `03_Entwurf/` - Architektur (noch zu erstellen)
  - `04_Implementierung/` - Quellcode
  - `05_Test/` - Testdokumentation
- **Docker-Setup** angepasst: Kein separater DB-Container für SQLite nötig

### Entscheidungen
- [ADR-004](entscheidungen/ADR-004-vorgehensmodell.md): V-Modell als Vorgehensmodell

---

## 2025-03-21 - Projektstart

### Hinzugefügt
- **Lastenheft v1.0** erstellt (`Lastenheft.md`)
  - Funktionale Anforderungen (FA-100 bis FA-801)
  - Nichtfunktionale Anforderungen (NFA-100 bis NFA-502)
  - Systemarchitektur-Überblick
  - Abnahmekriterien

### Entscheidungen
- [ADR-001](entscheidungen/ADR-001-technologie-stack.md): Technologie-Stack bestätigt
- [ADR-002](entscheidungen/ADR-002-datenbank.md): SQLite als Datenbank gewählt
- [ADR-003](entscheidungen/ADR-003-projektdokumentation.md): Dokumentationsstruktur festgelegt

### Protokoll
- [2025-03-21](protokolle/2025-03-21-projektstart.md): Initiales Planungsgespräch
