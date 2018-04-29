<template>
  <div>
      <file-browser />
  </div>
</template>

<script>
import fileBrowser from '../components/fileBrowser.vue'

export default {
  data () {
    return {
      d: '/'
    }
  },
  computed: {
    appName: function () {
      return this.$store.state.data.appName
    }
  },
  components: { fileBrowser }
}
</script>
