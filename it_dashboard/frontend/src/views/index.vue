<template>
  <TabView>
    <TabPanel header="BÁO CÁO BÁN HÀNG">
      <div class="row justify-content-center mb-3">
        <div class="col-lg-12">
          <div class="row">
            <!-- <div class="col-lg-2">
              <b>Bỏ qua Khách hàng</b>
              <KhachhangExceptionTreeSelect
                :multiple="true"
                v-model:modelValue="kh_exception"
                @close="refresh"
              ></KhachhangExceptionTreeSelect>
            </div> -->
            <div class="col-lg-2">
              <b>Tỉnh</b>
              <TinhTreeSelect
                :multiple="true"
                v-model:modelValue="tinhs"
                @close="refresh"
              ></TinhTreeSelect>
            </div>
            <div class="col-lg-2">
              <b>Mã nhóm</b>
              <ManhomTreeSelect
                :multiple="true"
                v-model:modelValue="nhoms"
                @close="refresh"
              >
              </ManhomTreeSelect>
            </div>
            <div class="col-lg-6">
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
                  <button
                    class="btn btn-primary"
                    @click="view = 'tungaydenngay'"
                  >
                    Từ ngày đến ngày
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
                  <button class="btn btn-primary" @click="view = 'nam'">
                    Năm
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="row justify-content-center">
        <div class="col-lg-4 col-xl-4">
          <BlockUI :blocked="list_waiting.HomeBadge">
            <div class="card">
              <div class="card-body">
                <span
                  class="font-weight-bold"
                  style="color: blue; font-size: 15px"
                  >DOANH THU</span
                >
                <h3 class="my-3">{{ formatNumber(thanhtien) }} VND</h3>
              </div>
              <!--end card-body-->
            </div>
            <!--end card-->
          </BlockUI>
        </div>
        <!--end col-->
        <div class="col-lg-4 col-xl-4">
          <BlockUI :blocked="list_waiting.HomeBadge">
            <div class="card">
              <div class="card-body">
                <span
                  class="font-weight-bold"
                  style="color: purple; font-size: 15px"
                  >SỐ LƯỢNG BÁN RA</span
                >
                <h3 class="my-3">{{ formatNumber(soluong) }} SP</h3>
              </div>
              <!--end card-body-->
            </div>
          </BlockUI>
          <!--end card-->
        </div>
        <!--end col-->
        <div class="col-lg-4 col-xl-4">
          <BlockUI :blocked="list_waiting.HomeBadge">
            <div class="card">
              <div class="card-body">
                <span
                  class="font-weight-bold"
                  style="color: green; font-size: 15px"
                  >SỐ LƯỢNG MÃ SẢN PHẨM BÁN RA</span
                >
                <h3 class="my-3">{{ formatNumber(soluongsp) }} SP</h3>
              </div>
              <!--end card-body-->
            </div>
          </BlockUI>
          <!--end card-->
        </div>
        <!--end col-->
      </div>
      <div class="row">
        <div class="col-xl-12">
          <draggable
            v-model="listsort"
            handle=".handle"
            :group="{ name: 'people', pull: 'clone', put: false }"
            ghost-class="ghost"
            item-key="id"
            class="row"
            @end="onEnd"
          >
            <template #item="{ element }">
              <template v-if="element.name == 'phanloaikh'">
                <div class="col-xl-3 col-lg-4">
                  <BlockUI :blocked="list_waiting.HomeBadge">
                    <div class="card">
                      <div class="card-body">
                        <div class="d-flex mt-0 mb-3">
                          <h4 class="header-title mt-0 mb-3">Khách hàng</h4>
                          <div class="ml-auto">
                            <div class="handle">
                              <i class="fas fa-arrows-alt"></i>
                            </div>
                          </div>
                        </div>
                        <div class="d-flex align-items-center chart1">
                          <Chart v-bind="chart1" />
                          <div
                            id="legend-container-chart1"
                            class="ml-3 w-100"
                          ></div>
                        </div>
                        <!--end /div-->
                      </div>
                      <!--end card-body-->
                    </div>
                  </BlockUI>
                  <!--end card-->
                </div>
              </template>
              <template v-else-if="element.name == 'phanloai'">
                <div class="col-xl-3 col-lg-4">
                  <BlockUI :blocked="list_waiting.HomeBadge">
                    <div class="card">
                      <div class="card-body">
                        <div class="d-flex mt-0 mb-3">
                          <h4 class="header-title mt-0 mb-3">
                            Phân loại khách hàng
                          </h4>
                          <div class="ml-auto">
                            <div class="handle">
                              <i class="fas fa-arrows-alt"></i>
                            </div>
                          </div>
                        </div>
                        <div class="">
                          <div class="d-flex align-items-center chart2">
                            <Chart v-bind="chart2" />
                            <div
                              id="legend-container-chart2"
                              class="ml-3 w-100"
                            ></div>
                          </div>
                        </div>
                        <!--end /div-->
                      </div>
                      <!--end card-body-->
                    </div>
                  </BlockUI>
                  <!--end card-->
                </div>
              </template>
              <template v-else-if="element.name == 'doanhthu'">
                <div class="col-xl-6 col-lg-8">
                  <BlockUI :blocked="list_waiting.doanhthu">
                    <div class="card">
                      <div class="card-body">
                        <div class="d-flex mt-0 mb-3">
                          <div class="header-title">Doanh thu</div>
                          <div
                            class="crypto-report-history d-flex justify-content-end ml-auto"
                          >
                            <ul class="nav" id="time">
                              <li class="nav-item">
                                <a
                                  class="nav-link"
                                  :class="timetype == 'Month' ? 'active' : ''"
                                  title="Month"
                                  href="#"
                                  @click="changeTimeType($event, 'Month')"
                                  >Month</a
                                >
                              </li>
                              <li class="nav-item">
                                <a
                                  class="nav-link"
                                  :class="timetype == 'Year' ? 'active' : ''"
                                  title="Year"
                                  href="#"
                                  @click="changeTimeType($event, 'Year')"
                                  >Year</a
                                >
                              </li>
                            </ul>
                            <div class="handle">
                              <i class="fas fa-arrows-alt"></i>
                            </div>
                          </div>
                        </div>

                        <div class="chart4" id="">
                          <Chart v-bind="chart4" />
                        </div>
                        <!--end /div-->
                      </div>
                      <!--end card-body-->
                    </div>
                  </BlockUI>

                  <!--end card-->
                </div>
              </template>

              <template v-else-if="element.name == 'topsp'">
                <div class="col-xl-3 col-lg-4">
                  <BlockUI :blocked="list_waiting.topsp">
                    <div class="card">
                      <div class="card-body">
                        <div class="d-flex mt-0 mb-3">
                          <div class="header-title d-flex align-items-center">
                            Top
                            <input
                              class="form-control mx-2"
                              style="width: 50px"
                              v-model="limit_topsp"
                              @change="load_topsp"
                            />sản phẩm
                          </div>
                          <div class="ml-auto">
                            <div class="handle">
                              <i class="fas fa-arrows-alt"></i>
                            </div>
                          </div>
                        </div>
                        <div class="">
                          <!-- <Chart type="bar" :data="chartData" :options="chartOptions" /> -->
                          <table class="table mb-0" id="table_user">
                            <thead class="thead-light">
                              <tr>
                                <th class="border-top-0">Sản phẩm</th>
                                <th class="border-top-0">Số lượng</th>
                                <th class="border-top-0">Doanh thu</th>
                              </tr>
                              <!--end tr-->
                            </thead>
                            <tbody>
                              <tr v-for="tr of topsp" :key="tr">
                                <td>
                                  <router-link :to="'chitiet?mahh=' + tr.mahh">
                                    {{ tr.tenhh }}
                                  </router-link>
                                </td>
                                <td>
                                  {{ formatNumber(tr.soluong) }} {{ tr.dvt }}
                                </td>
                                <td>{{ formatNumber(tr.doanhthu) }}</td>
                              </tr>
                            </tbody>
                          </table>
                          <!--end table-->
                        </div>
                        <!--end /div-->
                      </div>
                      <!--end card-body-->
                    </div>
                  </BlockUI>
                  <!--end card-->
                </div>
              </template>

              <template v-else-if="element.name == 'topnhom'">
                <div class="col-xl-3 col-lg-4">
                  <BlockUI :blocked="list_waiting.topnhom">
                    <div class="card">
                      <div class="card-body">
                        <div class="d-flex mt-0 mb-3">
                          <div class="header-title d-flex align-items-center">
                            Top
                            <input
                              class="form-control mx-2"
                              style="width: 50px"
                              v-model="limit_topnhom"
                              @change="load_topnhom"
                            />nhóm hàng
                          </div>
                          <div class="ml-auto">
                            <div class="handle">
                              <i class="fas fa-arrows-alt"></i>
                            </div>
                          </div>
                        </div>

                        <div class="">
                          <!-- <Chart type="bar" :data="chartData" :options="chartOptions" /> -->
                          <table class="table mb-0" id="table_user">
                            <thead class="thead-light">
                              <tr>
                                <th class="border-top-0">Nhóm</th>
                                <th class="border-top-0">Doanh thu</th>
                              </tr>
                              <!--end tr-->
                            </thead>
                            <tbody>
                              <tr v-for="tr of topnhom" :key="tr">
                                <td>
                                  <router-link :to="'chitiet?nhom=' + tr.nhom">
                                    {{ tr.tennhom }}
                                  </router-link>
                                </td>
                                <td>{{ formatNumber(tr.data) }}</td>
                              </tr>
                            </tbody>
                          </table>
                          <!--end table-->
                        </div>
                        <!--end /div-->
                      </div>
                      <!--end card-body-->
                    </div>
                  </BlockUI>
                  <!--end card-->
                </div>
              </template>

              <template v-else-if="element.name == 'phanloaidonvi'">
                <div class="col-xl-3 col-lg-4">
                  <BlockUI :blocked="list_waiting.HomeBadge">
                    <div class="card">
                      <div class="card-body">
                        <div class="d-flex mt-0 mb-3">
                          <h4 class="header-title mt-0 mb-3">
                            Doanh số theo đơn vị
                          </h4>
                          <div class="ml-auto">
                            <div class="handle">
                              <i class="fas fa-arrows-alt"></i>
                            </div>
                          </div>
                        </div>
                        <div class="d-flex align-items-center chart3">
                          <Chart v-bind="chart3" />
                          <div
                            id="legend-container-chart3"
                            class="ml-3 w-100"
                          ></div>
                        </div>
                        <!--end /div-->
                      </div>
                      <!--end card-body-->
                    </div>
                  </BlockUI>
                  <!--end card-->
                </div>
              </template>

              <template v-else-if="element.name == 'toptdv'">
                <div class="col-xl-3 col-lg-4">
                  <BlockUI :blocked="list_waiting.toptdv">
                    <div class="card">
                      <div class="card-body">
                        <div class="d-flex mt-0 mb-3">
                          <div class="header-title d-flex align-items-center">
                            Top
                            <input
                              class="form-control mx-2"
                              style="width: 50px"
                              v-model="limit_toptdv"
                              @change="load_toptdv"
                            />trình dược viên
                          </div>
                          <div class="ml-auto">
                            <div class="handle">
                              <i class="fas fa-arrows-alt"></i>
                            </div>
                          </div>
                        </div>
                        <div class="">
                          <!-- <Chart type="bar" :data="chartData" :options="chartOptions" /> -->
                          <table class="table mb-0" id="table_user">
                            <thead class="thead-light">
                              <tr>
                                <th class="border-top-0">Mã</th>
                                <th class="border-top-0">Tên</th>
                                <th class="border-top-0">Doanh thu</th>
                              </tr>
                              <!--end tr-->
                            </thead>
                            <tbody>
                              <tr v-for="tr of toptdv" :key="tr">
                                <td>
                                  {{ tr.matdv }}
                                </td>
                                <td>
                                  {{ tr.tentdv }}
                                </td>
                                <td>{{ formatNumber(tr.data) }}</td>
                              </tr>
                            </tbody>
                          </table>
                          <!--end table-->
                        </div>
                        <!--end /div-->
                      </div>
                      <!--end card-body-->
                    </div>
                  </BlockUI>
                  <!--end card-->
                </div>
              </template>

              <template v-else-if="element.name == 'toptinh'">
                <div class="col-xl-3 col-lg-4">
                  <BlockUI :blocked="list_waiting.toptinh">
                    <div class="card">
                      <div class="card-body">
                        <div class="d-flex mt-0 mb-3">
                          <div class="header-title d-flex align-items-center">
                            Top
                            <input
                              class="form-control mx-2"
                              style="width: 50px"
                              v-model="limit_toptinh"
                              @change="load_toptinh"
                            />tỉnh
                          </div>
                          <div class="ml-auto">
                            <div class="handle">
                              <i class="fas fa-arrows-alt"></i>
                            </div>
                          </div>
                        </div>
                        <div class="">
                          <!-- <Chart type="bar" :data="chartData" :options="chartOptions" /> -->
                          <table class="table mb-0" id="table_user">
                            <thead class="thead-light">
                              <tr>
                                <th class="border-top-0">Mã</th>
                                <th class="border-top-0">Tên</th>
                                <th class="border-top-0">Doanh thu</th>
                              </tr>
                              <!--end tr-->
                            </thead>
                            <tbody>
                              <tr v-for="tr of toptinh" :key="tr">
                                <td>
                                  <router-link
                                    :to="'chitiet?tinh=' + tr.matinh"
                                  >
                                    {{ tr.matinh }}
                                  </router-link>
                                </td>
                                <td>
                                  {{ tr.tentinh }}
                                </td>
                                <td>{{ formatNumber(tr.data) }}</td>
                              </tr>
                            </tbody>
                          </table>
                          <!--end table-->
                        </div>
                        <!--end /div-->
                      </div>
                      <!--end card-body-->
                    </div>
                  </BlockUI>
                  <!--end card-->
                </div>
              </template>

              <template v-else-if="element.name == 'topkh'">
                <div class="col-xl-3 col-lg-6">
                  <BlockUI :blocked="list_waiting.topkh">
                    <div class="card">
                      <div class="card-body">
                        <div class="d-flex mt-0 mb-3">
                          <div class="header-title d-flex align-items-center">
                            Top
                            <input
                              class="form-control mx-2"
                              style="width: 50px"
                              v-model="limit_topkh"
                              @change="load_topkh"
                            />khách hàng
                          </div>
                          <div class="ml-auto">
                            <div class="handle">
                              <i class="fas fa-arrows-alt"></i>
                            </div>
                          </div>
                        </div>
                        <div class="">
                          <!-- <Chart type="bar" :data="chartData" :options="chartOptions" /> -->
                          <table class="table mb-0" id="table_user">
                            <thead class="thead-light">
                              <tr>
                                <th class="border-top-0">Mã</th>
                                <th class="border-top-0">Tên</th>
                                <th class="border-top-0">Doanh thu</th>
                              </tr>
                              <!--end tr-->
                            </thead>
                            <tbody>
                              <tr v-for="tr of topkh" :key="tr">
                                <td>
                                  <router-link :to="'chitiet?makh=' + tr.makh">
                                    {{ tr.makh }}
                                  </router-link>
                                </td>
                                <td>
                                  {{ tr.donvi }}
                                </td>
                                <td>{{ formatNumber(tr.data) }}</td>
                              </tr>
                            </tbody>
                          </table>
                          <!--end table-->
                        </div>
                        <!--end /div-->
                      </div>
                      <!--end card-body-->
                    </div>
                  </BlockUI>
                  <!--end card-->
                </div>
              </template>
              <template v-else-if="element.name == 'tonkho'">
                <div class="col-xl-6 col-lg-6">
                  <BlockUI :blocked="list_waiting.tonkho">
                    <div class="card">
                      <div class="card-body">
                        <div class="d-flex mt-0 mb-3">
                          <div class="header-title d-flex align-items-center">
                            Top
                            <input
                              class="form-control mx-2"
                              style="width: 50px"
                              v-model="limit_toptonkho"
                              @change="load_tonkho"
                            />
                            tồn kho
                          </div>
                          <div class="ml-auto">
                            <div class="handle">
                              <i class="fas fa-arrows-alt"></i>
                            </div>
                          </div>
                        </div>
                        <div class="">
                          <!-- <Chart type="bar" :data="chartData" :options="chartOptions" /> -->
                          <table class="table mb-0" id="table_user">
                            <thead class="thead-light">
                              <tr>
                                <th class="border-top-0">Mã</th>
                                <th class="border-top-0">Tên</th>
                                <th class="border-top-0">Số lượng</th>
                              </tr>
                              <!--end tr-->
                            </thead>
                            <tbody>
                              <tr v-for="tr of tonkho" :key="tr">
                                <td>
                                  {{ tr.mahh }}
                                </td>
                                <td>
                                  {{ tr.tenhh }}
                                </td>
                                <td>
                                  {{ formatNumber(tr.soluong_ton) }}
                                  {{ tr.dvt }}
                                </td>
                              </tr>
                            </tbody>
                          </table>
                          <!--end table-->
                        </div>
                        <!--end /div-->
                      </div>
                      <!--end card-body-->
                    </div>
                  </BlockUI>
                  <!--end card-->
                </div>
              </template>
            </template>
          </draggable>
        </div>
      </div>
    </TabPanel>
    <TabPanel header="TỒN KHO">
      <Tonkho></Tonkho>
    </TabPanel>
    <TabPanel header="TÌNH TRẠNG SẢN PHẨM">
      <Tinhtrangsanpham></Tinhtrangsanpham>
    </TabPanel>
    <TabPanel header="BOM CHECKLIST">
      <Bomchecklist></Bomchecklist>
    </TabPanel>
    <TabPanel header="NHÂN SỰ">
      <Nhansu></Nhansu>
    </TabPanel>
  </TabView>

  <Loading :waiting="waiting"></Loading>
</template>
<script setup>
import TabView from "primevue/tabview";
import TabPanel from "primevue/tabpanel";
import draggable from "vuedraggable";
import { onMounted, ref, watch } from "vue";
import TinhTreeSelect from "../components/TreeSelect/TinhTreeSelect.vue";
import ManhomTreeSelect from "../components/TreeSelect/ManhomTreeSelect.vue";
import Chart from "primevue/chart";
import Api from "../api/Api";
import Loading from "../components/Loading.vue";
import Calendar from "primevue/calendar";
import { formatNumber } from "../utilities/util";
import { htmlLegendPlugin } from "../service/legend";
import BlockUI from "primevue/blockui";
import Tonkho from "../components/admin/tonkho.vue";
import Tinhtrangsanpham from "../components/admin/tinhtrangsanpham.vue";
import Bomchecklist from "../components/admin/bomchecklist.vue";
import Nhansu from "../components/admin/nhansu.vue";

const listsort = ref([
  {
    name: "phanloaikh",
  },
  {
    name: "phanloai",
  },
  {
    name: "phanloaidonvi",
  },
  {
    name: "toptinh",
  },
  {
    name: "doanhthu",
  },
  {
    name: "topsp",
  },
  {
    name: "topnhom",
  },
  {
    name: "toptdv",
  },
  {
    name: "topkh",
  },
  // {
  //   name: "tonkho"
  // }
]);
const thanhtien = ref(0);
const soluong = ref(0);
const soluongsp = ref(0);
const limit_topsp = ref(5);
const limit_topnhom = ref(5);
const limit_toptdv = ref(5);
const limit_toptinh = ref(5);
const limit_topkh = ref(5);
const limit_toptonkho = ref(1000005);

const timetype = ref("Month");
const waiting = ref(false);
const list_waiting = ref({});
const reinit = (e) => {
  console.log(e);
};
//type="pie" :data="chartData1" :options="chartOptions" :plugins="plugins" :width="width":height="height"
const chartOptions = ref();
const chartData1 = ref({
  labels: [],
  datasets: [],
});
const chartData2 = ref({
  labels: [],
  datasets: [],
});

const chartData3 = ref({
  labels: [],
  datasets: [],
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
        containerID: "legend-container-chart1",
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
        containerID: "legend-container-chart2",
      },
    },
  },
  plugins: [htmlLegendPlugin],
  height: 300,
});

const chart3 = ref({
  type: "pie",
  options: {
    responsive: false,
    plugins: {
      legend: {
        display: false,
      },
      htmlLegend: {
        // ID of the container to put the legend in
        containerID: "legend-container-chart3",
      },
    },
  },
  plugins: [htmlLegendPlugin],
  height: 300,
});
const chart4 = ref({
  type: "line",
  options: {
    plugins: {
      legend: {
        display: false,
      },
    },
    scales: {
      x: {
        offset: true,
      },
    },
  },
  height: 300,
});
const topsp = ref([]);
const topnhom = ref([]);
const toptdv = ref([]);
const toptinh = ref([]);
const topkh = ref([]);
const tonkho = ref([]);

const kh_exception = ref();
const tinhs = ref();
const nhoms = ref();
const currentDate = new Date();
const dates = ref([new Date(currentDate.getFullYear(), 0, 1), currentDate]);
const nam = ref(currentDate.getFullYear());
const list_nam = ref([2020, 2021, 2022, 2023, 2024, 2025, 2026, 2027]);
// const nam = ref(2022);
// const dates = ref([new Date(2022, 0, 1), new Date(2022, 11, 31)]);

const view = ref("tungaydenngay");
const maxDate = ref(new Date());
const changeTimeType = (e, type) => {
  e.preventDefault();
  timetype.value = type;
  load_doanhthu();
};
const changeNam = () => {
  dates.value = [new Date(nam.value, 0, 1), new Date(nam.value, 11, 31)];
  refresh();
};
const refresh = () => {
  list_waiting.value.HomeBadge = true;
  Api.HomeBadge(
    kh_exception.value,
    tinhs.value,
    nhoms.value,
    dates.value[0],
    dates.value[1]
  ).then((res) => {
    list_waiting.value.HomeBadge = false;
    thanhtien.value = res.thanhtien;
    soluong.value = res.soluong;
    soluongsp.value = res.soluongsp;
    chart1.value.data = res.phanloaikh;
    chart2.value.data = res.phanloai;
    chart3.value.data = res.phanloaidonvi;
  });
  load_topsp();
  load_topnhom();
  load_toptdv();
  load_toptinh();
  load_topkh();

  load_doanhthu();
  // load_tonkho()
};
const saveDates = (e) => {
  localStorage.setItem("date", JSON.stringify(dates.value));
};
const onEnd = (e) => {
  // console.log(e);
  localStorage.setItem("sort", JSON.stringify(listsort.value));
};
const load_topsp = () => {
  list_waiting.value.topsp = true;
  Api.topsp(
    kh_exception.value,
    tinhs.value,
    nhoms.value,
    dates.value[0],
    dates.value[1],
    limit_topsp.value
  ).then((res) => {
    list_waiting.value.topsp = false;
    topsp.value = res.data;
  });
};
const load_topnhom = () => {
  list_waiting.value.topnhom = true;
  Api.topnhom(
    kh_exception.value,
    tinhs.value,
    nhoms.value,
    dates.value[0],
    dates.value[1],
    limit_topnhom.value
  ).then((res) => {
    list_waiting.value.topnhom = false;
    topnhom.value = res.data;
  });
};

const load_toptdv = () => {
  list_waiting.value.toptdv = true;
  Api.toptdv(
    kh_exception.value,
    tinhs.value,
    nhoms.value,
    dates.value[0],
    dates.value[1],
    limit_toptdv.value
  ).then((res) => {
    list_waiting.value.toptdv = false;
    toptdv.value = res.data;
  });
};
const load_toptinh = () => {
  list_waiting.value.toptinh = true;
  Api.toptinh(
    kh_exception.value,
    tinhs.value,
    nhoms.value,
    dates.value[0],
    dates.value[1],
    limit_toptinh.value
  ).then((res) => {
    list_waiting.value.toptinh = false;
    toptinh.value = res.data;
  });
};
const load_topkh = () => {
  list_waiting.value.topkh = true;
  Api.topkh(
    kh_exception.value,
    tinhs.value,
    nhoms.value,
    dates.value[0],
    dates.value[1],
    limit_topkh.value
  ).then((res) => {
    list_waiting.value.topkh = false;
    topkh.value = res.data;
  });
};

const load_doanhthu = () => {
  list_waiting.value.doanhthu = true;
  Api.doanhthu(
    kh_exception.value,
    tinhs.value,
    nhoms.value,
    dates.value[0],
    dates.value[1],
    timetype.value
  ).then((res) => {
    list_waiting.value.doanhthu = false;
    chart4.value.data = res.doanhthu;
  });
};
const load_tonkho = () => {
  list_waiting.value.tonkho = true;
  Api.tonkho(limit_toptonkho.value).then((res) => {
    list_waiting.value.tonkho = false;
    tonkho.value = res.data;
  });
};
onMounted(() => {
  const cacheSort = localStorage.getItem("sort") || "[]";
  let data = JSON.parse(cacheSort);
  if (data.length > 0) {
    listsort.value = data;
  }
  let date = localStorage.getItem("date") || "[]";
  date = JSON.parse(date);
  if (date.length > 0) {
    dates.value = [new Date(date[0]), new Date(date[1])];
    nam.value = new Date(date[0]).getFullYear();
  }
  // console.log(dates.value);
  refresh();

  chart1.value.width = ($(".chart1").width() * 65) / 100;
  chart1.value.height = chart1.value.width;

  chart2.value.width = ($(".chart2").width() * 65) / 100;
  chart2.value.height = chart2.value.width;

  chart3.value.width = ($(".chart3").width() * 65) / 100;
  chart3.value.height = chart3.value.width;

  chart4.value.width = $(".chart4").width();
});
watch(dates, (count, prevCount) => {
  saveDates();
});
</script>
<style>
.handle {
  cursor: move;
}

.p-tabview-panels {
  background: transparent !important;
  padding: 10px 0px !important;
}
</style>
