import { ref, computed } from "vue";
import { defineStore } from "pinia";
import Api from "../api/Api";

export const useGeneral = defineStore("General", () => {
  const category = ref([]);
  const trinhdo = ref([]);
  const chuyenmon = ref([]);
  const area = ref([]);
  const loaihd = ref([]);
  const chucvu = ref([]);
  const department = ref([]);
  const shifts = ref([]);
  const khuvuc = ref([]);
  const persondepartments = ref([]);
  const fetchCategory = () => {
    if (category.value.length) return;
    Api.category().then((res) => {
      category.value = res;
    });
  }
  const fetchKhuvuc = () => {
    if (khuvuc.value.length) return;
    // WHApi.khuvuc().then((res) => {
    //   khuvuc.value = res;
    // });
  }
  const fetchDepartment = () => {
    if (department.value.length) return;
    Api.department().then((res) => {
      department.value = res;
    });
  }
  const fetchArea = () => {
    if (area.value.length) return;
    Api.area().then((res) => {
      area.value = res;
    });
  }
  const fetchShifts = () => {
    if (shifts.value.length) return;
    Api.shifts().then((res) => {
      shifts.value = res;
    });
  }
  const fetchTrinhdo = () => {
    if (trinhdo.value.length) return;
    Api.trinhdo().then((res) => {
      trinhdo.value = res;
    });
  }
  const fetchLoaihd = () => {
    if (loaihd.value.length) return;
    Api.loaihd().then((res) => {
      loaihd.value = res;
    });
  }
  const fetchChuyenmon = () => {
    if (chuyenmon.value.length) return;
    Api.chuyenmon().then((res) => {
      chuyenmon.value = res;
    });
  }
  const fetchPersonDepartments = (cache = true, date_from = null) => {
    if (cache == true && persondepartments.value.length) return;
    Api.persondepartments(date_from).then((res) => {
      persondepartments.value = res;
    });
  }
  const fetchChucvu = () => {
    if (chucvu.value.length) return;
    Api.chucvu().then((res) => {
      chucvu.value = res;
    });
  }
  return {
    fetchCategory,
    fetchChuyenmon,
    fetchLoaihd,
    fetchArea,
    fetchDepartment,
    fetchTrinhdo,
    fetchChucvu,
    fetchShifts,
    fetchPersonDepartments,
    fetchKhuvuc,
    khuvuc,
    persondepartments,
    shifts,
    category,
    trinhdo,
    chuyenmon,
    area,
    loaihd,
    department,
    chucvu
  };
});
