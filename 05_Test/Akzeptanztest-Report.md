# Akzeptanztest-Report
## Catch The Rabbit - End-to-End Test Ergebnisse

**Testdatum:** 2026-01-13
**Testumgebung:** Windows 10, Node.js, Playwright
**Browser:** Chromium (headless)

---

## 1. Zusammenfassung

| Metrik | Wert |
|--------|------|
| **Gesamtanzahl E2E Tests** | 12 |
| **Status** | Bereit zur Ausführung |
| **Framework** | Playwright 1.40.0 |
| **Browser** | Chromium |

---

## 2. Definierte Testfälle

### 2.1 Spielstart Tests (AT-001 bis AT-003)

| Test-ID | Testfall | Erwartetes Ergebnis |
|---------|----------|---------------------|
| AT-001 | Startseite laden | Seite zeigt Rollenauswahl |
| AT-002 | Rolle "Hase" wählen | Spielfeld mit Hasen-Perspektive |
| AT-003 | Rolle "Kinder" wählen | Spielfeld mit Kinder-Perspektive |

### 2.2 Spielzug Tests (AT-004 bis AT-006)

| Test-ID | Testfall | Erwartetes Ergebnis |
|---------|----------|---------------------|
| AT-004 | Figur auswählen | Gültige Züge werden hervorgehoben |
| AT-005 | Gültigen Zug ausführen | Figur bewegt sich, KI reagiert |
| AT-006 | Ungültigen Zug versuchen | Zug wird abgelehnt |

### 2.3 KI Tests (AT-007 bis AT-008)

| Test-ID | Testfall | Erwartetes Ergebnis |
|---------|----------|---------------------|
| AT-007 | KI macht Zug | KI-Zug innerhalb 1 Sekunde |
| AT-008 | KI Reaktion auf Spielerzug | Sofortige KI-Antwort |

### 2.4 Spielende Tests (AT-009 bis AT-010)

| Test-ID | Testfall | Erwartetes Ergebnis |
|---------|----------|---------------------|
| AT-009 | Hase gewinnt | Siegmeldung "Hase gewinnt!" |
| AT-010 | Kinder gewinnen | Siegmeldung "Kinder gewinnen!" |

### 2.5 Bestenliste Tests (AT-011 bis AT-012)

| Test-ID | Testfall | Erwartetes Ergebnis |
|---------|----------|---------------------|
| AT-011 | Eintrag hinzufügen | Name in Bestenliste sichtbar |
| AT-012 | Bestenliste anzeigen | Sortierte Liste nach Zeit |

---

## 3. Playwright Konfiguration

```typescript
// playwright.config.ts
export default defineConfig({
  testDir: './e2e/tests',
  timeout: 30 * 1000,
  use: {
    baseURL: 'http://localhost:3000',
    trace: 'on-first-retry',
    screenshot: 'only-on-failure',
  },
  webServer: {
    command: 'npm run dev',
    url: 'http://localhost:3000',
    reuseExistingServer: !process.env.CI,
  },
});
```

---

## 4. Testskript Übersicht

Die E2E Tests befinden sich in:
```
04_Implementierung/frontend/e2e/tests/game.spec.ts
```

Enthaltene Tests:
- Startseite-Tests
- Rollenauswahl-Tests
- Spielzug-Tests
- KI-Interaktions-Tests
- Spielende-Tests
- Bestenlisten-Tests

---

## 5. Ausführungsanleitung

### 5.1 Voraussetzungen

1. Node.js installiert
2. Frontend-Dependencies installiert: `npm install`
3. Playwright installiert: `npx playwright install`

### 5.2 Tests ausführen

```bash
cd 04_Implementierung/frontend
npm run test:e2e
```

Oder mit Docker:
```bash
docker-compose up -d
npx playwright test
```

### 5.3 Test-Report generieren

```bash
npx playwright show-report
```

---

## 6. Bekannte Voraussetzungen

- Frontend muss unter `http://localhost:3000` laufen
- Backend muss unter `http://localhost:5000` laufen
- Browser-Installation erforderlich (Chromium)

---

## 7. Manuelle Akzeptanztests

Falls automatisierte Tests nicht ausführbar sind, können folgende manuelle Tests durchgeführt werden:

### Checkliste für manuelle Tests:

- [ ] Startseite lädt korrekt
- [ ] Rollenauswahl funktioniert
- [ ] Spielfeld wird korrekt angezeigt
- [ ] Figuren können bewegt werden
- [ ] KI reagiert auf Spielerzüge
- [ ] Spielende wird erkannt
- [ ] Bestenliste kann eingesehen werden
- [ ] Eintrag kann zur Bestenliste hinzugefügt werden

---

## 8. Fazit

Die E2E-Tests sind vollständig implementiert und bereit zur Ausführung. Sie decken alle kritischen Benutzerinteraktionen ab:

- Spielstart und Rollenauswahl
- Spielzüge und KI-Interaktion
- Spielende und Siegbedingungen
- Bestenlisten-Funktionalität

Zur vollständigen Testausführung ist eine laufende Frontend- und Backend-Instanz erforderlich.

---

**Erstellt von:** Automatisiertes Testsystem
**Version:** 1.0.0
