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
              <select
                class="form-control"
                style="width: 150px"
                v-model="qui"
                @change="load_bomchecklist"
              >
                <option value="1">Quí 1</option>
                <option value="2">Quí 2</option>
                <option value="3">Quí 3</option>
                <option value="4">Quí 4</option>
              </select>
              <select
                class="form-control"
                style="width: 150px"
                v-model="nam"
                @change="load_bomchecklist"
              >
                <option v-for="item of list_nam" :value="item" :key="item">
                  {{ item }}
                </option>
              </select>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <div class="row">
    <div class="col-xl-12 col-lg-12">
      <BlockUI :blocked="waiting">
        <div class="card">
          <div class="card-body">
            <div class="d-flex mt-0 mb-3">
              <div class="header-title d-flex align-items-center">
                Tình hình triển khai sản xuất
              </div>
            </div>
            <div class="">
              <DataTable
                :value="bomchecklist"
                paginator
                :rows="10"
                :rowsPerPageOptions="[10, 20, 50, 100]"
                :globalFilterFields="['masp_1', 'tensp']"
                v-model:filters="filters"
                filterDisplay="menu"
              >
                <template #header>
                  <div class="d-flex justify-content-end align-items-center">
                    <span class="mr-2">Tổng số: {{ bomchecklist.length }}</span>
                    <InputText
                      v-model="filters['global'].value"
                      placeholder="Tìm kiếm"
                      class="p-inputtext-sm"
                    />
                  </div>
                </template>
                <Column
                  field="masp_1"
                  header="Mã sản phẩm"
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
                  field="tensp"
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
                  field="dangbaoche"
                  header="Dạng bào chế"
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
                  field="dangdonggoi"
                  header="Dạng đóng gói"
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
                <Column field="hoanthanh" header="Hoàn thành" sortable>
                  <template #body="{ data }">
                    <span>{{ data.hoanthanh }}%</span>
                  </template>
                </Column>
                <Column
                  field="songay_sanxuat"
                  header="Số ngày sản xuất"
                  sortable
                >
                  <template #body="{ data }">
                    {{ data.songay_sanxuat }}
                  </template>
                </Column>
                <Column
                  field="songay_donggoi"
                  header="Số ngày đóng gói"
                  sortable
                >
                  <template #body="{ data }">
                    {{ data.songay_donggoi }}
                  </template>
                </Column>
                <Column field="songay_coa" header="COA FP(QC)" sortable>
                  <template #body="{ data }">
                    {{ data.songay_coa }}
                  </template>
                </Column>
                <Column
                  field="songay_qa"
                  header="Released certificate(QA)"
                  sortable
                >
                  <template #body="{ data }">
                    {{ data.songay_qa }}
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
import BlockUI from "primevue/blockui";
// import IconField from 'primevue/iconfield';
// import InputIcon from 'primevue/inputicon';
import InputText from "primevue/inputtext";
import Api from "../../api/Api";
import { formatDate, formatNumber } from "../../utilities/util";
import { FilterMatchMode } from "primevue/api";
import moment from "moment";

const list_nam = ref([2023, 2024, 2025, 2026, 2027]);
const view = ref("nam");
const qui = ref(2);
const nam = ref(2024);

const waiting = ref();
const bomchecklist = ref([]);
const filters = ref({
  global: { value: null, matchMode: FilterMatchMode.CONTAINS },
  masp_1: { value: null, matchMode: FilterMatchMode.CONTAINS },
  tensp: { value: null, matchMode: FilterMatchMode.CONTAINS },
  dangbaoche: { value: null, matchMode: FilterMatchMode.CONTAINS },
  dangdonggoi: { value: null, matchMode: FilterMatchMode.CONTAINS },
});
const load_bomchecklist = () => {
  waiting.value = true;
  Api.bomchecklist({ qui: qui.value, nam: nam.value }).then((res) => {
    waiting.value = false;
    bomchecklist.value = res.data;
  });
};
onMounted(() => {
  load_bomchecklist();
});
</script>