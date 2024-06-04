import { ref, computed } from "vue";
import Api from "../api/Api";
import { defineStore } from "pinia";
export const useProcess = defineStore("process", () => {
  const list_tinh = ref([]);
  const list_nhom = ref([]);
  const list_sanpham = ref([]);
  const list_khachhang = ref([]);
  const list_PLKH = ref([]);
  const list_khachhang_exception = ref([]);
  const fetchPLKH = () => {
    if (list_PLKH.value.length) return;
    return Api.list_PLKH().then((response) => {
      list_PLKH.value = response;
      return response;
    });
  };
  const fetchManhom = () => {
    if (list_nhom.value.length) return;
    return Api.list_nhom().then((response) => {
      list_nhom.value = response;
      return response;
    });
  };
  const fetchTinh = () => {
    if (list_tinh.value.length) return;
    return Api.list_tinh().then((response) => {
      list_tinh.value = response;
      return response;
    });
  };
  const fetchSanPham = () => {
    if (list_sanpham.value.length) return;
    return Api.list_sanpham().then((response) => {
      list_sanpham.value = response;
      return response;
    });
  };
  const fetchKhachHang = () => {
    if (list_khachhang.value.length) return;
    return Api.list_khachhang().then((response) => {
      list_khachhang.value = response;
      return response;
    });
  };
  const fetchKhachHangException = () => {
    if (list_khachhang_exception.value.length) return;
    return Api.list_khachhang_exception().then((response) => {
      list_khachhang_exception.value = response;
      return response;
    });
  };
  return {
    list_tinh,
    list_nhom,
    list_sanpham,
    list_khachhang,
    list_khachhang_exception,
    list_PLKH,
    fetchPLKH,
    fetchManhom,
    fetchTinh,
    fetchSanPham,
    fetchKhachHang,
    fetchKhachHangException,
  };
});
