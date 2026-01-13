# ADR-004: Vorgehensmodell

| Attribut | Wert |
|----------|------|
| Status | Akzeptiert |
| Datum | 2025-03-21 |
| Entscheider | Entwicklungsteam |

## Kontext

Für die strukturierte Entwicklung des Projekts "CatchTheRabbit" wird ein Vorgehensmodell benötigt, das:
- Klare Phasen und Artefakte definiert
- Qualitätssicherung durch korrespondierende Testphasen gewährleistet
- Nachvollziehbarkeit und Dokumentation unterstützt

## Entscheidung

Das **V-Modell** wird als Vorgehensmodell verwendet.

```
    Anforderungsanalyse ─────────────────────────── Abnahmetest
           │                                            │
           ▼                                            ▲
    Systemspezifikation ─────────────────────── Systemtest
           │                                            │
           ▼                                            ▲
    Systementwurf ───────────────────────── Integrationstest
           │                                            │
           ▼                                            ▲
    Komponentenentwurf ────────────────── Komponententest
           │                                            │
           ▼                                            ▲
           └──────────► Implementierung ◄───────────────┘
```

## Phasen und Artefakte

| Phase | Ordner | Artefakte |
|-------|--------|-----------|
| Anforderungsanalyse | `01_Anforderungen/` | Lastenheft |
| Systemspezifikation | `02_Spezifikation/` | Pflichtenheft, Use Cases |
| Systementwurf | `03_Entwurf/` | Architektur, API-Spezifikation, Datenmodell |
| Komponentenentwurf | `03_Entwurf/` | Komponentendiagramme, Klassendiagramme |
| Implementierung | `04_Implementierung/` | Quellcode (frontend/, backend/) |
| Komponententest | `05_Test/` | Unit-Tests, Testberichte |
| Integrationstest | `05_Test/` | Integrationstests, Testberichte |
| Systemtest | `05_Test/` | Systemtests, Testberichte |
| Abnahmetest | `05_Test/` | Abnahmeprotokoll |

## Korrespondenzen (Verifikation & Validierung)

| Linke Seite | Rechte Seite | Prüfung |
|-------------|--------------|---------|
| Lastenheft | Abnahmetest | Wurden alle Kundenanforderungen erfüllt? |
| Pflichtenheft | Systemtest | Erfüllt das System die spezifizierten Funktionen? |
| Architektur | Integrationstest | Arbeiten die Komponenten korrekt zusammen? |
| Komponentenentwurf | Komponententest | Funktioniert jede Komponente isoliert korrekt? |

## Begründung

- **Strukturierte Qualitätssicherung**: Jede Entwicklungsphase hat eine korrespondierende Testphase
- **Klare Artefakte**: Definierte Dokumente pro Phase
- **Nachvollziehbarkeit**: Anforderungen können bis zum Test zurückverfolgt werden
- **Passend für Projektgröße**: Überschaubares Projekt mit klaren Anforderungen

## Alternativen (verworfen)

| Alternative | Grund für Ablehnung |
|-------------|---------------------|
| Scrum/Agile | Für kleines Team mit klaren Anforderungen zu viel Overhead |
| Wasserfall | Keine explizite Testphasen-Korrespondenz |
| Kanban | Zu wenig Struktur für Dokumentationsanforderungen |

## Konsequenzen

### Positiv
- Klare Struktur und Meilensteine
- Qualitätssicherung durch korrespondierende Tests
- Gute Dokumentation für spätere Wartung

### Negativ
- Weniger flexibel bei Anforderungsänderungen
- Höherer initialer Dokumentationsaufwand
