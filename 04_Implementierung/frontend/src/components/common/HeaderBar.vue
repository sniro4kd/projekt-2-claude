<script setup lang="ts">
import { RouterLink, useRoute } from 'vue-router';
import { computed } from 'vue';

const route = useRoute();

const isHome = computed(() => route.name === 'home');
const isGame = computed(() => route.name === 'game');
const isLeaderboard = computed(() => route.name === 'leaderboard');
</script>

<template>
  <header class="header">
    <RouterLink to="/" class="logo">
      <img src="/favicon.svg" alt="Logo" class="logo-icon" />
      <span class="logo-text">Fang den Hasen!</span>
    </RouterLink>
    <nav class="nav">
      <RouterLink to="/" class="nav-link" :class="{ active: isHome }">Start</RouterLink>
      <RouterLink to="/game" class="nav-link" :class="{ active: isGame }">Spielen</RouterLink>
      <RouterLink to="/leaderboard" class="nav-link" :class="{ active: isLeaderboard }">Bestenliste</RouterLink>
    </nav>
  </header>
</template>

<style scoped>
.header {
  background: linear-gradient(135deg, #4caf50 0%, #2e7d32 100%);
  padding: 15px 30px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.2);
}

.logo {
  display: flex;
  align-items: center;
  gap: 10px;
}

.logo-icon {
  width: 40px;
  height: 40px;
}

.logo-text {
  font-size: 24px;
  font-weight: bold;
  color: white;
  text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.3);
}

.nav {
  display: flex;
  gap: 20px;
}

.nav-link {
  color: white;
  font-size: 16px;
  font-weight: 500;
  padding: 8px 16px;
  border-radius: 20px;
  transition: all 0.3s ease;
}

.nav-link:hover {
  background-color: rgba(255, 255, 255, 0.2);
}

.nav-link.active {
  background-color: rgba(255, 255, 255, 0.3);
}
</style>
