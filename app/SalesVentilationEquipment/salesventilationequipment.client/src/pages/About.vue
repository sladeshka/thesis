<template>
  <h1 class="title text-left">About the company</h1>
  <p>The company has been selling goods since the beginning of time.</p>
  <h2>Our stores</h2>
  <template v-if="!stores || stores?.length === 0">
    <p>
      We don't have stores.
    </p>
  </template>
  <template v-else>
    <div class="col-12" v-for="( store, index) in stores" :key="index">
      <div class="item card mb-4 row">
        <div class="card-body row">
          <div class="item-image col-md-6">
            Name: {{ store.name }}
          </div>
          <div class="item-image col-md-6">
            Address: {{ store.address }} $
          </div>
        </div>
      </div>
    </div>
  </template>
</template>

<script lang="ts">
import {runtimePublic} from "@/config/runtimeConfig";
import type {storesData} from "@/types/store";

export default {
  data() {
    return {
      stores: [] as storesData
    };
  },
  mounted() {
    try {
      this.setStores()
    } catch (e) {
      console.log(e);
    }
  },
  methods: {
    setStores: function (){
      fetch(runtimePublic.baseApiUrl + '/api/v1/stores')
        .then(response => response.json())
        .then(data => {
          this.stores = data.data;
          console.log(data)
        });
    },
  }
  };
</script>


