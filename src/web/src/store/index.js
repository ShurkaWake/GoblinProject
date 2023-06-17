import { createStore } from 'vuex'
import { authModule } from "@/store/authModule";
import {dialogModule} from "@/store/dialogModule";

export default createStore({
  state: {
    currency: {
      id: 2,
      name: "USD"
    }
  },

  mutations: {
    setCurrency(state, currency){
      state.currency = currency
    }
  },
  actions: {
  },
  modules: {
    auth: authModule,
    dialog: dialogModule
  }
})
