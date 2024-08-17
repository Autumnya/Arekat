//全局组件Header
<template>
    <div id="header_container" class="clear_float">
        <MainLogo />
        <NavList />
        <SearchBox />
        <LanguageSwitch />
        <SimpleUserInfo />
    </div>
</template>

<style scoped>
    #header_container{
        height: 66px;
        padding: 0px;
        margin: 0px;
        border: 0px;
        justify-content: space-between;
        align-items: center;
        background-color: rgb(126, 44, 220);
        box-shadow: 0px 0px 5px gray;
        display: flex;
        min-width: 1024px;
        position: relative;
    }
</style>

<script setup>
    import MainLogo from '../headerComponents/MainLogo.vue';
    import NavList from '../headerComponents/NavList.vue';
    import SearchBox from '../headerComponents/SearchBox.vue';
    import LanguageSwitch from '../headerComponents/LanguageSwitch.vue'
    import SimpleUserInfo from '../headerComponents/SimpleUserInfo.vue';
</script>