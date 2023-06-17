<script>
import {defineComponent} from "vue";
import {useI18n} from 'vue-i18n'
import {mapActions, mapState} from "vuex";
import LocaleChanger from "@/components/MainLayout/LocaleChanger.vue";
import NavigationButton from "@/components/MainLayout/NavigationButton.vue";

export default defineComponent({
  methods: {
    ...mapActions({
      logoutAction: "logout"
    }),

    closeDrawer() {
      this.$emit("closeDrawer")
    },

    exit() {
      this.logoutAction()
      this.$emit("closeDrawer")
    }
  },
  components: {NavigationButton, LocaleChanger},
  setup() {
    const { t } = useI18n({
      locale:"en",
      inheritLocale: true,
      useScope: 'local'
    })

    return { t }
  },
  props: {
    drawer: {
      type: Boolean,
      required: true
    }
  },

  computed: {
    ...mapState({
      isAuth: state => state.auth.isAuth,
      fullName: state => state.auth.userFullName,
      email: state => state.auth.userEmail,
      role: state => state.auth.userRole
    }),

    drawerState(){
      return this.drawer
    }
  }
})
</script>

<template>
  <v-navigation-drawer
      width="400"
      v-model="drawerState"
      location="start"
  >
    <v-container>
      <v-container v-if="isAuth"
                   class="d-flex flex-column">
        <router-link
          to="/"
          class="text-decoration-none"
        >
          <v-card
            prepend-icon="mdi-account"
            class="pa-2"
            v-bind:title='fullName'
          >
            <v-card-subtitle>{{email}}</v-card-subtitle>
          </v-card>
        </router-link>

        <NavigationButton
          link="/scales"
          :text="t('scales')"
          icon="mdi-scale-balance"
        />
        <NavigationButton
          link="/workers"
          :text="t('workers')"
          icon="mdi-account-hard-hat"
        />
        <v-divider/>
        <NavigationButton
          link="/unauthorized"
          :text="t('logOut')"
          icon="mdi-logout"
        />

      </v-container>
    </v-container>
  </v-navigation-drawer>
</template>

<i18n>
{
  "en": {
    "scales": "Scales",
    "workers": "Workers",
    "logOut": "Log out"
  },
  "uk": {
    "scales": "Ваги",
    "workers": "Робітники",
    "logOut": "Вийти"
  }
}
</i18n>

<style scoped>

</style>