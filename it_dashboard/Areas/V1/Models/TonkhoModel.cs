using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Vue.Models
{

    public class TonkhoModel
    {
        [Key]
        public string MAHH { get; set; }

        public string? TENHH { get; set; }
        public string? MANHOM { get; set; }
        public string? TENNHOM { get; set; }
        public string? DVT { get; set; }
        public string? SL_TONCUOIKY { get; set; }
        public decimal? SL_THUNG { get; set; }

        [NotMapped]
        public double? soluong_ton
        {
            get
            {
                var value = SL_TONCUOIKY.Replace(",", "");
                return double.Parse(value);
            }
        }

    }
}
