<script>
import {defineComponent} from "vue";
import {useI18n} from "vue-i18n";
import axios from "axios";
import store from "@/store";

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
    loading: false,
  }),

  props: {
    serialNumber: {
      type: String,
      required: true
    },
    scalesId: {
      type: Number,
      required: true
    }
  },

  computed: {
    getSerialNumber(){
      return this.serialNumber
    }
  },

  methods: {
    async deleteScales(){
      this.loading = true

      await axios.delete('/scales/' + this.scalesId)
        .then(response => {
          this.$emit('scalesDeleted')
        })
        .catch(response => {
          store.commit('setErrorMessage', response.response.data.errors[0])
          store.commit('setRedirectPath', "")
          store.commit('switchDialog')
        })

      this.loading = false
    }
  },
})
</script>

<template>
<v-card
  class="pa-2 text-decoration-none"
  prepend-icon="mdi-scale-balance"
  :title="getSerialNumber">
  <v-container
      class="d-flex justify-end">
    <v-btn
        :loading="loading"
        @click="deleteScales"
        color="red-darken-1"
        variant="text"
        :text="t('buttonName')">
    </v-btn>
  </v-container>
</v-card>
</template>

<i18n>
{
  "en": {
    "buttonName": "Delete"
  },
  "uk": {
    "buttonName": "Видалити"
  }
}
</i18n>

<style scoped>

</style>