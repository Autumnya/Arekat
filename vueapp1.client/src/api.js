import axios from "axios"

const BASE_API_URL = import.meta.env.VITE_BASE_API

const apiClient = axios.create({
    baseURL: BASE_API_URL,
    timeout: 10000, // 请求超时时间
});
apiClient.interceptors.request.use(
    (config) => {
      // 从 localStorage 获取 token
      const token = localStorage.getItem('arekat_v1_token'); 
  
      if (token) {
        // 如果 token 存在，将其添加到请求头中
        config.headers.Authorization = `Bearer ${token}`;
      }
      return config;
    },
    (error) => {
      // 请求错误时处理
      return Promise.reject(error);
    }
  );

export const url = {
    //登录(用户名/邮箱，密码)
    LOGIN: `${BASE_API_URL}/user-management/login`,
    //注册(用户名，密码，邮箱验证码)
    REGIST: `${BASE_API_URL}/user-management/regist`,
    //请求发送注册邮箱验证码()
    //body:{"token","questClass","email"}
    VERIFY_EMAIL: `${BASE_API_URL}/user-management/verify-email`,


    //用户页/曲目页 谱面列表(群体，ID)
    //param1:"user"或"song"
    //param2:id
    //param3:startIndex
    //param4:getAmount
    CHARTLIST: `${BASE_API_URL}/chart-management/chartlist`,
    //谱面详细信息()
    //routeParam:id
    CHART:`${BASE_API_URL}/chart-management/charts/`,
    //谱面页的谱面()
    //body:chartFilter
    CHARTS:`${BASE_API_URL}/chart-management/charts`,
    //谱面下载
    //param1:id
    CHART_DOWNLOAD:`${BASE_API_URL}/chart-management/download`,
    //发起一个上传请求
    CHART_UPLOAD:`${BASE_API_URL}/chart-management/upload`,
    //文件传输完毕后确认上传谱面
    //body:songListInfo
    //param1:guid
    CHART_UPLOAD_CONFIRM:`${BASE_API_URL}/chart-management/upload-confirm`,
    //上传base.ogg
    //body:base.ogg
    //param1:guid
    CHART_UPLOAD_baseogg:`${BASE_API_URL}/chart-management/upload/base.ogg`,
    //上传base.jpg
    //body:base.jpg
    //param1:guid
    CHART_UPLOAD_basejpg:`${BASE_API_URL}/chart-management/upload/base.jpg`,
    //上传base_256.jpg
    //body:base_256.jpg
    //param1:guid
    CHART_UPLOAD_base256jpg:`${BASE_API_URL}/chart-management/upload/base_256.jpg`,
    //上传0.aff
    //body:0.aff
    //param1:guid
    CHART_UPLOAD_0aff:`${BASE_API_URL}/chart-management/upload/0.aff`,
    //上传1.aff
    //body:1.aff
    //param1:guid
    CHART_UPLOAD_1aff:`${BASE_API_URL}/chart-management/upload/1.aff`,
    //上传2.aff
    //body:2.aff
    //param1:guid
    CHART_UPLOAD_2aff:`${BASE_API_URL}/chart-management/upload/2.aff`,
    //上传3.aff
    //body:3.aff
    //param1:guid
    CHART_UPLOAD_3aff:`${BASE_API_URL}/chart-management/upload/3.aff`,
    //谱面过一审，使谱面在网络上可见
    //param1:chartId
    CHART_CHECK_PASS:`${BASE_API_URL}/chart-management/check-pass`,


    //推荐列表()
    RECOMMAND_LISTS:`${BASE_API_URL}/chart-management/recommand-lists`,


    //最新公告()
    NOTICE_NEWEST: `${BASE_API_URL}/notice-management/notices/newest`,
    //公告页公告()
    NOTICE_LIST:  `${BASE_API_URL}/notice-management/notice-list`,
    //公告详情()
    //routeParam:id
    NOTICE: `${BASE_API_URL}/notice-management/notices/`,


    //用户搜索()
    //param:keywords
    USER_SEARCH:`${BASE_API_URL}/user-management/users`,
    //用户详细信息()
    //routeParam:id
    USER: `${BASE_API_URL}/user-management/users/`,
    //已登录的用户自身详细信息()
    SELF: `${BASE_API_URL}/user-management/users/self`,


    //曲师详细信息()
    //routeParam:id
    ARTIST: `${BASE_API_URL}/artist-management/artists/`,


    //曲师的曲目()
    //param1:artistId
    //param2:startIndex
    //param3:endIndex
    SONGLIST:`${BASE_API_URL}/song-management/songlist`,
    //谱面详细信息()
    //routeParam:id
    SONG: `${BASE_API_URL}/song-management/songs/`,
    //服务器当前配置信息()
    SERVER_CONFIG :`${BASE_API_URL}/server-management/config/`,
}