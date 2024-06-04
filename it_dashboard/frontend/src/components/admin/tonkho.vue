<template>
    <div class="row">
        <div class="col-xl-12 col-lg-12">
            <BlockUI :blocked="waiting">
                <div class="card">
                    <div class="card-body">
                        <div class="d-flex mt-0 mb-3">
                            <div class="header-title d-flex align-items-center">Tồn kho</div>

                        </div>
                        <div class="">
                            <DataTable :value="tonkho" paginator :rows="10" :rowsPerPageOptions="[10, 20, 50, 100]"
                                :globalFilterFields="['mahh', 'tenhh']" v-model:filters="filters" filterDisplay="menu">
                                <template #header>
                                    <div class="d-flex justify-content-end">
                                        <InputText v-model="filters['global'].value" placeholder="Tìm kiếm"
                                            class="p-inputtext-sm" />
                                    </div>
                                </template>
                                <Column field="mahh" header="Mã" sortable></Column>
                                <Column field="tenhh" header="Tên" sortable></Column>
                                <Column field="soluong_ton" header="Số lượng" sortable>
                                    <template #body="{ data }">
                                        {{ formatNumber(data.soluong_ton, 0) }} {{ data.dvt }}
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
import { onMounted, ref } from 'vue';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import BlockUI from 'primevue/blockui';
// import IconField from 'primevue/iconfield';
// import InputIcon from 'primevue/inputicon';
import InputText from 'primevue/inputtext';
import Api from '../../api/Api';
import { formatNumber } from "../../utilities/util"
import { FilterMatchMode } from 'primevue/api';

const waiting = ref();
const tonkho = ref([]);
const filters = ref({
    global: { value: null, matchMode: FilterMatchMode.CONTAINS },
});
const load_tonkho = () => {
    waiting.value = true;
    Api.tonkho(1000000000).then((res) => {
        waiting.value = false;
        tonkho.value = res.data;
    });
}
onMounted(() => {
    load_tonkho()
})
</script>