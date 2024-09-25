//登录与注册
<template>
    <div class="fullscreen_cover">
        <div ref="active_window" id="main_window">
            <div id="close_button" class="in_window_icon" 
            @mouseenter="mouseEnterButton($event.target)"
            @mouseleave="mouseLeaveButton($event.target)" 
            @click="closeWindow()">
                <svg t="1723545287579" class="icon" viewBox="0 0 1024 1024" version="1.1"
                    xmlns="http://www.w3.org/2000/svg" p-id="1422" width="30" height="30">
                    <path
                        d="M512 570.88l196.864 196.8 58.88-58.816L570.752 512l196.864-196.864-58.816-58.88L512 453.248 315.136 256.32l-58.88 58.88L453.248 512l-196.864 196.864 58.88 58.88L512 570.752z"
                        :fill="currentCloseIconColor" fill-opacity=".9" p-id="1423">
                    </path>
                </svg>
            </div>
            <div id = "scroll_area">
                <div id = "middle_container" @touchmove.prevent>
                    <div id="login_window" class="internal_window">
                        <span id="login_title">登录到 Arekat</span>
                        <div id="login_input_list">
                            <span class="in_window_text_element">用户名/邮箱</span>
                            <input v-model="emailLoginInput" class="in_window_inputbox" type="text">
                            <span class="in_window_text_element">密码</span>
                            <input v-model="passwordLoginInput" type='password' class="in_window_inputbox">
                        </div>
                    </div>
                    <div id="register_window" class="internal_window">
                        <span id="login_title">欢迎加入 Arekat</span>
                        <div id="login_input_list">
                            <span class="in_window_text_element">邮箱</span>
                            <input v-model="emailRegeistInput" class="in_window_inputbox" type="text">
                            <span class="in_window_text_element">密码</span>
                            <input v-model="passwordRegeistInput" type='password' class="in_window_inputbox">
                            <span class="in_window_text_element">邮箱验证码</span>
                            <div id="email_verify_line">
                                <input v-model="verifyCodeInput" id = "email_verify_input" class="in_window_inputbox" type="text">
                                <div id="send_button"
                                @mouseenter="mouseEnterButton($event.target)"
                                @mouseleave="mouseLeaveButton($event.target)"
                                @click="sendVerifyEmail()">
                                    <span id="send_button_text" :style="'color:'+currentSendButtonColor">{{ canSendVerifyCode() ? emailSenderText : '重新发送(' + emailSenderTimeRemaining + ')' }}</span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="in_window_last_line in_window_component">
                <span id="in_window_register_button" 
                :style="isMouseOnRegisterButton?underlineStyle:currentColorStyle"
                @mouseenter="mouseEnterRegisterButton()" 
                @mouseleave="mouseLeaveRegisterButton()"
                @click="switchLoginAndRegister()">{{ isInLoginWindow ? "我没有账号，注册一个" : "返回到登录"}}</span>
                <div id="enter_button" class="in_window_icon" 
                @mouseenter="mouseEnterButton($event.target)"
                @mouseleave="mouseLeaveButton($event.target)" 
                @click="loginQuest()">
                    <svg t="1723564881036" class="icon" viewBox="0 0 1024 1024" version="1.1"
                        xmlns="http://www.w3.org/2000/svg" p-id="3538" width="22" height="22">
                        <path
                            d="M755.008 480H160q-14.016 0-23.008 8.992T128 512t8.992 23.008T160 544h595.008l-234.016 232.992Q512 787.008 512 800t9.504 22.496T544 832t23.008-8.992l288-288Q864 524.992 864 512t-8.992-23.008l-288-288Q556.992 192 544 192t-22.496 9.504T512 224t8.992 23.008z"
                            p-id="3539" :fill="currentEnterIconColor">
                        </path>
                    </svg>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
    #main_window{
        display: inline;
        border-radius: 20px;
        background-color: white;
        margin: auto auto;
        box-shadow: 0px 0px 4px gray;
        width: 400px;
        padding: 8px;
        overflow: hidden;
    }
    #scroll_area{
        width: 400px;
        overflow: hidden;
    }
    #login_window{
        height: 220px;
    }
    #register_window{
        height: 280px;
    }
    #login_title{
        font-size: 36px;
        margin: 20px 20px;
        user-select: none;
    }
    #login_input_list{
        position: relative;
        top: 10px;
    }
    #in_window_register_button{
        cursor: pointer;
        user-select: none;
    }
    #middle_container{
        height: 220px;
        width: 800px;
        display: flex;
    }
    /*隐藏滚动条*/
    #email_verify_line{
        display: flex;
    }
    #email_verify_input{
        width: 40%;
    }
    #send_button{
        width: 40%;
        margin-top: 4px;
        margin-bottom: 8px;
        border-radius: 8px;
        border: 2px dotted gray;
        text-align: center;
        align-items: center;
        margin-left: 20px;
        display: flex;
        cursor: pointer;
    }
    #send_button_text{
        width: 100%;
        user-select: none;
    }

    .fullscreen_cover{
        display: flex;
        position: fixed;
        z-index: 10;
        background-color: rgba(0,0,0,0.4);
        min-width: 1000px;
        width: 100%;
        height: 100%;
        top: 0px;
        bottom: 0px;
        align-items: center;
    }
    .internal_window{
        display: inline-block;
        width: 400px;
    }
    .in_window_icon{
        display: flex;
        width: 40px;
        height: 40px;
        border-radius: 10px;
        margin-left: auto;
        cursor: pointer;
        align-items: center;
    }
    .in_window_text_element{
        display: block;
        margin-left: 6%;
        border-radius: 6px;
        width: 88%;
        user-select: none;
    }
    .in_window_inputbox{
        margin-left: 6%;
        margin-top: 4px;
        margin-bottom: 8px;
        border: 2px solid gray;
        border-radius: 8px;
        background-color: rgb(230,230,230);
        height: 36px;
        width: 86%;
        font-size: 20px;
        outline: 0px;
        padding-left: 3px;
        padding-right: 3px;
    }
    .in_window_last_line{
        display: flex;
        margin-top: 8px;
        margin-left: 6%;
        align-items: center;
        height: 40px;
    }
    .icon{
        margin: 0px auto;
    }
</style>

<script setup>
    import { ref,computed } from 'vue';
    import { useStore } from 'vuex';
    import { url } from '@/api';
    import axios from 'axios';
    import CryptoJS from 'crypto-js';

    const store = useStore();
    const themeColor = computed(() => store.state.themeColor);

    const currentColor = ref(themeColor);
    const currentCloseIconColor = ref(themeColor.value);
    const currentEnterIconColor = ref(themeColor.value);
    const currentSendButtonColor = ref(themeColor.value);
    const iconButtonColorArray = {
        close_button:currentCloseIconColor,
        enter_button:currentEnterIconColor,
        send_button:currentSendButtonColor
    };
    const isMouseOnRegisterButton = ref(false);
    const emailSenderText = ref("发送验证码");
    const emailSenderTimer = ref();
    const emailSenderTimeRemaining = computed(()=>store.state.emailSenderTimeRemaining)
    const canSendVerifyCode = () => {return emailSenderTimeRemaining.value <= 0};
    const emailLoginInput = ref('');
    const passwordLoginInput = ref('');
    const emailRegeistInput = ref('');
    const passwordRegeistInput = ref('');
    const verifyCodeInput = ref('');

    //是否正在登录状态，为否则为正在注册.value
    const isInLoginWindow = ref(true);

    //当前主题颜色
    const currentColorStyle = {
        color : currentColor.value,
    }
    //为某元素添加下划线
    const underlineStyle = {
        color : currentColor.value,
        textDecoration : "underline solid " + currentColor.value
    }

    //鼠标移入移出时更改颜色
    const mouseEnterButton = (target) =>{
        if(canSendVerifyCode()){
            target.style="background-color : " + themeColor.value + ";";
            iconButtonColorArray[target.id].value = "white";
        }
    }
    const mouseLeaveButton = (target) =>{
        target.style="background-color : white;";
        iconButtonColorArray[target.id].value = themeColor.value;
    }
    //鼠标移入移出时更改注册按钮状态
    const mouseEnterRegisterButton = () =>{
        isMouseOnRegisterButton.value = true;
    }
    const mouseLeaveRegisterButton = () =>{
        isMouseOnRegisterButton.value = false;
    }
    const encryptStringSHA256 = (originString) =>{ 
        return CryptoJS.SHA256(originString).toString(CryptoJS.enc.Hex);
    }
    //关闭窗口
    const closeWindow = () =>{
        store.commit('switchLoggingState');
    }
    //点击按钮切换登录注册界面
    const switchLoginAndRegister = () =>{
        var middleContainer = document.getElementById("middle_container");
        var scrollArea = document.getElementById("scroll_area");
        isInLoginWindow.value = !isInLoginWindow.value;
        if(isInLoginWindow.value)
        {
            scrollArea.scrollLeft = 0;
            middleContainer.style = "height: 220px";
        }
        else
        {
            scrollArea.scrollLeft = 400;
            middleContainer.style = "height: 280px";
        }
    }
    //每次递归让timer-1秒
    const timerSubOneSecond = (timer,timeRemainingName) =>{
        var timeRemaining = computed(()=>store.state[timeRemainingName]);
        if(timeRemaining.value <= 1){
            return;
        }
        timer = setTimeout(()=>{
            timerSubOneSecond(timer,timeRemainingName);
            store.commit('timeSubOne',timeRemainingName);
        },1000)
    }
    //发送确认邮件
    const sendVerifyEmail = () =>{
        if((!emailRegeistInput.value.includes('@')) || emailRegeistInput.value.length < 6)
            return;
        if(canSendVerifyCode()){
            console.log(url.VERIFY_EMAIL);
            axios.post(url.VERIFY_EMAIL,{
                "token": "string",
                "questClass": 0,
                "email": emailRegeistInput.value
            })
            .then(function (response) {
                // 请求成功，处理响应数据
                console.log(response.data);
                store.commit('setLoginData',response.data.token);
            })
            .catch(function (error) {
                // 请求失败，处理错误
                console.error(error);
            });
            store.commit('setEmailTimerRamainingTime');
            timerSubOneSecond(emailSenderTimer.value,'emailSenderTimeRemaining');
            emailSenderText.value = '重新发送';
        }
        else{
            console.log("没发");
        }
    }
    //点击enter按钮发起登录或注册请求
    const loginQuest = () =>{
        var pwd = '';
        if(isInLoginWindow.value){
            pwd = encryptStringSHA256(passwordLoginInput.value);
            axios.put(url.LOGIN,{
                "email":emailLoginInput.value,
                "password":pwd
            })
            .then(function(response) {
                var jwtToken = response.data.token;
                store.commit('setLoginData',{token : jwtToken});
                localStorage.setItem("arekat_v1_token",jwtToken);
                window.location.reload();
            })
            .catch(function(error) {
                alert(error);
            })
        }
        else{
            pwd = encryptStringSHA256(passwordRegeistInput.value);
            console.log(pwd);
            axios.post(url.REGIST,{
                "email":emailRegeistInput.value,
                "userName":emailRegeistInput.value,
                "password":pwd,
                "emailKey":verifyCodeInput.value
            })
            .then(function() {
                alert("Regeist success!");
                setTimeout(()=>{
                    window.location.reload();
                },2000);
            })
            .catch(function(error) {
                alert(error.response.data);
            })
        }
    }
</script>