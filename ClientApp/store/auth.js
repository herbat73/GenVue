import { authApi } from '../api/authApi'

const state = {
  loggedIn: false,
  authData: {
    un: null,
    token: null
  },
  rolesData: {
      isAdmin: false,
      isManager: false
  }, 
  api: null
}

const actions = {
  ensure_api ({commit, state}, endpoint) {
    return new Promise((resolve, reject) => {
      if (state.api === null) {
          authApi.create(endpoint)
          .then((api) => {
            commit('save_api', api)
            resolve(true)
          })
          .catch((e) => {
            reject(e)
          })
      } else {
        resolve(false)
      }
    })
  },
  authenticate ({commit, state}, auth) {
    return new Promise((resolve, reject) => {
      let resultData = {
        success: false
      }
      state.api.login(auth.un, auth.pw)
      .then(() => {
        resultData.success = true
        resultData.un = auth.un
        resultData.token = state.api.getToken()
        commit('login_result', resultData)
        commit('persist_auth', resultData)
        resolve()
      })
      .catch((e) => {
        commit('login_result', resultData)
        console.log(e)
        reject(new Error('authenticate login failed'))
      })
    })
  },
  attempt_reauthenticate ({commit, state}) {
    return new Promise((resolve, reject) => {
      let resultData = {
        success: false
      }
      let auth = {
        un: window.localStorage.getItem('auth.un'),
        token: window.localStorage.getItem('auth.token')
      }
      if (auth.un && auth.token) {
        state.api.reauth(auth.un, auth.token)
        .then(() => {
          resultData.success = true
          resultData.un = auth.un
          resultData.token = auth.token
          commit('login_result', resultData)
          resolve()
        })
        .catch((e) => {
          commit('login_result', resultData)
          console.log(e)
          reject(new Error('attempt_reauthenticate login failed'))
        })
      } else {
        reject('no stored auth available')
      }
    })
  },
  register_account ({commit, state}, auth) {
      return new Promise((resolve, reject) => {
      console.log('auth register_account called auth.username :' + auth.username)
      let resultData = {
        success: false
      }
      state.api.register(auth.firstname, auth.lastname, auth.username, auth.password, auth.role)
      .then(() => {
        resultData.success = true
        resolve()
      })
      .catch((e) => {
        console.log(e)
        reject(new Error('register failed'))
      })
    })
  },
  change_password ({commit, state}, pw) {
    return new Promise((resolve, reject) => {
      state.api.changePassword(pw.o, pw.n)
        .then((r) => {
          commit('login_result', { success: false })
          commit('persist_auth', false)
          resolve()
        })
        .catch((e) => reject(e))
    })
  },
  downloadFile ({commit, state}, id) {
    return new Promise((resolve, reject) => {
        state.api.downloadFileAPI(id)
    })
  },
  delete_all_files ({commit, state}) {
    return new Promise((resolve, reject) => {
      state.api.deleteAllFiles()
        .then((r) => {
          commit('login_result', { success: false })
          commit('persist_auth', false)
          resolve()
        })
        .catch((e) => reject(e))
    })
  },
  delete_account ({commit, state}) {
    return new Promise((resolve, reject) => {
      state.api.deleteAccount()
        .then((r) => {
          commit('login_result', { success: false })
          commit('persist_auth', false)
          resolve()
        })
        .catch((e) => reject(e))
    })
  },
  logout ({commit, state}) {
    return new Promise((resolve, reject) => {
      state.api.logout()
        .then((r) => {
          commit('login_result', { success: false })
          commit('persist_auth', false)
          commit('roles_result', { success: false })
          commit('persist_roles', false)
          resolve()
        })
        .catch((e) => reject(e))
    })
  },
  get_user_roles({ commit, state }) {
      return new Promise((resolve, reject) => {
          let resultData = {
              success: false
          }
          state.api.getUserRoles()
              .then(() => {
                  resultData.success = true
                  resultData.isAdmin = state.api.getIsUserAdmin()
                  resultData.isManager = state.api.getIsUserManager()
                  commit('roles_result', resultData)
                  commit('persist_roles', resultData)
                  resolve()
              })
              .catch((e) => {
                  commit('roles_result', resultData)
                  console.log(e)
                  reject(new Error('get user roles failed'))
              })
      })
  },
  
}

const mutations = {
  login_result (state, data) {
    if (data.success) {
      state.authData.un = data.un
      state.authData.token = data.token
      state.loggedIn = true
    } else {
      state.authData.un = null
      state.authData.token = null
      state.loggedIn = false
    }
  },
  persist_auth (state, data) {
    if (data && data.success) {
      window.localStorage.setItem('auth.un', data.un)
      window.localStorage.setItem('auth.token', data.token)
      console.log('saved auth')
    } else {
      window.localStorage.setItem('auth.un', null)
      window.localStorage.setItem('auth.token', null)
    }
  },
  roles_result(state, data) {
      if (data.success) {
          state.rolesData.isAdmin = data.isAdmin
          state.rolesData.isManager = data.isManager
      } else {
          state.rolesData.isAdmin = false
          state.rolesData.isManager = false
      }
  },
  persist_roles(state, data) {
      if (data && data.success) {
          window.localStorage.setItem('roles.isAdmin', data.isAdmin)
          window.localStorage.setItem('roles.isManager', data.isManager)
          console.log('saved roles')
      } else {
          window.localStorage.setItem('roles.isAdmin', false)
          window.localStorage.setItem('roles.isManager', false)
      }
  },
  save_api (state, api) {
    state.api = api
  }
}

const getters = {
  api_available (state) {
    return state.api !== null
  },
  auth_data (state) {
    return state.authData
  },
  is_logged_in (state) {
    return state.loggedIn
  },
  roles_data(state) {
      return state.rolesData
  },
  is_admin(state) {
      return state.rolesData.isAdmin
  },
  is_manager(state) {
      return state.rolesData.isManager
  },
  api (state) {
    return state.api
  }
}

export default {
  state,
  actions,
  mutations,
  getters
}
