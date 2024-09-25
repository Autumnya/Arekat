export default({
    //数据，相当于data
    "state": {
        "staticFilterInfo":{
            //谱面页分类器，“不限”为关键字，选择“不限”时将清除其他选项
            "filterItems":{
                "games":{
                    "id":"games",
                    "name":"分区",
                    "item":["Arcaea"],
                    "allowMultiChoose":true
                },
                "examineStage":{
                    "id":"examineStage",
                    "name":"审核阶段",
                    "item":["不限","test","stable"],
                    "allowMultiChoose":true
                },
                "difficulty":{
                    "id":"difficulty",
                    "name":"难度",
                    "item":["不限","9以下","9","9+","10","10+","11","11+","12","12+及以上"],
                    "allowMultiChoose":true
                },
                "date":{
                    "id":"date",
                    "name":"日期范围",
                    "item":["不限","今天上新","最近7天","最近30天"],
                    "allowMultiChoose":false
                },
                "order":{
                    "id":"order",
                    "name":"排序方式",
                    "item":["时间从新到旧","时间从旧到新","下载量从高到低","下载量从低到高"],
                    "allowMultiChoose":false
                }
            },

            "defaultChartFilter":{
                "games" : ["Arcaea"],
                "examineStage" : ["不限"],
                "difficulty" : ["不限"],
                "date" : "不限",
                "minPassDate" : ["1","1","2000"],
                "maxPassDate" : ["12","31","2100"],
                "order" : "时间从新到旧",

                "keyWords" : ""
            }
        },

        "currentChartFilter":{
            "games" : ["Arcaea"],
            "examineStage" : ["不限"],
            "difficulty" : ["不限"],
            "date" : "不限",
            "minPassDate" : ["1","1","2000"],
            "maxPassDate" : ["12","31","2100"],
            "order" : "时间从新到旧",

            "keywords" : ""
        },

    },
    //定义计算state数据的方法
    getters: {

    },
    //定义更改state中数据的非异步方法
    mutations: {
        setChartFilter(state,chartFilter){
            state.currentChartFilter = chartFilter;
        },
        setDefaultChartFilter(state){
            state.currentChartFilter = state.staticFilterInfo.defaultChartFilter;
        },
        //更改谱面筛选器的某一项，如果已经选择则取消选择，若无选项且可以多选则改为“不限，若无选项且不可多选则不做更改
        //参数：filterName:筛选类别，即行标题;item:选择的选项
        setChartFilterItem(state,{filterName,item}){
            if(state.currentChartFilter[filterName] == undefined)
                return;
            //执行关键词搜索
            if(filterName == "keywords")
            {
                state.currentChartFilter["keywords"] = item;
                return;
            }
            //更改不允许多选的项目
            if(!state.staticFilterInfo.filterItems[filterName].allowMultiChoose)
            {
                if(state.currentChartFilter[filterName] == item)
                {
                    return;
                }
                else
                {
                    state.currentChartFilter[filterName] = item;
                    return;
                }
            }
            if(item == "不限")
            {
                state.currentChartFilter[filterName] = ["不限"];
            }
            if(state.currentChartFilter[filterName].includes(item))
            {
                //软删除
                var itemIndex = state.currentChartFilter[filterName].indexOf(item);
                state.currentChartFilter[filterName][itemIndex] = undefined;
                var hasOption = false;
                state.currentChartFilter[filterName].forEach((element) => {
                    //检查到正常选项
                    if(element != undefined)
                    {
                        hasOption = true;
                    }
                });
                //未检查到正常选项（全为空）
                if(!hasOption)
                    state.currentChartFilter[filterName] = ["不限"];
            }
            else
            {
                if(state.currentChartFilter[filterName].includes("不限"))
                {
                    state.currentChartFilter[filterName] = [];
                }
                state.currentChartFilter[filterName].push(item);
            }
        }
    },
    //定义异步操作方法
    actions: {
      
    },
    //允许同时拥有多个store
    modules: {
      
    },
  })