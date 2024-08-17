//首页公告栏
<template>
    <div class = "main_container">
        <div id = "notice_main" @click="gotoNoticeContent(currentNotice.noticeId)">
            <span class = "notice_text">
                公告 : {{ currentNotice.title }}
            </span>
            <span class = "notice_text">
                {{ currentNotice.publishDate }}
            </span>
        </div>
    </div>
</template>

<style scoped>
    #notice_main{
        display: flex;
        justify-content: space-between;
        cursor: pointer;
        color: white;
    }
    #notice_main:hover{
        text-decoration: underline;
    }
    .notice_text{
        font-family: PingFang SC,HarmonyOS_Regular,Helvetica Neue,Microsoft YaHei,sans-serif!important;
        font-size: 16px;
        text-align: left;
    }
</style>

<script setup>
    import router from '@/router';

    const currentNotice = {
        noticeId:2024080101,
        title:"Arekat是什么?这是哪?",
        publishDate:"2024-8-1"
    };

    const gotoNoticeContent = (noticeId) =>{
        router.push({name:"notice",params:{noticeId}});
    }

    console.log(currentNotice);
</script>