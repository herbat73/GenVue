<template>
  <v-app id="inspire">
    
    <v-navigation-drawer
      fixed
      v-model="sidebar_v"
      app
    >
        <v-list dense>
            <template v-for="(item, i) in sidebar">
                <v-subheader v-if="item.header" v-text="item.header" />
                <v-divider v-else-if="item.divider" light />
                <template v-else>
                    <template v-if="!item.autoHide || (item.unauthRequired && !loggedIn) ||
                      (item.authRequired && loggedIn && !item.managerRoleRequired && !item.adminRoleRequired) ||
                      (item.authRequired && loggedIn && item.adminRoleRequired && userIsAdmin) ||
                      (item.authRequired && loggedIn && item.managerRoleRequired && (userIsManager || userIsAdmin))">
                        <v-list-tile :key="item.title"
                                     @click="item.router != null ? visitRoute(item.router) : visitUrl(item.link)" >
                            <v-list-tile-action>
                                <v-icon>{{ item.avatar }}</v-icon>
                            </v-list-tile-action>
                            <v-list-tile-content>
                                <v-list-tile-title>{{ item.title }}</v-list-tile-title>
                            </v-list-tile-content>
                        </v-list-tile>
                    </template>
                </template>
            </template>
        </v-list>
    </v-navigation-drawer>

    <v-toolbar color="indigo" dark fixed app>
      <v-toolbar-side-icon @click.native.stop="sidebar_v = !sidebar_v"></v-toolbar-side-icon>
      <v-toolbar-title v-text="appWithRoleName"></v-toolbar-title>
    </v-toolbar>
    <v-content>
      <v-container fluid>
        <v-slide-y-transition mode="out-in">
          <div class="content-container">
            <transition name="slide" mode="out-in">
              <router-view></router-view>
            </transition>
          </div>
        </v-slide-y-transition>
      </v-container>
    </v-content>
    <v-footer color="indigo" app>
        <span class="white--text">MIT license</span>
    </v-footer>
    
    <v-dialog persistent v-model="confirmDialogOpen" ref="confirmDialog">
      <v-card>
          <v-card-title>{{ confirmDialog.title }}</v-card-title>
          <v-card-text v-html="confirmDialog.content"></v-card-text>
          <v-card-actions>
              <v-btn class="blue--text darken-1" flat @click.native="onConfirmResult(false)">{{ confirmDialog.cancel }}</v-btn>
              <v-btn class="blue--text darken-1" flat @click.native="onConfirmResult(true)">{{ confirmDialog.ok }}</v-btn>
          </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog persistent v-model="promptDialogOpen" ref="promptDialog">
      <v-card>
          <v-card-title>{{ promptDialog.title }}</v-card-title>
          <v-card-text>
            <v-text-field :label="promptDialog.hint"  v-model="promptDialog.resp" required></v-text-field>
          </v-card-text>
          <v-card-actions>
              <v-btn class="blue--text darken-1" flat @click.native="onPromptResult(false)">{{ promptDialog.cancel }}</v-btn>
              <v-btn class="blue--text darken-1" flat @click.native="onPromptResult(true)">{{ promptDialog.ok }}</v-btn>
          </v-card-actions>
      </v-card>
    </v-dialog>
  </v-app>
</template>

<script>
  export default {
    data () {
      return {
        sidebar_v: true,
        dark_theme: false,
        sidebar: [
          { header: 'Quick Links' },
          {
            title: 'Home',
            avatar: 'home',
            router: '/'
          },
          {
            title: 'Upload files',
            avatar: 'backup',
            router: '/d',
            authRequired: true,
            managerRoleRequired: true,
            autoHide: true
          },
          {
            title: 'Admin Page',
            avatar: 'build',
            router: '/admin',
            authRequired: true,
            adminRoleRequired: true,
            autoHide: true
          },
          {
            title: 'Files',
            avatar: 'cloud',
            router: '/f',
            authRequired: true,
            autoHide: true
          },
          {
            title: 'Account',
            avatar: 'person',
            router: '/u',
            authRequired: true,
            autoHide: true
          },
          {
            title: 'Login',
            avatar: 'person',
            router: '/login',
            unauthRequired: true,
            autoHide: true
          },
          {
            title: 'Logout',
            avatar: 'exit_to_app',
            router: '/logout',
            authRequired: true,
            autoHide: true
          },
          {
            title: 'About',
            avatar: 'info',
            router: 'about'
          },
          { divider: true },
          { header: 'No auth API calls' },
          {
            title: 'Counter',
            avatar: 'copyright',
            router: 'counter'
          },
          { header: 'Test auth API calls' },
          {
            title: 'Customers',
            avatar: 'contacts',
            router: 'customers',
            authRequired: true,
          }
        ],
        confirmDialog: {
          title: '',
          content: '',
          ok: 'OK',
          cancel: 'Cancel',
          callback: null
        },
        promptDialog: {
          title: '',
          ok: 'OK',
          cancel: 'Cancel',
          hint: '',
          resp: '',
          callback: null
        },
        confirmDialogOpen: false,
        promptDialogOpen: false
      }
    },
    computed: {
      appName: function () {
        return this.$store.state.data.appName
      },
      loggedIn: function() {
        return this.$store.state.auth.loggedIn
      },
      userIsAdmin: function () {
          return this.$store.state.auth.rolesData.isAdmin
      },
      userIsManager: function () {
          return this.$store.state.auth.rolesData.isManager
      },
      appWithRoleName: function () {
          let roles = ''
          let user = ''
          if (this.$store.state.auth.rolesData.isManager) {
              roles = ' - Manager'
          }
          if (this.$store.state.auth.rolesData.isAdmin) {
              roles = ' - Admin'
          }
          if (this.$store.state.auth.authData.un != null) {
              user = this.$store.state.auth.authData.un
          }

          return this.appName + ' ' + user + roles
      },
      userName: function () {
          return this.$store.state.auth.un
      }
    },
    methods: {
      visitUrl(u) {
       window.open(u)
      },
      visitRoute(r) {
        this.$router.push(r)
      },
      showConfirm (tx, ti, cb, y, n) {
        y = y || 'OK'
        n = n || 'Cancel'
        this.confirmDialog.ok = y
        this.confirmDialog.cancel = n
        this.confirmDialog.content = tx
        this.confirmDialog.title = ti
        this.confirmDialog.callback = cb
        this.confirmDialogOpen = true
      },
      showPrompt: function (ti, h, cb, y, n) {
        y = y || 'OK'
        n = n || 'Cancel'
        this.promptDialog.ok = y
        this.promptDialog.cancel = n
        this.promptDialog.hint = h
        this.promptDialog.title = ti
        this.promptDialog.callback = cb
        this.promptDialogOpen = true
      },
      onConfirmResult (r) {
        this.confirmDialogOpen = false
        this.confirmDialog.callback(r)
        this.confirmDialog.callback = null
      },
      onPromptResult (r) {
        this.promptDialogOpen = false
        if (r && this.promptDialog.resp) {
          this.promptDialog.callback(this.promptDialog.resp)
        } else {
          this.promptDialog.callback(false)
        }
        this.promptDialog.resp = ''
        this.promptDialog.callback = null
      },
      visitUrl (u) {
        window.open(u, '_blank')
      }
    }
  }
</script>
