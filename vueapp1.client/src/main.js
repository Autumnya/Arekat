import StaticHeader from './components/globalComponents/StaticHeader.vue'
import StaticAside from './components/globalComponents/StaticAside.vue'
import StaticFooter from './components/globalComponents/StaticFooter.vue'
import './components/globalComponents/globalHeaderStyle.css'
import './components/globalComponents/globalMainpageStyle.css'

import { createApp } from 'vue'
import { useStore } from 'vuex'
import App from './App.vue'
import router from './router'
import axios from 'axios'
import storeIndex from './store/storeIndex'

const app = createApp(App);

app.use(useStore);
app.use(router);
app.use(storeIndex);

app.provide('$axios',axios);

app.component('StaticHeader',StaticHeader)
app.component('StaticAside ',StaticAside)
app.component('StaticFooter',StaticFooter)

app.mount('#app')