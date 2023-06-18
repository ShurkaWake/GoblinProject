<script>
export default {
  beforeRouteLeave(){
    this.$emit('closeDrawer')
  }
}
</script>

<template>
  <v-container fluid class="fill-height justify-center">
    <router-view/>
  </v-container>
</template>

<style scoped>

</style>