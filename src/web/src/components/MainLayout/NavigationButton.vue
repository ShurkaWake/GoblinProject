<script>
import {defineComponent} from "vue";

export default defineComponent({
  props: {
    icon: {
      type: String,
      required: false
    },
    text: {
      type: String,
      required: true
    },
    link: {
      type: String,
      required: true
    }
  }
})
</script>

<template>
  <router-link :to="link">
    <v-btn
        class="mt-4 justify-start"
        width="100%"
        :prepend-icon="icon"
        :text="text"
        variant="text"
        color="deep-purple-darken-1"></v-btn>
  </router-link>
</template>

<style scoped>

</style>