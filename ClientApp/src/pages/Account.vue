<template>
  <div>
    <h3 class="center mg-wrap">User Profile</h3>
    <h4>Manage Account ({{ username }})</h4>

    <div class="p-section">
      <h5>Resource Usage</h5>
      <div v-if="ready">
        <p>
          Using
          <b>{{ userInfo.usage }}</b> of <b>{{ userInfo.quota }}</b>
        </p>
      </div>
      <div v-else>
        <v-progress-circular indeterminate v-bind:size="60" class="primary--text"></v-progress-circular>
        <p>Retrieving Data</p>
      </div>
      <p></p>
    </div>
    <div class="p-section">
      <h5>Security</h5>
      <form v-on:submit.prevent="tryUpdatePassword">
        <v-layout row>
          <v-flex xs12 lg6>
            <v-text-field
              label="Current password"
              type="password"
              v-model="updatePassword.old"
            ></v-text-field>
          </v-flex>
        </v-layout>
        <v-layout row>
          <v-flex xs12 lg6>
            <v-text-field
              label="New password"
              type="password"
              v-model="updatePassword.password"
            ></v-text-field>
          </v-flex>
        </v-layout>
        <v-layout row>
          <v-flex xs12 lg6>
            <v-text-field
              label="Confirm new password"
              type="password"
              v-model="updatePassword.confirm"
            ></v-text-field>
          </v-flex>
        </v-layout>
        <p class="error--text">{{ updatePassword.err }}</p>
        <input type="submit" class="invisible"></input>
        <v-btn primary @click.native="tryUpdatePassword" :disabled="!updatePassword.e">Change Password</v-btn>
      </form>
    </div>
    <div>
      <h5>Danger Zone</h5>
      <v-btn class="md-raised md-warn" @click.native="deleteAllFiles">Delete All Files</v-btn>
      <v-btn class="md-raised md-warn" @click.native="deleteAccount">Delete Account</v-btn>
    </div>
  </div>
</template>

<script>
import HumanFilesizeMixin from '../mixins/util/filesize.js'

export default {
  mixins: [ HumanFilesizeMixin ],
  data () {
    return {
      userInfo: {
        quota: null,
        usage: null,
      },
      updatePassword: {
        old: '',
        password: '',
        confirm: '',
        err: '',
        e: true // enabled
      },
      ready: false
    }
  },
  computed: {
    appName: function () {
      return this.$store.state.data.appName
    },
    username: function () {
      return this.$store.getters.auth_data.un;
    }
  },
  methods: {
    tryUpdatePassword () {
      let vm = this
      if (!vm.updatePassword.e) return
      // make sure confirmation is correct
      if (vm.updatePassword.password.length < 8) {
        vm.updatePassword.err = 'password must be at least 8 characters. this is also validated on the server'
        return
      }
      if (vm.updatePassword.password !== vm.updatePassword.confirm) {
        vm.updatePassword.err = 'password confirmation does not match'
        return
      }
      vm.updatePassword.e = false
      // reset error message
      vm.updatePassword.err = ''
      // update password
      vm.$store.dispatch('change_password', {
        o: vm.updatePassword.old,
        n: vm.updatePassword.password
      })
        .then((response) => {
          // proceed
          vm.$router.replace('/')
          vm.updatePassword.e = true
        })
        .catch(function (error) {
          if (error) {
            console.log(error)
          }
          vm.updatePassword.err = 'password change failed. is the old password correct?'
          vm.updatePassword.e = true
        })
    },

    deleteAllFiles() {
      let vm = this
      vm.$root.showConfirm('Are you absolutely sure? All files that you have uploaded will be permanently removed. You will then be logged out.', 'Confirm Action', function (r) {
        if (r) {
          // request files nuke; this will log out
          vm.$store.dispatch('delete_all_files')
            .then(() => {
              // proceed
              vm.$router.replace('/')
            })
        }
      })
    },

    deleteAccount() {
      let vm = this
      vm.$root.showConfirm('Are you absolutely sure? Your account and all files that you have uploaded will be permanently removed.', 'Confirm Action', function (r1) {
        if (r1) {
          if (window.confirm('Your account will be deleted. Are you certain you would like to proceed?')) {
            // request account nuke; this will log out
            vm.$store.dispatch('delete_account')
              .then(() => {
                // proceed
                vm.$router.replace('/')
              })
          }
        }
      })
    }
  },
  mounted: function () {
    this.$store.dispatch('get_user_info', this.$store.getters.api)
      .then((ud) => {
        this.userInfo = {
          quota: this.humanFileSize(ud.quota),
          usage: this.humanFileSize(ud.usage)
        }
        this.ready = true
      })
  }
}
</script>
