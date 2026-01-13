<script setup lang="ts">
import { useRouter } from 'vue-router';
import { useGameStore } from '@/stores/gameStore';
import { ref } from 'vue';
import type { PlayerRole } from '@/types/game';

const router = useRouter();
const gameStore = useGameStore();

const selectedRole = ref<PlayerRole>('children');
const isStarting = ref(false);

async function startGame() {
  isStarting.value = true;
  try {
    await gameStore.startNewGame(selectedRole.value);
    router.push('/game');
  } catch (error) {
    console.error('Fehler beim Starten des Spiels:', error);
  } finally {
    isStarting.value = false;
  }
}
</script>

<template>
  <div class="home">
    <div class="welcome-card">
      <h1 class="title">Willkommen bei Fang den Hasen!</h1>
      <p class="description">
        Ein spannendes Strategiespiel für Jung und Alt. Wähle deine Seite und
        versuche, den Computer zu besiegen!
      </p>

      <div class="rules">
        <h2>Spielregeln</h2>
        <div class="rule-columns">
          <div class="rule-card rabbit">
            <h3>Der Hase</h3>
            <ul>
              <li>Bewegt sich diagonal in alle 4 Richtungen</li>
              <li>Ziel: Die untere Spielfeldseite erreichen</li>
              <li>Gewinnt, wenn kein Kind ihn mehr erreichen kann</li>
            </ul>
          </div>
          <div class="rule-card children">
            <h3>Die Kinder</h3>
            <ul>
              <li>Bewegen sich diagonal nach oben</li>
              <li>4 Kinder arbeiten zusammen</li>
              <li>Ziel: Den Hasen einkreisen</li>
            </ul>
          </div>
        </div>
      </div>

      <div class="role-selection">
        <h2>Wähle deine Rolle</h2>
        <div class="role-buttons">
          <button
            class="role-btn"
            :class="{ selected: selectedRole === 'rabbit' }"
            @click="selectedRole = 'rabbit'"
          >
            <span class="role-icon">🐰</span>
            <span class="role-name">Hase</span>
          </button>
          <button
            class="role-btn"
            :class="{ selected: selectedRole === 'children' }"
            @click="selectedRole = 'children'"
          >
            <span class="role-icon">👧👦</span>
            <span class="role-name">Kinder</span>
          </button>
        </div>
      </div>

      <button
        class="start-btn"
        @click="startGame"
        :disabled="isStarting"
      >
        {{ isStarting ? 'Spiel wird gestartet...' : 'Spiel starten!' }}
      </button>
    </div>
  </div>
</template>

<style scoped>
.home {
  max-width: 800px;
  width: 100%;
}

.welcome-card {
  background: white;
  border-radius: 20px;
  padding: 40px;
  box-shadow: 0 10px 40px rgba(0, 0, 0, 0.1);
}

.title {
  color: #2e7d32;
  font-size: 32px;
  text-align: center;
  margin-bottom: 15px;
}

.description {
  text-align: center;
  color: #666;
  font-size: 18px;
  margin-bottom: 30px;
}

.rules {
  margin-bottom: 30px;
}

.rules h2 {
  color: #333;
  text-align: center;
  margin-bottom: 20px;
}

.rule-columns {
  display: flex;
  gap: 20px;
}

.rule-card {
  flex: 1;
  padding: 20px;
  border-radius: 15px;
}

.rule-card.rabbit {
  background: linear-gradient(135deg, #fff3e0 0%, #ffe0b2 100%);
}

.rule-card.children {
  background: linear-gradient(135deg, #e8f5e9 0%, #c8e6c9 100%);
}

.rule-card h3 {
  margin-bottom: 10px;
  color: #333;
}

.rule-card ul {
  list-style: none;
  padding-left: 0;
}

.rule-card li {
  padding: 5px 0;
  padding-left: 20px;
  position: relative;
}

.rule-card li::before {
  content: '•';
  position: absolute;
  left: 0;
  color: #4caf50;
  font-weight: bold;
}

.role-selection {
  margin-bottom: 30px;
}

.role-selection h2 {
  color: #333;
  text-align: center;
  margin-bottom: 20px;
}

.role-buttons {
  display: flex;
  justify-content: center;
  gap: 30px;
}

.role-btn {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 10px;
  padding: 20px 40px;
  border: 3px solid #ddd;
  border-radius: 15px;
  background: white;
  transition: all 0.3s ease;
}

.role-btn:hover {
  border-color: #4caf50;
  transform: translateY(-3px);
}

.role-btn.selected {
  border-color: #4caf50;
  background: linear-gradient(135deg, #e8f5e9 0%, #c8e6c9 100%);
}

.role-icon {
  font-size: 40px;
}

.role-name {
  font-size: 18px;
  font-weight: 600;
  color: #333;
}

.start-btn {
  display: block;
  width: 100%;
  padding: 18px;
  font-size: 20px;
  font-weight: bold;
  color: white;
  background: linear-gradient(135deg, #4caf50 0%, #2e7d32 100%);
  border: none;
  border-radius: 30px;
  transition: all 0.3s ease;
}

.start-btn:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 5px 20px rgba(76, 175, 80, 0.4);
}

.start-btn:disabled {
  opacity: 0.7;
  cursor: not-allowed;
}
</style>
