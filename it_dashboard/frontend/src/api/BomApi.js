import repository from "../service/repository";

const resoure = "bom";
export default {
  get(mahh, colo) {
    return repository
      .get(`/v1/${resoure}/Get`, {
        params: { mahh: mahh, colo: colo },
        withCredentials: true // Gửi cookie cùng với yêu cầu
      })
      .then((res) => res.data);
  },
  table(params) {

    return repository
      .post(`/v1/${resoure}/table`, params, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
        withCredentials: true // Gửi cookie cùng với yêu cầu
      })
      .then((res) => res.data);
  },
  excel(params) {
    return repository
      .post(`/v1/${resoure}/excel`, params, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((res) => res.data);
  },

};
