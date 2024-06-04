using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Vue.Models
{

    public class Tinhtrangsanpham
    {
        [Key]
        public string MAHH_GOC_1 { get; set; }

        public string? TENHH { get; set; }
        public string? MALO_GOC { get; set; }
        public string? DVT { get; set; }
        public decimal? COLO_GOC { get; set; }
        public bool? HOANTHANH { get; set; }
        public decimal? SOLUONG_NHAP { get; set; }
        public string? DVT_NHAP { get; set; }

        public DateTime? NGAYDAUTIEN { get; set; }
        public DateTime? NGAYHOANTHANH { get; set; }

        public int? SONGAY { get; set; }

    }
}
