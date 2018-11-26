<template>
</template>

<script>
export default {
  mounted: function () {
    console.log('logout mounted')
    this.$store.dispatch('logout')
    // proceed
    this.$router.push('/')
  }
}
</script>
