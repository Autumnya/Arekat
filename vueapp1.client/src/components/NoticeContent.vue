// 通知的详细内容
<template>
    <div class="sub_container">
        <div class="main_container notice_container">
            <h3>{{ noticeJsonObject.title }}</h3>
            <span>{{ noticeJsonObject.arthor }} 发布于 {{ noticeJsonObject.publishDate }}</span>
            <div class="text_divide_line"></div>
            <div v-html="noticeJsonObject.text" class="text_container"></div>
        </div>
    </div>
</template>

<style scoped>
    .notice_container{
        text-align: left;
        color: rgb(240,240,240);
        >h3{
            margin: 8px 8px;
        }
        >span{
            margin: 8px 8px;
        }
    }
    .text_divide_line{
        width: 96%;
        height: 2px;
        background-color: rgb(240,240,240);
        margin: 20px;
    }
    .text_container{
        border-radius: 10px;
        padding: 12px;
    }
</style>

<script setup>


    var noticeJsonObject = {
        "title":"Arekat是什么?这是哪?",
        "arthorId":100001,
        "arthor":"ekat",
        "text":"Arekat是一个谱面交流社区",
        "publishDate":"2024-8-1",
        "viewTime":10
    }
</script>