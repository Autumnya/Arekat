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
                    <div style="position:absolute; right:0px;" class="list_row_item">
                        <span class = "item_text" :style="'color : ' + themeColor">重置所有选项</span>
                    </div>
                </div>
            </div>
            <div class = "divide_line"></div>
            <div id = "chart_info_container">
                <div class="chart_info" v-for="(chartInfo,index) in chartsJsonObjects.charts" :key="index">
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
        padding: 8px;
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
        width: 210px;
        height: 280px;
        background-color: white;
        border-radius: 4px;
        padding: 4px;
        margin: 4px;
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
    .chart_info:hover{
        box-shadow: 1px 1px 4px gray;
    }
</style>

<script setup>
    import { computed,ref } from 'vue'
    import { useStore } from 'vuex';
    import { useRoute,useRouter } from 'vue-router';

    const store = useStore();
    const route = useRoute();
    const router = useRouter();
    const themeColor = computed(()=>store.state.themeColor);
    const inputKeywords = defineModel();

    var chartPageInfoArray = computed(() => store.state.chartStore.staticFilterInfo.filterItem);
    var chartFilter = computed(() => store.state.chartStore.currentChartFilter);

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
        console.log("发送了一个请求");
        //chartsJsonObjects.value = {};
    }

    const gotoSongInfo = (songId) =>{
        router.push({name:"song",params:{songId}});
    }
    const gotoArtistInfo = (artistId) =>{
        router.push({name:"artist",params:{artistId}});
    }
    const searchStart = () =>{
        store.commit('setChartFilterItem',{filterName:"keywords",item:inputKeywords.value});
        router.push({name:"charts",query:{keywords:inputKeywords.value}});
    }

    //测试用chartResultJson
    var chartsJsonObjects = 
    {
        "chartAmount":18,
        "charts":[
            {
                "title":"testsongggggggggggggggggg",
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artist": "Camellia",
                "chartDesigner":"",
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "title":"testsongggggggggggggggggg",
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artist": "Camellia",
                "chartDesigner":"",
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "title":"testsongggggggggggggggggg",
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artist": "Camellia",
                "chartDesigner":"",
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "title":"testsongggggggggggggggggg",
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artist": "Camellia",
                "chartDesigner":"",
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "title":"testsongggggggggggggggggg",
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artist": "Camellia",
                "chartDesigner":"",
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "title":"testsongggggggggggggggggg",
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artist": "Camellia",
                "chartDesigner":"",
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "title":"testsongggggggggggggggggg",
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artist": "Camellia",
                "chartDesigner":"",
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "title":"testsongggggggggggggggggg",
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artist": "Camellia",
                "chartDesigner":"",
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "title":"testsongggggggggggggggggg",
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artist": "Camellia",
                "chartDesigner":"",
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "title":"testsongggggggggggggggggg",
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artist": "Camellia",
                "chartDesigner":"",
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "title":"testsongggggggggggggggggg",
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artist": "Camellia",
                "chartDesigner":"",
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "title":"testsongggggggggggggggggg",
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artist": "Camellia",
                "chartDesigner":"",
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "title":"testsongggggggggggggggggg",
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artist": "Camellia",
                "chartDesigner":"",
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "title":"testsongggggggggggggggggg",
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artist": "Camellia",
                "chartDesigner":"",
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "title":"testsongggggggggggggggggg",
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artist": "Camellia",
                "chartDesigner":"",
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "title":"testsongggggggggggggggggg",
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artist": "Camellia",
                "chartDesigner":"",
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "title":"testsongggggggggggggggggg",
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artist": "Camellia",
                "chartDesigner":"",
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            },
            {
                "title":"testsongggggggggggggggggg",
                "coverSrc":"../../src/assets/default/test_song_cover.jpg",
                "artist": "Camellia",
                "chartDesigner":"",
                "bpm":"200",
                "passDate":"2024-7-29",
                "ratingClass":2,
                "rating":10
            }
        ]
    };
</script>