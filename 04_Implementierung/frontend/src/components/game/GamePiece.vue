<script setup lang="ts">
import { computed } from 'vue';

const props = defineProps<{
  type: 'rabbit' | 'child';
  childIndex?: number;
  isSelected: boolean;
}>();

const childEmojis = ['👧', '👦', '👧', '👦'];
const emoji = computed(() => {
  if (props.type === 'rabbit') {
    return '🐰';
  }
  return childEmojis[props.childIndex ?? 0];
});
</script>

<template>
  <div
    class="game-piece"
    :class="{
      rabbit: type === 'rabbit',
      child: type === 'child',
      selected: isSelected
    }"
  >
    <span class="emoji">{{ emoji }}</span>
  </div>
</template>

<style scoped>
.game-piece {
  width: 80%;
  height: 80%;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 50%;
  transition: all 0.3s ease;
  z-index: 1;
}

.game-piece.rabbit {
  background: linear-gradient(135deg, #ff9800 0%, #f57c00 100%);
  box-shadow: 0 4px 10px rgba(255, 152, 0, 0.4);
}

.game-piece.child {
  background: linear-gradient(135deg, #4caf50 0%, #2e7d32 100%);
  box-shadow: 0 4px 10px rgba(76, 175, 80, 0.4);
}

.game-piece.selected {
  transform: scale(1.1);
  box-shadow: 0 0 15px 5px rgba(255, 235, 59, 0.6);
}

.emoji {
  font-size: 28px;
  user-select: none;
}
</style>
