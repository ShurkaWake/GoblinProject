<script>
import {defineComponent} from "vue";
import {useI18n} from "vue-i18n";

export default defineComponent({
  setup() {
    const { t } = useI18n({
      locale:"en",
      inheritLocale: true,
      useScope: 'local'
    })

    return { t }
  },

  data: () => ({
    searchQuery: ""
  }),

  props: {
    label: {
      type:String,
      required: true
    }
  },

  methods: {
    searchClick(){
      this.$emit('searchClick', this.searchQuery)
    }
  }
})
</script>

<template>
<v-container class="d-flex flex-row align-center">
  <v-text-field
    color="yellow-accent-3"
    :label="label"
    v-model="searchQuery"
    class="w-75 ma-4"
  />
  <v-btn
    class="w-25"
    prepend-icon="mdi-magnify"
    @click="searchClick"
    :text="t('buttonText')"
  />
</v-container>
</template>

<i18n>
{
  "en": {
    "buttonText": "Search"
  },
  "uk": {
    "buttonText": "Пошук"
  }
}
</i18n>

<style scoped>

</style>