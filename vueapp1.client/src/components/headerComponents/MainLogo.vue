//网站的logo
<template>
    <div id = "main_logo" class = "header_component" @click="gotoIndexpage()">
        <span class = "text_logo" >
            Arekat
        </span>
    </div>
</template>

<style scoped>
    @font-face {
        font-family: "Fino-Title-Bold";
        src: url("../../../src/assets/fonts/fino-title-bold.ttf");
    }

    #main_logo{
        width: 15%;
        height: 48px;
        cursor: pointer;
    }
    .text_logo{
        font-family: Fino-Title-Bold;
        font-size: 48px;
        color: white;
        margin: 0px auto;
        padding: 0px;
        user-select: none;
    }
</style>

<script setup>
    import { useRouter } from 'vue-router';

    const router = useRouter();

    const gotoIndexpage = () =>{
        router.push({name:"index"});
    }
</script>