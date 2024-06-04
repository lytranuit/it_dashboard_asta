using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Vue.Models
{

	public class RawDataModel
	{


		public string MAKH { get; set; }

		public string MAHH { get; set; }

        public string DVT { get; set; }
        public string MACH { get; set; }

		public string NHOM { get; set; }

		public string MATINH { get; set; }

		public string MATDV { get; set; }

		public string PHANLOAI { get; set; }

		public string PHANLOAIKHACHHANG { get; set; }
		public DateTime NgayLapHD { get; set; }

		public string TENDVBC { get; set; }

		public string? TENHH { get; set; }
		public string? TENNHOM { get; set; }
		public string? TENTINH { get; set; }

		public string? TENTDV { get; set; }
		public string? DONVI { get; set; }

		public double DOANHTHU { get; set; }

		public decimal TIEN { get; set; }

		public decimal SOLUONG { get; set; }

	}
}
