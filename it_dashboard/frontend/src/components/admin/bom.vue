<template>
  <div class="row clearfix">
    <Toast />
    <div class="col-12">
      <div class="d-flex card-header drag-handle">
        <div class="ml-auto">
          Lấy dữ liệu mua hàng:
          <Calendar
            v-model="dates"
            dateFormat="dd/mm/yy"
            placeholder="từ ngày - đến ngày"
            mask="99/99/9999"
            selectionMode="range"
            :hideOnRangeSelection="true"
            @hide="loadLazyData"
          />
        </div>
      </div>
      <section class="card card-fluid">
        <div class="card-body" style="overflow: auto; position: relative">
          <DataTable
            class="p-datatable-customers"
            showGridlines
            :value="datatable"
            :lazy="lazy"
            ref="dt"
            scrollHeight="70vh"
            :paginator="true"
            :rowsPerPageOptions="[10, 50, 100]"
            :rows="rows"
            :totalRecords="totalRecords"
            @page="onPage($event)"
            :rowHover="true"
            :loading="loading"
            responsiveLayout="scroll"
            :resizableColumns="true"
            columnResizeMode="expand"
            v-model:filters="filters"
            filterDisplay="menu"
            v-model:expandedRows="expandedRows"
          >
            <!--<template #header>
              <div class="row">
                <div class="col-md md:text-left text-center md:mb-0 mb-2">
                  <div style="width: 200px">
                    <treeselect
                      :options="columns"
                      v-model="showing"
                      multiple
                      :limit="0"
                      :limitText="(count) => 'Hiển thị: ' + count + ' cột'"
                      :normalizer="normalizer"
                    >
                    </treeselect>
                  </div>
                </div>
                <div class="col-md d-flex justify-content-end">
                   <div style="width: 300px" class="mr-2">
                    <Materials2
                      v-model="nvl"
                      @update:modelValue="loadLazyData"
                    ></Materials2>
                  </div>
                  <div style="width: 300px">
                    <KhuvucTreeSelect
                      v-model="mapl"
                      @update:modelValue="loadLazyData"
                    ></KhuvucTreeSelect>
                  </div> 
                </div>
              </div>
            </template>-->

            <template #empty> Không có dữ liệu. </template>

            <Column expander style="width: 5rem" />
            <Column
              v-for="col of columns"
              :field="col.data"
              :header="col.label"
              :key="col.data"
              :class="col.data"
              :showFilterMatchModes="false"
            >
              <template #body="slotProps">
                <template v-if="col.data == 'colo'">
                  <div>
                    {{ formatPrice(slotProps.data[col.data]) }}
                    {{ slotProps.data.dvt }}
                  </div>
                </template>
                <template v-else-if="col.data == 'status'">
                  <div v-if="slotProps.data[col.data] == 1">
                    <i class="fas fa-square" style="color: #dc2626"></i> Thiếu
                    tồn kho
                  </div>
                  <div v-else-if="slotProps.data[col.data] == 2">
                    <i class="fas fa-square" style="color: #ea580c"></i> Thiếu
                    hồ sơ
                  </div>
                  <div v-else-if="slotProps.data[col.data] == 3">
                    <i class="fas fa-square" style="color: #16a34a"></i> Đã sẵn
                    sàng
                  </div>
                </template>
                <div v-html="slotProps.data[col.data]" v-else></div>
              </template>

              <template
                #filter="{ filterModel, filterCallback }"
                v-if="col.filter == true"
              >
                <template v-if="col.data == 'status'">
                  <select
                    class="form-control"
                    v-model="filterModel.value"
                    @change="filterCallback()"
                  >
                    <option value="1">Thiếu tồn kho</option>
                    <option value="2">Thiếu hồ sơ</option>
                    <option value="3">Đã sẵn sàng</option>
                  </select>
                </template>
                <InputText
                  type="text"
                  v-model="filterModel.value"
                  @keydown.enter="filterCallback()"
                  class="p-column-filter"
                  v-else
                />
              </template>
            </Column>
            <template #expansion="slotProps">
              <div class="p-3">
                <h5 class="d-flex">
                  <span>Danh sách NVL</span>
                </h5>
                <TableChitiet :details="slotProps.data.details"></TableChitiet>
              </div>
            </template>
          </DataTable>
        </div>
      </section>
    </div>

    <Loading :waiting="waiting"></Loading>
  </div>
</template>

<script setup>
import { onMounted, ref, watch, computed } from "vue";
import Loading from "../../components/Loading.vue";
// import the component
import InputGroup from "primevue/inputgroup";
import DataTable from "primevue/datatable";
import { FilterMatchMode } from "primevue/api";
import Column from "primevue/column";
import Button from "primevue/button";
import Calendar from "primevue/calendar";
import InputText from "primevue/inputtext";

import Toast from "primevue/toast";
import { useToast } from "primevue/usetoast";
import BomApi from "../../api/BomApi";
import { useAuth } from "../../stores/auth";
import { useBom } from "../../stores/Bom";
import { storeToRefs } from "pinia";
import { formatDate, formatPrice } from "../../utilities/util";
import { useConfirm } from "primevue/useconfirm";
import moment from "moment";
import TableChitiet from "../../components/bom/TableChitiet.vue";
// import KhuvucTreeSelect from "../../components/TreeSelect/KhuvucTreeSelect.vue";
import { useRoute, useRouter } from "vue-router";
// import Materials2 from "../../components/TreeSelect/Materials2.vue";

const lazy = computed(() => {
  return filters.value.status?.value == null;
});
const toast = useToast();
const confirm = useConfirm();
const router = useRouter();
const store = useAuth();
const dates = ref([moment().add(-3, "M").toDate(), moment().toDate()]);
////Datatable
const nvl = ref();
const mapl = ref();
const datatable = ref();
const columns = ref([
  {
    label: "Mã thương mại",
    data: "mahh",
    className: "text-center",
    filter: true,
  },
  {
    label: "Tên thương mại",
    data: "tenhh",
    className: "text-center",
    filter: true,
  },
  {
    label: "Mã gốc",
    data: "mahh_goc",
    className: "text-center",
    filter: true,
  },
  {
    label: "Tên gốc",
    data: "tenhh_goc",
    className: "text-center",
    filter: true,
  },

  {
    label: "ĐVT",
    data: "dvt",
    className: "text-center",
    filter: true,
  },
  {
    label: "Cỡ lô",
    data: "colo",
    className: "text-center",
    filter: true,
  },
  {
    label: "Qui cách",
    data: "quicach",
    className: "text-center",
  },
  {
    label: "Trạng thái",
    data: "status",
    className: "text-center",
    filter: true,
  },
]);
const filters = ref({});
const totalRecords = ref(0);
const loading = ref(true);
const showing = ref([]);
const column_cache = "columns_Bom"; ////
const first = ref(0);
const rows = ref(10);
const draw = ref(0);
const selectedColumns = computed(() => {
  return columns.value.filter((col) => showing.value.includes(col.data));
});
const expandedRows = ref([]);
const normalizer = (node) => {
  return {
    id: node.data,
    label: node.label,
  };
};
const lazyParams = computed(() => {
  let data_filters = {
    mapl: mapl.value,
    nvl: nvl.value,
    dates: dates.value,
  };
  for (let key in filters.value) {
    data_filters[key] = filters.value[key].value;
  }
  return {
    draw: draw.value,
    start: first.value,
    length: rows.value,
    filters: data_filters,
  };
});
const dt = ref(null);
const excel = () => {
  loading.value = true;
  BomApi.excel(lazyParams.value).then((res) => {
    // console.log(res);
    window.open(res.link, "_blank");

    loading.value = false;
  });
};
const tinh_bom = () => {
  router.push("/bom/tinhbom");
};
////Form
const store_Bom = useBom();
const { model, headerForm, visibleDialog } = storeToRefs(store_Bom);
///Control
const waiting = ref(false);

////Data table
const loadLazyData = () => {
  loading.value = true;
  BomApi.table(lazyParams.value).then((res) => {
    // console.log(res);
    if (!res.data) store.logout();
    datatable.value = res.data;
    totalRecords.value = res.recordsFiltered;
    loading.value = false;
  });
};
const onPage = (event) => {
  // console.log(lazy.value);
  if (lazy.value == false) return;
  first.value = event.first;
  rows.value = event.rows;
  draw.value = draw.value + 1;
  loadLazyData();
};

///Form
const add = () => {
  store_Bom.reset();
  router.push("/bom/add");
};

const editRow = (m) => {
  window.open("/bom/chitiet?mahh=" + m.mahh + "&colo=" + m.colo, "_blank");
};
const copyRow = (m) => {
  window.open("/bom/copy?mahh=" + m.mahh + "&colo=" + m.colo, "_blank");
};

const confirmDelete = (m) => {
  confirm.require({
    message: "Bạn có muốn xóa row này?",
    header: "Xác nhận",
    icon: "pi pi-exclamation-triangle",
    accept: () => {
      BomApi.remove({ mahh: m.mahh, colo: m.colo }).then((res) => {
        waiting.value = false;
        if (res.success) {
          toast.add({
            severity: "success",
            summary: "Thành công",
            detail: "Đã xóa thành công!",
            life: 3000,
          });
        } else {
          toast.add({
            severity: "error",
            summary: "Lỗi",
            detail: res.message,
            life: 3000,
          });
        }
        loadLazyData();
      });
    },
  });
};
////Core
onMounted(() => {
  let cache = localStorage.getItem(column_cache);
  if (!cache) {
    showing.value = columns.value.map((item) => {
      return item.data;
    });
  } else {
    showing.value = JSON.parse(cache);
  }
  for (var col of columns.value) {
    if (!col.filter) continue;
    filters.value[col.data] = {
      value: null,
      matchMode: FilterMatchMode.CONTAINS,
    };
  }
  loadLazyData();
});
watch(showing, async (newa, old) => {
  localStorage.setItem(column_cache, JSON.stringify(newa));
});
watch(filters, async (newa, old) => {
  first.value = 0;
  loadLazyData();
});
</script>
<style>
.tenhoatchat {
  min-width: 500px;
}
.vue-treeselect__placeholder,
.vue-treeselect__single-value {
  font-weight: normal;
}
</style>