<script>
import {defineComponent} from "vue";
import {mapState} from "vuex";
import store from "@/store";

export default defineComponent({
  data: () => ({
    currentFormat: "dd.MM.yyyy",

    supportedFormats: [
      "dd.MM.yyyy",
      "MM.dd.yyyy",
      "yyyy.MM.dd"
    ]
  }),

  methods: {
    changeFormat(){
      store.commit("setDateFormat", this.currentFormat)
    }
  },

  mounted() {
    if (this.storeFormat !== undefined){
      this.currentFormat = this.storeFormat
    }
  },

  computed: {
    ...mapState({
      storeFormat: state => state.dateFormat
    })
  }
})
</script>

<template>
  <v-select
      v-model="currentFormat"
      :items="supportedFormats"
      @update:model-value="changeFormat"
  />
</template>

<style>

</style>