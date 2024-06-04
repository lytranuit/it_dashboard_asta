using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Vue.Models
{

    [Table("TBL_DANHMUCKHACHHANG_EXCEPTION")]
    public class TBL_DANHMUCKHACHHANG_EXCEPTIONModel
    {
        [Key]
        [Column("MAKH")]
        public string makh { get; set; }

        [Column("DONVI")]
        public string? donvi { get; set; }
    }
}
