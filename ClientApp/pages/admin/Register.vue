<template>
  <div>
    <div class="text-md-center">
      <h2>Register</h2>
      <v-layout row>
        <v-flex xs12 lg6 offset-lg3>
          <login-form mode="register"></login-form>
        </v-flex xs8>
      </v-layout>
    </div>
  </div>
</template>

<script>

import loginForm from '../../components/loginForm.vue'

export default {
  data () {
    return {
    }
  },
  components: {
    loginForm
  }
}
</script>
