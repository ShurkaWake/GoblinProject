<script>
import {defineComponent} from "vue";
import {useI18n} from "vue-i18n";
import VueDatePicker from '@vuepic/vue-datepicker';
import '@vuepic/vue-datepicker/dist/main.css'
import {mapState} from "vuex";
import {format} from "date-fns";
import axios from "axios";
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend,
  BarElement
} from 'chart.js'
import { Line } from 'vue-chartjs'
import { Bar } from "vue-chartjs";
import store from "@/store";

ChartJS.register(
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    Title,
    Tooltip,
    Legend,
    BarElement
)

export default defineComponent({
  components: {VueDatePicker, Line, Bar},

  setup() {
    const {t} = useI18n({
      locale: "en",
      inheritLocale: true,
      useScope: 'local'
    })

    return {t}
  },

  data: () => ({
    dateFrom: new Date(),
    dateTo: new Date(),
    loading: false,

    chartLabels: [],
    profitDots: [],
    incomeDots: [],
    costsDots: []
  }),

  computed: {
    ...mapState({
      storeDateFormat: state => state.dateFormat
    }),

    formatDateStart(date){
      return format(this.dateFrom, this.storeDateFormat)
    },

    formatDateEnd(date){
      return format(this.dateTo, this.storeDateFormat)
    },

    profitData() {
      return {
        labels: this.chartLabels,
        datasets: [
          {
            label: this.t('profit'),
            backgroundColor: "#5d3fd3",
            data: this.profitDots
          }
        ]
      }
    },

    incomeData() {
      return {
        labels: this.chartLabels,
        datasets: [
          {
            label: this.t('income'),
            backgroundColor: "#228B22",
            data: this.incomeDots
          }
        ]
      }
    },

    costsData() {
      return {
        labels: this.chartLabels,
        datasets: [
          {
            label: this.t('costs'),
            backgroundColor: "#EAE46A",
            data: this.costsDots
          }
        ]
      }
    }
  },

  methods: {
    async fetchStatistics(){
      this.loading = true
      await axios.get("statistic", {
        params: {
          from: this.dateFrom,
          to: this.dateTo
        }
      })
        .then(response => {
          this.chartLabels = []
          this.profitDots = []
          this.incomeDots = []
          this.costsDots = []

          const stat = response.data.data
          let profitCumulative = 0
          for (let i = 0; i < stat.length; i++){
            profitCumulative += stat[i].profit
            this.profitDots.push(profitCumulative)
            this.incomeDots.push(stat[i].income)
            this.costsDots.push(stat[i].costs)

            let date = new Date(stat[i].date)
            this.chartLabels.push(format(date, this.storeDateFormat))
          }
        })
        .catch(response => {
          store.commit('setErrorMessage', response.response.data.errors[0])
          store.commit('setRedirectPath', "/")
          store.commit('switchDialog')
        })
      this.loading = false
    }
  },

  beforeMount() {
    this.dateFrom.setDate(this.dateFrom.getDate() - 30)
  },

  mounted() {
    this.fetchStatistics()
  },

  beforeRouteLeave(){
    this.$emit('closeDrawer')
  }
})

</script>

<template>
<v-container class="d-flex flex-row justify-center">
  <v-col id="col">
    <v-label
      :text="t('dateFrom')"
      class="mt-4 ml-4 text-h6"
    />
    <VueDatePicker
      class="ma-4"
      v-model="dateFrom"
      :format="formatDateStart"
    />
    <v-label
      :text="t('dateTo')"
      class="mt-4 ml-4 text-h6"
    />
    <VueDatePicker
      class="ma-4"
      v-model="dateTo"
      :format="formatDateEnd"
    />
    <v-btn
      prepend-icon="mdi-magnify"
      @click="fetchStatistics"
      class="ml-6"
      color="deep-purple-darken-1"
      :loading="loading"
      :text="t('buttonText')"
    />
    <Line
      class="ma-4"
      :data="profitData"
    />
    <Bar
      class="ma-4"
      :data="incomeData"
    />
    <Bar
      class="ma-4"
      :data="costsData"
    />
  </v-col>
</v-container>
</template>

<i18n>
{
  "en": {
    "dateFrom": "From date ",
    "dateTo": "To date ",
    "profit": "Profit",
    "income": "Income",
    "costs": "Costs",
    "buttonText": "Get statistics"
  },
  "uk": {
    "dateFrom": "Починаючи з ",
    "dateTo": "Закінчуючи ",
    "profit": "Прибуток",
    "income": "Доходи",
    "costs": "Витрати",
    "buttonText": "Отримати статистику"
  }
}
</i18n>

<style scoped>
#col{
  max-width: 600px;
}

</style>