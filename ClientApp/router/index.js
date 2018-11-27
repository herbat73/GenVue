import Vue from 'vue'
import VueRouter from 'vue-router'
import { routes } from './routes'
import store from '../store'

Vue.use(VueRouter)

let router = new VueRouter({
  mode: 'history',
  routes
})

router.beforeEach((to, from, next) => {
  console.log('router.beforeEach to : ' + to.path)
  if (to.matched.some(record => record.meta.requiresAuth)) {
    // this route requires auth, check if logged in
    // if not, redirect to login page.
    if (!store.getters.is_logged_in) {
      console.log('this route requires auth and you are not logged in')
      next({
        path: '/login',
        query: { r: to.fullPath }
      })
    } else {
      if (to.matched.some(record => record.meta.requiresAdminRole)) {
        console.log('requiresAdminRole check')
        if (!store.getters.is_admin) {
          // and you are Not Admin
          next({ name: 'notinrole', query: { redirect: '/notinrole' } })
        } else {
          // and you are Admin, so ok'
          next()
        }
      } else {
        if (to.matched.some(record => record.meta.requiresManagerRole)) {
          if (!(store.getters.is_manager || this.$store.getters.is_admin)) {
            // and you are Not Manager not Admin
            next({ name: 'notinrole', query: { redirect: '/notinrole' } })
          } else {
            // and you are Admin, so ok'
            next()
          }
        } else {
          next()
        }
      }
    }
  } else {
    next() // make sure to always call next()!
  }
})

export default router
