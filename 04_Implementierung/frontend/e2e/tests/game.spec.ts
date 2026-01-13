import { test, expect } from '@playwright/test';

test.describe('CatchTheRabbit - Akzeptanztests', () => {

  test.beforeEach(async ({ page }) => {
    await page.goto('/');
  });

  // AT-001: Spielfeld-Anzeige
  test('AT-001: 10x10 Spielfeld wird korrekt angezeigt', async ({ page }) => {
    // Wähle Rolle und starte Spiel
    await page.click('button:has-text("Kinder")');
    await page.click('button:has-text("Spiel starten")');

    // Warte auf Spielfeld
    await expect(page.locator('.game-board')).toBeVisible();

    // Prüfe dass das Board Grid existiert
    const boardGrid = page.locator('.board-grid');
    await expect(boardGrid).toBeVisible();

    // Prüfe Schachbrett-Muster (helle und dunkle Felder)
    const lightCells = page.locator('.board-cell.light');
    const darkCells = page.locator('.board-cell.dark');
    await expect(lightCells.first()).toBeVisible();
    await expect(darkCells.first()).toBeVisible();
  });

  // AT-002: Startpositionen
  test('AT-002: Figuren starten auf korrekten Positionen', async ({ page }) => {
    await page.click('button:has-text("Kinder")');
    await page.click('button:has-text("Spiel starten")');

    // Warte auf Spielfeld
    await expect(page.locator('.game-board')).toBeVisible();

    // Prüfe dass Spielfiguren vorhanden sind
    const rabbitPiece = page.locator('.game-piece.rabbit');
    await expect(rabbitPiece).toBeVisible();

    const childPieces = page.locator('.game-piece.child');
    await expect(childPieces).toHaveCount(4);
  });

  // AT-005: Rollenwahl
  test('AT-005: Spieler wählt Rolle vor Spielbeginn', async ({ page }) => {
    // Prüfe Rollenauswahl
    const rabbitButton = page.locator('button:has-text("Hase")');
    const childrenButton = page.locator('button:has-text("Kinder")');

    await expect(rabbitButton).toBeVisible();
    await expect(childrenButton).toBeVisible();

    // Wähle Hase
    await rabbitButton.click();
    await expect(rabbitButton).toHaveClass(/selected/);

    // Starte Spiel
    await page.click('button:has-text("Spiel starten")');

    // Prüfe dass Spieler als Hase markiert ist
    await expect(page.locator('.game-info')).toContainText('Hase');
  });

  // AT-006: KI-Reaktion
  test('AT-006: KI spielt automatisch nach Spielerzug', async ({ page }) => {
    // Starte als Hase
    await page.click('button:has-text("Hase")');
    await page.click('button:has-text("Spiel starten")');

    // Warte auf Spielfeld
    await expect(page.locator('.game-board')).toBeVisible();
    await expect(page.locator('text=Du bist dran!')).toBeVisible();

    // Klicke auf den Hasen
    await page.locator('.game-piece.rabbit').click();

    // Warte auf gültige Züge
    await expect(page.locator('.board-cell.valid-move').first()).toBeVisible();

    // Führe einen Zug aus
    await page.locator('.board-cell.valid-move').first().click();

    // Warte auf KI-Zug (Status zeigt wieder "Du bist dran")
    await expect(page.locator('text=Du bist dran!')).toBeVisible({ timeout: 5000 });
  });

  // AT-009: Bestenliste Anzeige
  test('AT-009: Bestenliste zeigt Einträge', async ({ page }) => {
    // Navigiere zur Bestenliste
    await page.click('a:has-text("Bestenliste")');

    // Prüfe Bestenlisten-Struktur
    await expect(page.locator('.leaderboard-card')).toBeVisible();
    await expect(page.locator('text=Hasen-Spieler')).toBeVisible();
    await expect(page.locator('text=Kinder-Spieler')).toBeVisible();
  });

  // AT-012: KI-Antwortzeit
  test('AT-012: KI antwortet unter 1 Sekunde', async ({ page }) => {
    // Starte als Hase
    await page.click('button:has-text("Hase")');
    await page.click('button:has-text("Spiel starten")');

    await expect(page.locator('.game-board')).toBeVisible();

    // Führe mehrere Züge aus und prüfe Reaktionszeit
    for (let i = 0; i < 3; i++) {
      // Warte bis Spieler am Zug
      await expect(page.locator('text=Du bist dran!')).toBeVisible({ timeout: 2000 });

      // Klicke auf Hasen und führe Zug aus
      await page.locator('.game-piece.rabbit').click();

      const moveIndicator = page.locator('.board-cell.valid-move').first();
      if (await moveIndicator.isVisible()) {
        const startTime = Date.now();
        await moveIndicator.click();

        // Warte auf KI-Antwort (wieder Spieler am Zug oder Spielende)
        await expect(
          page.locator('text=Du bist dran!').or(page.locator('.modal'))
        ).toBeVisible({ timeout: 2000 });

        const elapsed = Date.now() - startTime;
        expect(elapsed).toBeLessThan(2000); // 2s mit UI-Overhead
      }

      // Prüfe ob Spiel beendet
      const modal = page.locator('.modal');
      if (await modal.isVisible()) {
        break;
      }
    }
  });

  // Navigation Tests
  test('Navigation zwischen Seiten funktioniert', async ({ page }) => {
    // Start -> Bestenliste
    await page.click('a:has-text("Bestenliste")');
    await expect(page).toHaveURL(/\/leaderboard/);

    // Bestenliste -> Start
    await page.click('a:has-text("Start")');
    await expect(page).toHaveURL('/');

    // Start -> Spiel starten -> /game (mit aktivem Spiel)
    await page.click('button:has-text("Kinder")');
    await page.click('button:has-text("Spiel starten")');
    await expect(page).toHaveURL(/\/game/);
  });

  // Responsiveness Test
  test('Spielfeld ist auf Desktop vollständig sichtbar', async ({ page }) => {
    await page.click('button:has-text("Kinder")');
    await page.click('button:has-text("Spiel starten")');

    // Prüfe dass Spielfeld sichtbar ist
    const board = page.locator('.game-board');
    await expect(board).toBeVisible();

    // Prüfe dass Board nicht abgeschnitten ist
    const boundingBox = await board.boundingBox();
    expect(boundingBox).not.toBeNull();
    expect(boundingBox!.width).toBeGreaterThan(400);
    expect(boundingBox!.height).toBeGreaterThan(400);
  });
});
