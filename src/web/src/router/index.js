import { createRouter, createWebHistory } from 'vue-router'
import CardPage from "@/views/CardPage.vue";
import UnauthorizedCard from "@/components/Unauthorized/UnauthorizedCard.vue";
import LoginCard from "@/components/Unauthorized/LoginCard.vue";
import UserPage from "@/views/UserPage.vue";
import ScalesPage from "@/views/ScalesPage.vue";
import ScalesAddCard from "@/components/Scales/ScalesAddCard.vue";
import WorkersPage from "@/views/WorkersPage.vue";
import WorkerCreateCard from "@/components/Worker/WorkerCreateCard.vue";
import StatisticsPage from "@/views/StatisticsPage.vue";
import BusinessCreateCard from "@/components/Business/BusinessCreateCard.vue";

const routes = [
  {
    path: '/card-page',
    name: 'home',
    component: CardPage,
    children: [
      {
        path: "/unauthorized",
        component: UnauthorizedCard
      },
      {
        path: "/login",
        component: LoginCard
      },
      {
        path: "/scales/add",
        component: ScalesAddCard
      },
      {
        path: "/workers/add",
        component: WorkerCreateCard
      },
      {
        path: "/business/add",
        component: BusinessCreateCard
      }
    ]
  },
  {
    path: '/',
    name: 'user',
    component: UserPage
  },
  {
    path: '/scales',
    name: 'scales',
    component: ScalesPage
  },
  {
    path: '/statistics',
    component: StatisticsPage
  },
  {
    path: '/workers',
    component: WorkersPage
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
