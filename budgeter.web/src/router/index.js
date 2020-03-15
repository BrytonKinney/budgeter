import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/upload',
    name: 'Uploads',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "upload" */ '../views/Uploads.vue')
  },
  {
    path: '/expense-breakdown',
    name: 'ExpenseBreakdown',
    component: () => import(/* webpackChunkName: "expense-breakdown" */ '../views/ExpenseBreakdown.vue')
  }
]

const router = new VueRouter({
  routes
})

export default router
