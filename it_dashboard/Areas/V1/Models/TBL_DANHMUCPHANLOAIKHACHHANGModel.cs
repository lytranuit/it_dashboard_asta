using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Vue.Models
{

	[Table("TBL_DANHMUCPHANLOAIKHACHHANG")]
	public class TBL_DANHMUCPHANLOAIKHACHHANGModel
	{
		[Key]
		public string phanloaikhachhang { get; set; }

		public string? tenphanloai { get; set; }
	}
}
