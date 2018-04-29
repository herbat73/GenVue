import Vue from 'vue'
import Vuetify from 'vuetify'
import VueRouter from 'vue-router'

// app
import App from './App.vue'

// routes
import routes from './routes'

// store
import store from './store/index'

// ui styles

//import './stylus/main.styl'

import 'vuetify/dist/vuetify.css'

// enable dev tools
Vue.config.devtools = true

// register plugins
Vue.use(VueRouter)
Vue.use(Vuetify)

// router setup
let router = new VueRouter({
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
                if (!(store.getters.is_manager || store.getters.is_admin)) {
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

let GenVueApp = Vue.component('app', App)

const app = new GenVueApp({
  el: '#app',
  router,
  store
})


