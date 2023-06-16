<script>
  import {defineComponent} from "vue";
  import {useI18n} from 'vue-i18n'

  export default defineComponent({
    setup() {
      const { t } = useI18n({
        locale:"en",
        inheritLocale: true,
        useScope: 'local'
      })

      return { t }
    }
  })
</script>

<template>
  <v-card
      class="mx-auto text-h5"
      max-width="400"
      :title="t('tittle')"
      prepend-icon="mdi-account-alert">
    <v-card-text>
      <p class="font-weight-medium" id="card-text">
        {{t('message')}}
      </p>
    </v-card-text>
    <v-card-actions class="justify-end">
      <router-link to="/login">
        <v-btn
            variant="text"
            class="mr-6"
            color="deep-purple-darken-1">
          {{t('action')}}
        </v-btn>
      </router-link>
    </v-card-actions>
  </v-card>
</template>

<i18n>
{
  "en": {
    "tittle": "You are not authorized",
    "message": "Please sign in",
    "action": "SIGN IN"
  },
  "uk": {
    "tittle": "Ви не авторизовані",
    "message": "Будь ласка увійдіть до свого облікового запису",
    "action": "УВІЙТИ"
  }
}
</i18n>

<style scoped>
#card-text{
  font-size: 16px;
}
</style>