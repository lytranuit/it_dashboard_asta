<template>
  <TreeSelect :options="list_kho" v-model="value" :multiple="multiple" :normalizer="normalizer"
    value-consists-of="LEAF_PRIORITY" :limit="0" :limitText="(count) => 'Đã chọn: ' + count + ' mã kho'" />
</template>
<script setup>
import { storeToRefs } from "pinia";
import { onMounted, computed } from "vue";
import { useAuth } from "../../stores/auth";
import { useGeneral } from "../../stores/general";
const store = useGeneral();
const store_auth = useAuth();
const { kho } = storeToRefs(store);
const { user } = storeToRefs(store_auth);
const props = defineProps({
  value: Array,
  multiple: { type: Boolean, default: true },
  only_access: { type: Boolean, default: false },
  disabled_00: { type: Boolean, default: true },
});
const emit = defineEmits(["update:value"]);
const value = computed({
  get() {
    return props.value;
  },
  set(value) {
    emit("update:value", value);
  },
});
const list_kho = computed(() => {
  return kho.value.filter((item) => {
    let re = true;
    if (props.only_access) {
      let khoTP = user.value.khoTP.map((item) => item.makho);
      // console.log(khoTP);
      re = khoTP.indexOf(item.makho) != -1;
    }
    return re;
  });
});
const normalizer = (node) => {
  // console.log(id);
  let disabled = false;
  if (node.makho == "00" && props.disabled_00) disabled = true;
  return {
    id: node.makho,
    label: node.makho + " - " + node.tenkho,
    isDisabled: disabled,
  };
};
onMounted(() => {
  store.fetchKho();
});
</script>