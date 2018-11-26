<template>
  <div>
    <h2 class="center">Dashboard</h2>
    <fileUploadWidget></fileUploadWidget>
  </div>
</template>

<script>
import fileUploadWidget from '../components/fileUploadWidget.vue'

export default {
  data () {
    return {
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
  components: { fileUploadWidget }
}
</script>