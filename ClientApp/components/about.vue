<template>
    <div class="text-md-center">
        <h5>About {{ appName }}</h5>
        <p>
            {{ appName }} is a hostable, fully featured web application that lets confidential users upload and share private files in the most secure way.
        </p>
        <div class="mg-top">
            <h5>Application Instance Information</h5>
            <h6 class="version-info">
                Application version (server): <code>{{ app_version }}</code>
            </h6>
        </div>
    </div>
</template>

<script>
export default {
  name: 'about',
  data () {
    return {
      app_version: window.$appVersion
    }
  },
  computed: {
    appName: function () {
      return this.$store.state.data.appName
    }
  }
}
</script>

<style>
.version-info {
  font-size: 1.2rem;
  font-weight: 400;
}
</style>
