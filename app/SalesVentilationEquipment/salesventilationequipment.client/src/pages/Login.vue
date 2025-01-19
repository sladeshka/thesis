<template>
  <h1 class="title text-left">Authorization</h1>
  <form @submit.prevent="login">
    <div class="auth-form row">
      <div class="col-12 mb-4">
        <input type="text" v-model="username" class="auth-form__input" placeholder="Login" />
      </div>
      <div class="col-12 mb-4">
        <input type="password" v-model="password" class="auth-form__input" placeholder="Password" />
      </div>
      <div class="col-12 mb-4">
        <button type="submit">Login</button>
      </div>
    </div>

  </form>
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import { runtimePublic } from "@/config/runtimeConfig";
  export default defineComponent({
    name: 'Login',
    data() {
      return {
        username: '',
        password: '',
      };
    },
    methods: {
      login() {
        fetch(runtimePublic.loginApiUrl + "/api/v1/signin", {
          headers: {
            "Content-Type": "application/json",
          },
          method: "POST",
          body: JSON.stringify({
            "login": this.username,
            "password": this.password
          })
        })
          .then(response => response.json())
          .then(data => {
            if(data?.code === 200 && data?.data) {
              sessionStorage.setItem('token', data.data);
              this.setContractor(data.data)
              window.location.replace("/");
            }
          });
      },
      setContractor(token: string) {
        fetch(runtimePublic.baseApiUrl + "/api/v1/contractors", {
          headers: {
            "Content-Type": "application/json",
            "Authorization": token
          },
          method: "GET",
        })
          .then(response => response.json())
          .then(data => {
            if(data?.code === 200 && data?.data) {
              sessionStorage.setItem('userData', JSON.stringify({
                id: data.data[0].id,
                isActive: true,
                name: data.data[0].name,
                JWTToken: token,
                info: data.data[0].contactInfo
              }));
              this.setCart(data.data)
            }
          });
      },
      setCart(token: string) {
        fetch(runtimePublic.baseApiUrl + "/api/v1/carts", {
          headers: {
            "Content-Type": "application/json",
            "Authorization": token
          },
          method: "GET",
        })
          .then(response => response.json())
          .then(data => {
            if(data?.code === 200 && data?.data) {
              sessionStorage.setItem('cartData', JSON.stringify({
                id: data.data[0].id,
                totalSum: data.data[0].totalSum,
                discount: data.data[0].discount,
              }));
            }
          });
      }
    },
  });
</script>

<style scoped>
.auth-form__input{
  border: unset;
  box-shadow: 0 2px 5px rgba(241, 230, 214, 0.9);
}
</style>
