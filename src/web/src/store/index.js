import { createStore } from 'vuex'
import { authModule } from "@/store/authModule";
import {dialogModule} from "@/store/dialogModule";

export default createStore({
  state: {
    currency: {
      id: 2,
      name: "USD"
    },

    dateFormat: "dd.MM.yyyy"
  },

  mutations: {
    setCurrency(state, currency){
      state.currency = currency
    },

    setDateFormat(state, format){
      state.dateFormat = format
    }
  },
  actions: {
  },
  modules: {
    auth: authModule,
    dialog: dialogModule
  }
})
