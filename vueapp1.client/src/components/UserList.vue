//显示搜索用户的结果
<template>
    <div class="sub_container">
        <div class="main_container">
            <h3>搜索结果: {{ searchResultJsonObject.amount }} 位用户</h3>
            <div id="search_result_container">
                <div class="user_item">
                    <span>用户</span>
                    <span>谱面数量</span>
                </div>
                <div class="list_item" v-for="(userItem,index) in searchResultJsonObject.users" :key="index">
                    <div class="list_divide_line"></div>
                    <div class="user_item" @click="gotoUserInfo(userItem.id)">
                        <div class="user_avator_and_name">
                            <img :src="userItem.avatorSrc" :alt="userItem.name">
                            <span>{{ userItem.name }}</span>
                        </div>
                        <span>{{ userItem.chartAmount }}</span>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
    .main_container{
        padding-left: 0px;
        padding-right: 0px;
        padding-top: 12px;
        >h3{
            color: rgb(240,240,240);
            font-size: 20px;
            text-align: left;
            margin-left: 12px;
            margin-bottom: 12px;
            margin-top: 0px;
        }
    }
    #search_result_container{
        width: auto;
        background-color: white;
        border-radius: 10px;
        padding: 12px;
        >.list_item{
            >.list_divide_line{
                height: 1px;
                background-color: gray;
                width: 96%;
                margin: 0px auto;
            }
        }
    }
    .user_item{
        display: flex;
        justify-content: space-between;
        text-align: center;
        font-size: 18px;
        align-items: center;
        margin: 8px;
        cursor: pointer;
        color: rgb(50,50,50);
        >span{
            width: 20%;
            display: block;
        }
        >.user_avator_and_name{
            display: flex;
            >img{
                width: 64px;
            }
            >span{
                margin: auto;
            }
        }
    }
    .user_item:hover{
        text-decoration: underline;
    }
</style>

<script setup>
    import { ref } from 'vue';
    import { useRouter } from 'vue-router';

    const router = useRouter();

    const gotoUserInfo = (aimId) =>{
        router.push({name:"user",params:{userId:aimId}});
    }

    const searchResultJsonObject = ref({
        amount:108,
        users:[
            {
                id:1000001,
                name:"ekat001",
                avatorSrc:"src/assets/default/teruteru.png",
                chartAmount:10
            },
            {
                id:1000001,
                name:"ekat001",
                avatorSrc:"src/assets/default/teruteru.png",
                chartAmount:10
            },
            {
                id:1000001,
                name:"ekat001",
                avatorSrc:"src/assets/default/teruteru.png",
                chartAmount:10
            },
            {
                id:1000001,
                name:"ekat001",
                avatorSrc:"src/assets/default/teruteru.png",
                chartAmount:10
            },
            {
                id:1000001,
                name:"ekat001",
                avatorSrc:"src/assets/default/teruteru.png",
                chartAmount:10
            },
            {
                id:1000001,
                name:"ekat001",
                avatorSrc:"src/assets/default/teruteru.png",
                chartAmount:10
            },
            {
                id:1000001,
                name:"ekat001",
                avatorSrc:"src/assets/default/teruteru.png",
                chartAmount:10
            },
            {
                id:1000001,
                name:"ekat001",
                avatorSrc:"src/assets/default/teruteru.png",
                chartAmount:10
            },
            {
                id:1000001,
                name:"ekat001",
                avatorSrc:"src/assets/default/teruteru.png",
                chartAmount:10
            },
            {
                id:1000001,
                name:"ekat001",
                avatorSrc:"src/assets/default/teruteru.png",
                chartAmount:10
            },
        ]
    })
</script>