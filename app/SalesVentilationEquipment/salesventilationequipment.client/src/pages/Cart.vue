<template v-if="user?.isActive" >
  <h1 class="title text-left">Cart</h1>
  <div class="row">
    <template v-if="!items || items?.length === 0">
      <p>
        Cart is empty, go to catalog.
      </p>
    </template>
    <template v-else>
      <div class="col-12" v-for="(item, index) in items" :key="index">
        <div class="item card mb-4 row">
          <div class="card-body row">
            <div class="item-image col-md-4">
              <div class="mb-4">
                <img :src="'http://localhost:8180/api/v1/files/' + item.productId + '/src.jpg'" class="item-image-img" alt="Image">
              </div>
              <div>
                <h5 class="item-name card-title">Name {{ item.name }}</h5>
              </div>

            </div>
            <div class="col col-md-4">
              <div>
                {{item.description}}
                {{item.feature}}
              </div>
            </div>
            <div class="col-md-4 row">
              <div class="item-price">
                <span class="text-muted">
                  Total by position: {{ item.unitPrice * item.quantity }}
                </span>
              </div>
              <div class="row mt-4">
                <div class="col-md-2">
                  <button @click="decreaseCartItemQuantity(item, index)">-</button>
                </div>
                <div class="col-md-2">
                  {{ getCartItemQuantity(item) }}
                </div>
                <div class="col-md-2">
                  <button @click="increaseCartItemQuantity(item)">+</button>
                </div>
                <div class="col-2">
                  <button @click="deleteCartItem(item, index)">x</button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="row mt-4 mb-4">
        <div class="cart-total-sum col col-md-10">
          The total order amount is {{ totalSum }} $
        </div>
        <div class="col col-md-2">
          <button @click="sendOrder()">Place an order</button>
        </div>
      </div>
</template>
</div>
</template>

<script lang="ts">
import type {cartData, cartItem, cartItems} from "@/types/cart";
import type {userData} from "@/types/user";
import {runtimePublic} from "@/config/runtimeConfig";
import type {catalogItem, catalogItems} from "@/types/catalog";
 export default {
    data() {
      return {
        items: [] as cartItems,
        products: [] as catalogItems,
        user: {} as userData,
        cart: {} as cartData,
        token: ''
      };
    },
    mounted() {
      try {
        this.user = JSON.parse(sessionStorage.getItem('userData'))
        this.cart = JSON.parse(sessionStorage.getItem('cartData'))
        this.setProduct()
        this.token = sessionStorage.getItem('token') ?? '';
        if(this.cart) {
          this.getCartItems()
        }
      } catch (e) {
        console.log(e);
      }
    },
    computed: {
        totalSum: function() {
          return this.items.reduce((total, item: cartItem) => {
            return total + (item.unitPrice * item.quantity);
          }, 0);
        },
    },
    methods: {
      sendOrder: function () {

      },
      getCartItems: function () {
        fetch( runtimePublic.baseApiUrl + '/api/v1/carts/' + this.cart.id + '/products')
          .then(response => response.json())
          .then(data => {
            this.items = data.data;
          });
      },
      setProduct: function (){
        fetch(runtimePublic.baseApiUrl + '/api/v1/products')
          .then(response => response.json())
          .then(data => {
            data.data.forEach((product : catalogItem) => {
              this.items.forEach((item : cartItem, index: number) => {
                if(product.id == item.productId){
                  item.name = product.name
                  item.description = product.description
                  item.feature = product.feature
                  this.items[index] = item
                }
              })
            })
          });
      },
      updateCartItem: function (item: cartItem) {
        fetch(runtimePublic.baseApiUrl + '/api/v1/carts/' + this.cart.id + '/products/' + item.id, {
            headers: {
              "Content-Type": "application/json",
            },
            method: "PUT",
            body: JSON.stringify(item)
          })
          .then(response => response.json())
          .then(data => {
            console.log(data);
          });
      },
      deleteCartItem: function (item: cartItem, index: number) {
        this.items.splice(index, 1);
        fetch(runtimePublic.baseApiUrl + '/api/v1/carts/' + this.cart.id + '/products/' + item.id, {
          headers: {
            "Content-Type": "application/json",
          },
          method: "DELETE"
        });
      },
      getCartItemQuantity: function (item: cartItem) {
        if(!item.quantity){
          item.quantity = 1;
        }
        return item.quantity;
      },
      increaseCartItemQuantity: function (item: cartItem) {
        if (item.quantity) {
          item.quantity++;
        } else {
          item.quantity = 1;
        }
        this.updateCartItem(item)
      },
      decreaseCartItemQuantity: function (item: cartItem, index: number) {
        if (item.quantity) {
          if (item.quantity > 1) {
            item.quantity--;
          } else {
            item.quantity = 0
          }
        } else {
          item.quantity = 0;
        }
        if(item.quantity) {
          this.updateCartItem(item)
        } else {
          this.deleteCartItem(item, index)
        }
      }
    }
  };
</script>

<style scoped>
.item-image {
  display: flex;
  justify-content: center;
  flex-direction: column;
}
.item-image-img {
  max-height: 160px;
}
.cart-total-sum{
  font-size: 24px;
}
</style>
