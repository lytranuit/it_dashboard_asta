

using iText.StyledXmlParser.Css.Resolve.Shorthand.Impl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spire.Xls;
using System.Collections;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Text.Json.Serialization;
using Vue.Data;
using Vue.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            var hocthuviec = _nhansuContext.PersonnelModel.Where(d => d.NGAYNGHIVIEC == null && (d.tinhtrang == "Học việc" || d.tinhtrang == "Thử việc" || d.tinhtrang == "Thử việc không bảo hiểm và không phép năm") && d.LOAIHD != "DV").Count();
            var dichvu = _nhansuContext.PersonnelModel.Where(d => d.NGAYNGHIVIEC == null && (d.LOAIHD == "DV" || d.tinhtrang == "Dịch vụ" || d.tinhtrang == "Dịch vụ không phép năm")).Count();


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
            return Json(json, new System.Text.Json.JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
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
                sort = $"{group.Key.year:D4}-{group.Key.month:D2}",
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
        [HttpPost]
        public async Task<JsonResult> nghiviec(DateTime date_from, DateTime date_to, string timetype = "Month")
        {
            //var biendong = new { labels = data.Select(d => d.label).ToList(), datasets = new List<Chart>() { new Chart { label = "Doanh thu", data = data.Select(d => d.data).ToList() } } };
            if (date_from.Kind == DateTimeKind.Utc)
            {
                date_from = date_from.ToLocalTime();
            }
            if (date_to.Kind == DateTimeKind.Utc)
            {
                date_to = date_to.ToLocalTime();
            }
            int fromYear = date_from.Year;
            int fromMonth = date_from.Month;
            int toYear = date_to.Year;
            int toMonth = date_to.Month;

            var salaryUser = _nhansuContext.SalaryUserModel.Include(d => d.salary)
                .Where(d => d.salary.deleted_at == null && d.salary.status == "Đã khóa"
                && (
                    // Cùng năm, trong khoảng tháng
                    (d.salary.nam == fromYear && d.salary.thang >= fromMonth && d.salary.nam == toYear && d.salary.thang <= toMonth)
                    // Năm nằm giữa
                    || (d.salary.nam > fromYear && d.salary.nam < toYear)
                    // Năm đầu, lớn hơn fromMonth
                    || (d.salary.nam == fromYear && d.salary.thang >= fromMonth && d.salary.nam < toYear)
                    // Năm cuối, nhỏ hơn toMonth
                    || (d.salary.nam == toYear && d.salary.thang <= toMonth && d.salary.nam > fromYear)
                ))
                .ToList();
            var data = new List<Chart1>();
            if (timetype == "Year")
            {
                data = salaryUser.GroupBy(d => new { year = d.salary.nam }).Select(group => new Chart1
                {
                    year = group.Key.year,
                    sort = $"{group.Key.year:D4}",
                    label = $"{group.Key.year:D4}",
                    data = (double)group.Sum(d => d.thuclanh),
                    data_nv = (double)group.Select(d => d.MANV).Distinct().Count()
                }).OrderBy(d => d.sort).ToList();

            }
            else
            {
                data = salaryUser.GroupBy(d => new { year = d.salary.nam, month = d.salary.thang }).Select(group => new Chart1
                {
                    year = group.Key.year,
                    month = group.Key.month,
                    sort = $"{group.Key.year:D4}-{group.Key.month:D2}",
                    label = group.Key.month + "/" + group.Key.year,
                    data = (double)group.Sum(d => d.thuclanh),
                    data_nv = (double)group.Count()
                }).OrderBy(d => d.sort).ToList();
            }
            foreach (var item in data)
            {
                var nghiviec = _nhansuContext.PersonnelModel
                    .Where(d => d.NGAYNGHIVIEC != null && d.NGAYNGHIVIEC.Value.Year == item.year);
                if (item.month > 0)
                {
                    nghiviec = nghiviec.Where(d => d.NGAYNGHIVIEC.Value.Month == item.month);
                }
                var count_nghiviec = nghiviec.Count();
                var count_nv = (int)item.data_nv;
                var tile = Math.Round(((double)count_nghiviec / count_nv) * 100, 2); // làm tròn 2 chữ số sau dấu phẩy

                item.data_nghiviec = count_nghiviec;
                item.tile = tile;
            }

            var biendong = new
            {
                labels = data.Select(d => d.label).ToList(),
                datasets = new List<Chart>() {
                    new Chart { label = "Tỉ lệ nghĩ việc", data = data.Select(d => d.tile).ToList(), type = "line", fill = false,yAxisID="B",stack="Stack 1" ,backgroundColor="#42cef1",borderColor = "#42cef1" },
                    new Chart { type="bar",label = "Nhân viên", data = data.Select(d => d.data_nv).ToList(),yAxisID="A" ,stack="Stack 0",backgroundColor="#FFB6C1"},
                    new Chart { type="bar",label = "Nghĩ việc", data = data.Select(d => 0-d.data_nghiviec).ToList(),yAxisID="A" ,stack="Stack 0" , backgroundColor = "red"},
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