# ADR-002: Datenbank

| Attribut | Wert |
|----------|------|
| Status | Akzeptiert |
| Datum | 2025-03-21 |
| Entscheider | Entwicklungsteam |

## Kontext

Für die persistente Speicherung der Bestenlisten wird eine Datenbank benötigt. Die Anforderungen sind:
- Geringe Datenmenge (nur Bestenlisten mit Top 20 Einträgen)
- Einfaches Deployment (Docker)
- Self-Hosting durch Kunden muss möglich sein

## Entscheidung

**SQLite** wird als Datenbank verwendet.

Die Persistenzschicht wird durch ein **Interface abstrahiert**, sodass SQLite als eine Implementierung austauschbar bleibt.

## Begründung

- **Einfachheit**: SQLite benötigt keinen separaten Datenbankserver
- **Portabilität**: Datenbankdatei kann einfach kopiert/gesichert werden
- **Ausreichend**: Für die geringe Datenmenge (2 Listen à 20 Einträge) überdimensioniert
- **Docker-freundlich**: Kein zusätzlicher Container für Datenbankserver notwendig (optional dennoch separater Container für saubere Trennung)
- **Austauschbarkeit**: Durch Interface-Abstraktion kann später auf SQL Server, PostgreSQL o.ä. gewechselt werden

## Alternativen (verworfen)

| Alternative | Grund für Ablehnung |
|-------------|---------------------|
| SQL Server | Overhead für einfache Bestenlisten zu hoch |
| PostgreSQL | Zusätzliche Komplexität ohne Mehrwert |
| JSON-Datei | Keine echte Datenbank, Concurrent Access problematisch |

## Konsequenzen

### Positiv
- Minimaler Deployment-Aufwand
- Keine Datenbank-Administration notwendig
- Flexibilität durch Interface-Abstraktion

### Negativ
- Bei sehr hoher Last (>1000 gleichzeitige Schreibzugriffe) könnte SQLite zum Bottleneck werden
- Für den aktuellen Anwendungsfall irrelevant
