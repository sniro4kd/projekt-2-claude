# ADR-001: Technologie-Stack

| Attribut | Wert |
|----------|------|
| Status | Akzeptiert |
| Datum | 2025-03-21 |
| Entscheider | Product Owner (vorgegeben) |

## Kontext

ERP_Champion GmbH möchte, dass das Projekt "CatchTheRabbit" von den hauseigenen Entwicklern gewartet werden kann. Daher muss sich die Architektur am bestehenden Technologie-Stack des Unternehmens orientieren.

## Entscheidung

Der Technologie-Stack wird wie folgt festgelegt:

| Komponente | Technologie |
|------------|-------------|
| Frontend | Vue.js 3 |
| Backend | ASP.NET Core (C#) |
| API-Kommunikation | REST + SignalR |
| Containerisierung | Docker + docker-compose |
| Entwicklung Frontend | Visual Studio Code |
| Entwicklung Backend | Visual Studio |

## Begründung

- **Vue.js**: Vorgabe durch ERP_Champion (bestehender Stack)
- **ASP.NET Core**: Vorgabe durch ERP_Champion (bestehender Stack)
- **SignalR**: Ermöglicht bidirektionale Echtzeit-Kommunikation für Spielzüge
- **REST**: Standard-API für CRUD-Operationen (z.B. Bestenlisten)
- **Docker**: Gewährleistet einfaches Deployment und Self-Hosting durch Kunden

## Konsequenzen

### Positiv
- Wartbarkeit durch hauseigene Entwickler gewährleistet
- Bewährter, stabiler Stack
- Gute Dokumentation und Community-Support

### Negativ
- Weniger Flexibilität bei der Technologiewahl
- Höhere Einstiegshürde für Entwickler ohne .NET-Erfahrung

## Referenzen

- Project Proposal CatchTheRabbit v1.0, Seite 2 (Architekturdiagramm)
