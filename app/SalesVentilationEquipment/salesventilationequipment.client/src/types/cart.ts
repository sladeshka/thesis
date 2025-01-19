export interface ICartData {
  "id": string,
  "totalSum": number,
  "discount": number
}
export type cartData = ICartData
export interface iCartItem {
  id: number
  cartId: string
  productId: string
  name: string
  unitPrice: number
  description: string
  feature: string
  quantity: number
}
export type cartItem = iCartItem
export type cartItems = Array<cartItem>
