export interface iCatalogItem {
  id: string
  name: string
  price: number
  description: string
  feature: string
  quantity: number
}

export type catalogItem = iCatalogItem;

export type catalogItems = Array<catalogItem>;
