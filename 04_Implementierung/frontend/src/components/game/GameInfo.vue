<script setup lang="ts">
import { computed } from 'vue';
import { useGameStore } from '@/stores/gameStore';
import { useRouter } from 'vue-router';

const gameStore = useGameStore();
const router = useRouter();

const playerRole = computed(() => gameStore.gameState?.playerRole);
const isPlayerTurn = computed(() => gameStore.isPlayerTurn);
const moveCount = computed(() => gameStore.gameState?.moveCount || 0);
const aiThinkingTime = computed(() => gameStore.gameState?.aiTotalThinkingTime || 0);
const status = computed(() => gameStore.gameState?.status);
const error = computed(() => gameStore.error);

const statusText = computed(() => {
  if (status.value === 'rabbitWins') return 'Der Hase hat gewonnen!';
  if (status.value === 'childrenWin') return 'Die Kinder haben gewonnen!';
  if (isPlayerTurn.value) return 'Du bist dran!';
  return 'KI denkt nach...';
});

const roleText = computed(() => {
  return playerRole.value === 'rabbit' ? 'Hase' : 'Kinder';
});

function formatTime(ms: number): string {
  if (ms < 1000) return `${ms} ms`;
  return `${(ms / 1000).toFixed(2)} s`;
}

function handleGiveUp() {
  if (confirm('Möchtest du wirklich aufgeben?')) {
    gameStore.giveUp();
  }
}

function goHome() {
  router.push('/');
}
</script>

<template>
  <div class="game-info">
    <div class="info-row">
      <div class="info-item">
        <span class="label">Du spielst als</span>
        <span class="value role">
          {{ playerRole === 'rabbit' ? '🐰' : '👧👦' }} {{ roleText }}
        </span>
      </div>
      <div class="info-item">
        <span class="label">Züge</span>
        <span class="value">{{ moveCount }}</span>
      </div>
      <div class="info-item">
        <span class="label">KI-Denkzeit</span>
        <span class="value time">{{ formatTime(aiThinkingTime) }}</span>
      </div>
    </div>

    <div class="status-row">
      <div class="status" :class="{ 'player-turn': isPlayerTurn, 'ai-turn': !isPlayerTurn && status === 'playing' }">
        {{ statusText }}
        <span v-if="!isPlayerTurn && status === 'playing'" class="thinking-dots">
          <span>.</span><span>.</span><span>.</span>
        </span>
      </div>
    </div>

    <div v-if="error" class="error">
      {{ error }}
    </div>

    <div class="actions" v-if="status === 'playing'">
      <button class="btn-secondary" @click="goHome">Abbrechen</button>
      <button class="btn-danger" @click="handleGiveUp">Aufgeben</button>
    </div>
  </div>
</template>

<style scoped>
.game-info {
  background: white;
  border-radius: 15px;
  padding: 20px 30px;
  box-shadow: 0 5px 20px rgba(0, 0, 0, 0.1);
  width: 100%;
  max-width: 500px;
}

.info-row {
  display: flex;
  justify-content: space-between;
  gap: 20px;
  margin-bottom: 15px;
}

.info-item {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.label {
  font-size: 12px;
  color: #666;
  text-transform: uppercase;
  margin-bottom: 5px;
}

.value {
  font-size: 18px;
  font-weight: 600;
  color: #333;
}

.value.role {
  color: #4caf50;
}

.value.time {
  font-family: monospace;
  color: #2196f3;
}

.status-row {
  text-align: center;
  margin-bottom: 15px;
}

.status {
  font-size: 20px;
  font-weight: 600;
  padding: 10px 20px;
  border-radius: 10px;
  display: inline-block;
}

.status.player-turn {
  background: linear-gradient(135deg, #e8f5e9 0%, #c8e6c9 100%);
  color: #2e7d32;
}

.status.ai-turn {
  background: linear-gradient(135deg, #fff3e0 0%, #ffe0b2 100%);
  color: #e65100;
}

.thinking-dots span {
  animation: blink 1.4s infinite;
}

.thinking-dots span:nth-child(2) {
  animation-delay: 0.2s;
}

.thinking-dots span:nth-child(3) {
  animation-delay: 0.4s;
}

@keyframes blink {
  0%, 80%, 100% { opacity: 0; }
  40% { opacity: 1; }
}

.error {
  background: #ffebee;
  color: #c62828;
  padding: 10px;
  border-radius: 8px;
  text-align: center;
  margin-bottom: 15px;
}

.actions {
  display: flex;
  justify-content: center;
  gap: 15px;
}

.btn-secondary {
  padding: 8px 20px;
  border: 2px solid #9e9e9e;
  background: white;
  color: #666;
  border-radius: 20px;
  font-weight: 500;
  transition: all 0.3s ease;
}

.btn-secondary:hover {
  background: #f5f5f5;
}

.btn-danger {
  padding: 8px 20px;
  border: none;
  background: linear-gradient(135deg, #ef5350 0%, #c62828 100%);
  color: white;
  border-radius: 20px;
  font-weight: 500;
  transition: all 0.3s ease;
}

.btn-danger:hover {
  transform: translateY(-2px);
  box-shadow: 0 3px 10px rgba(198, 40, 40, 0.3);
}
</style>
