using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Vue.Models
{

    [Table("TBL_DANHMUCKHO")]
    public class TBL_DANHMUCKHO
    {
        [Key]
        [Column("stt")]
        public int id { get; set; }
        //public string? manhom { get; set; }

        [Column("noinhan")]
        public string? makho { get; set; }
        [Column("diachi")]
        public string? tenkho { get; set; }
        public bool? is_thanhpham { get; set; }


    }
}
