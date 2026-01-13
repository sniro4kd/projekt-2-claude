export type PlayerRole = 'rabbit' | 'children';
export type GameStatus = 'playing' | 'rabbitwins' | 'childrenwin';

export interface Position {
  x: number;
  y: number;
}

export interface Move {
  pieceType: 'rabbit' | 'child';
  pieceIndex?: number;
  from: Position;
  to: Position;
}

export interface GameState {
  gameId: string;
  playerRole: PlayerRole;
  currentTurn: 'rabbit' | 'children';
  rabbit: Position;
  children: Position[];
  status: GameStatus;
  playerThinkingTimeMs: number;
}

export interface LeaderboardEntry {
  rank: number;
  nickname: string;
  thinkingTimeMs: number;
}

export interface Leaderboard {
  rabbit: LeaderboardEntry[];
  children: LeaderboardEntry[];
}

export interface MoveResponse {
  success: boolean;
  gameState?: GameState;
  aiMove?: Move;
  error?: string;
}

export interface CreateGameRequest {
  playerRole: 'rabbit' | 'children';
}

export interface MakeMoveRequest {
  pieceType: 'rabbit' | 'child';
  pieceIndex?: number;
  to: Position;
  thinkingTimeMs: number;
}
