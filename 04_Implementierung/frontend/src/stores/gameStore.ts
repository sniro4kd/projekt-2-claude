import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { GameState, Position, Move } from '@/types/game';
import { gameService } from '@/services/gameService';

export const useGameStore = defineStore('game', () => {
  // State
  const gameState = ref<GameState | null>(null);
  const selectedPiece = ref<{ type: 'rabbit' | 'child'; index?: number } | null>(null);
  const validMoves = ref<Position[]>([]);
  const isLoading = ref(false);
  const error = ref<string | null>(null);
  const turnStartTime = ref<number>(0);
  const currentThinkingTime = ref<number>(0);
  const timerInterval = ref<number | null>(null);

  // Getters
  const gameId = computed(() => gameState.value?.gameId || null);

  const isPlayerTurn = computed(() => {
    if (!gameState.value) return false;
    return gameState.value.currentTurn === gameState.value.playerRole;
  });

  const isGameOver = computed(() => {
    if (!gameState.value) return false;
    return gameState.value.status !== 'playing';
  });

  const winner = computed((): 'rabbit' | 'children' | null => {
    if (!gameState.value) return null;
    if (gameState.value.status === 'rabbitwins') return 'rabbit';
    if (gameState.value.status === 'childrenwin') return 'children';
    return null;
  });

  const playerWon = computed(() => {
    if (!gameState.value || !winner.value) return false;
    return winner.value === gameState.value.playerRole;
  });

  const formattedTime = computed(() => {
    const totalMs = (gameState.value?.playerThinkingTimeMs || 0) + currentThinkingTime.value;
    const seconds = Math.floor(totalMs / 1000);
    const minutes = Math.floor(seconds / 60);
    const remainingSeconds = seconds % 60;
    return `${minutes.toString().padStart(2, '0')}:${remainingSeconds.toString().padStart(2, '0')}`;
  });

  // Actions
  async function createGame(playerRole: 'rabbit' | 'children') {
    isLoading.value = true;
    error.value = null;

    try {
      gameState.value = await gameService.createGame(playerRole);
      selectedPiece.value = null;
      validMoves.value = [];
      currentThinkingTime.value = 0;

      if (isPlayerTurn.value) {
        startTimer();
      }
    } catch (e: unknown) {
      error.value = e instanceof Error ? e.message : 'Failed to create game';
    } finally {
      isLoading.value = false;
    }
  }

  async function selectPiece(type: 'rabbit' | 'child', index?: number) {
    if (!gameState.value || !isPlayerTurn.value) return;

    // Check if this piece belongs to the player
    if (gameState.value.playerRole === 'rabbit' && type !== 'rabbit') return;
    if (gameState.value.playerRole === 'children' && type !== 'child') return;

    selectedPiece.value = { type, index };

    try {
      validMoves.value = await gameService.getValidMoves(
        gameState.value.gameId,
        type,
        index
      );
    } catch {
      validMoves.value = [];
    }
  }

  async function makeMove(to: Position) {
    if (!gameState.value || !selectedPiece.value || !isPlayerTurn.value) return;

    isLoading.value = true;
    error.value = null;
    stopTimer();

    try {
      const response = await gameService.makeMove(gameState.value.gameId, {
        pieceType: selectedPiece.value.type,
        pieceIndex: selectedPiece.value.index,
        to,
        thinkingTimeMs: currentThinkingTime.value
      });

      if (response.success && response.gameState) {
        gameState.value = response.gameState;
        clearSelection();
        currentThinkingTime.value = 0;

        if (isPlayerTurn.value && !isGameOver.value) {
          startTimer();
        }
      } else {
        error.value = response.error || 'Move failed';
      }
    } catch (e: unknown) {
      error.value = e instanceof Error ? e.message : 'Failed to make move';
    } finally {
      isLoading.value = false;
    }
  }

  function clearSelection() {
    selectedPiece.value = null;
    validMoves.value = [];
  }

  function startTimer() {
    turnStartTime.value = Date.now();
    if (timerInterval.value) {
      clearInterval(timerInterval.value);
    }
    timerInterval.value = window.setInterval(() => {
      currentThinkingTime.value = Date.now() - turnStartTime.value;
    }, 100);
  }

  function stopTimer() {
    if (timerInterval.value) {
      clearInterval(timerInterval.value);
      timerInterval.value = null;
    }
  }

  function resetGame() {
    stopTimer();
    gameState.value = null;
    selectedPiece.value = null;
    validMoves.value = [];
    isLoading.value = false;
    error.value = null;
    currentThinkingTime.value = 0;
  }

  function isPositionValidMove(pos: Position): boolean {
    return validMoves.value.some(m => m.x === pos.x && m.y === pos.y);
  }

  function isPositionSelected(pos: Position): boolean {
    if (!selectedPiece.value || !gameState.value) return false;

    if (selectedPiece.value.type === 'rabbit') {
      return gameState.value.rabbit.x === pos.x && gameState.value.rabbit.y === pos.y;
    } else if (selectedPiece.value.index !== undefined) {
      const child = gameState.value.children[selectedPiece.value.index];
      return child.x === pos.x && child.y === pos.y;
    }
    return false;
  }

  return {
    // State
    gameState,
    selectedPiece,
    validMoves,
    isLoading,
    error,
    currentThinkingTime,
    // Getters
    gameId,
    isPlayerTurn,
    isGameOver,
    winner,
    playerWon,
    formattedTime,
    // Actions
    createGame,
    selectPiece,
    makeMove,
    clearSelection,
    resetGame,
    isPositionValidMove,
    isPositionSelected
  };
});
