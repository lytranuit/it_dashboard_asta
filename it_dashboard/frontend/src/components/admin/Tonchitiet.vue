<template>
  <div>
    <DataTable :value="chitiet" responsiveLayout="scroll" :resizableColumns="true" columnResizeMode="expand">
      <Column field="malo" header="Số lô" sortable>
        <template #body="slotProps">
          {{ slotProps.data.malo }}
        </template>
      </Column>
      <Column field="handung" header="Hạn dùng" sortable>
        <template #body="slotProps">
          {{ formatDate(slotProps.data.handung) }}
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
    </DataTable>
  </div>
</template>
<script setup>
import Column from "primevue/column";
import DataTable from "primevue/datatable";
import Tag from "primevue/tag";
import Button from "primevue/button";
import { computed, onMounted, ref } from "vue";
import { formatDate, formatNumber, formatPrice } from "../../utilities/util";
import { storeToRefs } from "pinia";
import moment from "moment";
import { useConfirm } from "primevue/useconfirm";
import { useToast } from "primevue/usetoast";
const props = defineProps({
  chitiet: { type: Object },
});
const toast = useToast();
const confirm = useConfirm();
const first = ref(0);
const rows = ref(10000);
const draw = ref(0);
onMounted(() => {
});
</script>
