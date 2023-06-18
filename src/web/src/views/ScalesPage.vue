<script>
import {defineComponent} from "vue";
import {useI18n} from "vue-i18n";
import ScalesCard from "@/components/Scales/ScalesCard.vue";
import SearchBar from "@/components/General/SearchBar.vue";
import axios from "axios";
import store from "@/store";

export default defineComponent({
  components: {ScalesCard, SearchBar},

  setup() {
    const { t } = useI18n({
      locale:"en",
      inheritLocale: true,
      useScope: 'local'
    })

    return { t }
  },

  data: () => ({
    scalesList: [],
    searchQuery: "",
    currentPage: 1,
    pageSize: 5,
    pageLength: 1
  }),

  methods: {
    async fetchScales(){
      await axios.get("/scales/all",{
        params: {
          SerialNumber: this.searchQuery,
          page: this.currentPage,
          perPage: this.pageSize
        }
      })
        .then(response => {
          this.scalesList = response.data.data
          this.pageLength = response.data.pageCount
        })
        .catch(response => {
          store.commit('setErrorMessage', response.response.data.errors[0])
          store.commit('setRedirectPath', "/")
          store.commit('switchDialog')
        })
    },
    search(event){
      this.searchQuery = event
      this.currentPage = 1
      this.fetchScales()
    }
  },
  mounted() {
    this.fetchScales()
  },

  beforeRouteLeave(){
    this.$emit('closeDrawer')
  }
})
</script>

<template>
<v-container class="d-flex flex-column justify-center" id="main">
  <SearchBar
      :label="t('searchLabel')"
      @searchClick="search"
  />
  <v-col
    id="col"
    class="justify-center"
    v-for="scales in scalesList"
    :key="scales.id"
  >
    <ScalesCard
      :scales-id="scales.id"
      :serial-number="scales.serialNumber"
      @scalesDeleted="fetchScales"
    />
  </v-col>
  <v-pagination
    :length="pageLength"
    v-model="currentPage"
    class="mt-6"
    @update:model-value="fetchScales"
  />

</v-container>
<router-link to="/scales/add">
  <v-btn
      id="addButton"
      color="primary"
      icon="mdi-plus"
  />
</router-link>

</template>

<i18n>
{
  "en": {
    "searchLabel": "Serial number"
  },
  "uk": {
    "searchLabel": "Серійний номер"
  }
}
</i18n>

<style scoped>
#main{
  max-width: 600px;
}

#addButton{
  position: fixed;
  right: 100px;
  bottom: 100px;
  width: 50px;
  height: 50px;
}
</style>