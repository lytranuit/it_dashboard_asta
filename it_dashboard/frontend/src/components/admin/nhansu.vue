<template>
  <div class="row justify-content-center">
    <div class="col-xl-12 col-lg-12">
      <BlockUI :blocked="waiting">
        <div class="row justify-content-center">
          <div class="col-md-6 col-lg-3">
            <div class="card report-card bg-secondary-gradient shadow-secondary">
              <div class="card-body">
                <div class="float-right">
                  <i class="fas fa-user report-main-icon bg-icon-secondary"></i>
                </div>
                <span class="badge badge-light text-secondary">Tổng số nhân sự</span>
                <h3 class="my-3">{{ tong }}</h3>
              </div>
            </div>
          </div>
          <div class="col-md-6 col-lg-3">
            <div class="card report-card bg-success-gradient shadow-success">
              <div class="card-body">
                <div class="float-right">
                  <i class="dripicons-checkmark report-main-icon bg-icon-success"></i>
                </div>
                <span class="badge badge-light text-success">Chính thức</span>
                <h3 class="my-3">{{ chinhthuc }}</h3>
              </div>
            </div>
          </div>
          <div class="col-md-6 col-lg-3">
            <div class="card report-card bg-purple-gradient shadow-purple">
              <div class="card-body">
                <div class="float-right">
                  <i class="dripicons-wallet report-main-icon bg-icon-purple"></i>
                </div>
                <span class="badge badge-light text-purple">Học việc/thử việc</span>
                <h3 class="my-3">{{ hocthuviec }}</h3>
              </div>
            </div>
          </div>
          <div class="col-md-6 col-lg-3">
            <div class="card report-card bg-warning-gradient shadow-warning">
              <div class="card-body">
                <div class="float-right">
                  <i class="fas fa-spinner report-main-icon bg-icon-warning"></i>
                </div>
                <span class="badge badge-light text-warning">Hợp đồng khác</span>
                <h3 class="my-3">{{ dichvu }}</h3>
              </div>
            </div>
          </div>
        </div>
      </BlockUI>
      <!--end card-->
    </div>
    <div class="col-xl-6 col-lg-4">
      <BlockUI :blocked="waiting">
        <div class="card">
          <div class="card-body">
            <div class="d-flex mt-0 mb-3">
              <h4 class="header-title mt-0 mb-3">Tỉ lệ lương</h4>
            </div>
            <div class="d-flex align-items-center chart11">
              <Chart v-bind="chart1" />
              <div id="legend-container-chart11" class="ml-3 w-100"></div>
            </div>
            <!--end /div-->
          </div>
          <!--end card-body-->
        </div>
      </BlockUI>
    </div>
    <div class="col-xl-6 col-lg-4">
      <BlockUI :blocked="waiting">
        <div class="card">
          <div class="card-body">
            <div class="d-flex mt-0 mb-3">
              <h4 class="header-title mt-0 mb-3">Tỉ lệ nhân sự</h4>
            </div>
            <div class="d-flex align-items-center chart22">
              <Chart v-bind="chart2" />
              <div id="legend-container-chart22" class="ml-3 w-100"></div>
            </div>
            <!--end /div-->
          </div>
          <!--end card-body-->
        </div>
      </BlockUI>
      <!--end card-->
    </div>

    <div class="col-xl-6 col-lg-12">
      <BlockUI :blocked="list_waiting.waiting3">
        <div class="card">
          <div class="card-body">
            <div class="d-flex mt-0 mb-3">
              <div class="header-title">Biến động</div>
              <div class="crypto-report-history d-flex justify-content-end ml-auto">
                <Calendar v-model="date" view="year" dateFormat="yy" @update:modelValue="biendong" />
              </div>
            </div>

            <div class="chart33" id="chart33">
              <Chart v-bind="chart" />
            </div>
            <!--end /div-->
          </div>
          <!--end card-body-->
        </div>
      </BlockUI>

      <!--end card-->
    </div>
    <div class="col-xl-6 col-lg-12">
      <BlockUI :blocked="list_waiting.waiting4">
        <div class="card">
          <div class="card-body">
            <div class="row">
              <div class="col-md mb-2 text-left">
                <h4 class="header-title mb-0 mr-3">Tỉ lệ nghĩ việc</h4>
              </div>
              <div class="col-md mb-2 text-center">
                <Calendar v-model="dates" selectionMode="range" :manualInput="false" inputClass="form-control-sm"
                  @hide="changeDate" dateFormat="mm/yy" view="month" />
              </div>
              <div class="col-md mb-2 text-right">
                <SelectButton v-model="type_nghiviec" :options="['Month', 'Year']" aria-labelledby="basic"
                  :pt="{ button: 'form-control-sm' }" @change="changeDate" />
              </div>
            </div>
            <div class="chart44" id="chart44">
              <Chart v-bind="chart4" />
            </div>
            <!--end /div-->
          </div>
          <!--end card-body-->
        </div>
      </BlockUI>

      <!--end card-->
    </div>
  </div>
</template>

<script setup>
import { onMounted, ref } from "vue";
import BlockUI from "primevue/blockui";
import Api from "../../api/Api";
import nhansuApi from "../../api/nhansuApi";
import Calendar from "primevue/calendar";
import moment from "moment";
import Chart from "primevue/chart";
import { formatNumber } from "../../utilities/util";

import { htmlLegendPlugin } from "../../service/legend";
import SelectButton from "primevue/selectbutton";
const waiting = ref();
const date = ref();
const tong = ref();
const chinhthuc = ref();
const hocthuviec = ref();
const dichvu = ref();
const department = ref();
const list_waiting = ref({});

const dates = ref([moment().startOf("year").toDate(), new Date()]);
const type_nghiviec = ref("Month");
const chart = ref({
  type: "line",
  options: {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        display: "top",
      },
    },
    scales: {
      A: {
        type: "linear",
        position: "left",
        min: 0,
      },
      B: {
        type: "linear",
        position: "right",
        min: 0,
      },
    },
    height: 300,
  },
});

const chart1 = ref({
  type: "pie",
  options: {
    responsive: false,
    plugins: {
      legend: {
        display: false,
      },
      htmlLegend: {
        // ID of the container to put the legend in
        containerID: "legend-container-chart11",
      },
      tooltip: {
        callbacks: {
          label: function (context) {
            console.log(context);
            var allData = context.dataset.data;
            var tooltipLabel = context.dataset.label;
            var tooltipData = context.parsed;
            var total = 0;
            for (var i in allData) {
              total += allData[i];
            }
            var tooltipPercentage = Math.round((tooltipData / total) * 100);
            return (
              tooltipLabel +
              ": " +
              formatNumber(tooltipData, 0) +
              " (" +
              tooltipPercentage +
              "%)"
            );
          },
        },
      },
    },
  },
  plugins: [htmlLegendPlugin],
  height: 300,
});

const chart2 = ref({
  type: "pie",
  options: {
    responsive: false,
    plugins: {
      legend: {
        display: false,
      },
      htmlLegend: {
        // ID of the container to put the legend in
        containerID: "legend-container-chart22",
      },
      tooltip: {
        callbacks: {
          label: function (context) {
            console.log(context);
            var allData = context.dataset.data;
            var tooltipLabel = context.dataset.label;
            var tooltipData = context.parsed;
            var total = 0;
            for (var i in allData) {
              total += allData[i];
            }
            var tooltipPercentage = Math.round((tooltipData / total) * 100);
            return (
              tooltipLabel +
              ": " +
              formatNumber(tooltipData, 0) +
              " (" +
              tooltipPercentage +
              "%)"
            );
          },
        },
      },
    },
  },
  plugins: [htmlLegendPlugin],
  height: 300,
});
const chart4 = ref({
  type: "line",
  options: {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        display: "top",
      },
    },
    scales: {
      A: {
        type: "linear",
        position: "left",
        // min: 0,
      },
      B: {
        type: "linear",
        position: "right",
        min: 0,
      },
    },
    height: 300,
  },
});
const load = () => {
  waiting.value = true;
  nhansuApi.get().then((res) => {
    waiting.value = false;
    tong.value = res.tong;
    chinhthuc.value = res.chinhthuc;
    hocthuviec.value = res.hocthuviec;
    dichvu.value = res.dichvu;
    chart1.value.data = res.tileluong;
    chart2.value.data = res.tilenhansu;
  });
};
const biendong = () => {
  list_waiting.value.waiting3 = true;
  nhansuApi
    .biendong({ nam: moment(date.value).year(), department: department.value })
    .then((res) => {
      list_waiting.value.waiting3 = false;
      chart.value.data = res.data;
      setTimeout(() => {
        $("#chart33 canvas").height(300);
      }, 100);
    });
};
const load_nghiviec = () => {
  list_waiting.value.waiting4 = true;
  nhansuApi
    .nghiviec({ date_from: dates.value[0], date_to: dates.value[1], timetype: type_nghiviec.value })
    .then((res) => {
      list_waiting.value.waiting4 = false;
      chart4.value.data = res.data;
      setTimeout(() => {
        $("#chart44 canvas").height(300);
      }, 100);
    });
};
const changeDate = () => {
  load_nghiviec();
};

onMounted(() => {
  date.value = moment().toDate();
  load();
  biendong();
  load_nghiviec();
});
</script>