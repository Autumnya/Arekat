//谱面的详细信息
<template>
    <div class="sub_container">
        <div id="song_info_container" class="main_container">
            <img class="song_cover" src="../../assets/default/test_song_cover.jpg" :alt="chartInfoJsonObject.title">
            <div class="song_info">
            <h2 @click="gotoSonginfo(chartInfoJsonObject.songId)">{{ chartInfoJsonObject.title }}</h2>
                <div class="info_container">Artist: 
                    <span v-for="(artistItem,index) in chartInfoJsonObject.artist" :key="index"
                    @click="gotoArtistInfo(artistItem.id)"> {{ artistItem.name }}</span>
                </div>
                <div class="info_container">Designer: 
                    <span v-for="(designerItem,index) in chartInfoJsonObject.designer" :key="index"
                    @click="gotoUserInfo(designerItem.id)"> {{ designerItem.name }}</span>
                </div>
                <span class="info_container">ChartID: {{ chartInfoJsonObject.id }}</span>
            </div>
        </div>

        <div id="chart_info_container" class="main_container">
            <h2>下载</h2>
            <div class="download_button" :style="downloadButtonStyle" @mouseenter="mouseEnterDownloadButton" @mouseleave="mouseLeaveDownloadButton" @click="downloadChart">
                <span>下载 Chart{{ chartInfoJsonObject.id }}.zip</span>
            </div>
            <h2>更新历史</h2>
            <div class="update_history_form">
                <div class="item_line">
                    <span class="item1">阶段</span>
                    <span class="item2">描述</span>
                    <span class="item3">更新日期</span>
                </div>
                <div class="form_item" v-for="(historyItem,index) in chartInfoJsonObject.updateHistory" :key="index">
                    <div class="form_divide_line"></div>
                    <div class="item_line">
                        <span class="item1 stage_icon" :style="historyItem.stage == 'stable' ? 'color : green' : 'color : red'">{{ historyItem.stage }}</span>
                        <span class="item2">{{ historyItem.description }}</span>
                        <span class="item3">{{ historyItem.time }}</span>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
    #song_info_container{
        background-color: rgb(240,240,240);
        display: flex;
    }
    #song_chartlist_container{
        width: 100%;
        background-color: white;
        padding: 12px 0px;
    }
    #chart_info_container{
        text-align: left;
        margin-top: 24px;
        background-color: white;
        >h2{
            padding-left: 24px;
        }
    }
    .song_cover{
        width: 128px;
    }
    .song_info{
        padding: 0px 20px;
        >h2{
            margin: 8px 0px;
            cursor: pointer;
            text-align: left;
        }
        >h2:hover{
            text-decoration: underline;
        }
    }
    .info_container{
        display: flex;
        margin-bottom: 4px;
        color: rgb(50,50,50);
        >span{
            display: block;
            cursor: pointer;
            margin-left: 8px;
        }
        >span:hover{
            text-decoration: underline;
        }
    }
    .update_history_form{
        width: 80%;
        margin: auto;
    }
    .form_divide_line{
        height: 1px;
        width: 100%;
        background-color: gray;
        margin: 8px auto;
    }
    .item_line{
        display: flex;
        text-align: center;
        >.stage_icon{
            font-family: 'Arial';
            font-weight: bold;
        }
        >.item1{
            width: 10%;
        }
        >.item2{
            width: 70%;
        }
        >.item3{
            width: 20%;
        }
    }
    .download_button{
        border-radius: 10px;
        width: 300px;
        height: 40px;
        position: relative;
        left: 20px;
        cursor: pointer;
        display: flex;
        >span{
            text-align: center;
            margin: auto;
        };
    }
</style>

<script setup>
    import { useStore } from 'vuex';
    import { useRoute,useRouter } from 'vue-router';
    import { computed, ref } from 'vue';
    import axios from 'axios';
    import { url } from '@/api';

    var token = localStorage.getItem("arekat_v1_token");
    const store = useStore();
    const route = useRoute();
    const router = useRouter();
    const themeColor = computed(() => store.state.themeColor);  
    const chartInfoJsonObject = ref({});

    axios.get(`${url.CHART}${route.params.chartId}`)
    .then(response=>{
        chartInfoJsonObject.value = response.data;
    })
    .catch(error=>{
        alert(error);
    })
    
    const downloadButtonStyleDefault = {
        border : "2px,solid," + themeColor.value,
        color : themeColor.value,
        backgroundColor : "white"
    }
    const downloadButtonStyleActive = {
        boxShadow : "0px 0px 3px black",
        border : "2px solid" + themeColor.value,
        color : "RGB(240,240,240)",
        backgroundColor : themeColor.value
    }
    const downloadButtonStyle = ref(downloadButtonStyleDefault);
    
    const gotoArtistInfo = (aimArtistId) =>{
        router.push({name:"artist",params:{artistId:aimArtistId}});
    }
    const gotoSonginfo = (aimId) =>{
        router.push({name:"song",params:{songId:aimId}});
    }
    const gotoUserInfo = (aimId) =>{
        router.push({name:"user",params:{userId:aimId}});
    }
    const downloadChart = () =>{
        if(token == "" || token == null)
        {
            alert("请登录后再进行操作");
            return;
        }
        axios.get(url.CHART_DOWNLOAD,{
            responseType:'blob',
            headers:{
                Authorization:`Bearer ${token}`,
            },
            params:{
                id:route.params.chartId
            }
        })
        .then(response=>{
            //创建一个下载链接
            const url = window.URL.createObjectURL(new Blob([response.data]));
            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', `Chart${route.params.chartId}.zip`); // 文件名
            document.body.appendChild(link);
            link.click();
        })
        .catch(error=>{
            alert(error);
        })
    }

    const mouseEnterDownloadButton = () =>{
        downloadButtonStyle.value = downloadButtonStyleActive
    }
    const mouseLeaveDownloadButton = () =>{
        downloadButtonStyle.value = downloadButtonStyleDefault
    }
    /*
        var chartInfoJsonObject = {
            id:1000001,
            songId:1000001,
            title:"testsong",
            artist:[
                {
                    "id":100001,
                    "name":"camellia",
                },
                {
                    "id":100001,
                    "name":"BlackY",
                }
            ],
            designer:[
                {
                    "id":100001,
                    "name":"ekat001"
                },
                {
                    "id":100002,
                    "name":"ekat002"
                },
            ],
            songCoverSrc:"../../assets/default/test_song_cover.jpg",

            updateHistory:[
                {
                    stage:"stable",
                    description:"没什么变化",
                    time:"2024-08-21 13:15",
                },
                {
                    stage:"test",
                    description:"没什么变化",
                    time:"2024-08-21 13:11",
                },
                {
                    stage:"test",
                    description:"没什么变化",
                    time:"2024-08-21 13:11",
                },
                {
                    stage:"test",
                    description:"没什么变化",
                    time:"2024-08-21 13:11",
                },
                {
                    stage:"test",
                    description:"没什么变化",
                    time:"2024-08-21 13:11",
                },
            ]
        }
    */
</script>