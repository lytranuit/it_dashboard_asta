<template>
  <div class="row justify-content-center">
    <ChartChiphi></ChartChiphi>
    <Chartbophan></Chartbophan>
    <ChartChiphiBophanTheo></ChartChiphiBophanTheo>
  </div>
  <Loading :waiting="waiting"></Loading>
</template>
  <script setup>
import { onMounted, ref } from "vue";
import Loading from "../../components/Loading.vue";
import { useAuth } from "../../stores/auth";
import { useRouter } from "vue-router";
import Chartbophan from "./Chartbophan.vue";
import ChartChiphi from "./ChartChiphi.vue";
import ChartChiphiBophanTheo from "./ChartChiphiBophanTheo.vue";
const store = useAuth();
const waiting = ref(false);

const router = useRouter();

// const now = new Date();
onMounted(() => {});
</script>