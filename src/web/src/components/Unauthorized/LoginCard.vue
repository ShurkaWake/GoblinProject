<script>
import {mapActions, mapMutations, mapState} from "vuex";
import {defineComponent} from "vue";
import {useI18n} from "vue-i18n";
import {th} from "vuetify/locale";

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
    show: false,
    email: "",
    password: "",
  }),

  computed: {
    ...mapState({
      loading:"loading"
    }),

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

    passwordRules(){
      return [
        value => {
          if (value) return true
          return this.t('passwordRequired')
        },
        value => {
          if (/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$/.test(value)) return true
          return this.t('passwordRegex')
        },
      ]
    }
  },

  methods: {
    ...mapMutations({
      setEmail: "setLoginEmail",
      setPassword: "setLoginPassword",
    }),
    ...mapActions({
      login: "login"
    }),
    submitLogin(){
      if (!this.valid){
        return
      }
      this.login()
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
          v-model="email"
          @update:model-value="setEmail"
          :rules="emailRules"
          color="deep-purple-darken-1"
          :label="t('email')"
          required>
      </v-text-field>
      <v-text-field
          v-model="password"
          @update:model-value="setPassword"
          color="deep-purple-darken-1"
          :append-icon="show ? 'mdi-eye' : 'mdi-eye-off'"
          :rules="passwordRules"
          :type="show ? 'text' : 'password'"
          :label="t('password')"
          @click:append="show = !show">
      </v-text-field>
      <v-container
          class="d-flex justify-end">
        <v-btn
            :loading="loading"
            @click="submitLogin"
            color="deep-purple-darken-1"
            type="submit"
            :text="t('tittle')">
        </v-btn>
      </v-container>
    </v-form>
  </v-card>
</template>

<i18n>
{
  "en": {
    "tittle": "Log In",
    "email": "Email",
    "password": "Password",
    "emailRequired": "Email is required",
    "emailValid": "Email must be valid",
    "passwordRequired": "Password is required",
    "passwordRegex": "Password should be between 6 and 50 characters and contain at least one lowercase letter, uppercase letter, special character and numeric symbol"
  },
  "uk": {
    "tittle": "Увійти",
    "email": "Електронна скринька",
    "password": "Пароль",
    "emailRequired": "Електронна скринька обов'язкова",
    "emailValid": "Електронна скринька некоректна",
    "passwordRequired": "Пароль обов'язковий",
    "passwordRegex": "Пароль має бути довжиною від 6 до 50 символів та містити прописні та друковані літери, цифри та спеціальні символи"
  }
}
</i18n>

<style scoped>
#form-title {
  width: inherit;
  padding: 8px;
  text-align: center;
}
</style>