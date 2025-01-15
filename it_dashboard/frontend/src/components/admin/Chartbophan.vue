<template>
  <div class="col-lg-4">
    <div class="card">
      <div class="card-body">
        <div class="row">
          <div class="col-md mb-2">
            <h4 class="header-title mb-0 mr-3">Bộ phận</h4>
          </div>
          <div class="col-md mb-2" style="text-align: end">
            <Calendar
              v-model="dates"
              selectionMode="range"
              :manualInput="false"
              inputClass="form-control-sm"
              @hide="changeDate"
              dateFormat="yy/mm/dd"
            />
          </div>
        </div>
        <div class="">
          <Chart v-bind="chart1" />
        </div>
        <!--end /div-->
      </div>
      <!--end card-body-->
    </div>
    <!--end card-->
  </div>
</template>
<script setup>
import { onMounted, ref } from "vue";
import Chart from "primevue/chart";
import moment from "moment";
import Calendar from "primevue/calendar";
import BuyApi from "../../api/BuyApi";

const chart1 = ref({
  type: "bar",
  options: {
    indexAxis: "y",
    maintainAspectRatio: false,
    aspectRatio: 0.6,
    plugins: {
      legend: {
        position: false,
      },
    },
    scales: {
      x: {
        ticks: {
          color: "#334155",
        },
        grid: {
          color: "#e2e8f0",
        },
      },
      y: {
        ticks: {
          color: "#334155",
        },
        grid: {
          color: "#e2e8f0",
        },
      },
    },
  },
  height: 300,
  class: "h-30rem",
});
const dates = ref([moment().subtract(3, "months").toDate(), new Date()]);
const changeDate = () => {
  load();
};

const load = () => {
  BuyApi.chiphibophan(dates.value[0], dates.value[1]).then((res) => {
    chart1.value.data = res.data;
  });
};
onMounted(() => {
  load();
});
</script>