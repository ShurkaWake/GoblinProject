<script>
import {defineComponent} from "vue";

export default defineComponent({
  data: () => ({
    currentLocale: null,

    supportedLocales: [
      {
        short: "en",
        long: "English",
      },
      {
        short: "uk",
        long: "Українська"
      }
    ]
  }),
  methods: {
    changeLocale(){
      this.$i18n.locale = this.currentLocale.short
    }
  },

  mounted() {
    let locale = this.$i18n.locale
    this.currentLocale = this.supportedLocales.find(x => locale === x.short)
  }
})
</script>

<template>
  <v-select
      v-model="currentLocale"
      id="locale"
      width="10px"
      :items="supportedLocales"
      item-title="long"
      @update:model-value="changeLocale"
      return-object
  />
</template>

<style>

#locale{
  max-width: 200px;
}
</style>