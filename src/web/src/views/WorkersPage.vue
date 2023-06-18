<script>
import {defineComponent} from "vue";
import {useI18n} from "vue-i18n";
import WorkerCard from "@/components/Worker/WorkerCard.vue";
import SearchBar from "@/components/General/SearchBar.vue";
import axios from "axios";
import store from "@/store";
import {mapState} from "vuex";

export default defineComponent({
  components: {WorkerCard, SearchBar},

  setup() {
    const { t } = useI18n({
      locale:"en",
      inheritLocale: true,
      useScope: 'local'
    })

    return { t }
  },

  data: () => ({
    workersList: [],
    searchQuery: "",
    currentPage: 1,
    pageSize: 5,
    pageLength: 1
  }),

  computed: {
    ...mapState({
      userRole: state => state.auth.userRole
    })
  },

  methods: {
    async fetchWorkers(){
      await axios.get("/user/workers",{
        params: {
          FullName: this.searchQuery,
          page: this.currentPage,
          perPage: this.pageSize
        }
      })
          .then(response => {
            this.workersList = response.data.data
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
      this.fetchWorkers()
    }
  },
  mounted() {
    this.fetchWorkers()
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
        v-for="worker in workersList"
        :key="worker.id"
    >
      <WorkerCard
          :role="worker.role"
          :email="worker.email"
          :full-name="worker.fullName"
          :worker-id="worker.id"
          @workerDeleted="fetchWorkers"
      />
    </v-col>
    <v-pagination
        :length="pageLength"
        v-model="currentPage"
        class="mt-6"
        @update:model-value="fetchWorkers"
    />

  </v-container>
  <router-link
    v-if="userRole === 'owner'"
    to="/workers/add"
  >
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
    "searchLabel": "Full name"
  },
  "uk": {
    "searchLabel": "Повне ім'я"
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