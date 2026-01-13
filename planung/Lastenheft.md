# Lastenheft: CatchTheRabbit

| Attribut | Wert |
|----------|------|
| Projekt | CatchTheRabbit |
| Auftraggeber | ERP_Champion GmbH |
| Product Owner | Frank Niederbronn B.Sc. |
| Version | 1.0 |
| Datum | 21.03.2025 |
| Status | Entwurf |

---

## 1. Einleitung

### 1.1 Zweck des Dokuments

Dieses Lastenheft beschreibt die Anforderungen an das Softwareprojekt "CatchTheRabbit" aus Sicht des Auftraggebers ERP_Champion GmbH. Es dient als verbindliche Grundlage für die Entwicklung und Abnahme des Systems.

### 1.2 Geltungsbereich

Das Dokument umfasst alle funktionalen und nichtfunktionalen Anforderungen an die Web-Applikation "CatchTheRabbit", einschließlich Frontend, Backend, KI-Komponente und Persistenzschicht.

### 1.3 Referenzen

| Dokument | Version | Datum |
|----------|---------|-------|
| Project Proposal CatchTheRabbit | 1.0 | 21.03.2025 |

### 1.4 Glossar

| Begriff | Definition |
|---------|------------|
| Hase | Spielfigur, die versucht, die obere Spielfeldreihe zu erreichen |
| Kinder | Vier Spielfiguren, die versuchen, den Hasen einzukesseln |
| Schwarze Felder | Spielbare Felder auf dem Schachbrett (wie bei Dame) |
| Bedenkzeit | Kumulierte Zeit, die ein Spieler für alle seine Züge benötigt |
| KI | Künstliche Intelligenz, die als Computergegner fungiert |

---

## 2. Allgemeine Beschreibung

### 2.1 Produktvision

CatchTheRabbit ist ein browserbasiertes Strategiespiel, das zur Kundenbindung an Kinder von ERP_Champion-Kunden ausgeliefert wird. Das Spiel kombiniert kindgerechtes Design mit anspruchsvoller Spielmechanik.

### 2.2 Zielgruppe

- Primär: Kinder von Kunden und Angestellten der ERP_Champion-Kunden
- Sekundär: Alle Interessierten mit Zugang zur Web-Applikation

### 2.3 Systemkontext

```
┌─────────────────────────────────────────────────────────────┐
│                        Browser                               │
│  ┌───────────────────────────────────────────────────────┐  │
│  │                 Vue.js Frontend                        │  │
│  └───────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
                    │ REST              ▲
                    │                   │ SignalR
                    ▼                   │
┌─────────────────────────────────────────────────────────────┐
│                    ASP.NET Backend                           │
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────────────┐  │
│  │ Controller  │→ │Business Logic│→ │    Data Access     │  │
│  └─────────────┘  └─────────────┘  └─────────────────────┘  │
│                                              │               │
│                                              ▼               │
│                                    ┌─────────────────────┐  │
│                                    │  Persistenz (SQLite)│  │
│                                    └─────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
```

### 2.4 Rahmenbedingungen

| Kategorie | Bedingung |
|-----------|-----------|
| Budget | 20.000 Euro |
| Zeitraum | März – Juni 2025 |
| Technologie Frontend | Vue.js |
| Technologie Backend | ASP.NET (C#) |
| Kommunikation | REST API + SignalR |
| Datenbank | SQLite (austauschbar durch Interface-Abstraktion) |
| Deployment | Docker mit docker-compose |
| Sprache | Deutsch |
| Zielplattform | Desktop-Browser |

---

## 3. Funktionale Anforderungen

### 3.1 Spielfeld und Spielfiguren

| ID | Anforderung | Priorität |
|----|-------------|-----------|
| FA-100 | Das Spielfeld ist ein 10x10 Schachbrett | Muss |
| FA-101 | Nur die schwarzen Felder sind bespielbar (50 Felder) | Muss |
| FA-102 | Es gibt genau 4 Kinder-Spielfiguren | Muss |
| FA-103 | Es gibt genau 1 Hasen-Spielfigur | Muss |
| FA-104 | Spielfiguren werden grafisch kindgerecht dargestellt | Muss |

### 3.2 Spielstart

| ID | Anforderung | Priorität |
|----|-------------|-----------|
| FA-200 | Vor Spielbeginn wählt der Spieler seine Rolle (Hase oder Kinder) | Muss |
| FA-201 | Die Startposition des Hasen wird zufällig auf einem schwarzen Feld der unteren 3 Reihen platziert | Muss |
| FA-202 | Die Startpositionen der 4 Kinder werden zufällig auf schwarzen Feldern der oberen 3 Reihen platziert | Muss |
| FA-203 | Jedes Kind belegt genau ein Feld; keine Überlappungen | Muss |

### 3.3 Spielregeln und Zugmechanik

| ID | Anforderung | Priorität |
|----|-------------|-----------|
| FA-300 | Der Hase führt den ersten Zug aus | Muss |
| FA-301 | Die Spieler ziehen abwechselnd | Muss |
| FA-302 | Der Hase kann sich diagonal nach oben-links, oben-rechts, unten-links oder unten-rechts bewegen (1 Feld) | Muss |
| FA-303 | Ein Kind kann sich nur diagonal nach unten-links oder unten-rechts bewegen (1 Feld) | Muss |
| FA-304 | Pro Zug darf der Kinder-Spieler genau ein Kind bewegen | Muss |
| FA-305 | Eine Spielfigur kann nicht auf ein bereits besetztes Feld ziehen | Muss |
| FA-306 | Spielfiguren können das Spielfeld nicht verlassen | Muss |

### 3.4 Siegbedingungen

| ID | Anforderung | Priorität |
|----|-------------|-----------|
| FA-400 | Der Hase gewinnt, wenn er eines der 5 schwarzen Felder der obersten Reihe erreicht | Muss |
| FA-401 | Die Kinder gewinnen, wenn der Hase keinen gültigen Zug mehr ausführen kann | Muss |
| FA-402 | Nach Spielende wird das Ergebnis angezeigt | Muss |
| FA-403 | Nach Spielende kann ein neues Spiel gestartet werden | Muss |

### 3.5 KI-Gegner

| ID | Anforderung | Priorität |
|----|-------------|-----------|
| FA-500 | Die KI übernimmt die Rolle, die der Spieler nicht gewählt hat | Muss |
| FA-501 | Die KI berechnet ihren Zug innerhalb von maximal 1 Sekunde | Muss |
| FA-502 | Die KI soll eine ausgeglichene Gewinnrate von ca. 50% erreichen (unabhängig von der Rolle) | Soll |
| FA-503 | Die Spielstärke der KI soll durch Anpassung der Ausgangsregeln oder des Algorithmus balanciert werden können | Soll |

### 3.6 Bestenlisten

| ID | Anforderung | Priorität |
|----|-------------|-----------|
| FA-600 | Es existieren zwei separate Bestenlisten: eine für Siege als Hase, eine für Siege als Kinder | Muss |
| FA-601 | Nur Spieler, die gegen die KI gewinnen, werden in die Bestenliste aufgenommen | Muss |
| FA-602 | Der Spieler gibt bei Sieg einen Nicknamen ein (freie Texteingabe) | Muss |
| FA-603 | Die Bestenliste wird nach Bedenkzeit sortiert (aufsteigend: kürzere Zeit = bessere Platzierung) | Muss |
| FA-604 | Es werden die Top 20 Einträge pro Liste angezeigt | Muss |
| FA-605 | Die Bestenlisten sind für alle Besucher öffentlich einsehbar | Muss |
| FA-606 | Die Bestenlisten werden persistent gespeichert | Muss |

### 3.7 Benutzeroberfläche

| ID | Anforderung | Priorität |
|----|-------------|-----------|
| FA-700 | Die Oberfläche zeigt das Spielfeld mit allen Figuren | Muss |
| FA-701 | Gültige Zugmöglichkeiten werden visuell hervorgehoben | Soll |
| FA-702 | Die aktuelle Bedenkzeit des Spielers wird angezeigt | Muss |
| FA-703 | Es ist erkennbar, welcher Spieler am Zug ist | Muss |
| FA-704 | Das ERP_Champion-Branding ist sichtbar integriert | Muss |
| FA-705 | Die Oberfläche ist kindgerecht gestaltet (Farben, Grafiken) | Muss |

### 3.8 Audio

| ID | Anforderung | Priorität |
|----|-------------|-----------|
| FA-800 | Soundeffekte für Spielereignisse (Zug, Sieg, Niederlage) | Soll |
| FA-801 | Möglichkeit, Sound stumm zu schalten | Soll |

---

## 4. Nichtfunktionale Anforderungen

### 4.1 Performance

| ID | Anforderung | Priorität |
|----|-------------|-----------|
| NFA-100 | Die KI-Zugberechnung dauert maximal 1 Sekunde | Muss |
| NFA-101 | Die Benutzeroberfläche reagiert flüssig (keine spürbaren Verzögerungen bei Interaktionen) | Muss |
| NFA-102 | Die Anwendung soll mit mindestens 100 gleichzeitigen Spielern umgehen können | Soll |

### 4.2 Zuverlässigkeit

| ID | Anforderung | Priorität |
|----|-------------|-----------|
| NFA-200 | Die Bestenlisten gehen bei Neustart des Servers nicht verloren | Muss |
| NFA-201 | Spielstände müssen nicht persistent sein (Refresh = Spielabbruch erlaubt) | Info |

### 4.3 Wartbarkeit

| ID | Anforderung | Priorität |
|----|-------------|-----------|
| NFA-300 | Die Persistenzschicht ist durch ein Interface abstrahiert (SQLite als Implementierung, austauschbar) | Muss |
| NFA-301 | Die Architektur folgt dem vorgegebenen Schichtenmodell (Controller → Business Logic → Data Access → Persistenz) | Muss |
| NFA-302 | Der Code ist so strukturiert, dass hauseigene Entwickler Bugfixes und neue Features umsetzen können | Muss |
| NFA-303 | Unit-Tests für kritische Komponenten (Spiellogik, KI) | Soll |

### 4.4 Portabilität

| ID | Anforderung | Priorität |
|----|-------------|-----------|
| NFA-400 | Die Anwendung läuft in Docker-Containern | Muss |
| NFA-401 | Docker-compose wird für die Orchestrierung verwendet | Muss |
| NFA-402 | Separate Container für Frontend, Backend und Datenbank | Muss |
| NFA-403 | Kunden können die Anwendung selbst hosten | Soll |

### 4.5 Usability

| ID | Anforderung | Priorität |
|----|-------------|-----------|
| NFA-500 | Die Anwendung ist ohne Anleitung bedienbar (intuitives Design) | Soll |
| NFA-501 | Zielgruppe: Kinder (kindgerechte Sprache und Gestaltung) | Muss |
| NFA-502 | Unterstützung aktueller Desktop-Browser (Chrome, Firefox, Edge) | Muss |

---

## 5. Systemarchitektur (Überblick)

### 5.1 Komponentendiagramm

```
┌────────────────────────────────────────────────────────────────────┐
│                         Docker Compose                              │
│                                                                     │
│  ┌──────────────────┐  ┌──────────────────┐  ┌──────────────────┐  │
│  │   Frontend       │  │    Backend       │  │   Datenbank      │  │
│  │   Container      │  │    Container     │  │   Container      │  │
│  │                  │  │                  │  │                  │  │
│  │  ┌────────────┐  │  │  ┌────────────┐  │  │  ┌────────────┐  │  │
│  │  │  Vue.js    │  │  │  │ ASP.NET    │  │  │  │  SQLite    │  │  │
│  │  │  WebApp    │◄─┼──┼─►│   API      │◄─┼──┼─►│            │  │  │
│  │  │            │  │  │  │            │  │  │  │            │  │  │
│  │  └────────────┘  │  │  └────────────┘  │  │  └────────────┘  │  │
│  │                  │  │                  │  │                  │  │
│  │  - Spielfeld UI  │  │  - Controller    │  │  - Bestenlisten  │  │
│  │  - Bedienung     │  │  - Spiellogik    │  │                  │  │
│  │  - Animationen   │  │  - KI-Engine     │  │                  │  │
│  │  - Sound         │  │  - Data Access   │  │                  │  │
│  │                  │  │                  │  │                  │  │
│  └──────────────────┘  └──────────────────┘  └──────────────────┘  │
│                                                                     │
└────────────────────────────────────────────────────────────────────┘
```

### 5.2 Technologie-Stack

| Schicht | Technologie |
|---------|-------------|
| Frontend | Vue.js 3 |
| Backend | ASP.NET Core (C#) |
| API-Kommunikation | REST + SignalR |
| Datenbank | SQLite |
| Containerisierung | Docker + docker-compose |
| Entwicklung Frontend | Visual Studio Code |
| Entwicklung Backend | Visual Studio |

---

## 6. Lieferumfang

| Liefergegenstand | Beschreibung |
|------------------|--------------|
| Quellcode | Vollständiger, dokumentierter Quellcode (Frontend + Backend) |
| Docker-Konfiguration | Dockerfile(s) und docker-compose.yml für Deployment |
| Datenbankschema | SQLite-Datenbankschema für Bestenlisten |
| Technische Dokumentation | Architektur- und Schnittstellendokumentation |
| Testfälle | Unit-Tests für Spiellogik und KI-Komponente |

---

## 7. Abnahmekriterien

| ID | Kriterium |
|----|-----------|
| AK-01 | Das Spiel ist über einen Webbrowser erreichbar und spielbar |
| AK-02 | Die Spielregeln werden korrekt umgesetzt (Zugmechanik, Siegbedingungen) |
| AK-03 | Die KI reagiert innerhalb von 1 Sekunde |
| AK-04 | Siege werden in der Bestenliste gespeichert und bleiben nach Neustart erhalten |
| AK-05 | Die Anwendung startet via docker-compose ohne manuelle Konfiguration |
| AK-06 | Das ERP_Champion-Branding ist sichtbar |
| AK-07 | Die Oberfläche ist kindgerecht gestaltet |

---

## 8. Anhang

### 8.1 Offene Punkte

| ID | Thema | Status |
|----|-------|--------|
| OP-01 | Analyse der KI-Gewinnrate und ggf. Balancing der Startpositionen | Offen |
| OP-02 | Finale Auswahl des KI-Algorithmus (z.B. Minimax, Alpha-Beta, MCTS) | Offen |
| OP-03 | Design-Mockups für kindgerechte Oberfläche | Offen |
| OP-04 | Auswahl/Erstellung von Sound-Assets | Offen |

### 8.2 Änderungshistorie

| Version | Datum | Autor | Änderung |
|---------|-------|-------|----------|
| 1.0 | 21.03.2025 | - | Initiale Version basierend auf Project Proposal |
