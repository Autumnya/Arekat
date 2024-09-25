//谱面上传页
<template>
    <div class = "sub_container">
        <div id="upload_container" class="main_container">
            <p>注意! 离开此界面或打开一个新的此页面将会导致页面过期, 上传失败, 请尽量保证在整个上传过程中保持该页面活动. </p>
            <h2>在这里上传你的作品</h2> 
            <h3>请填入songlist信息</h3>
            <div id="upload_form">
                <div class="upload_form_item">
                    <p>曲目id</p>
                    <input class="text_input" v-model="song_id" type="text">
                </div>
            </div>
            <div id="upload_form">
                <div class="upload_form_item">
                    <p>Bpm</p>
                    <input class="text_input" v-model="bpm" type="text">
                </div>
            </div>
            <div id="upload_form">
                <div class="upload_form_item">
                    <p>BpmBase</p>
                    <input class="text_input" v-model="bpm_base" type="text">
                </div>
            </div>
            <div id="upload_form">
                <div class="upload_form_item">
                    <p>PST难度定数</p>
                    <input class="text_input" v-model="rating_pst" style="max-width: 120px;" type="text">
                    <p>plus</p>
                    <div class = "rating_plus_checkbox" :style="(checkedRatingPlus[0] ? `background-color:white` : `background-color:${themeColor}`)" @click="checkRatingPlus(0)"></div>
                </div>
            </div>
            <div id="upload_form">
                <div class="upload_form_item">
                    <p>PRE难度定数</p>
                    <input class="text_input" v-model="rating_pre" style="max-width: 120px;" type="text">
                    <p>plus</p>
                    <div class = "rating_plus_checkbox" :style="(checkedRatingPlus[1] ? `background-color:white` : `background-color:${themeColor}`)" @click="checkRatingPlus(1)"></div>
                </div>
            </div>
            <div id="upload_form">
                <div class="upload_form_item">
                    <p>FTR难度定数</p>
                    <input class="text_input" v-model="rating_ftr" style="max-width: 120px;" type="text">
                    <p>plus</p>
                    <div class = "rating_plus_checkbox" :style="(checkedRatingPlus[2] ? `background-color:white` : `background-color:${themeColor}`)" @click="checkRatingPlus(2)"></div>
                </div>
            </div>
            <div id="upload_form">
                <div class="upload_form_item">
                    <p>BYD难度定数</p>
                    <input class="text_input" v-model="rating_byd" style="max-width: 120px;" type="text">
                    <p>plus</p>
                    <div class = "rating_plus_checkbox" :style="(checkedRatingPlus[3] ? `background-color:white` : `background-color:${themeColor}`)" @click="checkRatingPlus(3)"></div>
                </div>
            </div>
            <div id="upload_form">
                <div class="upload_form_item">
                    <p>试听起始时间(ms)</p>
                    <input class="text_input" v-model="audio_start" type="text">
                </div>
            </div>
            <div id="upload_form">
                <div class="upload_form_item">
                    <p>试听结束时间(ms)</p>
                    <input class="text_input" v-model="audio_end" type="text">
                </div>
            </div>
            <div id="upload_form">
                <div class="upload_form_item">
                    <p>光/暗/无色侧</p>
                    <input class="text_input" v-model="side" type="text">
                </div>
            </div>
            <div id="upload_form">
                <div class="upload_form_item">
                    <p>背景</p>
                    <input class="text_input" v-model="bg" type="text">
                </div>
            </div>
            <div id="upload_form">
                <div class="upload_form_item">
                    <p>版本号</p>
                    <input class="text_input" v-model="version" type="text">
                </div>
            </div>
            <h3>请填入上传必需文件</h3>
            <div id="upload_form">
                <div class="upload_form_item">
                    <p>曲目音源(base.ogg)</p>
                    <input class="file_input" type="file" @change="uploadOggFile">
                </div>
            </div>
            <div id="upload_form">
                <div class="upload_form_item">
                    <p>曲绘文件(base.jpg)</p>
                    <input class="file_input" type="file" @change="uploadJpgFile">
                </div>
            </div>
            <div id="upload_form">
                <div class="upload_form_item">
                    <p>曲绘文件_256(base.jpg)</p>
                    <input class="file_input" type="file" @change="uploadJpg256File">
                </div>
            </div>
            <div id="upload_form">
                <div class="upload_form_item">
                    <p>PST谱面文件(0.aff)</p>
                    <input class="file_input" type="file" @change="uploadAff0File">
                </div>
            </div>
            <div id="upload_form">
                <div class="upload_form_item">
                    <p>PRE谱面文件(1.aff)</p>
                    <input class="file_input" type="file" @change="uploadAff1File">
                </div>
            </div>
            <div id="upload_form">
                <div class="upload_form_item">
                    <p>FTR谱面文件(2.aff)</p>
                    <input class="file_input" type="file" @change="uploadAff2File">
                </div>
            </div>
            <div id="upload_form">
                <div class="upload_form_item">
                    <p>BYD谱面文件(3.aff)</p>
                    <input class="file_input" type="file" @change="uploadAff3File">
                </div>
            </div>
            <div id="upload_button" :style="`background-color:${themeColor}`" @click="uploadConfirm">
                <p>确认上传</p>
            </div>
        </div>
    </div>
</template>

<style scoped>
    #upload_container{
        text-align: left;
        background-color: rgb(240,240,240);
        position: relative;
        >p{
            margin-left: 20px;
        }
    }
    #upload_form{
        width: 100%;
    }
    #upload_button{
        cursor: pointer;
        width: 360px;
        height: 40px;
        border-radius: 8px;
        color: rgb(240,240,240);
        text-align: center;
        font-size: 18px;
        display: flex;
        margin: 16px auto;
        >p{
            margin: auto;
        }
    }
    #upload_button:hover{
        box-shadow: 1px 1px 4px black;
    }
    .upload_form_item{
        display: flex;
        margin: 10px;
        >p{
            font-size: 20px;
            margin: auto 8px;
        }
        >.text_input{
            font-size: 18px;
            border-radius: 10px;
            border: 1px solid;
            height: 36px;
            margin: auto 8px;
            width: 60%;
        }
        >.file_input{
            margin: auto 8px;
        }
    }
    .rating_plus_checkbox{
        border-radius: 6px;
        border: 3px solid white;
        width: 18px;
        height: 18px;
        margin: auto 8px;
        cursor: pointer;
        outline: 1px solid;
    }
</style>

<script setup>
    import { ref,computed } from 'vue';
    import axios from 'axios';
    import { url } from '@/api';
    import { useStore } from 'vuex';

    const store = useStore();
    const checkedRatingPlus = ref([false,false,false,false]);
    const guid = ref("");
    var token = localStorage.getItem("arekat_v1_token");
    const themeColor = computed(()=>store.state.themeColor);
    const song_id = ref("");
    const bpm = ref("");
    const bpm_base =ref("");
    const rating_pst = ref("");
    const rating_pre = ref("");
    const rating_ftr = ref("");
    const rating_byd = ref("");
    const audio_start = ref("");
    const audio_end = ref("");
    const side = ref("");
    const bg = ref("");
    const version =ref("");
    const userId = computed(()=>store.state.userId);

    axios.get(url.CHART_UPLOAD,{
        headers:{
            Authorization:`Bearer ${token}`
        },
    })
    .then(response=>{
        guid.value = response.data;
        console.log(response.data);
    })
    .catch(error=>
    {
        alert(error);
    })

    const checkRatingPlus = (id) =>{
        checkedRatingPlus.value[id] = !checkedRatingPlus.value[id];
    }

    const uploadOggFile = (event) => {
        const formData = new FormData();
        formData.append(`chartFileMusic`,event.target.files[0]);
        axios.post(url.CHART_UPLOAD_baseogg,formData,{
            headers:{
                'Authorization':`Bearer ${token}`,
                'Content-Type': 'multipart/form-data'
            },
            params:{
                guid:guid.value
            }
        })
        .then()
        .catch(error=>
        {
            alert(error);
        })
    }
    const uploadJpgFile = (event) => {
        const formData = new FormData();
        formData.append(`chartFileJpg`,event.target.files[0]);
        axios.post(url.CHART_UPLOAD_basejpg,formData,{
            headers:{
                'Authorization':`Bearer ${token}`,
                'Content-Type': 'multipart/form-data'
            },
            params:{
                guid:guid.value
            }
        })
        .then()
        .catch(error=>
        {
            alert(error);
        })
    }
    const uploadJpg256File = (event) => {
        const formData = new FormData();
        formData.append(`chartFileJpg256`,event.target.files[0]);
        axios.post(url.CHART_UPLOAD_base256jpg,formData,{
            headers:{
                'Authorization':`Bearer ${token}`,
                'Content-Type': 'multipart/form-data'
            },
            params:{
                guid:guid.value
            }
        })
        .then()
        .catch(error=>
        {
            alert(error);
        })
    }
    const uploadAff0File = (event) => {
        const formData = new FormData();
        formData.append(`chartFileAff0`,event.target.files[0]);
        axios.post(url.CHART_UPLOAD_0aff,formData,{
            headers:{
                'Authorization':`Bearer ${token}`,
                'Content-Type': 'multipart/form-data'
            },
            params:{
                guid:guid.value
            }
        })
        .then()
        .catch(error=>
        {
            alert(error);
        })
    }
    const uploadAff1File = (event) => {
        const formData = new FormData();
        formData.append(`chartFileAff1`,event.target.files[0]);
        axios.post(url.CHART_UPLOAD_1aff,formData,{
            headers:{
                'Authorization':`Bearer ${token}`,
                'Content-Type': 'multipart/form-data'
            },
            params:{
                guid:guid.value
            }
        })
        .then()
        .catch(error=>
        {
            alert(error);
        })
    }
    const uploadAff2File = (event) => {
        const formData = new FormData();
        formData.append(`chartFileAff2`,event.target.files[0]);
        axios.post(url.CHART_UPLOAD_2aff,formData,{
            headers:{
                'Authorization':`Bearer ${token}`,
                'Content-Type': 'multipart/form-data'
            },
            params:{
                guid:guid.value
            }
        })
        .then()
        .catch(error=>
        {
            alert(error);
        })
    }
    const uploadAff3File = (event) => {
        const formData = new FormData();
        formData.append(`chartFileAff3`,event.target.files[0]);
        axios.post(url.CHART_UPLOAD_3aff,formData,{
            headers:{
                'Authorization':`Bearer ${token}`,
                'Content-Type': 'multipart/form-data'
            },
            params:{
                guid:guid.value
            }
        })
        .then()
        .catch(error=>
        {
            alert(error);
        })
    }
    const uploadConfirm = () => {
        if(song_id.value == "")
        {
            alert("请输入曲目Id");
            return;
        }
        if(bpm.value == "")
        {
            alert("请输入Bpm");
            return;
        }
        if(audio_start.value == "")
        {
            alert("请输入AudioPreview");
            return;
        }
        if(audio_end.value == "")
        {
            alert("请输入AudioPreviewEnd");
            return;
        }
        if(side.value == "")
        {
            alert("请输入Side");
            return;
        }
        if(bg.value == "")
        {
            alert("请输入Bg");
            return;
        }
        var difficulties = [];
        if(rating_pst.value != "")
        {
            difficulties.push({
                "ratingClass":0,
                "chartDesigner":"designer",
                "jacketDesigner":"jacket",
                "rating":rating_pst.value,
                "ratingPlus":checkedRatingPlus.value[0]
            })
        }
        if(rating_pre.value != "")
        {
            difficulties.push({
                "ratingClass":1,
                "chartDesigner":"designer",
                "jacketDesigner":"jacket",
                "rating":rating_pst.value,
                "ratingPlus":checkedRatingPlus.value[1]
            })
        }
        if(rating_ftr.value != "")
        {
            difficulties.push({
                "ratingClass":2,
                "chartDesigner":"designer",
                "jacketDesigner":"jacket",
                "rating":rating_pst.value,
                "ratingPlus":checkedRatingPlus.value[2]
            })
        }
        if(rating_byd.value != "")
        {
            difficulties.push({
                "ratingClass":3,
                "chartDesigner":"designer",
                "jacketDesigner":"jacket",
                "rating":rating_pst.value,
                "ratingPlus":checkedRatingPlus.value[3]
            })
        }
        if(difficulties.length == 0)
        {
            alert("请输入至少一个难度");
            return;
        }
        console.log(userId.value);
        axios.post(url.CHART_UPLOAD_CONFIRM,{
            "songId":parseInt(song_id.value),
            "designerId":[parseInt(userId.value)],
            "bpm":bpm.value,
            "bpmBase":parseInt(bpm_base.value),
            "audioPreview":parseInt(audio_start.value),
            "audioPreviewEnd":parseInt(audio_end.value),
            "side":parseInt(side.value),
            "bg":bg.value,
            "version":version.value,
            "difficulties":difficulties
        },{
            headers:{
                'Authorization':`Bearer ${token}`,
            },
            params:{
                guid:guid.value
            }
        })
        .then(()=>{
            alert("Upload success!");
        })
        .catch(error => {
            console.log(error);
        })
    }
</script>