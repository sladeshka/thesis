import { createRouter, createWebHistory } from 'vue-router';
import Catalog from '../pages/Catalog.vue';
import Login from '../pages/Login.vue';
import About from '../pages/About.vue';
import Cart from '../pages/Cart.vue';
import Orders from '../pages/Orders.vue';

const routes = [
  { path: '/', component: Catalog },
  { path: '/login', component: Login },
  { path: '/about', component: About },
  { path: '/cart', component: Cart },
  { path: '/orders', component: Orders },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
