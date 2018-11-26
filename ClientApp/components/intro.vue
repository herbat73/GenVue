<template>
  <div class="intro">
    <div class="text-md-center">
      <img src="/static/images/icon.png" />
      <h1>{{ appName }}</h1>
    </div>
    <div class="text-md-center">
      <p>
        just a test web application for file sharing
      </p>
    </div>
  </div>
</template>

<script>
export default {
  name: 'intro',
  data () {
    return {
    }
  },
  computed: {
    appName: function () {
      return this.$store.state.data.appName
    }
  }
}
</script>
