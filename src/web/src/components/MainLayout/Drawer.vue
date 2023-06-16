<script>
import {defineComponent} from "vue";
import {useI18n} from 'vue-i18n'
import {mapState} from "vuex";
import LocaleChanger from "@/components/MainLayout/LocaleChanger.vue";

export default defineComponent({
  components: {LocaleChanger},
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
      isAuth: state => state.isAuth,
      fullName: state => state.userFullName,
      email: state => state.userEmail
    })
  }
})
</script>

<template>
  <v-navigation-drawer
      width="400"
      v-model="drawer"
      location="start"
  >
    <v-container>
      <v-container v-if="isAuth"
                   class="d-flex flex-column">
        <v-card
            prepend-icon="mdi-account"
            class="pa-2"
            v-bind:title='fullName'>
          <v-card-subtitle>{{email}}</v-card-subtitle>
        </v-card>

      </v-container>
    </v-container>
  </v-navigation-drawer>
</template>

<style scoped>

</style>