<template>
    <div class = 'sub_container'>
        <NoticeBar />
        <div v-for="(reclist,index) in reclistArray" :key="index" class="rcm_div">
            <h2>
                {{ reclist.mainTitle }}
            </h2>
            <h3>
                {{ reclist.subTitle }}
            </h3>
            <div class = "main_container chart_info_container">
                <div v-for="(chartInfo,index) in reclist.chartlist.charts" :key="index" class="chart_info">
                    <img :src="chartInfo.coverSrc" :alt="chartInfo.title" class="song_cover_img">
                    <div class="song_info">
                        <div class="song_title">
                            <span @click="gotoSongInfo(chartInfo.songId)">{{ chartInfo.title }}</span>
                        </div>
                        <div class="artist">
                            <span @click="gotoArtistInfo(chartInfo.artistId)">{{ chartInfo.artist }}</span>
                        </div>
                    </div>
                </div>
                <div class="look_more" @click="lookMoreCharts(reclist.chartlist.chartFilter)">
                    <span>查看更多>></span>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
    .rcm_div{
        text-align: left;
        position: relative;
        >h2{
            position: relative;
            left: 16px;
            color: black;
            font-size: 40px;
            margin-bottom: 12px;
            letter-spacing: 4px;
            cursor: pointer;
        }
        >h3{
            position: relative;

            left: 40px;
            color: rgb(40, 40, 40);
            font-size: 24px;
            margin-top: 12px;
            cursor: pointer;
        }
        >h2:hover{
            text-decoration: underline;
        }
        >h3:hover{
            text-decoration: underline;
        }
        >h3::before{
            content: '——  ';
        }
    }
    .chart_info_container{
        display: flex;
        overflow: auto;
    }
    .chart_info{
        width: 210px;
        height: 280px;
        background-color: white;
        border-radius: 4px;
        padding: 4px;
        margin-right: 8px;
        >.song_cover_img{
            height: 75%;
            cursor: pointer;
        }
        >.song_info{
            height: 25%;
            display: inline-block7;
            >.song_title{
                width: 95%;
                height: 50%;
                font-size: 26px;
                cursor: pointer;
                display: flex;
                align-items: center;
                padding: 0px 4px;
                overflow: hidden;
            }
            >.artist{
                width: 95%;
                height: 40%;
                cursor: pointer;
                display: flex;
                align-items: center;
                padding: 0px 4px;
                overflow: hidden;
            }
        }
    }
    .chartInfo:hover{
        box-shadow: 0px 0px 6px black;
    }
    .song_title:hover{
        overflow: visible;
        outline: auto;
        text-decoration: underline;
    }
    .artist:hover{
        overflow: visible;
        outline: auto;
        text-decoration: underline;
    }
    .look_more{
        width: 120px;
        height: 280px;
        background-color: white;
        border-radius: 4px;
        padding: 4px;
        cursor: pointer;
        text-align: center;
        display: flex;
        font-size: 16px;
        >span{
            margin: auto auto;
        }
    }
    .look_more:hover{
        background-color: gray;
        color: white;
        text-decoration: underline;
    }
</style>

<script setup>
    import NoticeBar from "./homeComponents/NoticeBar.vue";

    import { computed, ref } from "vue";
    import axios from "axios";
    import { useStore } from "vuex";
    import { useRouter } from 'vue-router';

    const store = useStore();
    const router = useRouter();

    const lookMoreCharts = (newChartFilter) =>{
        store.commit('setChartFilter',{chartFilter:newChartFilter});
        router.push('/charts');
    }
    
    const gotoSongInfo = (songId) =>{
        router.push({name:"song",params:{songId}});
    }
    const gotoArtistInfo = (artistId) =>{
        router.push({name:"artist",params:{artistId}});
    }

    /*
    //推荐列表生成器
    //params:(主标题，副标题，谱面筛选条件(JSON资源路径))
    //return:生成的推荐列表对象
    function reclistCreater(mainTitle,subTitle,recInfoSrc)
    {
        var reclist = new Object();
        reclist.mainTitle = mainTitle;
        reclist.subTitle = subTitle;
        reclist.recInfoSrc = recInfoSrc;
        reclistArray.value.push(reclist);
    }
    */
   //首页推荐列表（测试）
    var reclistArray = [
        {
            mainTitle:"最新最热！",
            subTitle:"浏览一周内下载量最高的谱面",
            chartlist:{
                chartFilter:{
                    games : ["Arcaea"],
                    examineStage : ["test","stable"],
                    minDiff : 1,
                    maxDiff : 12,
                    minPassDate : ["","",""],
                    maxPassDate : ["","",""],
                    keyWords : [],
    
                    sortOrder : {
                        sortBy : "download",
                        orderBy : "desc"
                    }
                },
                charts:[
                    {
                        chartId:"100001",
                        songId:"testsongggggggggggggggggg",
                        title:"testsongggggggggggggggggg",
                        coverSrc:"../../src/assets/default/test_song_cover.jpg",
                        artistId: "1000",
                        artist: "Camellia",
                        chartDesignerId:"8000",
                        chartDesigner:"",
                        bpm:"200",
                        passDate:"2024-7-29",
                        ratingClass:2,
                        rating:10
                    },
                    {
                        chartId:"100001",
                        songId:"testsongggggggggggggggggg",
                        title:"testsongggggggggggggggggg",
                        coverSrc:"../../src/assets/default/test_song_cover.jpg",
                        artistId: "1000",
                        artist: "Camellia",
                        chartDesignerId:"8000",
                        chartDesigner:"",
                        bpm:"200",
                        passDate:"2024-7-29",
                        ratingClass:2,
                        rating:10
                    },
                    {
                        chartId:"100001",
                        songId:"testsongggggggggggggggggg",
                        title:"testsongggggggggggggggggg",
                        coverSrc:"../../src/assets/default/test_song_cover.jpg",
                        artistId: "1000",
                        artist: "Camellia",
                        chartDesignerId:"8000",
                        chartDesigner:"",
                        bpm:"200",
                        passDate:"2024-7-29",
                        ratingClass:2,
                        rating:10
                    },
                    {
                        chartId:"100001",
                        songId:"testsongggggggggggggggggg",
                        title:"testsongggggggggggggggggg",
                        coverSrc:"../../src/assets/default/test_song_cover.jpg",
                        artistId: "1000",
                        artist: "Camellia",
                        chartDesignerId:"8000",
                        chartDesigner:"",
                        bpm:"200",
                        passDate:"2024-7-29",
                        ratingClass:2,
                        rating:10
                    },
                    {
                        chartId:"100001",
                        songId:"testsongggggggggggggggggg",
                        title:"testsongggggggggggggggggg",
                        coverSrc:"../../src/assets/default/test_song_cover.jpg",
                        artistId: "1000",
                        artist: "Camellia",
                        chartDesignerId:"8000",
                        chartDesigner:"",
                        bpm:"200",
                        passDate:"2024-7-29",
                        ratingClass:2,
                        rating:10
                    },
                    {
                        chartId:"100001",
                        songId:"testsongggggggggggggggggg",
                        title:"testsongggggggggggggggggg",
                        coverSrc:"../../src/assets/default/test_song_cover.jpg",
                        artistId: "1000",
                        artist: "Camellia",
                        chartDesignerId:"8000",
                        chartDesigner:"",
                        bpm:"200",
                        passDate:"2024-7-29",
                        ratingClass:2,
                        rating:10
                    },
                ]
            }
        },
        {
            mainTitle:"最受好评！",
            subTitle:"浏览一个月内综合评价最高的谱面",
            chartlist:{
                chartFilter:{
                    games : ["Arcaea"],
                    examineStage : ["test","stable"],
                    minDiff : 1,
                    maxDiff : 12,
                    minPassDate : ["","",""],
                    maxPassDate : ["","",""],
                    keyWords : [],
    
                    sortOrder : {
                        sortBy : "download",
                        orderBy : "desc"
                    }
                },
                charts:[
                    {
                        chartId:"100001",
                        songId:"testsongggggggggggggggggg",
                        title:"testsongggggggggggggggggg",
                        coverSrc:"../../src/assets/default/test_song_cover.jpg",
                        artistId: "1000",
                        artist: "Camellia",
                        chartDesignerId:"8000",
                        chartDesigner:"",
                        bpm:"200",
                        passDate:"2024-7-29",
                        ratingClass:2,
                        rating:10
                    },
                ]
            }
        },
        {
            mainTitle:"不知道写什么！",
            subTitle:"浏览一周内下载量最高的谱面",
            chartlist:{
                chartFilter:{
                    games : ["Arcaea"],
                    examineStage : ["test","stable"],
                    minDiff : 1,
                    maxDiff : 12,
                    minPassDate : ["","",""],
                    maxPassDate : ["","",""],
                    keyWords : [],
    
                    sortOrder : {
                        sortBy : "download",
                        orderBy : "desc"
                    }
                },
                charts:[
                    {
                        chartId:"100001",
                        songId:"testsongggggggggggggggggg",
                        title:"testsongggggggggggggggggg",
                        coverSrc:"../../src/assets/default/test_song_cover.jpg",
                        artistId: "1000",
                        artist: "Camellia",
                        chartDesignerId:"8000",
                        chartDesigner:"",
                        bpm:"200",
                        passDate:"2024-7-29",
                        ratingClass:2,
                        rating:10
                    },
                ]
            }
        }]
</script>