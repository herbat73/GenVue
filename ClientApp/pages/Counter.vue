<template>
  <div>
      <div class="text-md-center">
          <h1>Counter</h1>
          <v-layout row>
              <v-flex xs12 lg6 offset-lg3>
                  <h2>This is a simple example of a Vue.js component and Vuex</h2>
                  <p>Test for Vuex, no login required.</p>

                  <p>
                      Current count (Vuex): <strong>{{ currentCount }}</strong>
                  </p>
                  <p>
                      Auto count: <strong>{{ autoCount }}</strong>
                  </p>

                  <v-btn @click.native="incrementCounter()" color="primary" raised ripple>Increment</v-btn>
                  <v-btn @click.native="resetCounter()" color="warning" raised ripple>Reset</v-btn>
              </v-flex>

          </v-layout>
      </div>
  </div>
</template>

<script>

    export default {
        data() {
            return {
                autoCount: 0,
            }
        },

        computed: {
            currentCount: function () {
                return this.$store.getters.counter
            },
        },

        methods: {
            incrementCounter: function () {
                var counter = this.currentCount + 1
                this.$store.dispatch('setCounter', {counter : counter})
            },
            resetCounter: function () {
                this.$store.dispatch('setCounter', {counter : 0})
                this.autoCount = 0
            }
        },

        created() {
            setInterval(() => {
                this.autoCount += 1
            }, 1000)
        }
    }
</script>

