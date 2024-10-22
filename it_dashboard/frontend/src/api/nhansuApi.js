import repository from "../service/repository";

const resoure = "nhansu";
export default {

  biendong(params) {
    return repository
      .post(`/v1/${resoure}/biendong`, params, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((res) => res.data);
  },
  get() {
    return repository.get(`/v1/${resoure}/get`).then((res) => res.data);
  }, 
  department() {
    return repository.get(`/v1/${resoure}/department`).then((res) => res.data);
  },
};
