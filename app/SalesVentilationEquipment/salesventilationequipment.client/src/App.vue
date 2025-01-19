<template>
  <Header />
  <div class="container mt-4">
    <router-view />
  </div>
  <Footer />
</template>

<script lang="ts">
  import Header from './components/Header.vue';
  import Footer from './components/Footer.vue';

  export default {
    name: 'App',
    mounted() {
      const userData = sessionStorage.getItem('userData');
      if(!userData && !window.location.href.includes("login")) {
        window.location.replace("/login");
      }
    },
    components: {
      Header,
      Footer
    },
  };
</script>

<style scoped>

</style>


