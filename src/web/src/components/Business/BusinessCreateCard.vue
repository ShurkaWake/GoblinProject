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
    businessName: "",
    businessLocation: "",
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
    businessNameRules() {
      return [
        value => {
          if (value) return true
          return this.t('businessNameRequired')
        },
        value => {
          if (value.length > 5) return true
          return this.t('businessNameLength')
        }
      ]
    },

    locationRules() {
      return [
        value => {
          if (value) return true
          return this.t('businessLocationRequired')
        }
      ]
    },

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
    async createBusiness(){
      this.loading = true
      await axios.post('/business', {
          businessCreateModel: {
            name: this.businessName,
            location: this.businessLocation
          },
          userCreateModel: {
            fullName: this.fullName,
            email: this.email
          }
        })
        .then(response => {
          store.commit('setErrorMessage', this.t('businessCreated'))
          store.commit('setRedirectPath', "")
          store.commit('switchDialog')
        })
        .catch(response => {
          console.log(response)
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
          v-model="businessName"
          :rules="businessNameRules"
          color="deep-purple-darken-1"
          :label="t('businessNameLabel')"
      />
      <v-text-field
          v-model="businessLocation"
          :rules="locationRules"
          color="deep-purple-darken-1"
          :label="t('businessLocationLabel')"
      />
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
      <v-container
          class="d-flex flex-row justify-end"
      >
        <v-btn
            :loading="loading"
            @click="createBusiness"
            color="deep-purple-darken-1"
            type="submit"
            :text="t('create')">
        </v-btn>
      </v-container>
    </v-form>
  </v-card>
</template>

<i18n>
{
  "en": {
    "tittle": "Create business",
    "emailRequired": "Email is required",
    "emailValid": "Email must be valid",
    "create": "Create",
    "businessCreated": "Business was successfully created",
    "fullnameRequired": "Full Name cannot be empty",
    "emailLabel": "Email",
    "fullnameLabel": "Full name",
    "businessNameLabel": "Name of business",
    "businessLocationLabel": "Location of business",
    "businessNameRequired": "Business name is required",
    "businessNameLength": "Business name should be at least 6 character length",
    "businessLocationRequired": "Business location is required"
  },
  "uk": {
    "tittle": "Створити компанію",
    "emailRequired": "Електронна скринька обов'язкова",
    "emailValid": "Електронна скринька некоректна",
    "create": "Створити",
    "businessCreated": "Компанію було успішно створено",
    "fullnameRequired": "Ім'я не може бути пустим",
    "emailLabel": "Електронна скринька",
    "fullnameLabel": "Повне ім'я",
    "businessNameLabel": "Назва компанії",
    "businessLocationLabel": "Місце розміщення компанії",
    "businessNameRequired": "Назва бізнесу обов'язкова",
    "businessNameLength": "Назва бізнесу має містити щонайменше 6 символів",
    "businessLocationRequired": "Локація бізнесу обов'язкова"
  }
}
</i18n>

<style scoped>

</style>