﻿

using iText.StyledXmlParser.Css.Resolve.Shorthand.Impl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Security;
using Spire.Xls;
using System.Collections;
using System.Data;
using System.Reflection.Emit;
using System.Text.Json.Serialization;
using Vue.Data;
using Vue.Models;

namespace it_template.Areas.V1.Controllers
{

    public class NhansuController : BaseController
    {
        private readonly IConfiguration _configuration;

        private readonly NhansuContext _nhansuContext;

        public NhansuController(ItContext context, IConfiguration configuration, NhansuContext nhansuContext) : base(context)
        {
            _configuration = configuration;
            _nhansuContext = nhansuContext;

        }
        public async Task<JsonResult> get()
        {
            //var biendong = new { labels = data.Select(d => d.label).ToList(), datasets = new List<Chart>() { new Chart { label = "Doanh thu", data = data.Select(d => d.data).ToList() } } };
            var tong = _nhansuContext.PersonnelModel.Where(d => d.NGAYNGHIVIEC == null).Count();
            var chinhthuc = _nhansuContext.PersonnelModel.Where(d => d.NGAYNGHIVIEC == null && d.tinhtrang == "Chính thức" && d.LOAIHD != "DV").Count();
            var hocthuviec = _nhansuContext.PersonnelModel.Where(d => d.NGAYNGHIVIEC == null && (d.tinhtrang == "Học việc" || d.tinhtrang == "Thử việc") && d.LOAIHD != "DV").Count();
            var dichvu = _nhansuContext.PersonnelModel.Where(d => d.NGAYNGHIVIEC == null && d.LOAIHD == "DV").Count();


            var nhansu = _nhansuContext.PersonnelModel.Where(d => d.NGAYNGHIVIEC == null).GroupBy(d => d.MAPHONG).Select(group => new
            {
                label = _nhansuContext.DepartmentModel.SingleOrDefault(d => d.MAPHONG == group.Key).TENPHONG,
                data = group.Count()
            }).ToList();
            var tilenhansu = new { labels = nhansu.Select(d => d.label).ToList(), datasets = new List<Chart>() { new Chart { label = "Nhân sự", data = nhansu.Select(d => (double)d.data).ToList() } } };

            var luong = _nhansuContext.PersonnelModel.Where(d => d.NGAYNGHIVIEC == null).GroupBy(d => d.MAPHONG).Select(group => new
            {
                label = _nhansuContext.DepartmentModel.SingleOrDefault(d => d.MAPHONG == group.Key).TENPHONG,
                data = group.Sum(d => d.tong_thunhap)
            }).ToList();
            var tileluong = new { labels = luong.Select(d => d.label).ToList(), datasets = new List<Chart>() { new Chart { label = "Lương", data = luong.Select(d => (double)d.data).ToList() } } };

            var json = new { tong, chinhthuc, hocthuviec, dichvu, tileluong, tilenhansu };
            return Json(json);
        }

        [HttpPost]
        public async Task<JsonResult> biendong(int nam, string department)
        {
            //var biendong = new { labels = data.Select(d => d.label).ToList(), datasets = new List<Chart>() { new Chart { label = "Doanh thu", data = data.Select(d => d.data).ToList() } } };
            var salaryUser = _nhansuContext.SalaryUserModel.Include(d => d.salary).Where(d => d.salary.deleted_at == null && d.salary.status == "Đã khóa" && d.salary.nam == nam).ToList();
            if (department != null)
            {
                salaryUser = salaryUser.Where(d => d.MABOPHAN == department).ToList();
            }
            var data = salaryUser.GroupBy(d => new { year = d.salary.nam, month = d.salary.thang }).Select(group => new Chart1
            {
                sort = group.Key.year + "-" + group.Key.month,
                label = group.Key.month + "/" + group.Key.year,
                data = (double)group.Sum(d => d.thuclanh),
                data_nv = (double)group.Count()
            }).OrderBy(d => d.sort).ToList();
            var biendong = new
            {
                labels = data.Select(d => d.label).ToList(),
                datasets = new List<Chart>() {
                    new Chart { label = "Tổng lương", data = data.Select(d => d.data).ToList(), type = "line", fill = false,yAxisID="A" },
                    new Chart { type="bar",label = "Tổng NV", data = data.Select(d => d.data_nv).ToList(),yAxisID="B" },
                }
            };

            return Json(new
            {
                data = biendong
            }, new System.Text.Json.JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
        }

        public async Task<JsonResult> department()
        {
            var All = _nhansuContext.DepartmentModel.ToList();
            //var jsonData = new { data = ProcessModel };
            return Json(All, new System.Text.Json.JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
        }
    }
}