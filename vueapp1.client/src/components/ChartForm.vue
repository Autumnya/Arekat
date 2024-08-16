<template>
    <div class = "sub_container">
        <div class="chart_form">
            <div v-for="(row,index) in chartPageInfoArray" :key="index" class = "list_row">
                <div class="row_content">
                    <div class = "list_row_title">
                        <span class = "item_text">{{ row.name }} :</span>
                    </div>
                    <div v-for="(row_item,sub_index) in row.item" :key="sub_index" class="list_row_item" :style="defineItemStyle(row.id,row_item)" @click="switchSelectItem($event.target)">
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
                    <input id="keyword_inputbox">
                    <div class="list_row_item">
                        <span class = "item_text" :style="'color : ' + themeColor">搜索</span>
                    </div>
                    <div style="position:absolute; right:0px;" class="list_row_item">
                        <span class = "item_text" :style="'color : ' + themeColor">重置所有选项</span>
                    </div>
                </div>
            </div>
            <div class = "divide_line"></div>
        </div>
    </div>
</template>

<style scoped>
    #keyword_inputbox{
        border-radius: 8px;
        border: 1px solid;
        border-color: gray;
        width: 30%;
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
</style>

<script setup>
    import { computed,ref } from 'vue'
    import { useStore } from 'vuex';

    const store = useStore();
    const themeColor = computed(()=>store.state.themeColor);

    var chartPageInfoArray = computed(() => store.state.chartStore.staticFilterInfo.filterItem);
    var chartFilter = computed(() => store.state.chartStore.currentChartFilter);

    const defineItemStyle = (row_id,row_item) => {
        if(chartFilter.value[row_id].includes(row_item)){
            return {
                backgroundColor : themeColor.value,
                color : "rgb(240,240,240)",
            };
        }
        return {};
    }
    const switchSelectItem = (target) => {
        
    }

    //默认为空，展示未筛选的默认谱面列表
    //const chartFilter = ref(undefined);
    //
    //const games = ["Arcaea"];
    //const examineStage = ["test","stable"];
    //
    //const minDiff = ref(undefined);
    //const maxDiff = ref(undefined);
    //const minPassDate = ref(["","",""]);
    //const maxPassDate = ref(["","",""]);
    //const keyWords = ref([]);



    //该组件是否需要被更新
    //const timer = ref(setTimeout())

    //筛选条件发生变化时重置定时器，无变化500ms后刷新组件内容
    // function ResetTimer()
    // {
    //     clearTimeout(timer.value);
    //     timer.value = setTimeout(RefreshList(),500);
    // }
    // function RefreshList()
    // {
    // 
    // }

</script>