<template>
  <div class="row">
    <div class="col-xl-12 col-lg-12">
      <BlockUI :blocked="waiting">
        <div class="card">
          <div class="card-body">
            <div class="d-flex mt-0 mb-3">
              <div class="header-title d-flex align-items-center">Tồn kho</div>
              <div class="d-flex ml-auto align-items-center" style="width: 300px;">
                <Kho v-model="kho" @update:modelValue="load_tonkho" :disabled_00="false"></Kho>
              </div>
            </div>
            <div class="">
              <DataTable :value="tonkho" paginator :rows="10000" :rowsPerPageOptions="[10, 20, 50, 10000]"
                :globalFilterFields="['mahh', 'tenhh']" v-model:filters="filters" filterDisplay="menu"
                v-model:expandedRows="expandedRows">
                <template #header>

                  <div class="d-flex justify-content-end align-items-center">
                    <span class="mr-2">Tổng số: {{ tonkho.length }}</span>
                    <InputText v-model="filters['global'].value" placeholder="Tìm kiếm" class="p-inputtext-sm" />
                  </div>
                </template>

                <Column expander style="width: 1rem" />
                <Column field="mahh" header="Mã" sortable :showFilterMatchModes="false">
                  <template #filter="{ filterModel, filterCallback }">
                    <InputText v-model="filterModel.value" type="text" @keydown.enter="filterCallback()" />
                  </template>
                </Column>
                <Column field="tenhh" header="Tên" sortable :showFilterMatchModes="false">
                  <template #filter="{ filterModel, filterCallback }">
                    <InputText v-model="filterModel.value" type="text" @keydown.enter="filterCallback()" />
                  </template>
                </Column>
                <Column field="loaihh" header="Tên nhóm" sortable :showFilterMatchModes="false">
                  <template #filter="{ filterModel, filterCallback }">
                    <InputText v-model="filterModel.value" type="text" @keydown.enter="filterCallback()" />
                  </template>
                </Column>

                <Column field="soluong" header="Số lượng" sortable>
                  <template #body="{ data }">
                    {{ formatNumber(data.soluong, 0) }} {{ data.dvt }}
                  </template>
                </Column>

                <Column field="sothung" header="Số thùng" sortable>
                  <template #body="{ data }">
                    {{ formatNumber(data.sothung, 2) }} Thùng
                  </template>
                </Column>
                <Column field="dongia" header="Đơn giá" sortable>
                  <template #body="{ data }">
                    {{ formatPrice(data.dongia, 0) }} VNĐ
                  </template>
                </Column>
                <Column field="thanhtien" header="Thành tiền" sortable>
                  <template #body="{ data }">
                    {{ formatPrice(data.thanhtien, 0) }} VND
                  </template>
                </Column>

                <template #expansion="slotProps">
                  <div class="p-3">
                    <h5 class="d-flex">Chi tiết</h5>
                    <Tonchitiet :chitiet="slotProps.data.chitiet"></Tonchitiet>
                  </div>
                </template>

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
import { formatDate, formatNumber, formatPrice } from "../../utilities/util";
import { FilterMatchMode } from "primevue/api";
import { computed } from "vue";
import Tonchitiet from "./Tonchitiet.vue";
import Kho from "../TreeSelect/Kho.vue";

const kho = ref();
const expandedRows = ref([]);
const waiting = ref();
const tonkho = ref([]);
const sosanpham = computed(() => {
  let total = [];

  if (tonkho.value) {
    for (let ton of tonkho.value) {
      if (!total.includes(ton.mahh)) {
        total.push(ton.mahh);
      }
    }
  }

  return total.length;
});
const is_chitiet = ref(false);
const filters = ref({
  global: { value: null, matchMode: FilterMatchMode.CONTAINS },
  mahh: { value: null, matchMode: FilterMatchMode.CONTAINS },
  tenhh: { value: null, matchMode: FilterMatchMode.CONTAINS },
  manhom: { value: null, matchMode: FilterMatchMode.CONTAINS },
  tennhom: { value: null, matchMode: FilterMatchMode.CONTAINS },
});
const load_tonkho = () => {
  waiting.value = true;
  Api.tonkho({ is_chitiet: is_chitiet.value, kho: kho.value }).then((res) => {
    waiting.value = false;
    tonkho.value = res.data;
  });
};
const calculateTotal = (name) => {
  let total = 0;

  if (tonkho.value) {
    for (let ton of tonkho.value) {
      if (ton.mahh === name) {
        total += ton.soluong;
      }
    }
  }

  return total;
};
onMounted(() => {
  load_tonkho();
});
</script>