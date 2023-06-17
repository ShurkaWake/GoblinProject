<script>
import {defineComponent} from "vue";
import {useI18n} from "vue-i18n";
import {mapState} from "vuex";
import BusinessCard from "@/components/Business/BusinessCard.vue"
import ChangePasswordCard from "@/components/User/ChangePasswordCard.vue";
import ChangeFullNameCard from "@/components/User/ChangeFullNameCard.vue";
import CurrencySelector from "@/components/User/CurrencySelector.vue";

export default defineComponent({
  components: {CurrencySelector, BusinessCard, ChangePasswordCard, ChangeFullNameCard},

  setup() {
    const {t} = useI18n({
      locale: "en",
      inheritLocale: true,
      useScope: 'local'
    })

    return {t}
  },

  computed: {
    ...mapState({
      fullName: state => state.auth.userFullName,
      email: state => state.auth.userEmail,
      role: state => state.auth.userRole
    })
  },

  beforeRouteLeave(){
    this.$emit('closeDrawer')
  }
})
</script>

<template>
<v-container class="d-flex flex-row justify-center">
  <v-col id="col">
    <v-card
        prepend-icon="mdi-account"
        class="pa-2 ma-4"
        v-bind:title='fullName'>
      <v-card-subtitle>{{email}}</v-card-subtitle>
      <v-card-text>{{t(role.toString())}}</v-card-text>
    </v-card>
    <BusinessCard class="ma-4"/>
    <ChangePasswordCard class="ma-4"/>
    <ChangeFullNameCard class="ma-4"/>
    <v-card
        class="pa-2 ma-4"
        :title="this.t('chooseCurrency')">
      <CurrencySelector/>
    </v-card>
  </v-col>
</v-container>
</template>

<i18n>
{
  "en": {
    "owner": "Owner",
    "manager": "Manager",
    "admin": "Admin",
    "chooseCurrency": "Choose currency used in app"
  },
  "uk": {
    "owner": "Власник",
    "manager": "Управляючий",
    "admin": "Адміністратор",
    "chooseCurrency": "Виберіть валюту у якій бажаєте бачити статистику"
  }
}
</i18n>

<style scoped>
#col{
  max-width: 600px;
}
</style>