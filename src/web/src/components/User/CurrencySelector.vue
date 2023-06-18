<script>
import {defineComponent} from "vue";
import axios from "axios";
import {mapState} from "vuex";
import store from "@/store";

export default defineComponent({
  data: () => ({
    currentCurrency:{
      id: 2,
      name: "USD"
    },

    supportedCurrency: [
      {
        id: 1,
        name: "UAH"
      },
      {
        id: 2,
        name: "USD"
      },
      {
        id: 3,
        name: "EUR"
      }
    ]
  }),

  methods: {
    changeCurrency(){
      store.commit("setCurrency", this.currentCurrency)
      axios.defaults.headers.common['currency'] = this.storeCurrency.id
    }
  },

  mounted() {
    if (this.storeCurrency !== undefined){
      this.currentCurrency = this.storeCurrency
    }
  },

  computed: {
    ...mapState({
      storeCurrency: state => state.currency
    })
  }
})
</script>

<template>
  <v-select
      v-model="currentCurrency"
      id="currency-selector"
      :items="supportedCurrency"
      item-title="name"
      @update:model-value="changeCurrency"
      return-object
  />
</template>

<style>

#currency-selector{
  max-width: 200px;
}
</style>