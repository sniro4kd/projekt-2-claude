<script setup lang="ts">
import { ref, computed } from 'vue';
import type { GameStatus, PlayerRole } from '@/types/game';

const props = defineProps<{
  playerWon: boolean;
  gameStatus: GameStatus;
  playerRole: PlayerRole;
  aiThinkingTime: number;
  submittedRank: number | null;
}>();

const emit = defineEmits<{
  submitScore: [nickname: string];
  newGame: [];
  viewLeaderboard: [];
}>();

const nickname = ref('');
const isSubmitting = ref(false);
const hasSubmitted = ref(false);

const title = computed(() => {
  if (props.playerWon) {
    return 'Herzlichen Glückwunsch!';
  }
  return 'Spiel beendet';
});

const message = computed(() => {
  if (props.gameStatus === 'rabbitWins') {
    if (props.playerRole === 'rabbit') {
      return 'Du hast als Hase gewonnen! Der Hase ist entkommen!';
    }
    return 'Der Hase ist entkommen! Die KI hat gewonnen.';
  }
  if (props.gameStatus === 'childrenWin') {
    if (props.playerRole === 'children') {
      return 'Du hast als Kinder gewonnen! Der Hase wurde gefangen!';
    }
    return 'Der Hase wurde gefangen! Die KI hat gewonnen.';
  }
  return '';
});

function formatTime(ms: number): string {
  if (ms < 1000) return `${ms} ms`;
  return `${(ms / 1000).toFixed(2)} s`;
}

async function submitScore() {
  if (!nickname.value.trim()) return;

  isSubmitting.value = true;
  emit('submitScore', nickname.value.trim());
  hasSubmitted.value = true;
  isSubmitting.value = false;
}
</script>

<template>
  <div class="modal-overlay">
    <div class="modal" :class="{ won: playerWon, lost: !playerWon }">
      <div class="modal-header">
        <h2>{{ title }}</h2>
        <p class="message">{{ message }}</p>
      </div>

      <div class="stats">
        <div class="stat">
          <span class="stat-label">KI-Denkzeit</span>
          <span class="stat-value">{{ formatTime(aiThinkingTime) }}</span>
        </div>
      </div>

      <div v-if="playerWon && !hasSubmitted" class="submit-section">
        <p class="submit-info">
          Trage deinen Namen ein, um in die Bestenliste zu kommen!
        </p>
        <div class="input-group">
          <input
            v-model="nickname"
            type="text"
            placeholder="Dein Nickname"
            maxlength="20"
            @keyup.enter="submitScore"
          />
          <button
            class="btn-submit"
            @click="submitScore"
            :disabled="!nickname.trim() || isSubmitting"
          >
            {{ isSubmitting ? 'Wird gespeichert...' : 'Eintragen' }}
          </button>
        </div>
      </div>

      <div v-if="submittedRank !== null" class="rank-info">
        <p>Du bist auf Platz <strong>{{ submittedRank }}</strong> der Bestenliste!</p>
      </div>

      <div class="actions">
        <button class="btn-primary" @click="emit('newGame')">
          Neues Spiel
        </button>
        <button class="btn-secondary" @click="emit('viewLeaderboard')">
          Bestenliste ansehen
        </button>
      </div>
    </div>
  </div>
</template>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.6);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 100;
}

.modal {
  background: white;
  border-radius: 20px;
  padding: 40px;
  max-width: 450px;
  width: 90%;
  text-align: center;
  animation: slideIn 0.3s ease;
}

@keyframes slideIn {
  from {
    transform: translateY(-20px);
    opacity: 0;
  }
  to {
    transform: translateY(0);
    opacity: 1;
  }
}

.modal.won {
  border-top: 5px solid #4caf50;
}

.modal.lost {
  border-top: 5px solid #ff9800;
}

.modal-header h2 {
  font-size: 28px;
  margin-bottom: 10px;
}

.modal.won .modal-header h2 {
  color: #2e7d32;
}

.modal.lost .modal-header h2 {
  color: #e65100;
}

.message {
  color: #666;
  font-size: 16px;
  margin-bottom: 25px;
}

.stats {
  background: #f5f5f5;
  padding: 15px;
  border-radius: 10px;
  margin-bottom: 25px;
}

.stat {
  display: flex;
  flex-direction: column;
  gap: 5px;
}

.stat-label {
  font-size: 12px;
  color: #666;
  text-transform: uppercase;
}

.stat-value {
  font-size: 24px;
  font-weight: bold;
  color: #2196f3;
  font-family: monospace;
}

.submit-section {
  margin-bottom: 25px;
}

.submit-info {
  color: #666;
  margin-bottom: 15px;
}

.input-group {
  display: flex;
  gap: 10px;
}

.input-group input {
  flex: 1;
  padding: 12px 15px;
  border: 2px solid #e0e0e0;
  border-radius: 10px;
  font-size: 16px;
  outline: none;
  transition: border-color 0.3s ease;
}

.input-group input:focus {
  border-color: #4caf50;
}

.btn-submit {
  padding: 12px 20px;
  background: linear-gradient(135deg, #4caf50 0%, #2e7d32 100%);
  color: white;
  border: none;
  border-radius: 10px;
  font-weight: 600;
  transition: all 0.3s ease;
}

.btn-submit:hover:not(:disabled) {
  transform: translateY(-2px);
}

.btn-submit:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.rank-info {
  background: linear-gradient(135deg, #e8f5e9 0%, #c8e6c9 100%);
  padding: 15px;
  border-radius: 10px;
  margin-bottom: 25px;
}

.rank-info p {
  color: #2e7d32;
  font-size: 18px;
}

.rank-info strong {
  font-size: 24px;
}

.actions {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.btn-primary {
  padding: 15px;
  background: linear-gradient(135deg, #4caf50 0%, #2e7d32 100%);
  color: white;
  border: none;
  border-radius: 25px;
  font-size: 18px;
  font-weight: 600;
  transition: all 0.3s ease;
}

.btn-primary:hover {
  transform: translateY(-2px);
  box-shadow: 0 5px 20px rgba(76, 175, 80, 0.4);
}

.btn-secondary {
  padding: 12px;
  background: white;
  color: #666;
  border: 2px solid #e0e0e0;
  border-radius: 25px;
  font-size: 16px;
  font-weight: 500;
  transition: all 0.3s ease;
}

.btn-secondary:hover {
  border-color: #4caf50;
  color: #4caf50;
}
</style>
