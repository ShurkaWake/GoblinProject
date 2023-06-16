import { createApp } from 'vue'
import axios from "axios";
import App from './App.vue'
import router from './router'
import store from './store'
import vuetify from './plugins/vuetify'
import { loadFonts } from './plugins/webfontloader'
import i18n from "@/i18n";

loadFonts()
axios.defaults.baseURL = "https://localhost:7291/api"
axios.defaults.headers.post['Content-Type'] = "application/json"

createApp(App)
    .use(router)
    .use(store)
    .use(i18n)
    .use(vuetify)
    .mount('#app')
