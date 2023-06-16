import { createStore } from 'vuex'
import { authModule } from "@/store/authModule";
import {dialogModule} from "@/store/dialogModule";

export default createStore({
  state: {
  },
  getters: {
  },
  mutations: {
  },
  actions: {
  },
  modules: {
    auth: authModule,
    dialog: dialogModule
  }
})
