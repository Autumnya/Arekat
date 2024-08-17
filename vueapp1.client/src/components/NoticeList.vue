//通知页主组件
<template>
    <div class="sub_container">
        <div class="main_container">
            <div v-for="(notice_item,index) in noticeListJsonObject.notices" :key="index" class="notice_item">
                <div class="notice_info_line" @click="gotoNoticeInfo(notice_item.noticeId)">
                    <span>{{ notice_item.title }}</span>
                    <span>{{ notice_item.publishDate }}</span>
                </div>
                <div v-if="index != noticeListJsonObject.notices.length - 1" class="notice_item_divide_line"></div>
            </div>
        </div>
    </div>
</template>

<style scoped>
    .notice_info_line{
        display: flex;
        justify-content: space-between;
        cursor: pointer;
        padding: 0px 20px;
        color: rgb(240,240,240);
    }
    .notice_info_line:hover{
        text-decoration: underline;
    }
    .notice_item_divide_line{
        width: 96%;
        height: 1px;
        background-color: rgb(240,240,240);
        margin: 12px auto;
    }
</style>

<script setup>
    import { useRouter } from 'vue-router';

    const router = useRouter();

    const gotoNoticeInfo = (aimNoticeId) =>{
        router.push({name:"notice",params:{noticeId:aimNoticeId}});
    }

    var noticeListJsonObject = {
        notices:[
            {
                "noticeId":2024080101,
                "title":"Arekat是什么?这是哪?",
                "publishDate":"2024-8-1",
            },
            {
                "noticeId":2024080101,
                "title":"Arekat是什么?这是哪?",
                "publishDate":"2024-8-1",
            },
            {
                "noticeId":2024080101,
                "title":"Arekat是什么?这是哪?",
                "publishDate":"2024-8-1",
            },
            {
                "noticeId":2024080101,
                "title":"Arekat是什么?这是哪?",
                "publishDate":"2024-8-1",
            },
            {
                "noticeId":2024080101,
                "title":"Arekat是什么?这是哪?",
                "publishDate":"2024-8-1",
            },
        ]
    }
</script>