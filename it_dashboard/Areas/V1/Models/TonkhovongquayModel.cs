using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Vue.Models
{

	public class TonkhovongquayModel
	{
		[Key]
		public string MAHH1 { get; set; }

		public string? TENHH { get; set; }
		public string? MANHOM { get; set; }
		public string? DVT { get; set; }
		public decimal? SL_TONCUOIKY { get; set; }

		public decimal? T_12 { get; set; }
		public decimal? T_9 { get; set; }
		public decimal? T_6 { get; set; }
		public decimal? T_3 { get; set; }

	}
}
