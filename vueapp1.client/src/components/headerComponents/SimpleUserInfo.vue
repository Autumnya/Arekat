//顶栏的用户头像与名称
<template>
    <div id = "user_container" class = "header_component">
        <div v-if = "isLogged" class = "user_div">
            <img src = "../../assets/default/user_profile_pic.png" class = "profile_pic">
            <svg t="1721653008820" class="icon" viewBox="0 0 1024 1024" version="1.1"
                xmlns="http://www.w3.org/2000/svg" p-id="984" width="20" height="20">
                <path d="M226.56 413.44l58.88-58.88L512 581.12l226.56-226.56 58.88 58.88L512 698.88 226.56 413.44z"
                    fill="#ffffff" fill-opacity=".9" p-id="985">
                </path>
            </svg>
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
        width: 15%;
    }
    .profile_pic{
        width: 40px;
        height: 40px;
        border-radius: 5px;
    }
    .user_div{
        margin: 0px auto;
        cursor: pointer;
        align-items: center;
        text-align: center;
        display: flex;
    }
</style>

<script  setup>
    import LoginWindow from './LoginWindow.vue';
    import { ref,computed } from 'vue';
    import { useStore } from 'vuex';

    const store = useStore();
    const isLogged = ref(false);
    const isLogging = computed(() => store.state.isLogging);

    const hasToken = () =>{
        var token = computed(() => store.state.token);
        return token.value != "";
    }

    //用户组件点击事件
    //若不是登录状态则显示登录窗口
    const onUserModuleClick = () =>{
        store.commit('switchLoggingState');
    }
</script>