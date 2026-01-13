<script setup lang="ts">
import { computed } from 'vue';

const props = defineProps<{
  row: number;
  col: number;
  isValidMove: boolean;
  isSelected: boolean;
}>();

const emit = defineEmits<{
  click: [];
}>();

const isDark = computed(() => (props.row + props.col) % 2 === 1);
</script>

<template>
  <div
    class="board-cell"
    :class="{
      dark: isDark,
      light: !isDark,
      'valid-move': isValidMove,
      selected: isSelected
    }"
    @click="emit('click')"
  >
    <slot />
    <div v-if="isValidMove" class="move-indicator" />
  </div>
</template>

<style scoped>
.board-cell {
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: all 0.2s ease;
}

.board-cell.light {
  background-color: #f0d9b5;
}

.board-cell.dark {
  background-color: #b58863;
}

.board-cell:hover {
  filter: brightness(1.1);
}

.board-cell.valid-move {
  cursor: pointer;
}

.board-cell.valid-move::after {
  content: '';
  position: absolute;
  width: 100%;
  height: 100%;
  background-color: rgba(76, 175, 80, 0.3);
  pointer-events: none;
}

.board-cell.selected {
  box-shadow: inset 0 0 0 3px #ffeb3b;
}

.move-indicator {
  position: absolute;
  width: 30%;
  height: 30%;
  background-color: rgba(76, 175, 80, 0.6);
  border-radius: 50%;
  pointer-events: none;
}
</style>
