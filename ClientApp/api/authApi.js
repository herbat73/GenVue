import axios from 'axios'

// this data must be same like in db table OpenIddictApplications
// otherwise call for auth token will be rejected
const client_id = 'mvc'
const client_secret = '101564A5-E7FE-42CB-B10D-61EF6A8F3651'

export class authApi {
  constructor (endpoint) {
    this.ep = endpoint
    this.init()
  }

  static create (endpoint) {
    return new Promise((resolve, reject) => {
      let api = new authApi(endpoint)
      resolve(api)
    })
  }

  init (token = null) {
    this.token = token
    this.username = null
    this.isAdmin = false
    this.isManager = false
    this.ax()
  }

  ax () {
    this.axios = axios.create({
      baseURL: this.ep,
      responseType: 'json'
    })
  }

  /* actions */
  login (un, pw) {
    return new Promise((resolve, reject) => {
      this.ax()

      const config = {
          headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
      }

      const params = new URLSearchParams();
      params.append('grant_type', 'password');
      params.append('client_id', client_id);
      params.append('client_secret', client_secret);
      params.append('username', un);
      params.append('password', pw);
        
      this.axios.post('/api/connect/token', params.toString(), config)
          .then((res) => {
            if (res.status !== 200) return reject(authApiErrors.CredentialError())
            this.token = res.data.access_token
            this.username = un
            resolve()
          }).catch((err) => {
            reject(err)
          })
    })
  }

  downloadFileAPI(id) {
      console.log('downloadFileAPI ' + id)

      let xhr = new XMLHttpRequest()
      xhr.open("POST", "/api/download", true)
      xhr.setRequestHeader('Authorization', 'Bearer ' + this.token)
      xhr.setRequestHeader("Content-type", "application/x-www-form-urlencoded")
      xhr.responseType = 'blob'

      const params = new URLSearchParams()
      params.append('id', id)

      xhr.send(params.toString())

      xhr.onload = function () {
          console.log('status : ' + xhr.status)

          if (xhr.status == 200) {
              var filename = "";
              var disposition = xhr.getResponseHeader('Content-Disposition');

              if (disposition && disposition.indexOf('attachment') !== -1) {
                  var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                  var matches = filenameRegex.exec(disposition);
                  if (matches != null && matches[1]) filename = matches[1].replace(/['"]/g, '');
              }

              var contentType = xhr.getResponseHeader('Content-Type')

              var blob = new Blob([xhr.response], { type: contentType })

              var link = document.createElement('a');
              link.href = window.URL.createObjectURL(blob);
              link.download = filename;
              link.click();
          }

      }    
  }

  reauth (un, token) {
    return new Promise((resolve, reject) => {
      this.ax()
      this.axios.post('/reauth', {
        username: un,
        token: token
      }).then((res) => {
        if (res.status !== 200) return reject(authApiErrors.CredentialError())
        this.token = res.data.token
        this.username = res.data.user.username
        resolve()
      }).catch((err) => {
        reject(err)
      })
    })
  }

  register(firstname, lastname, username, password, role) {
      console.log('register called username ' + username)
      return new Promise((resolve, reject) => {
          this.ax()

          const config = {
              headers: {
                  'Content-Type': 'application/x-www-form-urlencoded',
                  'Authorization': 'Bearer ' + this.token
              },
          }

          const params = new URLSearchParams();
          params.append('firstname', firstname);
          params.append('lastname', lastname);
          params.append('username', username);
          params.append('password', password);
          params.append('role', role);

          this.axios.post('/api/register', params.toString(), config)
              .then((res) => {
              console.log('register res status ' + res.status)  
              if (res.status !== 200) return reject(SpeercsErrors.RegistrationError())
              resolve()
          }).catch((err) => {
              reject(err)
          })
      })
  }

  logout () {
      return new Promise((resolve, reject) => {
        console.log('logout called.')
        this.token = null
        this.init()
        resolve()
    })
  }

  changePassword (old, newp) {
    return new Promise((resolve, reject) => {
      this.ax()
      this.axios.patch('/changepassword', {
        username: this.username,
        oldPassword: old,
        newPassword: newp
      })
        .then((res) => {
          console.log('password changed successfully')
          resolve(res)
        })
        .catch((e) => reject(e))
    })
  }

  deleteAllFiles () {
    this.ax()
    return this.axios.delete('/api/nuke/files')
  }

  deleteAccount () {
    this.ax()
    return this.axios.delete('/api/nuke/user')
  }

  getUserInfo() {
    return new Promise((resolve, reject) => {
      this.ax()
      const config = {
          headers: { 'Authorization': 'Bearer ' + this.token },
      }

      this.axios.get('/api/userinfo', config)
        .then((res) => {
          resolve({
            quota: res.data.quota,
            usage: res.data.usage
          })
        }).catch((err) => {
          reject(err)
        })
    })

  }

  getUserRoles() {
      return new Promise((resolve, reject) => {
          this.ax()
          const config = {
              headers: { 'Authorization': 'Bearer ' + this.token },
          }

          this.axios.get('/api/userroles', config)
              .then((res) => {
                  if (res.status !== 200) return reject(authApiErrors.CredentialError())
                  this.isAdmin = res.data.IsAdmin
                  this.isManager = res.data.IsManager

                  console.log('isAdmin : ' + this.isAdmin)

                  resolve()
              }).catch((err) => {
                  reject(err)
              })
      })

  }

  /* getters */
  getToken() { return this.token }

  getIsUserAdmin() { return this.isAdmin }

  getIsUserManager() { return this.isManager }

}

class authApiErrors {
  static CredentialError () {
    return new Error('invalid credentials')
  }
  static KeyError () {
    return new Error('invalid api key')
  }
  static RegistrationError() {
      return new Error('invalid registration call')
  }
}
