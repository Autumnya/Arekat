import { createRouter, createWebHistory } from "vue-router";

import HomeIndex from "@/components/HomeIndex.vue";
import ChartForm from "@/components/ChartForm.vue";
import NoticeList from "@/components/NoticeList.vue";
import UserInfo from "@/components/infopageComponents/UserInfo.vue";
import SongInfo from "@/components/infopageComponents/SongInfo.vue";
import NoticeContent from "@/components/NoticeContent.vue";
import ArtistInfo from "@/components/infopageComponents/ArtistInfo.vue";
import NotFound from "@/components/NotFound.vue";

//定义路由对象（规则）
const routes = [
    {
        path:"/index",
        name:"index",
        component:HomeIndex
    },
    {
        path:"/",
        redirect:"/index"
    },
    {
        path:"/charts",
        name:"charts",
        component:ChartForm
    },
    {
        path:"/chart/:chartId",
        name:"chart",
        component:ChartForm
    },
    {
        path:"/notices",
        name:"notices",
        component:NoticeList
    },
    {
        path:"/notice/:noticeId",
        name:"notice",
        component:NoticeContent
    },
    {
        path:"/user/:userId",
        name:"user",
        component:UserInfo
    },
    {
        path:"/song/:songId",
        name:"song",
        component:SongInfo
    },
    {
        path:"/artist/:artistId",
        name:"artist",
        component:ArtistInfo
    },
    {
        path:"/:pathMatch(.*)*",
        name:"notFound",
        component:NotFound
    },
]

const router = createRouter({
    history:createWebHistory(),routes
})

export default router