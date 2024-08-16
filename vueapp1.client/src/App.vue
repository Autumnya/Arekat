<template>
  
  <StaticHeader />

  <div class="mainpage_bg">
    <RouterView />
  </div>

  <StaticFooter />

</template>

<style scoped>
  .mainpage_bg{
    background-color: rgb(230,230,230);
    width: 100%;
    min-width: 1024px;
    display: flex;
    min-height: 1000px;
  }
</style>

<script setup>

import { RouterView } from 'vue-router';

import StaticFooter from './components/globalComponents/StaticFooter.vue';
import StaticHeader from './components/globalComponents/StaticHeader.vue';

</script>