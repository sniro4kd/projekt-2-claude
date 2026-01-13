<script setup lang="ts">
import { computed } from 'vue';
import { useGameStore } from '@/stores/gameStore';
import BoardCell from './BoardCell.vue';
import GamePiece from './GamePiece.vue';

const gameStore = useGameStore();

const boardSize = 10;

const rabbitPosition = computed(() => gameStore.gameState?.rabbit);
const childrenPositions = computed(() => gameStore.gameState?.children || []);
const validMoves = computed(() => gameStore.validMoves);
const selectedPiece = computed(() => gameStore.selectedPiece);
const isPlayerTurn = computed(() => gameStore.isPlayerTurn);
const playerRole = computed(() => gameStore.gameState?.playerRole);

function isValidMove(row: number, col: number): boolean {
  // row maps to y, col maps to x
  return validMoves.value.some(m => m.y === row && m.x === col);
}

function isRabbitAt(row: number, col: number): boolean {
  // row maps to y, col maps to x
  return rabbitPosition.value?.y === row && rabbitPosition.value?.x === col;
}

function getChildIndexAt(row: number, col: number): number {
  // row maps to y, col maps to x
  return childrenPositions.value.findIndex(p => p.y === row && p.x === col);
}

function isSelected(row: number, col: number): boolean {
  if (!selectedPiece.value) return false;

  if (selectedPiece.value.type === 'rabbit') {
    return isRabbitAt(row, col);
  } else {
    const childIndex = selectedPiece.value.index;
    if (childIndex !== undefined) {
      const pos = childrenPositions.value[childIndex];
      return pos?.y === row && pos?.x === col;
    }
  }
  return false;
}

function handleCellClick(row: number, col: number) {
  if (!isPlayerTurn.value) return;

  // Wenn auf ein gültiges Zug-Feld geklickt wird
  if (isValidMove(row, col) && selectedPiece.value) {
    gameStore.makeMove({ x: col, y: row });
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
  grid-template-columns: repeat(10, 48px);
  grid-template-rows: repeat(10, 48px);
  gap: 2px;
  width: fit-content;
  height: fit-content;
}
</style>
