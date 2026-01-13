# ADR-003: Projektdokumentation und Gesprächsverlauf

| Attribut | Wert |
|----------|------|
| Status | Akzeptiert |
| Datum | 2025-03-21 |
| Entscheider | Entwicklungsteam |

## Kontext

Der gesamte Entwicklungsprozess soll nachvollziehbar dokumentiert und in Git versioniert werden. Dies umfasst:
- Entscheidungen während der Entwicklung
- Gesprächsverläufe und Abstimmungen
- Änderungen an Anforderungen

## Entscheidung

Eine **kombinierte Dokumentationsstruktur** wird verwendet (angepasst für V-Modell):

```
projekt-2-claude/
├── 00_Projektmanagement/      # ADRs, Protokolle, Proposal
│   ├── entscheidungen/        # Architecture Decision Records (ADRs)
│   ├── Projektprotokoll.md
│   └── Proposal_*.pdf
├── 01_Anforderungen/          # Lastenheft
├── 02_Spezifikation/          # Pflichtenheft
├── 03_Entwurf/                # Architektur, Datenmodell
├── 04_Implementierung/        # Quellcode
├── 05_Test/                   # Testdokumentation
└── CHANGELOG.md               # Übersicht aller Änderungen
```

## Begründung

- **ADRs**: Standardformat für technische/architektonische Entscheidungen, gut durchsuchbar
- **Protokolle**: Ermöglichen Nachvollziehbarkeit des gesamten Prozesses
- **CHANGELOG**: Schneller Überblick über den Projektfortschritt
- **Git**: Versionierung ermöglicht Nachvollziehbarkeit aller Änderungen

## Konsequenzen

### Positiv
- Vollständige Nachvollziehbarkeit
- Strukturierte Dokumentation
- Einfache Navigation durch standardisierte Formate

### Negativ
- Dokumentationsaufwand während der Entwicklung
- Muss konsequent gepflegt werden

## Format-Vorgaben

### ADR-Dateinamen
`00_Projektmanagement/entscheidungen/ADR-XXX-kurzbeschreibung.md`

### Protokoll
Konsolidiert in `00_Projektmanagement/Projektprotokoll.md`
