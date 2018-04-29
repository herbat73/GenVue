<template>
  <v-card class="grey lighten-4 elevation-0">
    <v-card-text>
      <v-container fluid>
        <v-layout row>  
          <v-flex xs10 offset-xs1 lg6 offset-lg3>
            <template v-if="mode === 'login'">
              <p class="text-xs-center">
                test hint: use admin@genvue.com, manager@genvue.com, user@genvue.com and password : 123PassWord.!
              </p>
              <br/>   
              <v-text-field
                name="username-input"
                label="E-mail"
                v-model="un"
              ></v-text-field>
              <v-text-field
                name="password-input"
                label="Password"
                type="password"
                v-model="pw"
              ></v-text-field>
              <v-subheader class="error--text" v-if="err !== null">{{ err }}</v-subheader>
              <div class="center">
                <v-btn color="primary" @click.native="proceed_login" :disabled="!canProceed" raised ripple>Login</v-btn>
              </div>
            </template>
            <template v-else-if="mode === 'register'">
                <v-text-field name="firstName-input"
                              label="First name"
                              v-model="firstname"></v-text-field>
                <v-text-field name="lastName-input"
                              label="Last name"
                              v-model="lastname"></v-text-field>
              <v-text-field
                name="username-input"
                label="E-mail"
                v-model="un"
              ></v-text-field>
              <v-text-field
                name="password-input"
                label="Password"
                type="password"
                v-model="pw"
              ></v-text-field>
                <v-radio-group v-model="role" row>
                    <v-radio label="User" value="User"></v-radio>
                    <v-radio label="Manager" value="Manager"></v-radio>
                    <v-radio label="Admin" value="Admin"></v-radio>
                </v-radio-group>
              <v-subheader class="error--text" v-if="err !== null">{{ err }}</v-subheader>
              <div class="center">
                <v-btn @click.native="proceed_register" color="primary" :disabled="!canProceed" raised ripple>Register</v-btn>
              </div>
            </template>
          </v-flex>
        </v-layout>
      </v-container>
    </v-card-text>
  </v-card>
</template>

<script>
export default {
  name: 'loginForm',
  props: ['mode'],
  data () {
      console.log('data mode : ' + this.mode)
      if (this.mode=='login') {
        return {
          un: 'admin@genvue.com',
          pw:'123PassWord.!',
          err: null,
          canProceed: true
        }
      } else {
        return {
          un: 'foo@niepodam.pl',
          pw:'123PassWord.!',
          err: null,
          canProceed: true,
          role: 'User',
          firstname: 'John',
          lastname: 'Dole'
        }
      }
  },
  methods: {
    attempt_relogin () {
      this.$store.dispatch('ensure_api', `${window.location.origin}/`)
        .then(() => {
          this.$store.dispatch('attempt_reauthenticate')
            .then(() => {
              console.log('reauthenticated successfully')
              // proceed
              this.onProceed()
            })
        })
    },
    proceed_login() {
        this.canProceed = false
        let b = {
            un: this.un,
            pw: this.pw
        }
        this.$store.dispatch('ensure_api', `${window.location.origin}/`)
            .then(() => {
                this.$store.dispatch('authenticate', b)
                    .then(() => {
                        console.log('login successful')
                        this.$store.dispatch('get_user_roles')
                            .then(() => {
                                console.log('get roles finished')
                                // proceed
                                this.onProceed()
                            })
                            .catch((e) => {
                                this.err = 'cannot get roles info data'
                                console.log('get roles failure', e)
                            })
                    })
                    .catch((e) => {
                        this.canProceed = true
                        this.err = 'invalid credentials'
                        console.log('login failure', e)
                    })
            })
    },
    proceed_register () {
        this.canProceed = false
        console.log('proceed_register un : ' + this.un)
        let b = {
        firstname: this.firstname,
        lastname: this.lastname,
        username: this.un,
        password: this.pw,
        role: this.role
        }

        console.log('proceed_register b.username : ' + b.username)
       
        this.$store.dispatch('ensure_api', `${window.location.origin}/`)
        .then((rs) => {
            this.$store.dispatch('register_account', b)
            .then(() => {
                console.log('registration successful')
                // proceed
                this.onUserRegistrationComplete()
            })
            .catch((e) => {
                this.canProceed = true
                this.err = 'registration failed'
                if (e.response) {
                this.err += `: ${e.response.data}`
                }
                console.log('registration failure', e)
            })
        })
    },
    onProceed () {
      if (this.$route.query.r) {
        this.$router.push(this.$route.query.r)
      } else {
        this.$router.push('/f')
      }
    },
    onUserRegistrationComplete () {
        this.$router.push('/admin')
    }

  },
  mounted () {
    //this.attempt_relogin()
  }
}
</script>
