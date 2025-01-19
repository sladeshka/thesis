<template>
  <h1 class="title text-left">
    Catalog
  </h1>
  <div class="row">
    <div class="col-md-4" v-for="product in products" :key="product.id">
      <div class="item card mb-4">
        <div class="card-body">
          <div class="item-image mb-4">
            <img :src="'http://localhost:8180/api/v1/files/' + product.id + '/src.jpg'" class="item-image-img" alt="Image">
          </div>
          <h5 class="item-name card-title">{{ product.name }}</h5>
          <div class="item-price">
            <span class="text-muted">{{ product.price }} $</span>
          </div>
          <div class="item-description">
            <p class="item-short-description card-text">{{ product.description }}</p>
            <p class="item-short-feature card-text">{{ product.feature }}</p>
          </div>
          <div class="row mt-4">
            <div class="col-2">
              <button @click="decreaseQuantity(product)">-</button>
            </div>
            <div class="item-short-quantity col-2">
              {{ getQuantity(product) }}
            </div>
            <div class="col-2">
              <button @click="increaseQuantity(product)">+</button>
            </div>
            <div class="col-6 d-flex justify-content-end">
              <button @click="addProductToCart(product)">To cart</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import type {catalogItem, catalogItems} from "@/types/catalog";
  import {runtimePublic} from "@/config/runtimeConfig";
  import type {cartData} from "@/types/cart";

  export default defineComponent({
    data() {
      return {
        products: [] as catalogItems,
        cart: {} as cartData
      };
    },
    mounted() {
      this.setCart()
      this.setProduct()

    },
    methods: {
      addProductToCart: function (product: catalogItem) {
        fetch(runtimePublic.baseApiUrl + '/api/v1/carts/' + this.cart.id + '/products', {
          headers: {
            "Content-Type": "application/json",
          },
          method: "POST",
          body: JSON.stringify({
            productId: product.id,
            quantity: product.quantity
          })
        })
          .then(response => response.json())
          .then(data => {
            product = data.data;
          });
      },
      setCart: function (){
        console.log(JSON.stringify(sessionStorage.getItem('cartData')))
        this.cart = JSON.parse(sessionStorage.getItem('cartData'));
      },
      setProduct: function (){
        fetch(runtimePublic.baseApiUrl + '/api/v1/products')
          .then(response => response.json())
          .then(data => {
            this.products = data.data;
          });
      },
      getQuantity: function (product: catalogItem) {
        if(!product.quantity){
          product.quantity = 1;
        }
        return product.quantity;
      },
      increaseQuantity: function (product: catalogItem) {
        if (product.quantity) {
          product.quantity++;
        } else {
          product.quantity = 1;
        }
      },
      decreaseQuantity: function (product: catalogItem) {
        if (product.quantity) {
          if (product.quantity > 1) {
            product.quantity--;
          } else {
            product.quantity = 0
          }
        } else {
          product.quantity = 0;
        }
      }
    }
  });
</script>

<style scoped>
.item-image {
  display: flex;
  justify-content: center;
}
.item-image-img{
  max-height: 160px;
}
.item-price{
  color: rgba(107, 142, 35, 1);
}
.item-short-quantity{
  color: rgba(107, 142, 35, 1);
}
.item-description{
  min-height: 160px;
}
.item-short-description,
.item-short-feature
{
  color: rgba(214, 154, 107, 1);
}
</style>
