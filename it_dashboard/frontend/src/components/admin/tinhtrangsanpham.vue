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
    <div class="col-xl-12 col-lg-12">
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
              >
                <template #header>
                  <div class="d-flex justify-content-end">
                    <InputText
                      v-model="filters['global'].value"
                      placeholder="Tìm kiếm"
                      class="p-inputtext-sm"
                    />
                  </div>
                </template>
                <Column
                  field="MAHH_GOC_1"
                  header="Mã sản phẩm"
                  sortable
                ></Column>
                <Column field="TENHH" header="Tên sản phẩm" sortable></Column>
                <Column field="MALO_GOC" header="Số lô" sortable></Column>
                <Column field="SOLUONG_NHAP" header="Số lượng" sortable>
                  <template #body="{ data }">
                    {{ formatNumber(data.SOLUONG_NHAP || 0, 0) }}
                    {{ data.DVT_NHAP }}
                  </template>
                </Column>
                <Column field="HOANTHANH" header="Trạng thái" sortable>
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
import BlockUI from "primevue/blockui";
// import IconField from 'primevue/iconfield';
// import InputIcon from 'primevue/inputicon';
import InputText from "primevue/inputtext";
import Api from "../../api/Api";
import { formatDate, formatNumber } from "../../utilities/util";
import { FilterMatchMode } from "primevue/api";
import moment from "moment";

const view = ref("nam");
const dates = ref([
  new moment().startOf("year").toDate(),
  new moment().endOf("year").toDate(),
]);
const waiting = ref();
const tinhtrangsanpham = ref([]);
const filters = ref({
  global: { value: null, matchMode: FilterMatchMode.CONTAINS },
});
const load_tinhtrangsanpham = () => {
  waiting.value = true;
  Api.tinhtrangsanpham({ dates: dates.value }).then((res) => {
    waiting.value = false;
    tinhtrangsanpham.value = res.data;
  });
};
onMounted(() => {
  load_tinhtrangsanpham();
});
</script>