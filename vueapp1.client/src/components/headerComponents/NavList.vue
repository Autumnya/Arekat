//header中的导航按钮
<template>
    <div id = "nav_div" class="header_component">
        <ul class="nav_list">
            <!-- 使用鼠标事件
            <li v-for = "(navItem,index) in navList" :key="index" :class="{nav_border:true,nav_border_underline:index==currentPageIndex||selectedIndex==index}" @mouseenter="setUnderLine(index)" @mouseleave="unsetUnderLine(index)">
            -->
            <li v-for = "(navItem,index) in navList" :key="index" class="nav_border" :class="{nav_border_underline : router.currentRoute.value.path == navRoute[index]}" @click="pageRoute(index)">
                <span class="header_clickable_text">{{ navItem }}</span>
            </li>
        </ul>
    </div>
</template>

<style scoped>
    #nav_div{
        width: 20%;
    }
    .nav_list{
        height: 40px;
        list-style: none;
        display: flex;
        text-align: center;
        align-items: center;
        width: 100%;
        padding: 0px;
        margin: 0px;
        > li{
            list-style-type: none;
            width: 34%;
            height: 54px;
            text-align: center;
            align-items: center;
            margin: 0px auto;
            display: flex;
            cursor: pointer;
        }
    }
    .nav_border{
        border-top: 6px solid rgba(0,0,0,0);
        border-bottom: 6px solid rgba(0,0,0,0);
    }
    .nav_border_underline{
        border-top: 6px solid rgba(0,0,0,0);
        border-bottom: 6px solid white;
    }
    .nav_border:hover{
        border-bottom: 6px solid white;
    }
</style>

<script setup>
    import { ref } from 'vue';
    import { useRouter } from 'vue-router';

    const navList = ["首页","谱面","公告"];
    const navRoute = ["/index","/charts","/notices"]

    /*鼠标事件
    const selectedIndex = ref(-1);
    //设置选中项的下方白线
    const setUnderLine = (index) => {
        if(index == currentPageIndex.value)
            return;
        selectedIndex.value = index;
    }
    const unsetUnderLine = (index) =>
    {
        if(index == currentPageIndex.value)
            return;
        if(selectedIndex.value == index)
            selectedIndex.value = -1;
    }
    */

    const router = useRouter()

    const pageRoute = (index) => {
        router.push(navRoute[index]);
    }

</script>