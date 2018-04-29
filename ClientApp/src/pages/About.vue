<template>
  <div>
    <intro></intro>
    <about></about>
  </div>
</template>

<script>
import intro from '../components/intro.vue'
import about from '../components/about.vue'

export default {
  data () {
    return {
    }
  },
  computed: {
    appName: function () {
      return this.$store.state.data.appName
    }
  },
  components: {
    about,
    intro
  }
}
</script>