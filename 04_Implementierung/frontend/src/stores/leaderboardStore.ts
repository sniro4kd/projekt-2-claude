import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { Leaderboard } from '@/types/game';
import { leaderboardService } from '@/services/leaderboardService';

export const useLeaderboardStore = defineStore('leaderboard', () => {
  const leaderboard = ref<Leaderboard | null>(null);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  async function fetchLeaderboard() {
    isLoading.value = true;
    error.value = null;

    try {
      leaderboard.value = await leaderboardService.getLeaderboard();
    } catch (e: unknown) {
      error.value = e instanceof Error ? e.message : 'Failed to fetch leaderboard';
    } finally {
      isLoading.value = false;
    }
  }

  async function submitScore(gameId: string, nickname: string): Promise<number | null> {
    isLoading.value = true;
    error.value = null;

    try {
      const response = await leaderboardService.submitScore(gameId, nickname);
      if (response.success && response.rank) {
        await fetchLeaderboard();
        return response.rank;
      }
      error.value = response.error || 'Failed to submit score';
      return null;
    } catch (e: unknown) {
      error.value = e instanceof Error ? e.message : 'Failed to submit score';
      return null;
    } finally {
      isLoading.value = false;
    }
  }

  return {
    leaderboard,
    isLoading,
    error,
    fetchLeaderboard,
    submitScore
  };
});
