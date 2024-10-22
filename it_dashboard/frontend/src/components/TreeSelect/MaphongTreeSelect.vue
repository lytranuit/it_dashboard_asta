<template>
  <TreeSelect
    placeholder="Phòng ban"
    :options="department"
    :multiple="multiple"
    :normalizer="normalizer"
    :modelValue="modelValue"
    :required="required"
    :append-to-body="appendToBody"
    @update:modelValue="emit('update:modelValue', $event)"
    zIndex="3000"
  >
  </TreeSelect>
</template>

<script setup>
import { storeToRefs } from "pinia";
import { computed, onMounted, ref } from "vue";
import nhansuApi from "../../api/nhansuApi";
import { useGeneral } from "../../stores/general";
const props = defineProps({
  appendToBody: {
    type: Boolean,
    default: true,
  },
  modelValue: {
    type: [String, Array],
  },
  multiple: {
    type: Boolean,
    default: false,
  },
  required: {
    type: Boolean,
    default: false,
  },
});
const emit = defineEmits(["update:modelValue"]);
const department = ref();
const normalizer = (node) => {
  return {
    id: node.MAPHONG,
    label: node.TENPHONG,
  };
};
onMounted(() => {
  nhansuApi.department().then((res) => {
    department.value = res;
  });
});
</script>
