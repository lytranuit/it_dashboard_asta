using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Vue.Models
{

    [Table("TBL_DANHMUCHANGHOA")]
    public class TBL_DANHMUCHANGHOAModel
    {
        [Key]
        public string mahh { get; set; }

        public string? tenhh { get; set; }
        public string? nhom { get; set; }
        public decimal? sl1 { get; set; }
        public decimal? sl2 { get; set; }
        public decimal? sl3 { get; set; }
        public decimal? giaban { get; set; }
    }
}
