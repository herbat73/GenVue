<template>
  <div class="intro">
    <div class="text-md-center">
        <img src="iconadminpage.png" />
        <h1 class="app-title">{{ appName }}</h1>
    </div>
      <div class="text-md-center">
          <p>
              this is Admin page
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
