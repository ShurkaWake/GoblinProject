<script>
import {defineComponent} from "vue";
import {useI18n} from "vue-i18n";
import store from "@/store";
import {mapActions} from "vuex";
import axios from "axios";

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
    loading: false,

    fullName: "",
  }),

  computed: {
    fullnameRules(){
      return [
        value => {
          if (value) return true
          return this.t('fullnameRequired')
        },
      ]
    }
  },

  methods: {
    async changeFullname(){
      if (!this.valid){
        return
      }

      this.loading = true
      await axios.patch("/user", {
        fullName: this.fullName
      })
          .then(response => {
            store.commit('setErrorMessage', this.t('fullNameChanged'))
            store.commit('setRedirectPath', "")
            store.commit('setFullName', this.fullName)
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
  <v-card class="pa-4">
    <v-form
        fast-fail
        v-model="valid"
        validate-on="input"
        @submit.prevent
    >
      <h3 id="form-title">{{t('tittle')}}</h3>
      <v-text-field
          v-model="fullName"
          color="deep-purple-darken-1"
          :rules="fullnameRules"
          type='text'
          :label="t('fullName')"
      />
      <v-container
          class="d-flex justify-end">
        <v-btn
            :loading="loading"
            @click="changeFullname"
            color="deep-purple-darken-1"
            type="submit"
            :text="t('change')">
        </v-btn>
      </v-container>
    </v-form>
  </v-card>
</template>

<i18n>
{
  "en": {
    "tittle": "Change full name",
    "fullName": "Full Name",
    "fullnameRequired": "Full Name cannot be empty",
    "change": "Change",
    "fullNameChanged": "Full Name was successfully changed"
  },
  "uk": {
    "tittle": "Змінити повне ім'я",
    "fullName": "Повне ім'я",
    "fullnameRequired": "Ім'я не може бути пустим",
    "change": "Змінити",
    "fullNameChanged": "Ім'я було успішно змінено"
  }
}
</i18n>

<style scoped>

</style>