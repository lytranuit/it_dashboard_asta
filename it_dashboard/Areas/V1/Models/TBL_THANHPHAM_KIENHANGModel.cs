using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Vue.Models
{

    [Table("TBL_THANHPHAM_KIENHANG")]
    public class TBL_THANHPHAM_KIENHANGModel
    {
        [Key]
        public string qrcode { get; set; }
        public string? qrcode_image { get; set; }
        public string? code { get; set; }
        public string mahh { get; set; }

        public DateTime? ngaysx { get; set; }
        public string? tinhtrang { get; set; }


        public decimal? soluongbandau { get; set; }
        public decimal? soluong { get; set; }
        public string? tenhh { get; set; }
        public string? malo { get; set; }
        public DateTime? handung { get; set; }

        public string? sopkn { get; set; }
        public string? makho { get; set; }
        public string? dvt { get; set; }
        public string? quicach { get; set; }
        public string? phanloai { get; set; }
        public string? dangbaoche { get; set; }
        public string? sodk { get; set; }

        public string? sophieuDG { get; set; }
        public int? DG_id { get; set; }
        //[ForeignKey("DG_id")]
        //public virtual XUATModel? DG { get; set; }

        //public List<TBL_THANHPHAM_KIENHANG_NHAPXUATModel> nhapXuatModels { get; set; }
        public string? mavt { get; set; }

        public string? mota { get; set; }
        public string? loaihh { get; set; }
        public bool? is_thungle { get; set; }

        public DateTime? created_at { get; set; }
        public DateTime? deleted_at { get; set; }
        public DateTime? updated_at { get; set; }

        public DateTime? ngayxuatxuong { get; set; }
    }
}
