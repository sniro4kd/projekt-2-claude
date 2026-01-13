<script setup lang="ts">
import HeaderBar from '@/components/common/HeaderBar.vue';
</script>

<template>
  <div class="app">
    <HeaderBar />
    <main class="main-content">
      <router-view />
    </main>
    <footer class="footer">
      <span>ERP_Champion GmbH</span>
    </footer>
  </div>
</template>

<style scoped>
.app {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
  background-color: #e3f2fd;
}

.main-content {
  flex: 1;
  padding: 20px;
  display: flex;
  justify-content: center;
  align-items: flex-start;
}

.footer {
  background-color: #607d8b;
  color: white;
  padding: 10px;
  text-align: center;
  font-size: 14px;
}
</style>
