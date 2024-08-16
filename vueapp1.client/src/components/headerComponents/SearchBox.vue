<template>
    <div id = "search_box_container">
        <div class="search_box header_component">
            <!-- 下拉菜单选择搜索选项 -->
            <div class="dropmenu_component in_box_component" style="left: 20px;" @click="showDropdownMenu">
                <span class="header_clickable_text">{{ currentOption }}</span>
                <svg t="1721653008820" viewBox="0 0 1024 1024" version="1.1"
                    xmlns="http://www.w3.org/2000/svg" p-id="984" width="20" height="20">
                    <path d="M226.56 413.44l58.88-58.88L512 581.12l226.56-226.56 58.88 58.88L512 698.88 226.56 413.44z"
                        fill="#ffffff" fill-opacity=".9" p-id="985" style = "margin: auto;">
                    </path>
                </svg>
                <Transition name = "drop_fade">
                    <div v-if = "isShowingDropdownMenu" class = "dropdown_menu">
                        <div v-for = "(optionItem,index) in options" :key = "index" :name = "optionItem">
                        <span class = "dropmenu_item" @click.stop = "changeMenuOption(index)">{{ optionItem }}</span>
                        <div v-if = "index != options.length - 1" class="divide_line_dropdown_menu">
                        </div>
                    </div>
                </div>
                </Transition>
                <div class="divide_line_search_box"></div>
            </div>
            <input class="input_box" :placeholder=defaultText[options.indexOf(currentOption)]>
            <div class="search_component in_box_component">
                <div class="divide_line_search_box"></div>
                <svg t="1721709688206" class="icon" viewBox="0 0 1024 1024" version="1.1"
                    xmlns="http://www.w3.org/2000/svg" p-id="4454" width="20" height="20">
                    <path
                        d="M769.323 708.992l182.741 182.699-60.373 60.373-182.699-182.741a382.293 382.293 0 0 1-239.659 84.01c-211.968 0-384-172.032-384-384s172.032-384 384-384 384 172.032 384 384a382.293 382.293 0 0 1-84.01 239.659z m-85.59-31.659a297.685 297.685 0 0 0 84.267-208c0-165.034-133.675-298.666-298.667-298.666-165.034 0-298.666 133.632-298.666 298.666C170.667 634.325 304.299 768 469.333 768a297.685 297.685 0 0 0 208-84.267l6.4-6.4z"
                        fill="#CDCDCD" p-id="4455">
                    </path>
                </svg>
            </div>
        </div>
    </div>
</template>

<style scoped>
    #search_box_container{
        width: 30%;
        position: relative;
    }
    .search_box{
        height: 40px;
        border: 0px, solid;
        border-radius: 20px;
        background-color: rgba(0, 0, 0, 0.65);
        color: rgb(220, 220, 220);
        position: relative;
    }
    .in_box_component{
        position: absolute;
        display: flex;
        height: 40px;
        align-items: center;
        font-family: PingFang SC,HarmonyOS_Regular,Helvetica Neue,Microsoft YaHei,sans-serif!important;
        user-select: none;
    }
    li{
        list-style: none;
    }
    .input_box{
        min-width: 10px;
        margin: 0px 0px;
        border: 0px;
        background-color: rgba(0, 0, 0, 0);
        color: rgb(220, 220, 220);
        font-size: 17px;
        outline: none;
        width: auto;
        position: absolute;
        left: 110px;
        right: 80px;
        font-family: PingFang SC,HarmonyOS_Regular,Helvetica Neue,Microsoft YaHei,sans-serif!important;
    }
    .search_component{
        width: 60px;
        right: 12px;
        cursor: pointer;
    }
    .divide_line_search_box{
        width: 1px;
        height: 20px;
        margin-left: 10px;
        margin-right: 10px;
        padding: 0px;
        background-color: rgba(120, 120, 120, 0.8);
    }
</style>

<script setup>
    import { ref } from 'vue';

    const isShowingDropdownMenu = ref(false);
    const options = ["谱面","用户","主题"];
    const defaultText = [
        "搜索曲目、曲师、谱师",
        "搜索用户，支持搜索关键字、id",
        "搜索主题"
    ]
    const currentOption = ref(options[0]);
    
    const showDropdownMenu = () => {
        isShowingDropdownMenu.value = !isShowingDropdownMenu.value;
    }
    const changeMenuOption = (key) => {
        isShowingDropdownMenu.value = false;
        currentOption.value = options[key];
    }
</script>