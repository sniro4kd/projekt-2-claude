import axios from 'axios';
import type { Leaderboard, LeaderboardEntry } from '@/types/game';

const API_BASE = '/api';

interface AddEntryResponse {
  success: boolean;
  rank?: number;
  entry?: LeaderboardEntry;
  error?: string;
}

export const leaderboardService = {
  async getLeaderboard(): Promise<Leaderboard> {
    const response = await axios.get<Leaderboard>(`${API_BASE}/leaderboard`);
    return response.data;
  },

  async submitScore(gameId: string, nickname: string): Promise<AddEntryResponse> {
    const response = await axios.post<AddEntryResponse>(`${API_BASE}/leaderboard`, {
      gameId,
      nickname
    });
    return response.data;
  }
};
