<script>
import {defineComponent} from "vue";
import {useI18n} from "vue-i18n";
import axios from "axios";
import store from "@/store";

export default defineComponent({
  setup() {
    const {t} = useI18n({
      locale: "en",
      inheritLocale: true,
      useScope: 'local'
    })

    return {t}
  },

  data: () => ({
    businessName: "",
    businessLocation: ""
  }),

  methods: {
    async fetchBusiness(){
      await axios.get('/business')
          .then(response => {
            this.businessName = response.data.data.name
            this.businessLocation = response.data.data.location
          })
          .catch(response => {
            store.commit('setErrorMessage', response.response.data.errors[0])
            store.commit('setRedirectPath', "")
            store.commit('switchDialog')
          })
    }
  },

  mounted() {
    this.fetchBusiness()
  }
})
</script>

<template>
  <v-card
      prepend-icon="mdi-domain"
      class="pa-2"
      v-bind:title='businessName'>
    <v-card-subtitle id="subtittle">{{businessLocation}}</v-card-subtitle>
  </v-card>
</template>

<style scoped>
#subtittle{
  font-size: 18px;
}
</style>