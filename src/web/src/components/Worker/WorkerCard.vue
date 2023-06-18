<script>
import {defineComponent} from "vue";
import {useI18n} from "vue-i18n";
import axios from "axios";
import store from "@/store";
import {mapState} from "vuex";

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
    workerId: {
      type: String,
      required: true
    },
    fullName: {
      type: String,
      required: true
    },
    role: {
      type: String,
      required: true
    },
    email: {
      type: String,
      required: true
    }
  },

  computed: {
    getRole(){
      return this.role
    },

    getFullName(){
      return this.fullName
    },

    getEmail(){
      return this.email
    },

    ...mapState({
      userRole: state => state.auth.userRole
    })
  },

  methods: {
    async fireWorker(){
      this.loading = true

      await axios.delete('/user/workers/' + this.workerId)
          .then(response => {
            this.$emit('workerDeleted')
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
      prepend-icon="mdi-account-hard-hat"
      :title="getFullName"
      :subtitle="getEmail">
    <v-label :text="t(getRole)"/>
    <v-container
        class="d-flex justify-end">
      <v-btn
          v-if="userRole === 'owner'"
          :loading="loading"
          @click="fireWorker"
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
    "owner": "Owner",
    "manager": "Manager",
    "admin": "Admin",
    "foreman": "Foreman",
    "buttonName": "Fire"
  },
  "uk": {
    "owner": "Власник",
    "manager": "Управляючий",
    "admin": "Адміністратор",
    "foreman": "Бригадир",
    "buttonName": "Звільнити"
  }
}
</i18n>

<style scoped>

</style>