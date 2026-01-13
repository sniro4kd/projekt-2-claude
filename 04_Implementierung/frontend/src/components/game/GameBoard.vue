<script setup lang="ts">
import { computed } from 'vue';
import { useGameStore } from '@/stores/gameStore';
import BoardCell from './BoardCell.vue';
import GamePiece from './GamePiece.vue';
import type { Position } from '@/types/game';

const gameStore = useGameStore();

const boardSize = 10;

const rabbitPosition = computed(() => gameStore.gameState?.rabbitPosition);
const childrenPositions = computed(() => gameStore.gameState?.childrenPositions || []);
const validMoves = computed(() => gameStore.validMoves);
const selectedPiece = computed(() => gameStore.selectedPiece);
const isPlayerTurn = computed(() => gameStore.gameState?.isPlayerTurn);
const playerRole = computed(() => gameStore.gameState?.playerRole);

function isValidMove(row: number, col: number): boolean {
  return validMoves.value.some(m => m.row === row && m.col === col);
}

function isRabbitAt(row: number, col: number): boolean {
  return rabbitPosition.value?.row === row && rabbitPosition.value?.col === col;
}

function getChildIndexAt(row: number, col: number): number {
  return childrenPositions.value.findIndex(p => p.row === row && p.col === col);
}

function isSelected(row: number, col: number): boolean {
  if (!selectedPiece.value) return false;

  if (selectedPiece.value.type === 'rabbit') {
    return isRabbitAt(row, col);
  } else {
    const childIndex = selectedPiece.value.index;
    if (childIndex !== undefined) {
      const pos = childrenPositions.value[childIndex];
      return pos?.row === row && pos?.col === col;
    }
  }
  return false;
}

function handleCellClick(row: number, col: number) {
  if (!isPlayerTurn.value) return;

  // Wenn auf ein gültiges Zug-Feld geklickt wird
  if (isValidMove(row, col) && selectedPiece.value) {
    const from = getSelectedPosition();
    if (from) {
      gameStore.makeMove({ row: from.row, col: from.col }, { row, col });
    }
    return;
  }

  // Wenn auf den Hasen geklickt wird (nur wenn Spieler Hase ist)
  if (isRabbitAt(row, col) && playerRole.value === 'rabbit') {
    gameStore.selectPiece('rabbit');
    return;
  }

  // Wenn auf ein Kind geklickt wird (nur wenn Spieler Kinder ist)
  const childIndex = getChildIndexAt(row, col);
  if (childIndex !== -1 && playerRole.value === 'children') {
    gameStore.selectPiece('child', childIndex);
    return;
  }

  // Sonst: Auswahl aufheben
  gameStore.clearSelection();
}

function getSelectedPosition(): Position | null {
  if (!selectedPiece.value) return null;

  if (selectedPiece.value.type === 'rabbit') {
    return rabbitPosition.value || null;
  } else {
    const childIndex = selectedPiece.value.index;
    if (childIndex !== undefined) {
      return childrenPositions.value[childIndex] || null;
    }
  }
  return null;
}
</script>

<template>
  <div class="game-board">
    <div class="board-grid">
      <template v-for="row in boardSize" :key="row">
        <BoardCell
          v-for="col in boardSize"
          :key="`${row}-${col}`"
          :row="row - 1"
          :col="col - 1"
          :is-valid-move="isValidMove(row - 1, col - 1)"
          :is-selected="isSelected(row - 1, col - 1)"
          @click="handleCellClick(row - 1, col - 1)"
        >
          <GamePiece
            v-if="isRabbitAt(row - 1, col - 1)"
            type="rabbit"
            :is-selected="selectedPiece?.type === 'rabbit'"
          />
          <GamePiece
            v-else-if="getChildIndexAt(row - 1, col - 1) !== -1"
            type="child"
            :child-index="getChildIndexAt(row - 1, col - 1)"
            :is-selected="selectedPiece?.type === 'child' && selectedPiece?.index === getChildIndexAt(row - 1, col - 1)"
          />
        </BoardCell>
      </template>
    </div>
  </div>
</template>

<style scoped>
.game-board {
  background: #8b4513;
  padding: 15px;
  border-radius: 15px;
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.3);
}

.board-grid {
  display: grid;
  grid-template-columns: repeat(10, 1fr);
  gap: 2px;
  width: 500px;
  height: 500px;
}
</style>
