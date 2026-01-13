import axios from 'axios';
import type { GameState, MoveResponse, CreateGameRequest, MakeMoveRequest, Position } from '@/types/game';

const API_BASE = '/api';

export const gameService = {
  async createGame(playerRole: 'rabbit' | 'children'): Promise<GameState> {
    const response = await axios.post<GameState>(`${API_BASE}/game/new`, {
      playerRole
    } as CreateGameRequest);
    return response.data;
  },

  async makeMove(gameId: string, request: MakeMoveRequest): Promise<MoveResponse> {
    const response = await axios.post<MoveResponse>(
      `${API_BASE}/game/${gameId}/move`,
      request
    );
    return response.data;
  },

  async getGame(gameId: string): Promise<GameState> {
    const response = await axios.get<GameState>(`${API_BASE}/game/${gameId}`);
    return response.data;
  },

  async getValidMoves(gameId: string, pieceType: string, pieceIndex?: number): Promise<Position[]> {
    const params = new URLSearchParams({ pieceType });
    if (pieceIndex !== undefined) {
      params.append('pieceIndex', pieceIndex.toString());
    }
    const response = await axios.get<Position[]>(
      `${API_BASE}/game/${gameId}/valid-moves?${params}`
    );
    return response.data;
  }
};
