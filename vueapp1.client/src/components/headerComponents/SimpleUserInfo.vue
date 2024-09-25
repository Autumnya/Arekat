//顶栏的用户头像与名称
<template>
    <div id = "user_container" class = "header_component">
        <div v-if = "isLogged" class = "user_div" @click.stop="showDropdownMenu()">
            <span class="header_user_name header_clickable_text">{{ selfInfoObject.userName }}</span>
            <img src = "../../assets/default/user_profile_pic.png" class = "profile_pic">
            <svg t="1721653008820" class="icon" viewBox="0 0 1024 1024" version="1.1"
                xmlns="http://www.w3.org/2000/svg" p-id="984" width="20" height="20">
                <path d="M226.56 413.44l58.88-58.88L512 581.12l226.56-226.56 58.88 58.88L512 698.88 226.56 413.44z"
                    fill="#ffffff" fill-opacity=".9" p-id="985">
                </path>
            </svg>
            <Transition name = "drop_fade">
            <div v-if = "isShowingDropdownMenu" class = "dropdown_menu">
                <div v-for="(userOptionItem,index) in userOptions" :key="index">
                    <a :href="dropmenuUrl[index]" class="dropmenu_item" style="font-weight: normal;">{{ userOptionItem }}</a>
                    <div v-if="index != userOptions.length - 1" class="divide_line_dropdown_menu">
                    </div>
                </div>
            </div>
            </Transition>
        </div>
        <div v-else class="user_div" @click="onUserModuleClick()">
            <span class="header_clickable_text" style="text-decoration: underline;">
                登录/注册
            </span>
        </div>
    </div>
    <LoginWindow v-if="isLogging"></LoginWindow>
</template>

<style scoped>
    #user_container{
        width: 20%;
        position: relative;
        margin: auto;
    }
    .profile_pic{
        width: 40px;
        height: 40px;
        border-radius: 5px;
    }
    .user_div{
        margin: auto;
        cursor: pointer;
        align-items: center;
        text-align: center;
        display: flex;
        position: relative;
    }.header_user_name{
        max-width: 40%;
        text-overflow: ellipsis;
        overflow: hidden;
        margin: 8px;
    }
</style>

<script  setup>
    const BASE_URL = import.meta.env.VITE_BASE_URL
    import axios from 'axios';
    import LoginWindow from './LoginWindow.vue';
    import { ref,computed } from 'vue';
    import { useStore } from 'vuex';
    import { url } from '@/api';

    const store = useStore();
    const isLogged = ref(false);
    const isLogging = computed(() => store.state.isLogging);
    var token = localStorage.getItem("arekat_v1_token");
    const isShowingDropdownMenu = ref(false);
    const selfInfoObject = ref({});
    const userOptions = ["个人信息","上传谱面"];
    const dropmenuUrl = ref([]);

    if(token != null)
    {
        axios.get(url.SELF,{headers:{
            Authorization:`Bearer ${token}`
        }})
        .then(response => {
            selfInfoObject.value=response.data;
            isLogged.value = true;
            store.commit('setUserId',selfInfoObject.value.userId);
            dropmenuUrl.value = [
                BASE_URL + '/user/' + selfInfoObject.value.userId,
                BASE_URL + '/chart-upload'
            ]
        })
        .catch(error => {
            alert(error + '\n请重新登录');
        })
    }

    //用户组件点击事件
    //若不是登录状态则显示登录窗口
    const onUserModuleClick = () =>{
        store.commit('switchLoggingState');
    }

    const showDropdownMenu = () =>{
        isShowingDropdownMenu.value = !isShowingDropdownMenu.value;
    }
</script>