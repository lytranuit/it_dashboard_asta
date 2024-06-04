<template>
  <TreeSelect :options="options" :multiple="multiple" :modelValue="modelValue" :name="name" :required="required"
    :append-to-body="true" @update:modelValue="emit('update:modelValue', $event)"></TreeSelect>
</template>

<script setup>
import { storeToRefs } from "pinia";
import { computed, onMounted } from "vue";
import { useProcess } from "../../stores/process";
import Api from "../../api/Api";
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
    default: "manhom",
  },
});
const emit = defineEmits(["update:modelValue"]);
const store = useProcess();
const { list_nhom } = storeToRefs(store);

const options = computed(() => {
  // console.log(list_tinh.value)
  return list_nhom.value.map((item) => {
    return { id: item.nhom, label: item.nhom + " - " + item.tennhom };
  })
})
onMounted(() => {
  store.fetchManhom();
});
</script>
