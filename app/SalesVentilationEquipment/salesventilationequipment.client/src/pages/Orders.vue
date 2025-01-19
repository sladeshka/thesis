<template>
  <h1 class="title text-left">My orders</h1>
  <div class="row">
    <template v-if="!orders || orders?.length === 0">
      <p>
        There will be a list of your orders here.
      </p>
    </template>
    <template v-else>
      <div class="col-12" v-for="(order, index) in orders" :key="index">
        <div class="item card mb-4 row">
          <div class="card-body row">
            <div class="item-image col-md-4">
              Order: {{order.id}}
            </div>
            <div class="item-image col-md-4">
              Total order amount: {{order.cart.totalSum}} $
            </div>
            <div class="item-image col-md-4">
              Order status: {{order.orderStatus}}
            </div>
          </div>
        </div>
      </div>
    </template>
  </div>
</template>

<script lang="ts">
import {runtimePublic} from "@/config/runtimeConfig";
import type {userData} from "@/types/user";
import type {orderData, ordersWithCartsData} from "@/types/order";
import type {cartData} from "@/types/cart";

export default {
  data() {
    return {
      orders: [] as ordersWithCartsData,
      user: {} as userData,
      cart: {} as cartData,
      token: ''
    };
  },
  mounted() {
    try {
      this.user = JSON.parse(sessionStorage.getItem('userData'))
      this.token = sessionStorage.getItem('token') ?? '';
      this.setOrders()
    } catch (e) {
      console.log(e);
    }
  },
  methods: {
    setOrders: function (){
      fetch(runtimePublic.baseApiUrl + '/api/v1/orders')
        .then(response => response.json())
        .then(data => {
          data.data.forEach((order : orderData) => {
            this.orders.push({
              "id": order.id,
              "contractorId": order.contractorId,
              "cartId": order.cartId,
              "orderStatus": order.orderStatus,
              "cart": this.getCart(order.cartId)
            })
          })
          console.log(data)
          console.log(this.orders)
        });
    },
    getCart: function (id: string) {
      fetch( runtimePublic.baseApiUrl + '/api/v1/carts/' + id)
        .then(response => response.json())
        .then(data => {
          this.orders.forEach((order : orderData, index: number) => {
            if(data.data.id == order.cartId){
              this.orders[index].cart =  data.data
            }
          })
        });
    },
  }
};
</script>
