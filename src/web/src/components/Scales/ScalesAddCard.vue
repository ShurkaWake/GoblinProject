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
    valid: false,
    serialNumber: "",
    loading: false
  }),

  computed: {
    serialRules(){
      return [
        value => {
          if (value) return true
          return this.t('serialNumberRequired')
        },
        value => {
          if (value.length === 12) return true
          return this.t('serialNumberLength')
        },
      ]
    },
  },

  methods: {
    async addScales(){
      this.loading = true
      await axios.post('scales', {
        serialNumber: this.serialNumber
      })
        .then(response => {
          store.commit('setErrorMessage', this.t('scalesAdded'))
          store.commit('setRedirectPath', "/scales")
          store.commit('switchDialog')
        })
        .catch(response => {
          store.commit('setErrorMessage', response.response.data.errors[0])
          store.commit('setRedirectPath', "")
          store.commit('switchDialog')
        })

      this.loading = false
    }
  }
})
</script>

<template>
  <v-card class="pa-4" width="400">
    <v-form
        fast-fail
        v-model="valid"
        id="login-form"
        validate-on="input"
        @submit.prevent
    >
      <h1 id="form-title">{{t('tittle')}}</h1>
      <v-text-field
          v-model="serialNumber"
          :rules="serialRules"
          color="deep-purple-darken-1"
          :label="t('serialNumberLabel')"
          required>
      </v-text-field>
      <v-btn
          :loading="loading"
          @click="addScales"
          color="deep-purple-darken-1"
          type="submit"
          :text="t('add')">
      </v-btn>
    </v-form>
  </v-card>
</template>

<i18n>
{
  "en": {
    "tittle": "Add scales",
    "serialNumberLabel": "Serial number",
    "serialNumberRequired": "Serial number required",
    "serialNumberLength": "Serial number must be 12 characters long",
    "add": "Add",
    "scalesAdded": "Scales was successfully added"
  },
  "uk": {
    "tittle": "Додати ваги",
    "serialNumberLabel": "Серійний номер",
    "serialNumberRequired": "Серійний номер обов'язковий",
    "serialNumberLength": "Серійний номер має бути довжиною в 12 символів",
    "add": "Додати",
    "scalesAdded": "Ваги успішно додані"
  }
}
</i18n>

<style scoped>

</style>