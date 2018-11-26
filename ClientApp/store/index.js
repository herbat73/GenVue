import Vue from 'vue'
import Vuex from 'vuex'

// store modules
import auth from './auth'
import files from './files'
import data from './data'
import counter from './counter'

Vue.use(Vuex)

export default new Vuex.Store({
  modules: {
    auth,
    files,
    data,
    counter
  }
})
