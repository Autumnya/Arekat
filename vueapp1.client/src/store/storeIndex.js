import { createStore } from 'vuex'

import chartStore from "./chartStore"

export default createStore({
  //数据，相当于data
  state: {
    themeColor: "rgb(126, 44, 220)",
    currentLanguage: "zh_CN",

    isLogging: false,
    searchBoxOption: "chart",
    searchKeyword: "",

    emailSenderTimeRemaining : 0,

    token:"",
  },
  //定义计算state数据的方法
  getters: {

  },
  //定义更改state中数据的非异步方法
  mutations: {
    setDefaultThemeColor(state){
      state.themeColor = "rgb(126, 44, 220)";
    },
    setThemeColor(state,newColor){
      state.themeColor = newColor;
    },
    switchLoggingState(state){
      state.isLogging = !state.isLogging;
    },
    setEmailTimerRamainingTime(state){
      state.emailSenderTimeRemaining = 6;
    },
    timeSubOne(state,remainingTimeName){
      state[remainingTimeName] -= 1;
    },
  },
  //定义异步操作方法
  actions: {

  },
  //允许同时拥有多个store
  modules: {
    chartStore:chartStore
  },
})