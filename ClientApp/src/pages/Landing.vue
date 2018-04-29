<template>
  <div>
    <intro></intro>
    <div class="mg-top">
      <v-layout row>
        <v-flex xs12 md8 offset-md2>
          <div class="text-md-center">
            <template v-for="(item, ix) in quicklinks">
              <v-btn v-if="!item.autoHide || (item.unauthRequired && !loggedIn) || (item.authRequired && loggedIn)"
                :color="item.color"
                @click.native="item.router != null ? visitRoute(item.router) : visitUrl(item.link)">
                {{ item.name }}
                <v-icon right>{{ item.icon }}</v-icon>
              </v-btn>
            </template>
          </div>
        </v-flex>
      </v-layout>
    </div>
  </div>
</template>

<script>
import intro from '../components/intro.vue'

export default {
  data () {
    return {
      quicklinks: [
        {
          name: 'Source',
          icon: 'code',
          link: 'https://github.com/herbat73/GenVue',
          color: 'warning'
        },
        {
          name: 'Log In',
          icon: 'person',
          router: '/login',
          autoHide: true,
          unauthRequired: true,
          color: 'primary'
        },
        {
          name: 'Log Out',
          icon: 'exit_to_app',
          router: '/logout',
          autoHide: true,
          authRequired: true,
          color: 'purple'
        },
        {
          name: 'Support',
          icon: 'favorite',
          link: 'http://figaro.com.pl',
          color: 'teal'
        }
      ]
    }
  },
  computed: {
    loggedIn: function() {
      return this.$store.state.auth.loggedIn
    }
  },
  methods: {
    visitUrl (u) {
      window.open(u)
    },
    visitRoute (r) {
      this.$router.push(r)
    }
  },
  components: { intro }
}
</script>

<style>
.title-subcontent {
  margin-top: 2%;
}
</style>
