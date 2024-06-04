import repository from "../service/repository";

const resoure = "api";
export default {
  process(id) {
    return repository
      .get(`/v1/${resoure}/process`, { params: { id: id } })
      .then((res) => res.data);
  },
  list_PLKH() {
    return repository.get(`/v1/${resoure}/list_PLKH`).then((res) => res.data);
  },
  list_nhom() {
    return repository.get(`/v1/${resoure}/list_nhom`).then((res) => res.data);
  },
  list_tinh() {
    return repository.get(`/v1/${resoure}/list_tinh`).then((res) => res.data);
  },
  list_sanpham() {
    return repository
      .get(`/v1/${resoure}/list_sanpham`)
      .then((res) => res.data);
  },
  list_khachhang() {
    return repository
      .get(`/v1/${resoure}/list_khachhang`)
      .then((res) => res.data);
  },
  list_khachhang_exception() {
    return repository
      .get(`/v1/${resoure}/list_khachhang_exception`)
      .then((res) => res.data);
  },
  ProcessGroupWithProcess() {
    return repository
      .get(`/v1/${resoure}/ProcessGroupWithProcess`)
      .then((res) => res.data);
  },
  processgroup() {
    return repository
      .get(`/v1/${resoure}/processgroup`)
      .then((res) => res.data);
  },
  ProcessVersion(id) {
    return repository
      .get(`/v1/${resoure}/ProcessVersion`, { params: { id: id } })
      .then((res) => res.data);
  },
  execution(execution_id) {
    return repository
      .get(`/v1/${resoure}/execution`, { params: { id: execution_id } })
      .then((res) => res.data);
  },
  TransitionByExecution(execution_id) {
    return repository
      .get(`/v1/${resoure}/TransitionByExecution`, {
        params: { execution_id: execution_id },
      })
      .then((res) => res.data);
  },
  ActivityByExecution(execution_id) {
    return repository
      .get(`/v1/${resoure}/ActivityByExecution`, {
        params: { execution_id: execution_id },
      })
      .then((res) => res.data);
  },
  CustomBlockByExecution(execution_id) {
    return repository
      .get(`/v1/${resoure}/CustomBlockByExecution`, {
        params: { execution_id: execution_id },
      })
      .then((res) => res.data);
  },
  HomeBadge(kh_exception, list_tinh, list_nhom, tungay, denngay) {
    return repository
      .post(
        `/v1/${resoure}/homebadge`,
        {
          kh_exception: kh_exception,
          list_tinh: list_tinh,
          list_nhom: list_nhom,
          tungay: tungay,
          denngay: denngay,
        },
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      )
      .then((res) => res.data);
  },
  topsp(kh_exception, list_tinh, list_nhom, tungay, denngay, limit) {
    return repository
      .post(
        `/v1/${resoure}/topsp`,
        {
          kh_exception: kh_exception,
          list_tinh: list_tinh,
          list_nhom: list_nhom,
          tungay: tungay,
          denngay: denngay,
          limit: limit,
        },
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      )
      .then((res) => res.data);
  },
  topnhom(kh_exception, list_tinh, list_nhom, tungay, denngay, limit) {
    return repository
      .post(
        `/v1/${resoure}/topnhom`,
        {
          kh_exception: kh_exception,
          list_tinh: list_tinh,
          list_nhom: list_nhom,
          tungay: tungay,
          denngay: denngay,
          limit: limit,
        },
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      )
      .then((res) => res.data);
  },

  toptdv(kh_exception, list_tinh, list_nhom, tungay, denngay, limit) {
    return repository
      .post(
        `/v1/${resoure}/toptdv`,
        {
          kh_exception: kh_exception,
          list_tinh: list_tinh,
          list_nhom: list_nhom,
          tungay: tungay,
          denngay: denngay,
          limit: limit,
        },
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      )
      .then((res) => res.data);
  },
  toptinh(kh_exception, list_tinh, list_nhom, tungay, denngay, limit) {
    return repository
      .post(
        `/v1/${resoure}/toptinh`,
        {
          kh_exception: kh_exception,
          list_tinh: list_tinh,
          list_nhom: list_nhom,
          tungay: tungay,
          denngay: denngay,
          limit: limit,
        },
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      )
      .then((res) => res.data);
  },
  topkh(kh_exception, list_tinh, list_nhom, tungay, denngay, limit) {
    return repository
      .post(
        `/v1/${resoure}/topkh`,
        {
          kh_exception: kh_exception,
          list_tinh: list_tinh,
          list_nhom: list_nhom,
          tungay: tungay,
          denngay: denngay,
          limit: limit,
        },
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      )
      .then((res) => res.data);
  },
  doanhthu(kh_exception, list_tinh, list_nhom, tungay, denngay, timetype) {
    return repository
      .post(
        `/v1/${resoure}/doanhthu`,
        {
          kh_exception: kh_exception,
          list_tinh: list_tinh,
          list_nhom: list_nhom,
          tungay: tungay,
          denngay: denngay,
          timetype: timetype,
        },
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      )
      .then((res) => res.data);
  },

  tonkho(limit) {
    return repository
      .post(
        `/v1/${resoure}/tonkho`,
        {
          limit: limit,
        },
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      )
      .then((res) => res.data);
  },

  tinhtrangsanpham(params) {
    return repository
      .post(`/v1/${resoure}/tinhtrangsanpham`, params, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((res) => res.data);
  },
  bomchecklist(params) {
    return repository
      .post(`/v1/${resoure}/bomchecklist`, params, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((res) => res.data);
  },
  tonkhovongquay(params) {
    return repository
      .post(`/v1/${resoure}/tonkhovongquay`, params, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((res) => res.data);
  },
  tonkhovongquayplkh(params) {
    return repository
      .post(`/v1/${resoure}/tonkhovongquayplkh`, params, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((res) => res.data);
  },
  chitiet(list_sanpham, list_khachhang, list_tinh, list_nhom, tungay, denngay) {
    return repository
      .post(
        `/v1/${resoure}/chitiet`,
        {
          list_sanpham: list_sanpham,
          list_khachhang: list_khachhang,
          list_tinh: list_tinh,
          list_nhom: list_nhom,
          tungay: tungay,
          denngay: denngay,
        },
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      )
      .then((res) => res.data);
  },
  tableUser() {
    return repository
      .post(
        `/v1/${resoure}/tableUser`,
        {},
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      )
      .then((res) => res.data);
  },

  tableProcess() {
    return repository
      .post(
        `/v1/${resoure}/tableProcess`,
        {},
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      )
      .then((res) => res.data);
  },
  saveprocess(params) {
    return repository
      .post(`/v1/${resoure}/saveprocess`, params, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((res) => res.data);
  },
  morecomment(execution_id, from_id) {
    return repository
      .get(`/v1/${resoure}/morecomment`, {
        params: { execution_id: execution_id, from_id: from_id },
      })
      .then((res) => res.data);
  },
  events(execution_id) {
    return repository
      .get(`/v1/${resoure}/events`, {
        params: { execution_id: execution_id },
      })
      .then((res) => res.data);
  },
  saveprocess(params) {
    return repository
      .post(`/v1/${resoure}/saveprocess`, params, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((res) => res.data);
  },

  updateexecution(params) {
    return repository
      .post(`/v1/${resoure}/updateexecution`, params, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((res) => res.data);
  },
  createexecution(params) {
    return repository
      .post(`/v1/${resoure}/createexecution`, params, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((res) => res.data);
  },
  createcustomblock(params) {
    return repository
      .post(`/v1/${resoure}/createcustomblock`, params, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((res) => res.data);
  },
  updatecustomblock(params) {
    return repository
      .post(`/v1/${resoure}/updatecustomblock`, params, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((res) => res.data);
  },
  createactivity(params) {
    return repository
      .post(`/v1/${resoure}/createactivity`, params, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((res) => res.data);
  },
  updateactivity(params) {
    return repository
      .post(`/v1/${resoure}/updateactivity`, params, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((res) => res.data);
  },
  createtransition(params) {
    return repository
      .post(`/v1/${resoure}/createtransition`, params, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((res) => res.data);
  },
  uploadFile(params) {
    return repository
      .post(`/v1/${resoure}/uploadFile`, params, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((res) => res.data);
  },
  addcomment(params) {
    return repository
      .post(`/v1/${resoure}/addcomment`, params, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((res) => res.data);
  },
  SaveSign(params) {
    return repository
      .post(`/v1/${resoure}/SaveSign`, params, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((res) => res.data);
  },
};
