using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Vue.Models
{

	public class MaNhomModel
	{
		[Key]
		public string Nhom { get; set; }

		public string? tennhom { get; set; }
	}
}
