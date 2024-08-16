//首页公告栏
<template>
    <div class = "main_container">
        <div id = "notice_main">
            <span class = "notice_text">
                公告 : {{ currentNotice }}
            </span>
        </div>
    </div>
</template>

<style scoped>
    #notice_main{
        text-align: left;
    }
    .notice_text{
        text-decoration: underline;
        font-family: PingFang SC,HarmonyOS_Regular,Helvetica Neue,Microsoft YaHei,sans-serif!important;
        cursor: pointer;
        color: white;
        font-size: 16px;
        text-align: left;
    }
</style>

<script setup>
    import { ref } from 'vue';

    const currentNotice = ref("Arekat是什么?这是哪?");
</script>