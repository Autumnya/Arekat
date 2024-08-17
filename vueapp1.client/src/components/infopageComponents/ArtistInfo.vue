//曲师信息
<template>
    <div class="sub_container">
        <div id="artist_info_container" class="main_container">
            <img class="artist_avator" src="../../assets/default/test_song_cover.jpg" :alt="artistInfoJsonObject.artistName">
            <div class="artist_info">
                <h2>{{ artistInfoJsonObject.artistName }}</h2>
            </div>
        </div>
        <div id="artist_songlist_overcontainer" class="main_container">
            <h3>曲目总数:{{ artistInfoJsonObject.songAmount }}</h3>
            <div id="artist_songlist_container">
                <div class="song_item" v-for="(songitem,index) in artistSongsJsonObject.songs" :key="index" @click="gotoSongInfo(songitem.songId)">
                    <div class="song_info_main_container">
                        <div class="song_item_left_container">
                            <img class="song_cover" src="../../assets/default/test_song_cover.jpg" :alt="songitem.title">
                            <span>{{ songitem.title }}</span>
                        </div>
                        <span>谱面总数: {{ songitem.chartAmount }}</span>
                    </div>
                    <div v-if="index != artistInfoJsonObject.songAmount - 1" class="songlist_divide_line"></div>    
                </div>
            </div>
        </div>  
    </div>
</template>

<style scoped>
    #artist_info_container{
        background-color: rgb(240,240,240);
        display: flex;
    }
    #artist_songlist_overcontainer{
        padding-left: 0px;
        padding-right: 0px;
        margin-top: 24px;
        >h3{
            text-align: left;
            color: rgb(240,240,240);
            margin-top: 0px;
            margin-bottom: 18px;
            margin-left: 20px;
        }
    }
    #artist_songlist_container{
        width: 100%;
        background-color: white;
        padding: 12px 0px;
    }
    .artist_avator{
        width: 256px;
    }
    .artist_info{
        padding: 0px 20px;
    }
    .songlist_divide_line{
        width: 96%;
        height: 1px;
        background-color: gray;
        margin: 12px auto;
    }
    .song_item{
        padding: 0px 12px;
        cursor: pointer;
        justify-content: space-between;
    }
    .song_item:hover{
        text-decoration: underline;
    }
    .song_cover{
        width: 40px;
        height: 40px;
    }
    .song_info_main_container{
        display: flex;
        justify-content: space-between;
        align-items: center;
    }
    .song_item_left_container{
        display: flex;
        align-items: center;
        >span{
            margin-left: 12px;
            font-size: 20px;
        }
    }
</style>

<script setup>
    import router from '@/router';

    const gotoSongInfo = (aimSongId) =>{
        router.push({name:"song",params:{songId:aimSongId}});
    }

    var artistInfoJsonObject = {
        "artistName":"Camellia",
        "artistPicSrc":"../../assets/default/test_song_cover.jpg",
        "songAmount":28
    }
    //每次最多查30个
    var artistSongsJsonObject = {
        songs:[
            {
                "songId":"100001",
                "title":"testsong",
                "songCoverSrc":"../../assets/default/test_song_cover.jpg",
                "chartAmount":122,
            },
            {
                "songId":"100001",
                "title":"testsong",
                "songCoverSrc":"../../assets/default/test_song_cover.jpg",
                "chartAmount":122,
            },
            {
                "songId":"100001",
                "title":"testsong",
                "songCoverSrc":"../../assets/default/test_song_cover.jpg",
                "chartAmount":122,
            },
            {
                "songId":"100001",
                "title":"testsong",
                "songCoverSrc":"../../assets/default/test_song_cover.jpg",
                "chartAmount":122,
            },
            {
                "songId":"100001",
                "title":"testsong",
                "songCoverSrc":"../../assets/default/test_song_cover.jpg",
                "chartAmount":122,
            },
            {
                "songId":"100001",
                "title":"testsong",
                "songCoverSrc":"../../assets/default/test_song_cover.jpg",
                "chartAmount":122,
            },
            {
                "songId":"100001",
                "title":"testsong",
                "songCoverSrc":"../../assets/default/test_song_cover.jpg",
                "chartAmount":122,
            },
            {
                "songId":"100001",
                "title":"testsong",
                "songCoverSrc":"../../assets/default/test_song_cover.jpg",
                "chartAmount":122,
            },
            {
                "songId":"100001",
                "title":"testsong",
                "songCoverSrc":"../../assets/default/test_song_cover.jpg",
                "chartAmount":122,
            },
            {
                "songId":"100001",
                "title":"testsong",
                "songCoverSrc":"../../assets/default/test_song_cover.jpg",
                "chartAmount":122,
            },
            {
                "songId":"100001",
                "title":"testsong",
                "songCoverSrc":"../../assets/default/test_song_cover.jpg",
                "chartAmount":122,
            },
            {
                "songId":"100001",
                "title":"testsong",
                "songCoverSrc":"../../assets/default/test_song_cover.jpg",
                "chartAmount":122,
            },
            {
                "songId":"100001",
                "title":"testsong",
                "songCoverSrc":"../../assets/default/test_song_cover.jpg",
                "chartAmount":122,
            },
            {
                "songId":"100001",
                "title":"testsong",
                "songCoverSrc":"../../assets/default/test_song_cover.jpg",
                "chartAmount":122,
            },
            {
                "songId":"100001",
                "title":"testsong",
                "songCoverSrc":"../../assets/default/test_song_cover.jpg",
                "chartAmount":122,
            },
            {
                "songId":"100001",
                "title":"testsong",
                "songCoverSrc":"../../assets/default/test_song_cover.jpg",
                "chartAmount":122,
            },
            {
                "songId":"100001",
                "title":"testsong",
                "songCoverSrc":"../../assets/default/test_song_cover.jpg",
                "chartAmount":122,
            },
            {
                "songId":"100001",
                "title":"testsong",
                "songCoverSrc":"../../assets/default/test_song_cover.jpg",
                "chartAmount":122,
            },
            {
                "songId":"100001",
                "title":"testsong",
                "songCoverSrc":"../../assets/default/test_song_cover.jpg",
                "chartAmount":122,
            },
            {
                "songId":"100001",
                "title":"testsong",
                "songCoverSrc":"../../assets/default/test_song_cover.jpg",
                "chartAmount":122,
            },
            {
                "songId":"100001",
                "title":"testsong",
                "songCoverSrc":"../../assets/default/test_song_cover.jpg",
                "chartAmount":122,
            },
            {
                "songId":"100001",
                "title":"testsong",
                "songCoverSrc":"../../assets/default/test_song_cover.jpg",
                "chartAmount":122,
            },
            {
                "songId":"100001",
                "title":"testsong",
                "songCoverSrc":"../../assets/default/test_song_cover.jpg",
                "chartAmount":122,
            },
            {
                "songId":"100001",
                "title":"testsong",
                "songCoverSrc":"../../assets/default/test_song_cover.jpg",
                "chartAmount":122,
            },
            {
                "songId":"100001",
                "title":"testsong",
                "songCoverSrc":"../../assets/default/test_song_cover.jpg",
                "chartAmount":122,
            },
            {
                "songId":"100001",
                "title":"testsong",
                "songCoverSrc":"../../assets/default/test_song_cover.jpg",
                "chartAmount":122,
            },
            {
                "songId":"100001",
                "title":"testsong",
                "songCoverSrc":"../../assets/default/test_song_cover.jpg",
                "chartAmount":122,
            },
            {
                "songId":"100001",
                "title":"testsong",
                "songCoverSrc":"../../assets/default/test_song_cover.jpg",
                "chartAmount":122,
            },
        ]
    }
</script>