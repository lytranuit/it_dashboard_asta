<template>
  <TreeSelect :options="options" :multiple="multiple" :modelValue="modelValue" :name="name" :required="required"
    :append-to-body="true" @update:modelValue="emit('update:modelValue', $event)"></TreeSelect>
</template>

<script setup>
import { storeToRefs } from "pinia";
import { computed, onMounted } from "vue";
import { useProcess } from "../../stores/process";
const props = defineProps({
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
  name: {
    type: String,
    default: "tinh",
  },
});
const emit = defineEmits(["update:modelValue"]);
const store = useProcess();
const { list_tinh } = storeToRefs(store);
const options = computed(() => {
  // console.log(list_tinh.value)
  return list_tinh.value.map((item) => {
    return { id: item.matinh, label: item.tentinh ? item.tentinh : item.matinh };
  })
})
onMounted(() => {
  store.fetchTinh();
});
</script>
