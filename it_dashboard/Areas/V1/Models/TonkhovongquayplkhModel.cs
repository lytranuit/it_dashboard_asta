using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Vue.Models
{

    public class TonkhovongquayplkhModel
    {
        [Key]
        public string MAHH1 { get; set; }

        public string? TENHH { get; set; }
        public string? MANHOM { get; set; }
        public string? DVT { get; set; }
        public decimal? SL_TONCUOIKY { get; set; }

        public decimal? T_ { get; set; }

    }
}
