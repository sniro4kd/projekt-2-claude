<script setup lang="ts">
import { onMounted, computed } from 'vue';
import { useLeaderboardStore } from '@/stores/leaderboardStore';
import { RouterLink } from 'vue-router';

const leaderboardStore = useLeaderboardStore();

const rabbitEntries = computed(() => leaderboardStore.leaderboard?.rabbitLeaderboard || []);
const childrenEntries = computed(() => leaderboardStore.leaderboard?.childrenLeaderboard || []);

function formatTime(ms: number): string {
  if (ms < 1000) {
    return `${ms} ms`;
  }
  return `${(ms / 1000).toFixed(2)} s`;
}

function formatDate(dateString: string): string {
  const date = new Date(dateString);
  return date.toLocaleDateString('de-DE', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric'
  });
}

onMounted(() => {
  leaderboardStore.fetchLeaderboard();
});
</script>

<template>
  <div class="leaderboard-view">
    <div class="leaderboard-card">
      <h1 class="title">Bestenliste</h1>
      <p class="subtitle">Die schnellsten Siege gegen die KI</p>

      <div v-if="leaderboardStore.isLoading" class="loading">
        Lade Bestenliste...
      </div>

      <div v-else-if="leaderboardStore.error" class="error">
        {{ leaderboardStore.error }}
      </div>

      <div v-else class="leaderboards">
        <div class="leaderboard-section">
          <h2 class="section-title">
            <span class="icon">🐰</span>
            Hasen-Spieler
          </h2>
          <div v-if="rabbitEntries.length === 0" class="empty">
            Noch keine Einträge vorhanden
          </div>
          <table v-else class="leaderboard-table">
            <thead>
              <tr>
                <th class="rank">#</th>
                <th class="name">Name</th>
                <th class="time">KI-Zeit</th>
                <th class="date">Datum</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(entry, index) in rabbitEntries" :key="entry.id">
                <td class="rank">
                  <span class="rank-badge" :class="{ gold: index === 0, silver: index === 1, bronze: index === 2 }">
                    {{ index + 1 }}
                  </span>
                </td>
                <td class="name">{{ entry.nickname }}</td>
                <td class="time">{{ formatTime(entry.aiThinkingTimeMs) }}</td>
                <td class="date">{{ formatDate(entry.createdAt) }}</td>
              </tr>
            </tbody>
          </table>
        </div>

        <div class="leaderboard-section">
          <h2 class="section-title">
            <span class="icon">👧👦</span>
            Kinder-Spieler
          </h2>
          <div v-if="childrenEntries.length === 0" class="empty">
            Noch keine Einträge vorhanden
          </div>
          <table v-else class="leaderboard-table">
            <thead>
              <tr>
                <th class="rank">#</th>
                <th class="name">Name</th>
                <th class="time">KI-Zeit</th>
                <th class="date">Datum</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(entry, index) in childrenEntries" :key="entry.id">
                <td class="rank">
                  <span class="rank-badge" :class="{ gold: index === 0, silver: index === 1, bronze: index === 2 }">
                    {{ index + 1 }}
                  </span>
                </td>
                <td class="name">{{ entry.nickname }}</td>
                <td class="time">{{ formatTime(entry.aiThinkingTimeMs) }}</td>
                <td class="date">{{ formatDate(entry.createdAt) }}</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      <div class="actions">
        <RouterLink to="/" class="back-btn">
          Zurück zum Start
        </RouterLink>
      </div>
    </div>
  </div>
</template>

<style scoped>
.leaderboard-view {
  max-width: 900px;
  width: 100%;
}

.leaderboard-card {
  background: white;
  border-radius: 20px;
  padding: 40px;
  box-shadow: 0 10px 40px rgba(0, 0, 0, 0.1);
}

.title {
  color: #2e7d32;
  font-size: 32px;
  text-align: center;
  margin-bottom: 5px;
}

.subtitle {
  text-align: center;
  color: #666;
  margin-bottom: 30px;
}

.loading, .error {
  text-align: center;
  padding: 40px;
  color: #666;
}

.error {
  color: #c62828;
}

.leaderboards {
  display: flex;
  gap: 30px;
}

.leaderboard-section {
  flex: 1;
}

.section-title {
  display: flex;
  align-items: center;
  gap: 10px;
  font-size: 20px;
  color: #333;
  margin-bottom: 15px;
  padding-bottom: 10px;
  border-bottom: 2px solid #e0e0e0;
}

.icon {
  font-size: 24px;
}

.empty {
  text-align: center;
  color: #999;
  padding: 30px;
  background: #f5f5f5;
  border-radius: 10px;
}

.leaderboard-table {
  width: 100%;
  border-collapse: collapse;
}

.leaderboard-table th,
.leaderboard-table td {
  padding: 12px 8px;
  text-align: left;
}

.leaderboard-table th {
  color: #666;
  font-weight: 600;
  font-size: 12px;
  text-transform: uppercase;
  border-bottom: 2px solid #e0e0e0;
}

.leaderboard-table td {
  border-bottom: 1px solid #f0f0f0;
}

.leaderboard-table tbody tr:hover {
  background: #f9f9f9;
}

.rank {
  width: 50px;
  text-align: center !important;
}

.rank-badge {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 28px;
  height: 28px;
  border-radius: 50%;
  background: #e0e0e0;
  font-weight: bold;
  font-size: 14px;
}

.rank-badge.gold {
  background: linear-gradient(135deg, #ffd700 0%, #ffb300 100%);
  color: #5d4037;
}

.rank-badge.silver {
  background: linear-gradient(135deg, #e0e0e0 0%, #bdbdbd 100%);
  color: #424242;
}

.rank-badge.bronze {
  background: linear-gradient(135deg, #cd7f32 0%, #8b4513 100%);
  color: white;
}

.name {
  font-weight: 500;
}

.time {
  font-family: monospace;
  color: #4caf50;
  font-weight: 600;
}

.date {
  color: #999;
  font-size: 14px;
}

.actions {
  margin-top: 30px;
  text-align: center;
}

.back-btn {
  display: inline-block;
  padding: 12px 30px;
  background: linear-gradient(135deg, #4caf50 0%, #2e7d32 100%);
  color: white;
  border-radius: 25px;
  font-weight: 600;
  transition: all 0.3s ease;
}

.back-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 5px 20px rgba(76, 175, 80, 0.4);
}
</style>
