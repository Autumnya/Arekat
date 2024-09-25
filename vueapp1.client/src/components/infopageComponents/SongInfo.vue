//曲目的详细信息
<template>
    <div class="sub_container">
        <div id="song_info_container" class="main_container">
            <img class="song_cover" src="../../assets/default/test_song_cover.jpg" :alt="songInfoJsonObject.title">
            <div class="song_info">
                <h2>{{ songInfoJsonObject.title }}</h2>
                <div class="artist_area" v-for="(artistitem,index) in songInfoJsonObject.artist" :key="index">
                    <span @click="gotoArtistInfo(artistitem.artistId)">{{ artistitem.artistName }}</span>
                </div>
            </div>
        </div>
        <div id="song_chartlist_overcontainer" class="main_container">
            <h3>谱面总数:{{ songInfoJsonObject.chartAmount }}</h3>
            <div id="song_chartlist_container">
                <div class="chart_item chart_info_main_container">
                    <span>谱面id</span>
                    <span>难度</span>
                    <span>作者</span>
                    <span>最后更新时间</span>
                    <span>下载次数</span>
                </div>
                <div class="chart_item" v-for="(chartitem,index) in songChartsJsonObject.charts" :key="index" @click="gotoChartInfo(chartitem.id)">
                    <div class="chartlist_divide_line"></div>    
                    <div class="chart_info_main_container">
                        <span>{{ chartitem.id }}</span>
                        <span>{{ chartitem.difficulty }}</span>
                        <div>
                            <span v-for="(designerItem,index) in chartitem.designer" :key="index" @click.stop="gotoUserInfo(designerItem.id)">
                            {{ designerItem.name }}
                            </span>
                        </div>
                        <span>{{ chartitem.uploadDate }}</span>
                        <span>{{ chartitem.downloadCount }}</span>
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
    #song_chartlist_overcontainer{
        padding-left: 0px;
        padding-right: 0px;
        margin-top: 24px;
        >h3{
            text-align: left;
            color: rgb(240,240,240);
            margin-top: 0px;
            margin-bottom: 18px;
            margin-left: 20px;
        }
    }
    #song_chartlist_container{
        width: 100%;
        background-color: white;
        padding: 12px 0px;
    }
    .song_cover{
        width: 256px;
    }
    .song_info{
        padding: 0px 20px;
    }
    .artist_area{
        text-align: left;
        >span{
            cursor: pointer;
            display: block;
            margin-right: 10px;
        }
        >span:hover{
            text-decoration: underline;
        }
    }
    .chartlist_divide_line{
        width: 96%;
        height: 1px;
        background-color: gray;
        margin: 12px auto;
    }
    .chart_item{
        padding: 0px 16px;
        justify-content: space-between;
        color: rgb(50,50,50);
    }
    .chart_info_main_container{
        display: flex;
        justify-content: space-between;
        align-items: center;
        cursor: pointer;
        >span{
            display: block;
            width: 20%;
        }
        >div{
            border-radius: 4px;
            width: 20%;
            display: inline;
            >span{
                display: block;
                border-radius: 4px;
                padding: 4px;
                margin: auto;
                overflow: hidden;
                >span{
                    display: block;
                }
            }
            >span:hover{
                outline: 1px solid;
            }
        }
    }
    .chart_info_main_container:hover{
        text-decoration: underline;
    }
</style>

<script setup>
    import { ref,onMounted,onUnmounted } from 'vue';
    import router from '@/router';
    import { useRoute } from 'vue-router';
    import axios from 'axios';
    import { url } from '@/api';

    const route = useRoute();
    const chartPage = ref(0);
    const chartPageLength = ref(30);
    const atBottom = ref(false);
    const isLoadingNewCharts = ref(false);

    const songInfoJsonObject = ref({});
    const songChartsJsonObject = ref({charts:[]});

    axios.get(`${url.SONG}${route.params.songId}`)
    .then(response => {
        songInfoJsonObject.value = response.data;
    })
    .catch(error => {
        alert(error);
    })

    axios.get(`${url.CHARTLIST}`,{
        params:{
            "member":"song",
            "id":`${route.params.songId}`,
            "startIndex":chartPage.value * chartPageLength.value,
            "getAmount":chartPageLength.value
        }
    })
    .then(response=>{
        songChartsJsonObject.value.charts.push(...response.data.charts);
        console.log(response.data.charts);
    })
    .catch(error=>{
        alert(error);
    })

    const gotoChartInfo = (aimChartId) =>{
        router.push({name:"chart",params:{chartId:aimChartId}});
    }
    /*
        var songInfoJsonObject = {
            title:"testsong",
            artist:[
                {
                    "artistId":100001,
                    "artistName":"camellia",
                }
            ],
            songCoverSrc:"../../assets/default/test_song_cover.jpg",
            chartAmount:180
        }
    */
    const gotoArtistInfo = (aimArtistId) =>{
        router.push({name:"artist",params:{artistId:aimArtistId}});
    }
    const gotoUserInfo = (aimUserId) =>{
        router.push({name:"user",params:{userId:aimUserId}});
    }
    const getChartsNextPage = () =>{
        if(isLoadingNewCharts.value)
            return;
        isLoadingNewCharts.value = true;
        axios.get(url.CHARTLIST,{
            params:{
                "member":"song",
                "id":`${route.params.songId}`,
                "startIndex":songChartsJsonObject.value.charts.length,
                "getAmount":chartPageLength.value
            }
        })
        .then(response=>{
            songChartsJsonObject.value.charts.push(...response.data.charts);
            isLoadingNewCharts.value = false;
        })
        .catch(error=>{
            alert(error);
        })
    }
    // 检测是否滚动到页面底部的函数
    const checkScroll = () => {
        const scrollPosition = window.scrollY + window.innerHeight; // 当前的滚动位置
        const pageHeight = document.documentElement.scrollHeight; // 页面总高度
        // 判断是否滚动到页面底部
        if (scrollPosition >= pageHeight - 10) { // 给一点偏差
            atBottom.value = true;
            if(isLoadingNewCharts.value == false && songChartsJsonObject.value.charts.length < songInfoJsonObject.value.chartAmount){
                getChartsNextPage();
            }
        } else {
            atBottom.value = false;
        }
    };
    // 在组件挂载时，添加 scroll 事件监听
    onMounted(() => {
        window.addEventListener('scroll', checkScroll);
    });
    // 在组件卸载时，移除 scroll 事件监听
    onUnmounted(() => {
        window.removeEventListener('scroll', checkScroll);
    });
    /*
        //每次最多查30个
        const songChartsJsonObject = ref({
            charts:[
                {
                    chartID:100001,
                    game:"Arcaea",
                    difficulty:"10",
                    ratingClass:2,
                    designer:[
                        {
                            userId:10000001,
                            userName:"ekat001"
                        }
                    ],
                    downloadCount:80,
                    uploadDate:"2024-08-10"
                },
                {
                    chartID:100001,
                    game:"Arcaea",
                    difficulty:"10",
                    ratingClass:2,
                    designer:[
                        {
                            userId:10000001,
                            userName:"ekat001"
                        }
                    ],
                    downloadCount:80,
                    uploadDate:"2024-08-10"
                },
                {
                    chartID:100001,
                    game:"Arcaea",
                    difficulty:"10",
                    ratingClass:2,
                    designer:[
                        {
                            userId:10000001,
                            userName:"ekat001"
                        },
                        {
                            userId:10000002,
                            userName:"ekat002"
                        },
                        {
                            userId:10000003,
                            userName:"ekat003"
                        },
                    ],
                    downloadCount:80,
                    uploadDate:"2024-08-10"
                },
                {
                    chartID:100001,
                    game:"Arcaea",
                    difficulty:"10",
                    ratingClass:2,
                    designer:[
                        {
                            userId:10000001,
                            userName:"ekat001"
                        }
                    ],
                    downloadCount:80,
                    uploadDate:"2024-08-10"
                },
                {
                    chartID:100001,
                    game:"Arcaea",
                    difficulty:"10",
                    ratingClass:2,
                    designer:[
                        {
                            userId:10000001,
                            userName:"ekat001"
                        }
                    ],
                    downloadCount:80,
                    uploadDate:"2024-08-10"
                },
                {
                    chartID:100001,
                    game:"Arcaea",
                    difficulty:"10",
                    ratingClass:2,
                    designer:[
                        {
                            userId:10000001,
                            userName:"ekat001"
                        }
                    ],
                    downloadCount:80,
                    uploadDate:"2024-08-10"
                },
                {
                    chartID:100001,
                    game:"Arcaea",
                    difficulty:"10",
                    ratingClass:2,
                    designer:[
                        {
                            userId:10000001,
                            userName:"ekat001"
                        }
                    ],
                    downloadCount:80,
                    uploadDate:"2024-08-10"
                },
                {
                    chartID:100001,
                    game:"Arcaea",
                    difficulty:"10",
                    ratingClass:2,
                    designer:[
                        {
                            userId:10000001,
                            userName:"ekat001"
                        }
                    ],
                    downloadCount:80,
                    uploadDate:"2024-08-10"
                },
                {
                    chartID:100001,
                    game:"Arcaea",
                    difficulty:"10",
                    ratingClass:2,
                    designer:[
                        {
                            userId:10000001,
                            userName:"ekat001"
                        }
                    ],
                    downloadCount:80,
                    uploadDate:"2024-08-10"
                },
            ]
        })
    */
</script>