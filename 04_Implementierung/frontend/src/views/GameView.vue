<script setup lang="ts">
import { computed, onMounted, onUnmounted, ref, watch } from 'vue';
import { useRouter } from 'vue-router';
import { useGameStore } from '@/stores/gameStore';
import { useLeaderboardStore } from '@/stores/leaderboardStore';
import GameBoard from '@/components/game/GameBoard.vue';
import GameInfo from '@/components/game/GameInfo.vue';
import GameOverModal from '@/components/game/GameOverModal.vue';

const router = useRouter();
const gameStore = useGameStore();
const leaderboardStore = useLeaderboardStore();

const showGameOverModal = ref(false);
const submittedRank = ref<number | null>(null);

const hasGame = computed(() => gameStore.gameState !== null);
const isGameOver = computed(() =>
  gameStore.gameState?.status === 'rabbitwins' ||
  gameStore.gameState?.status === 'childrenwin'
);
const playerWon = computed(() => {
  if (!gameStore.gameState) return false;
  const status = gameStore.gameState.status;
  const playerRole = gameStore.gameState.playerRole;
  return (status === 'rabbitwins' && playerRole === 'rabbit') ||
         (status === 'childrenwin' && playerRole === 'children');
});

watch(isGameOver, (gameOver) => {
  if (gameOver) {
    showGameOverModal.value = true;
  }
});

async function handleSubmitScore(nickname: string) {
  if (gameStore.gameId) {
    const rank = await leaderboardStore.submitScore(gameStore.gameId, nickname);
    submittedRank.value = rank;
  }
}

function handleNewGame() {
  showGameOverModal.value = false;
  submittedRank.value = null;
  router.push('/');
}

function handleViewLeaderboard() {
  showGameOverModal.value = false;
  router.push('/leaderboard');
}

onMounted(() => {
  if (!hasGame.value) {
    router.push('/');
  }
});

onUnmounted(() => {
  // Clean up when leaving the game view
});
</script>

<template>
  <div class="game-view" v-if="hasGame">
    <div class="game-container">
      <GameInfo />
      <GameBoard />
    </div>
    <GameOverModal
      v-if="showGameOverModal"
      :player-won="playerWon"
      :game-status="gameStore.gameState?.status || 'playing'"
      :player-role="gameStore.gameState?.playerRole || 'children'"
      :ai-thinking-time="gameStore.gameState?.playerThinkingTimeMs || 0"
      :submitted-rank="submittedRank"
      @submit-score="handleSubmitScore"
      @new-game="handleNewGame"
      @view-leaderboard="handleViewLeaderboard"
    />
  </div>
  <div v-else class="loading">
    <p>Lade Spiel...</p>
  </div>
</template>

<style scoped>
.game-view {
  width: 100%;
  max-width: 900px;
}

.game-container {
  display: flex;
  flex-direction: column;
  gap: 20px;
  align-items: center;
}

.loading {
  text-align: center;
  font-size: 18px;
  color: #666;
}
</style>
