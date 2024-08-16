//语言切换器
<template>
    <div id = "lang_switch_container" class = "header_component">
        <div class = "lang_switch_div header_clickable_text dropmenu_component" @click.stop = "showDropdownMenu">
            <span class = "lang_text">
                {{ currentLang }}
            </span>
            <svg t="1721653008820" class="icon" viewBox="0 0 1024 1024" version="1.1"
                xmlns="http://www.w3.org/2000/svg" p-id="984" width="20" height="20">
                <path d="M226.56 413.44l58.88-58.88L512 581.12l226.56-226.56 58.88 58.88L512 698.88 226.56 413.44z"
                    fill="#ffffff" fill-opacity=".9" p-id="985">
                </path>
            </svg>
            <Transition name = "drop_fade">
            <div v-if = "isShowingDropdownMenu" class = "dropdown_menu">
                <div v-for="(langItem,index) in t_lang" :key="index" :name="langItem">
                    <span class="dropmenu_item" style="font-weight: normal;" @click.stop="changeMenuOption(index)">{{ langItem }}</span>
                    <div v-if="index != t_lang.length - 1" class="divide_line_dropdown_menu">
                    </div>
                </div>
            </div>
            </Transition>
        </div>
    </div>
</template>

<style scoped>
    #lang_switch_container{
        width: 20%;
        position: relative;
    }
    .lang_switch_div{
        margin: 0px auto;
        position: relative;
        display: flex;
    }
    .lang_text{
        text-decoration: underline;
    }
</style>

<script setup>
    import { ref } from 'vue';

    const isShowingDropdownMenu = ref(false);
    const t_lang = ["简体中文","日本語","English"];
    const currentLang = ref(t_lang[0])

    const showDropdownMenu = () => {
        isShowingDropdownMenu.value = !isShowingDropdownMenu.value;
    }
    const changeMenuOption = (key) => {
        isShowingDropdownMenu.value = false;
        currentLang.value = t_lang[key];
    }
</script>