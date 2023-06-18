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
    email: "",
    fullName: "",
    loading: false,

    selected: "manager",
    roles: [
      "manager",
      "foreman"
    ]
  }),

  computed: {
    emailRules(){
      return [
        value => {
          if (value) return true
          return this.t('emailRequired')
        },
        value => {
          if (/.+@.+\..+/.test(value)) return true
          return this.t('emailValid')
        },
      ]
    },

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
    async addWorker(){
      this.loading = true
      await axios.post('/user', {
        fullName: this.fullName,
        email: this.email,
      },
    {
      params: {
        role: this.selected
      }
        })
        .then(response => {
          store.commit('setErrorMessage', this.t('workerAdded'))
          store.commit('setRedirectPath', "/workers")
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
      validate-on="input"
      @submit.prevent
    >
      <h1 id="form-title">{{t('tittle')}}</h1>
      <v-text-field
        v-model="email"
        :rules="emailRules"
        color="deep-purple-darken-1"
        :label="t('emailLabel')"
      />
      <v-text-field
        v-model="fullName"
        :rules="fullnameRules"
        color="deep-purple-darken-1"
        :label="t('fullnameLabel')"
      />
      <v-select
        v-model="selected"
        :items="roles"
      />
      <v-container
        class="d-flex flex-row justify-end"
      >
        <v-btn
            :loading="loading"
            @click="addWorker"
            color="deep-purple-darken-1"
            type="submit"
            :text="t('add')">
        </v-btn>
      </v-container>
    </v-form>
  </v-card>
</template>

<i18n>
{
  "en": {
    "tittle": "Add worker",
    "emailRequired": "Email is required",
    "emailValid": "Email must be valid",
    "add": "Add",
    "workerAdded": "Worker was successfully added",
    "manager": "Manager",
    "foreman": "Foreman",
    "fullnameRequired": "Full Name cannot be empty",
    "emailLabel": "Email",
    "fullnameLabel": "Full name"
  },
  "uk": {
    "tittle": "Додати робітника",
    "emailRequired": "Електронна скринька обов'язкова",
    "emailValid": "Електронна скринька некоректна",
    "add": "Додати",
    "workerAdded": "Робітника успішно додано",
    "manager": "Управляючий",
    "foreman": "Бригадир",
    "fullnameRequired": "Ім'я не може бути пустим",
    "emailLabel": "Електронна скринька",
    "fullnameLabel": "Повне ім'я"
  }
}
</i18n>

<style scoped>

</style>