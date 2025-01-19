import type {cartData} from "@/types/cart";

export interface IOrderData {
  "id": string,
  "contractorId": string,
  "cartId": string
  "orderStatus": string
}

export type orderData = IOrderData
export type ordersData = Array<orderData>

export interface IOrderWithCartData {
  "id": string,
  "contractorId": string,
  "cartId": string,
  "orderStatus": string,
  "cart": cartData
}

export type orderWithCartData = IOrderWithCartData
export type ordersWithCartsData = Array<IOrderWithCartData>
