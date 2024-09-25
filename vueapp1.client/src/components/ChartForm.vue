<template>
    <div id="chartpage_container" class = "sub_container">
        <div class="chart_form">
            <div v-for="(row,index) in chartPageInfoArray" :key="index" class = "list_row">
                <div class="row_content">
                    <div class = "list_row_title">
                        <span class = "item_text">{{ row.name }} :</span>
                    </div>
                    <div v-for="(row_item,sub_index) in row.item" :key="sub_index" class="list_row_item" :style="defineItemStyle(row.id,row_item)" @click="switchSelectItem(row.id,row_item)">
                        <span class = "item_text">{{ row_item }}</span>
                    </div>
                </div>
                <div class = "divide_line"></div>
            </div>
            <div class = "list_row">
                <div class="row_content">
                    <div class = "list_row_title">
                        <span class = "item_text">关键词 :</span>  
                    </div> 
                    <input v-model="inputKeywords" id="keyword_inputbox">
                    <div class="list_row_item" @click="searchStart()">
                        <span class = "item_text" :style="'color : ' + themeColor">搜索</span>
                    </div>
                    <div style="position:absolute; right:0px;" class="list_row_item" @click="resetFilter">
                        <span class = "item_text" :style="'color : ' + themeColor">重置所有选项</span>
                    </div>
                </div>
            </div>
            <div class = "divide_line"></div>
            <div id = "chart_info_container">
                <div class="chart_info" v-for="(chartInfo,index) in chartsJsonObjects.charts" :key="index">
                    <img :src="(chartInfo.coverSrc != '' ? chartInfo.coverSrc : '/assets/default/no_image.png')" :alt="chartInfo.title" class="song_cover_img" @click="gotoChartInfo(chartInfo.chartId)">
                    <div class="song_info">
                        <div class="song_title">
                            <span @click="gotoSongInfo(chartInfo.songId)">{{ chartInfo.title }}</span>
                        </div>
                        <div class="artist">
                            <span v-for="(artistItem,index) in chartInfo.artists" :key="index" @click="gotoArtistInfo(artistItem.id)">{{ artistItem.name }} </span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
    #chartpage_container{
        min-width: 925px;
    }
    #keyword_inputbox{
        border-radius: 8px;
        border: 1px solid;
        border-color: gray;
        width: 30%;
        font-size: 16px;
    }
    #chart_info_container{
        padding: 4px;
        width: 98%;
        margin: 0px auto;
        display:flex;
        flex-wrap: wrap;
        justify-content: space-evenly;
        background-color: rgb(240,240,240);
        border-radius: 10px;
    }
    .chart_form{
        display: inline-block;
        width: 100%;
        border: 12px solid;
        background-color: white;
        border-color: white;
        border-radius: 20px;
        box-shadow: 0px 0px 8px gray;
    }
    .row_content{
        display: flex;
    }
    .list_row_title{
        margin: 6px;
        padding: 0px 4px;
        width: 80px;
        text-align: left;
        font-weight: 600;
        display:flex;
    }
    .list_row_item{
        margin: 0px 4px;
        padding: 6px 7px;
        border-radius: 6px;
        cursor: pointer;
    }
    .list_row_item:hover{
        box-shadow: 1px 1px 2px gray;
    }
    .divide_line{
        height: 1px;
        width: 98%;
        margin: 5px auto;
        background-color: rgb(160,160,160);
    }
    .item_text{
        user-select: none;
    }
    .chart_info{
        width: 207px;
        height: 276px;
        background-color: white;
        border-radius: 4px;
        padding: 4px;
        margin: 4px;
        white-space: nowrap;
        text-overflow: ellipsis;
        >.song_cover_img{
            height: 75%;
            cursor: pointer;
        }
        >.song_info{
            height: 25%;
            text-align: left;
            >.song_title{
                width: 95%;
                height: 50%;
                font-size: 26px;
                cursor: pointer;
                display: flex;
                align-items: center;
                padding: 0px 4px;
                overflow: hidden;
                border-radius: 4px;
            }
            >.song_title:hover{
                text-decoration: underline;
                outline: 2px solid;
            }
            >.artist{
                width: 95%;
                height: 40%;
                cursor: pointer;
                display: flex;
                align-items: center;
                padding: 0px 4px;
                overflow: hidden;
                border-radius: 4px;
            }
            >.artist:hover{
                text-decoration: underline;
                outline: 2px solid;
            }
        }
    }
    .chart_info:hover{
        box-shadow: 1px 1px 4px gray;
    }
</style>

<script setup>
    import { computed,ref,onMounted,onUnmounted } from 'vue'
    import { useStore } from 'vuex';
    import { useRoute,useRouter } from 'vue-router';
    import axios from 'axios';
    import { url } from '@/api';

    const store = useStore();
    const route = useRoute();
    const router = useRouter();
    const themeColor = computed(()=>store.state.themeColor);
    const inputKeywords = defineModel();
    const chartPage = ref(0);
    const chartsPerPage = ref(60);
    const atBottom = ref(false);
    const isAddingNewCharts = ref(false);

    var chartPageInfoArray = computed(() => store.state.chartStore.staticFilterInfo.filterItems);
    var chartFilter = computed(() => store.state.chartStore.currentChartFilter);
    const chartsJsonObjects = ref({
        "chartAmount" : 0,
        "charts" : []
    });
    
    //组件更新计时器
    const timer = ref();

    inputKeywords.value = route.query.keywords;

    const defineItemStyle = (row_id,row_item) => {
        if(chartFilter.value[row_id].includes(row_item)){
            return {
                backgroundColor : themeColor.value,
                color : "rgb(240,240,240)",
            };
        }
        return {};
    }
    const switchSelectItem = (rowId,selectedItem) => {
        store.commit('setChartFilterItem',{
            filterName:rowId,
            item:selectedItem
        });
        resetTimer();
    }

    //筛选条件发生变化时重置定时器，无变化1000ms后刷新组件内容
    function resetTimer()
    {
        clearTimeout(timer.value);
        timer.value = setTimeout(() => refreshList(),1000);
    }
    function refreshList()
    {
        chartPage.value = 0;
        chartsJsonObjects.value = {
            "chartAmount" : 0,
            "charts" : []
        };
        getChartsNextPage();
    }

    const gotoChartInfo = (chartId) =>{
        router.push({name:"chart",params:{chartId}});
    }
    const gotoSongInfo = (songId) =>{
        router.push({name:"song",params:{songId}});
    }
    const gotoArtistInfo = (artistId) =>{
        router.push({name:"artist",params:{artistId}});
    }
    const searchStart = () =>{
        store.commit('setChartFilterItem',{filterName:"keywords",item:inputKeywords.value});
        refreshList();
    }
    const resetFilter = () =>{
        store.commit('setDefaultChartFilter');
        refreshList();
    }
    // 检测是否滚动到页面底部的函数
    const checkScroll = () => {
        const scrollPosition = window.scrollY + window.innerHeight; // 当前的滚动位置
        const pageHeight = document.documentElement.scrollHeight; // 页面总高度
        // 判断是否滚动到页面底部
        if (scrollPosition >= pageHeight - 10) { // 给一点偏差
            atBottom.value = true;
            if(chartsJsonObjects.value.charts.length < chartsJsonObjects.value.chartAmount){
                getChartsNextPage();
            }
        } else {
            atBottom.value = false;
        }
    };
    const getChartsNextPage = () =>{
        if(isAddingNewCharts.value)
            return;
        isAddingNewCharts.value = true;
        axios.put(url.CHARTS,chartFilter.value,{
            params:{
                "startIndex" : chartPage.value * chartsPerPage.value,
                "getAmount" : chartsPerPage.value
            },
        })
        .then(response => {
            chartsJsonObjects.value.chartAmount = response.data.chartAmount;
            chartsJsonObjects.value.charts.push(...response.data.charts);
            isAddingNewCharts.value = false;
        })
        .catch(error => {
            alert(error);
        })
    }
    // 在组件挂载时，添加 scroll 事件监听
    onMounted(() => {
        window.addEventListener('scroll', checkScroll);
        getChartsNextPage();
    });
    // 在组件卸载时，移除 scroll 事件监听
    onUnmounted(() => {
        window.removeEventListener('scroll', checkScroll);
    });
    /*
    //测试用chartResultJson
    var chartsJsonObjects = 
    {
        "chartAmount":180,
        "charts":[
            {
                "chartId":100001,
                "title":"testsongggggggggggggggggg",
                "songId":100002,
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artists":[
                    {
                        "artistId":100003,
                        "artistName":"Camellia",
                    }
                ],
                "chartDesigners":[
                    {
                        "userId":100001,
                        "userName":"ekat088",
                    }
                ],
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "chartId":100001,
                "title":"testsongggggggggggggggggg",
                "songId":100002,
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artists":[
                    {
                        "artistId":100003,
                        "artistName":"Camellia",
                    }
                ],
                "chartDesigners":[
                    {
                        "userId":100001,
                        "userName":"ekat088",
                    }
                ],
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "chartId":100001,
                "title":"testsongggggggggggggggggg",
                "songId":100002,
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artists":[
                    {
                        "artistId":100003,
                        "artistName":"Camellia",
                    }
                ],
                "chartDesigners":[
                    {
                        "userId":100001,
                        "userName":"ekat088",
                    }
                ],
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "chartId":100001,
                "title":"testsongggggggggggggggggg",
                "songId":100002,
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artists":[
                    {
                        "artistId":100003,
                        "artistName":"Camellia",
                    }
                ],
                "chartDesigners":[
                    {
                        "userId":100001,
                        "userName":"ekat088",
                    }
                ],
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "chartId":100001,
                "title":"testsongggggggggggggggggg",
                "songId":100002,
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artists":[
                    {
                        "artistId":100003,
                        "artistName":"Camellia",
                    }
                ],
                "chartDesigners":[
                    {
                        "userId":100001,
                        "userName":"ekat088",
                    }
                ],
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "chartId":100001,
                "title":"testsongggggggggggggggggg",
                "songId":100002,
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artists":[
                    {
                        "artistId":100003,
                        "artistName":"Camellia",
                    }
                ],
                "chartDesigners":[
                    {
                        "userId":100001,
                        "userName":"ekat088",
                    }
                ],
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "chartId":100001,
                "title":"testsongggggggggggggggggg",
                "songId":100002,
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artists":[
                    {
                        "artistId":100003,
                        "artistName":"Camellia",
                    }
                ],
                "chartDesigners":[
                    {
                        "userId":100001,
                        "userName":"ekat088",
                    }
                ],
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "chartId":100001,
                "title":"testsongggggggggggggggggg",
                "songId":100002,
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artists":[
                    {
                        "artistId":100003,
                        "artistName":"Camellia",
                    }
                ],
                "chartDesigners":[
                    {
                        "userId":100001,
                        "userName":"ekat088",
                    }
                ],
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "chartId":100001,
                "title":"testsongggggggggggggggggg",
                "songId":100002,
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artists":[
                    {
                        "artistId":100003,
                        "artistName":"Camellia",
                    }
                ],
                "chartDesigners":[
                    {
                        "userId":100001,
                        "userName":"ekat088",
                    }
                ],
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "chartId":100001,
                "title":"testsongggggggggggggggggg",
                "songId":100002,
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artists":[
                    {
                        "artistId":100003,
                        "artistName":"Camellia",
                    }
                ],
                "chartDesigners":[
                    {
                        "userId":100001,
                        "userName":"ekat088",
                    }
                ],
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "chartId":100001,
                "title":"testsongggggggggggggggggg",
                "songId":100002,
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artists":[
                    {
                        "artistId":100003,
                        "artistName":"Camellia",
                    }
                ],
                "chartDesigners":[
                    {
                        "userId":100001,
                        "userName":"ekat088",
                    }
                ],
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "chartId":100001,
                "title":"testsongggggggggggggggggg",
                "songId":100002,
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artists":[
                    {
                        "artistId":100003,
                        "artistName":"Camellia",
                    }
                ],
                "chartDesigners":[
                    {
                        "userId":100001,
                        "userName":"ekat088",
                    }
                ],
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "chartId":100001,
                "title":"testsongggggggggggggggggg",
                "songId":100002,
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artists":[
                    {
                        "artistId":100003,
                        "artistName":"Camellia",
                    }
                ],
                "chartDesigners":[
                    {
                        "userId":100001,
                        "userName":"ekat088",
                    }
                ],
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "chartId":100001,
                "title":"testsongggggggggggggggggg",
                "songId":100002,
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artists":[
                    {
                        "artistId":100003,
                        "artistName":"Camellia",
                    }
                ],
                "chartDesigners":[
                    {
                        "userId":100001,
                        "userName":"ekat088",
                    }
                ],
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "chartId":100001,
                "title":"testsongggggggggggggggggg",
                "songId":100002,
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artists":[
                    {
                        "artistId":100003,
                        "artistName":"Camellia",
                    }
                ],
                "chartDesigners":[
                    {
                        "userId":100001,
                        "userName":"ekat088",
                    }
                ],
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "chartId":100001,
                "title":"testsongggggggggggggggggg",
                "songId":100002,
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artists":[
                    {
                        "artistId":100003,
                        "artistName":"Camellia",
                    }
                ],
                "chartDesigners":[
                    {
                        "userId":100001,
                        "userName":"ekat088",
                    }
                ],
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "chartId":100001,
                "title":"testsongggggggggggggggggg",
                "songId":100002,
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artists":[
                    {
                        "artistId":100003,
                        "artistName":"Camellia",
                    }
                ],
                "chartDesigners":[
                    {
                        "userId":100001,
                        "userName":"ekat088",
                    }
                ],
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "chartId":100001,
                "title":"testsongggggggggggggggggg",
                "songId":100002,
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artists":[
                    {
                        "artistId":100003,
                        "artistName":"Camellia",
                    }
                ],
                "chartDesigners":[
                    {
                        "userId":100001,
                        "userName":"ekat088",
                    }
                ],
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "chartId":100001,
                "title":"testsongggggggggggggggggg",
                "songId":100002,
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artists":[
                    {
                        "artistId":100003,
                        "artistName":"Camellia",
                    }
                ],
                "chartDesigners":[
                    {
                        "userId":100001,
                        "userName":"ekat088",
                    }
                ],
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "chartId":100001,
                "title":"testsongggggggggggggggggg",
                "songId":100002,
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artists":[
                    {
                        "artistId":100003,
                        "artistName":"Camellia",
                    }
                ],
                "chartDesigners":[
                    {
                        "userId":100001,
                        "userName":"ekat088",
                    }
                ],
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
        ]
    };
    */
</script>