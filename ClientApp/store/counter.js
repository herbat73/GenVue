import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

// STATE
const state = {
    counter : 0,
}

// MUTATIONS
const mutations = {
    setMainCounter(state, obj) {
        state.counter = obj.counter
    }
}

// ACTIONS
const actions = {
    setCounter({ commit }, obj) {
        commit('setMainCounter', obj)
    }
}

// GETTERS
const getters = {
    counter (state) {
        return state.counter
    }
}

export default {
    state,
    mutations,
    actions,
    getters
}
