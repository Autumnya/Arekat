export default({
    //数据，相当于data
    state: {
        staticFilterInfo:{
            //谱面页分类器，“不限”为关键字，选择“不限”时将清除其他选项
            filterItem:[
                {
                    id:"games",
                    name:"分区",
                    item:["Arcaea"],
                    allowMultiChoose:true
                },
                {
                    id:"examineStage",
                    name:"审核阶段",
                    item:["test","stable"],
                    allowMultiChoose:true
                },
                {
                    id:"difficulty",
                    name:"难度",
                    item:["不限","9以下","9","9+","10","10+","11","11+","12","12+及以上"],
                    allowMultiChoose:true
                },
                {
                    id:"date",
                    name:"日期范围",
                    item:["不限","今天上新","最近7天","最近30天","自定义"],
                    allowMultiChoose:false
                },
                {
                    id:"order",
                    name:"排序方式",
                    item:["时间从新到旧","时间从旧到新","难度从高到低","难度从低到高","下载量从高到低","下载量从低到高"],
                    allowMultiChoose:false
                }
            ],

            defaultChartFilter:{
                games : ["Arcaea"],
                examineStage : ["test","stable"],
                difficulty : ["不限"],
                date : "不限",
                minPassDate : ["","",""],
                maxPassDate : ["","",""],
                order : "时间从新到旧",

                keyWords : []
            }
        },

        currentChartFilter:{
            games : ["Arcaea"],
            examineStage : ["test","stable"],
            difficulty : ["不限"],
            date : "不限",
            minPassDate : ["","",""],
            maxPassDate : ["","",""],
            order : "时间从新到旧",

            keyWords : []
        },

    },
    //定义计算state数据的方法
    getters: {

    },
    //定义更改state中数据的非异步方法
    mutations: {
        setDefaultChartFilter(state,chartFilter){
            state.currentChartFilter = chartFilter;
        },
        setChartFilter(state){
            state.currentChartFilter = state.staticFilterInfo.defaultChartFilter;
        },
        //更改谱面筛选器的某一项，如果已经选择则取消选择，若无选项且可以多选则改为“不限，若无选项且不可多选则不做更改
        //参数：filterName:筛选类别，即行标题;item:选择的选项
        setChartFilterItem(state,filterName,item){
            var selectedFilter = state.currentChartFilter[filterName];
            if(selectedFilter == undefined)
                return;
            if(selectedFilter == item)
            {
                if(selectedFilter.includes(item))
                {
                    selectedFilter[item] = undefined;
                    selectedFilter.forEach(element => {
                        //检查到正常选项
                        if(element)
                        {
                            return;
                        }
                        //未检查到正常选项（全为空）
                        
                    });
                }
                else
                {
                    selectedFilter.push(item);
                }
            }
            selectedFilter = item;
        },
    },
    //定义异步操作方法
    actions: {
      
    },
    //允许同时拥有多个store
    modules: {
      
    },
  })