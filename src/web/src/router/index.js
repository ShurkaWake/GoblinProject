import { createRouter, createWebHistory } from 'vue-router'
import CardPage from "@/views/CardPage.vue";
import UnauthorizedCard from "@/components/Unauthorized/UnauthorizedCard.vue";
import LoginCard from "@/components/Unauthorized/LoginCard.vue";

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
      }
    ]
  },
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
