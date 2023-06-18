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
    show: false,
    loading: false,

    oldPassword: "",
    newPassword: "",
    newPasswordRepeat: "",
  }),

  computed: {
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
    ...mapActions({
      logout:"logout"
    }),

    async changePassword(){
      if (!this.valid){
        return
      }

      this.loading = true
      await axios.patch("/user/password", {
        oldPassword: this.oldPassword,
        newPassword: this.newPassword
      })
          .then(response => {
            store.commit('setErrorMessage', this.t('passwordChanged'))
            store.commit('setRedirectPath', "/unauthorized")
            this.logout()
            store.commit('switchDialog')
          })
          .catch(response => {
            store.commit('setErrorMessage', response.response.data.errors[0])
            store.commit('setRedirectPath', "")
            store.commit('switchDialog')
          })
      this.loading = false
    },

    matchingPasswords() {
      if (this.newPassword === undefined || this.newPasswordRepeat === undefined) {
        return this.t('passwordSame')
      }
      return this.newPassword === this.newPasswordRepeat ? true : this.t('passwordSame');
    },
  }
})
</script>

<template>
  <v-card class="pa-4">
    <v-form
        fast-fail
        v-model="valid"
        id="login-form"
        validate-on="input"
        @submit.prevent
    >
      <h3 id="form-title">{{t('tittle')}}</h3>
      <v-text-field
          v-model="oldPassword"
          color="yellow-accent-3"
          :append-icon="show ? 'mdi-eye' : 'mdi-eye-off'"
          :rules="passwordRules"
          :type="show ? 'text' : 'password'"
          :label="t('oldPassword')"
          @click:append="show = !show">
      </v-text-field>
      <v-text-field
          v-model="newPassword"
          color="deep-purple-darken-1"
          :append-icon="show ? 'mdi-eye' : 'mdi-eye-off'"
          :rules="passwordRules.concat(matchingPasswords)"
          :type="show ? 'text' : 'password'"
          :label="t('newPassword')"
          @click:append="show = !show">
      </v-text-field>
      <v-text-field
          v-model="newPasswordRepeat"
          color="deep-purple-darken-1"
          :append-icon="show ? 'mdi-eye' : 'mdi-eye-off'"
          :rules="passwordRules.concat(matchingPasswords)"
          :type="show ? 'text' : 'password'"
          :label="t('repeatNewPassword')"
          @click:append="show = !show">
      </v-text-field>
      <v-container
          class="d-flex justify-end">
        <v-btn
            :loading="loading"
            @click="changePassword"
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
    "tittle": "Change Password",
    "oldPassword": "Current password",
    "newPassword": "New password",
    "repeatNewPassword": "Repeat new password",
    "passwordRequired": "Password is required",
    "passwordRegex": "Password should be between 6 and 50 characters and contain at least one lowercase letter, uppercase letter, special character and numeric symbol",
    "passwordSame": "Passwords must be the same",
    "passwordChanged": "Password was successfully changed",
    "change": "Change"
  },
  "uk": {
    "tittle": "Змінити пароль",
    "email": "Електронна скринька",
    "oldPassword": "Старий пароль",
    "newPassword": "Новий пароль",
    "repeatNewPassword": "Повторіть новий пароль",
    "passwordRequired": "Пароль обов'язковий",
    "passwordRegex": "Пароль має бути довжиною від 6 до 50 символів та містити прописні та друковані літери, цифри та спеціальні символи",
    "passwordSame": "Паролі мають співпадати",
    "passwordChanged": "Пароль було успішно змінено",
    "change": "Змінити"
  }
}
</i18n>

<style scoped>

</style>