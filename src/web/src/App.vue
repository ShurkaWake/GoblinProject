<template>
  <v-app>
      <v-layout height="100%" width="100%">
        <AppBar @drawer-click="changeDrawerState"/>
        <Drawer :drawer="drawer"></Drawer>
        <v-main>
          <router-view/>
        </v-main>
      </v-layout>

      <v-dialog
          v-model="dialogState"
          width="auto">
        <v-card>
          <v-card-text>
            {{ message }}
          </v-card-text>
          <v-card-actions>
            <v-btn color="deep-purple-darken-1" block @click="closeDialog">Close</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
  </v-app>
</template>

<script>
import AppBar from "@/components/MainLayout/AppBar.vue";
import Drawer from "@/components/MainLayout/Drawer.vue";
import {mapMutations, mapState} from "vuex";
import router from "@/router";

export default {
  name: 'App',
  components: {AppBar, Drawer},

  data: () => ({
    drawer: false,
  }),

  methods: {
    changeDrawerState() {
      this.drawer = !this.drawer
    },

    closeDialog(){
      this.changeDialog()
      if (this.redirectPath !== ""){
        router.push(this.redirectPath)
      }
    },

    ...mapMutations({
      changeDialog:"switchDialog",
    })
  },

  computed: {
    ...mapState({
      dialogState: state => state.dialog.dialog,
      message: state => state.dialog.message,
      redirectPath: state => state.dialog.redirectPath
    }),
  }
}
</script>
