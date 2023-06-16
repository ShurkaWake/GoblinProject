<script setup>
import LocaleChanger from "@/components/MainLayout/LocaleChanger.vue";

defineEmits(['drawerClick'])
</script>

<script>
import {defineComponent} from "vue";
import {mapState} from "vuex";

export default defineComponent({
  methods: {
    changeDrawerState(event){
      this.$emit('drawerClick')
    }
  },

  computed: {
    ...mapState({
      isAuth: state => state.auth.isAuth,
    })
  }
})
</script>

<template>
  <v-app-bar
      app
      class="bg-green-darken-1 align-center"
      height="80"
  >
    <v-col cols="auto">
      <v-app-bar-nav-icon
          @click.stop="changeDrawerState"
          :disabled="!isAuth"
      />
    </v-col>
    <v-col cols="auto">
      <router-link
          class="text-decoration-none text-white"
          to="/">
        <v-toolbar-title>Goblin Project</v-toolbar-title>
      </router-link>
    </v-col>
    <v-spacer/>
    <v-col cols="auto">
      <LocaleChanger id="locale"/>
    </v-col>

  </v-app-bar>
</template>

<style>
</style>