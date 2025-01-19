<template>
  <header class="header sticky-top bg-light">
    <nav class="navbar navbar-expand-lg navbar-light">
      <div class="container">
        <a class="navbar-brand" href="/">
          Shop
        </a>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
          <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNav">
          <ul class="navbar-nav">
            <li class="nav-item">
              <router-link to="/" v-if="user?.isActive">Catalog</router-link>
            </li>
            <li class="nav-item">
              <router-link to="/about">About</router-link>
            </li>
            <li v-if="!user?.isActive" class="nav-item" >
              <router-link to="/login">Authorization</router-link>
            </li>
            <li v-if="user?.isActive" class="nav-item" >
              <a href="#" @click="logout()">Logout</a>
            </li>
            <li v-if="user?.isActive" class="nav-item" >
              <router-link to="/cart">Cart</router-link>
            </li>
            <li v-if="user?.isActive" class="nav-item" >
              <router-link to="/orders">Orders</router-link>
            </li>
          </ul>
        </div>
      </div>
</nav>
  </header>
</template>

<script lang="ts">
  import type {userData} from "@/types/user";

  export default {
    name: 'Header',
    data() {
      return {
        user: {} as userData
      };
    },
    mounted() {
      try {
        this.user = JSON.parse(sessionStorage.getItem('userData'))
        console.log(this.user)
      } catch (e) {
        console.log(e);
      }
    },
    methods: {
      logout: function () {
        sessionStorage.clear()
        window.location.replace("/login");
      },
    }
  };
</script>

<style scoped>
  .header {
    background-color: #f1e6d6;
    box-shadow: 0 2px 5px rgba(241, 230, 214, 0.9);
  }
  .navbar-brand {
    color: rgba(214, 154, 107, 1);
  }
</style>
