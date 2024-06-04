using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Vue.Models
{

    public class Bomchecklist
    {
        [Key]
        public string masp_1 { get; set; }

        public string? tensp { get; set; }
        public string? dangbaoche { get; set; }
        public string? dangdonggoi { get; set; }
        public double? hoanthanh { get; set; }

        public DateTime? han { get; set; }

        public double? songay_sanxuat { get; set; }
        public double? songay_donggoi { get; set; }
        public double? songay_coa { get; set; }
        public double? songay_qa { get; set; }

    }
}
