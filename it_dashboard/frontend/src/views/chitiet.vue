<template>
  <div class="row justify-content-center mb-3">
    <div class="col-lg-2 align-self-center">
      <h2 class="mb-0">CHI TIẾT</h2>
    </div>
    <div class="col-lg-10">
      <div class="row">
        <div class="col-lg-2">
          <b>Sản phẩm</b>
          <SanphamTreeSelect
            :multiple="true"
            v-model:modelValue="sanphams"
            @update:modelValue="refresh"
          >
          </SanphamTreeSelect>
        </div>
        <div class="col-lg-2">
          <b>Khách hàng</b>
          <KhachhangTreeSelect
            :multiple="true"
            v-model:modelValue="khachhangs"
            @update:modelValue="refresh"
          >
          </KhachhangTreeSelect>
        </div>
        <div class="col-lg-2">
          <b>Tỉnh</b>
          <TinhTreeSelect
            :multiple="true"
            v-model:modelValue="tinhs"
            @update:modelValue="refresh"
          ></TinhTreeSelect>
        </div>
        <div class="col-lg-2">
          <b>Mã nhóm</b>
          <ManhomTreeSelect
            :multiple="true"
            v-model:modelValue="nhoms"
            @update:modelValue="refresh"
          ></ManhomTreeSelect>
        </div>
        <div class="col-lg-4">
          <b>Thời gian</b>
          <div>
            <div
              class="d-flex align-items-center"
              style="gap: 10px"
              v-if="view == 'nam'"
            >
              <select
                class="form-control"
                style="width: 150px"
                v-model="nam"
                @change="changeNam"
              >
                <option v-for="item of list_nam" :value="item" :key="item">
                  {{ item }}
                </option>
              </select>
              <button class="btn btn-primary" @click="view = 'tungaydenngay'">
                Thời gian
              </button>
            </div>
            <div
              class="d-flex align-items-center"
              style="gap: 10px"
              v-if="view == 'tungaydenngay'"
            >
              <Calendar
                v-model="dates"
                selectionMode="range"
                class="p-inputtext-sm w-100"
                :manualInput="true"
                dateFormat="dd/mm/yy"
                @hide="refresh"
                :maxDate="maxDate"
              />
              <button class="btn btn-primary" @click="view = 'nam'">Năm</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <div class="row">
    <div class="col-xl-12">
      <BlockUI :blocked="list_waiting.chitiet">
        <div class="card">
          <div class="card-body">
            <!-- <Chart type="bar" :data="chartData" :options="chartOptions" /> -->
            <table class="table mb-0" id="table_user">
              <thead class="thead-light">
                <tr>
                  <th class="border-top-0">Sản phẩm</th>
                  <th class="border-top-0">Khách hàng</th>
                  <th class="border-top-0">Số lượng</th>
                  <th class="border-top-0">DVT</th>
                  <th class="border-top-0">Doanh thu</th>
                </tr>
                <!--end tr-->
              </thead>
              <tbody>
                <tr v-for="tr of chitiet" :key="tr.tenhh">
                  <td>
                    {{ tr.tenhh }}
                  </td>
                  <td>
                    {{ tr.donvi }}
                  </td>
                  <td>{{ formatNumber(tr.soluong) }}</td>
                  <td>
                    {{ tr.dvt }}
                  </td>
                  <td>{{ formatNumber(tr.doanhthu) }}</td>
                </tr>
              </tbody>
            </table>
            <!--end table-->
            <!--end /div-->
          </div>
          <!--end card-body-->
        </div>
      </BlockUI>
      <!--end card-->
    </div>
  </div>
  <Loading :waiting="waiting"></Loading>
</template>
<script setup>
import { onMounted, ref } from "vue";
import SanphamTreeSelect from "../components/TreeSelect/SanphamTreeSelect.vue";
import TinhTreeSelect from "../components/TreeSelect/TinhTreeSelect.vue";
import ManhomTreeSelect from "../components/TreeSelect/ManhomTreeSelect.vue";
import Chart from "primevue/chart";
import Api from "../api/Api";
import Loading from "../components/Loading.vue";
import Calendar from "primevue/calendar";
import { formatNumber } from "../utilities/util";
import KhachhangTreeSelect from "../components/TreeSelect/KhachhangTreeSelect.vue";
import BlockUI from "primevue/blockui";

import { useRoute } from "vue-router";
const route = useRoute();

const waiting = ref();
const tinhs = ref();
const nhoms = ref();
const sanphams = ref();
const khachhangs = ref();
const currentDate = new Date();
const dates = ref([new Date(currentDate.getFullYear(), 0, 1), currentDate]);
const nam = ref(currentDate.getFullYear());
const list_nam = ref([2020, 2021, 2022, 2023, 2024, 2025, 2026, 2027]);
// const nam = ref(2022);
// const dates = ref([new Date(2022, 0, 1), new Date(2022, 11, 31)]);

const view = ref("tungaydenngay");
const maxDate = ref(new Date());
const list_waiting = ref({});
const chitiet = ref([]);
const changeNam = () => {
  dates.value = [new Date(nam.value, 0, 1), new Date(nam.value, 11, 31)];
  refresh();
};

const load_chitiet = () => {
  list_waiting.value.chitiet = true;
  Api.chitiet(
    sanphams.value,
    khachhangs.value,
    tinhs.value,
    nhoms.value,
    dates.value[0],
    dates.value[1]
  ).then((res) => {
    list_waiting.value.chitiet = false;
    chitiet.value = res.data;
  });
};
const refresh = () => {
  load_chitiet();
};
onMounted(() => {
  // console.log(route);
  if (route.query.mahh) {
    sanphams.value = [route.query.mahh];
  }

  if (route.query.makh) {
    khachhangs.value = [route.query.makh];
  }
  if (route.query.tinh) {
    tinhs.value = [route.query.tinh];
  }
  if (route.query.nhom) {
    nhoms.value = [route.query.nhom];
  }

  let date = localStorage.getItem("date") || "[]";
  date = JSON.parse(date);
  if (date.length > 0) {
    dates.value = [new Date(date[0]), new Date(date[1])];
    nam.value = new Date(date[0]).getFullYear();
  }
  refresh();
});
</script>