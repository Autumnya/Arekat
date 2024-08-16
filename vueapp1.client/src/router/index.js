import { createRouter, createWebHistory } from "vue-router";

import HomeIndex from "@/components/HomeIndex.vue";
import ChartForm from "@/components/ChartForm.vue";
import NoticeList from "@/components/NoticeList.vue";
import UserProfile from "@/components/UserProfile.vue";

import NotFound from "@/components/NotFound.vue";

//定义路由对象（规则）
const routes = [
    {
        path:"/index",
        name:"Index",
        component:HomeIndex
    },
    {
        path:"/",
        redirect:"/index"
    },
    {
        path:"/charts",
        name:"Charts",
        component:ChartForm
    },
    {
        path:"/notice",
        name:"Notice",
        component:NoticeList
    },
    {
        path:"/userpage",
        name:"Userpage",
        component:UserProfile
    },
    {
        path:"/:pathMatch(.*)*",
        name:"NotFound",
        component:NotFound
    },
]

const router = createRouter({
    history:createWebHistory(),routes
})

export default router