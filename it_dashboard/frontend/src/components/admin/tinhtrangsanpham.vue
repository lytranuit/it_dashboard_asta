<template>
  <div class="row justify-content-center mb-3">
    <div class="col-lg-3 align-self-center">
      <!-- <h2 class="mb-0">BÁO CÁO BÁN HÀNG</h2> -->
    </div>
    <div class="col-lg-9">
      <div class="row">
        <div class="col-lg-6"></div>
        <div class="col-lg-6">
          <b>Thời gian</b>
          <div>
            <div class="d-flex align-items-center" style="gap: 10px">
              <Calendar
                v-model="dates"
                selectionMode="range"
                class="p-inputtext-sm w-100"
                :manualInput="true"
                dateFormat="dd/mm/yy"
                @hide="load_tinhtrangsanpham"
              />
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <div class="row">
    <div class="col-xl-3">
      <BlockUI :blocked="waiting">
        <div class="card">
          <div class="card-body">
            <span class="font-weight-bold" style="color: blue; font-size: 15px"
              >Tổng số sản phẩm</span
            >
            <h3 class="my-3">{{ sosanpham }}</h3>
          </div>
          <!--end card-body-->
        </div>
        <div class="card">
          <div class="card-body">
            <span class="font-weight-bold" style="color: green; font-size: 15px"
              >Sản phẩm hoàn thành</span
            >
            <h3 class="my-3">{{ sosanphamht }}</h3>
          </div>
          <!--end card-body-->
        </div>
        <div class="card">
          <div class="card-body">
            <span class="font-weight-bold" style="color: red; font-size: 15px"
              >Sản phẩm chưa hoàn thành</span
            >
            <h3 class="my-3">{{ sosanphamcht }}</h3>
          </div>
          <!--end card-body-->
        </div>
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
              <div class="ml-auto"></div>
            </div>
            <div class="">
              <!-- <Chart type="bar" :data="chartData" :options="chartOptions" /> -->
              <table class="table mb-0" id="table_user">
                <thead class="thead-light">
                  <tr>
                    <th class="border-top-0">Sản phẩm</th>
                    <th class="border-top-0 text-right" style="width: 120px">
                      Số lượng
                    </th>
                  </tr>
                  <!--end tr-->
                </thead>
                <tbody>
                  <tr v-for="tr of topsp" :key="tr">
                    <td>{{ tr.MAHH_GOC_1 }}-{{ tr.TENHH }}</td>
                    <td class="text-right">
                      {{ formatNumber(tr.SOLUONG_NHAP) }} {{ tr.DVT_NHAP }}
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
    </div>
    <div class="col-xl-9 col-lg-12">
      <BlockUI :blocked="waiting">
        <div class="card">
          <div class="card-body">
            <div class="d-flex mt-0 mb-3">
              <div class="header-title d-flex align-items-center">
                Tình trạng sản phẩm
              </div>
            </div>
            <div class="">
              <DataTable
                :value="tinhtrangsanpham"
                paginator
                :rows="10"
                :rowsPerPageOptions="[10, 20, 50, 100]"
                :globalFilterFields="['MAHH_GOC_1', 'TENHH']"
                v-model:filters="filters"
                filterDisplay="menu"
                @filter="tong"
              >
                <template #header>
                  <div class="d-flex justify-content-end align-items-center">
                    <span class="mr-2"
                      >Tổng số: {{ tinhtrangsanpham.length }}</span
                    >
                    <InputText
                      v-model="search"
                      placeholder="Tìm kiếm"
                      class="p-inputtext-sm"
                      @change="load_tinhtrangsanpham()"
                    />
                  </div>
                </template>
                <Column
                  field="MAHH_GOC_1"
                  header="Mã sản phẩm"
                  footer="Tổng"
                  sortable
                  :showFilterMatchModes="false"
                >
                  <template #filter="{ filterModel, filterCallback }">
                    <InputText
                      v-model="filterModel.value"
                      type="text"
                      @keydown.enter="filterCallback()"
                    /> </template
                ></Column>
                <Column
                  field="TENHH"
                  header="Tên sản phẩm"
                  sortable
                  :showFilterMatchModes="false"
                >
                  <template #filter="{ filterModel, filterCallback }">
                    <InputText
                      v-model="filterModel.value"
                      type="text"
                      @keydown.enter="filterCallback()"
                    /> </template
                ></Column>
                <Column
                  field="MALO_GOC"
                  header="Số lô"
                  sortable
                  :showFilterMatchModes="false"
                >
                  <template #filter="{ filterModel, filterCallback }">
                    <InputText
                      v-model="filterModel.value"
                      type="text"
                      @keydown.enter="filterCallback()"
                    /> </template
                ></Column>
                <Column
                  field="COLO_GOC"
                  header="Cỡ lô"
                  :footer="formatNumber(tong_colo || 0, 0)"
                  sortable
                  :showFilterMatchModes="false"
                >
                  <template #body="{ data }">
                    {{ formatNumber(data.COLO_GOC || 0, 0) }}
                  </template>
                  <template #filter="{ filterModel, filterCallback }">
                    <InputText
                      v-model="filterModel.value"
                      type="text"
                      @keydown.enter="filterCallback()"
                    /> </template
                ></Column>
                <Column
                  field="SOLUONG_NHAP"
                  header="Số lượng"
                  sortable
                  class="text-right"
                >
                  <template #body="{ data }">
                    {{ formatNumber(data.SOLUONG_NHAP || 0, 0) }}
                    {{ data.DVT_NHAP }}
                  </template>
                </Column>
                <Column
                  field="HOANTHANH"
                  header="Trạng thái"
                  sortable
                  :showFilterMatchModes="false"
                >
                  <template #body="{ data }">
                    <template v-if="data.HOANTHANH == 1">
                      <i class="far fa-check-circle text-success"></i> Hoàn
                      thành
                    </template>
                    <template v-else>
                      <i class="far fa-times-circle text-danger"></i> Chưa hoàn
                      thành
                    </template>
                  </template>
                  <template #filter="{ filterModel, filterCallback }">
                    <div style="width: 300px">
                      <SelectButton
                        v-model="filterModel.value"
                        :options="[
                          { name: 'Hoàn thành', value: true },
                          { name: 'Chưa hoàn thành', value: false },
                        ]"
                        optionLabel="name"
                        optionValue="value"
                        @change="filterCallback()"
                        aria-labelledby="basic"
                      />
                    </div>
                  </template>
                </Column>
                <Column field="NGAYDAUTIEN" header="Ngày bắt đầu" sortable>
                  <template #body="{ data }">
                    {{ formatDate(data.NGAYDAUTIEN) }}
                  </template>
                </Column>
                <Column field="NGAYHOANTHANH" header="Ngày hoàn thành" sortable>
                  <template #body="{ data }">
                    {{ formatDate(data.NGAYHOANTHANH) }}
                  </template>
                </Column>
                <Column field="SONGAY" header="Số ngày" sortable>
                  <template #body="{ data }">
                    {{ data.SONGAY }}
                  </template>
                </Column>
              </DataTable>
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
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import SelectButton from "primevue/selectbutton";
import BlockUI from "primevue/blockui";
// import IconField from 'primevue/iconfield';
// import InputIcon from 'primevue/inputicon';
import InputText from "primevue/inputtext";
import Api from "../../api/Api";
import { formatDate, formatNumber } from "../../utilities/util";
import { FilterMatchMode } from "primevue/api";
import moment from "moment";
import { computed } from "vue";

const view = ref("nam");
const dates = ref([
  new moment().startOf("year").toDate(),
  new moment().endOf("year").toDate(),
]);
const limit_topsp = ref(5);
const waiting = ref();
const tinhtrangsanpham = ref([]);
const tong_colo = ref();
const search = ref();
const filters = ref({
  global: { value: null, matchMode: FilterMatchMode.CONTAINS },
  MAHH_GOC_1: { value: null, matchMode: FilterMatchMode.CONTAINS },
  TENHH: { value: null, matchMode: FilterMatchMode.CONTAINS },
  MALO_GOC: { value: null, matchMode: FilterMatchMode.CONTAINS },
  COLO_GOC: { value: null, matchMode: FilterMatchMode.CONTAINS },
  HOANTHANH: { value: null, matchMode: FilterMatchMode.CONTAINS },
});
const sosanpham = computed(() => {
  // Đếm số lượng giá trị duy nhất của MAHH_GOC_1
  const uniqueCount = tinhtrangsanpham.value.length;
  return uniqueCount;
});
const sosanphamht = computed(() => {
  // Đếm số lượng giá trị duy nhất của MAHH_GOC_1
  const uniqueCount = tinhtrangsanpham.value.filter(
    (item) => item.HOANTHANH == 1
  ).length;
  return uniqueCount;
});
const sosanphamcht = computed(() => {
  // Đếm số lượng giá trị duy nhất của MAHH_GOC_1
  const uniqueCount = tinhtrangsanpham.value.filter(
    (item) => item.HOANTHANH != 1
  ).length;
  return uniqueCount;
});
const topsp = computed(() => {
  const groupedData = tinhtrangsanpham.value.reduce((acc, item) => {
    const key = `${item.MAHH_GOC_1}-${item.TENHH}`;

    if (!acc[key]) {
      acc[key] = {
        MAHH_GOC_1: item.MAHH_GOC_1,
        TENHH: item.TENHH,
        SOLUONG_NHAP: 0,
        DVT_NHAP: item.DVT_NHAP,
      };
    }

    acc[key].SOLUONG_NHAP += item.SOLUONG_NHAP ?? 0;

    return acc;
  }, {});

  // Chuyển object thành mảng kết quả
  const result = Object.values(groupedData)
    .sort((a, b) => b.SOLUONG_NHAP - a.SOLUONG_NHAP) // Sắp xếp giảm dần theo TOTAL_SONGAY
    .slice(0, limit_topsp.value); // Lấy 5 sản phẩm có tổng số ngày cao nhất

  return result;
});
const load_tinhtrangsanpham = () => {
  waiting.value = true;
  Api.tinhtrangsanpham({ dates: dates.value, search: search.value }).then(
    (res) => {
      waiting.value = false;
      // tong_colo.value = res.tong_colo;
      tinhtrangsanpham.value = res.data.map((item) => {
        item.HOANTHANH = item.HOANTHANH ? true : false;
        return item;
      });
    }
  );
};
const tong = (da) => {
  var filteredValue = da.filteredValue;
  filteredValue = filteredValue.reduce((acc, item) => {
    const key = `${item.MAHH_GOC_1}-${item.COLO_GOC}`;

    if (!acc[key]) {
      acc[key] = {
        MAHH_GOC_1: item.MAHH_GOC_1,
        COLO_GOC: item.COLO_GOC,
      };
    }
    return acc;
  }, {});
  console.log(filteredValue);
  filteredValue = Object.values(filteredValue);
  tong_colo.value = filteredValue.reduce((a, b) => a + b.COLO_GOC, 0);
  // console.log(tong_colo.value);
};
onMounted(() => {
  load_tinhtrangsanpham();
});
</script>