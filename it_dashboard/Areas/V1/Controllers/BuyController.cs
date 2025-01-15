


using iText.StyledXmlParser.Css.Resolve.Shorthand.Impl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Security;
using Spire.Xls;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Text.Json.Serialization;
using Vue.Data;
using Vue.Models;

namespace it_template.Areas.V1.Controllers
{

    public class BuyController : BaseController
    {
        private readonly IConfiguration _configuration;

        private readonly Buy_Context _buyContext;

        public BuyController(ItContext context, IConfiguration configuration, Buy_Context buyContext) : base(context)
        {
            _configuration = configuration;
            _buyContext = buyContext;

            var listener = _buyContext.GetService<DiagnosticSource>();
            (listener as DiagnosticListener).SubscribeWithAdapter(new CommandInterceptor());
        }

        [HttpPost]
        public async Task<JsonResult> chiphi(DateTime? tungay, DateTime? denngay, string timetype = "Month")
        {
            var cusdata = _buyContext.MuahangModel.Where(d => d.deleted_at == null && d.is_multiple_ncc != true && d.pay_at != null);
            if (tungay != null && tungay.HasValue)
            {
                if (tungay.Value.Kind == DateTimeKind.Utc)
                {
                    tungay = tungay.Value.ToLocalTime();
                }
                cusdata = cusdata.Where(d => d.pay_at >= tungay.Value);
            }
            if (denngay != null && denngay.HasValue)
            {
                if (denngay.Value.Kind == DateTimeKind.Utc)
                {
                    denngay = denngay.Value.ToLocalTime();
                }
                cusdata = cusdata.Where(d => d.pay_at <= denngay.Value);
            }
            var data = cusdata.Include(d => d.muahang_chonmua)
                .ThenInclude(d => d.chitiet)
                .ThenInclude(d => d.muahang_chitiet)
                .ThenInclude(d => d.dutru_chitiet).ThenInclude(d => d.dutru)
                .ToList();
            data = data.Where(d => d.muahang_chonmua != null).ToList();
            var list = new List<ChartDataBuy>();
            if (timetype == "Year")
            {
                list = data.GroupBy(d => new { d.pay_at.Value.Year }).Select(group => new ChartDataBuy
                {
                    sort = group.Key.Year.ToString(),
                    label = group.Key.Year.ToString(),
                    data = group.Sum(d => d.muahang_chonmua.tonggiatri),
                    data_nvl = group.Sum(d => d.muahang_chonmua.chitiet.Where(e => e.muahang_chitiet.dutru_chitiet.dutru.type_id == 1).Select(e => e.thanhtien_vat).Sum()),
                    data_hoachat = group.Sum(d => d.muahang_chonmua.chitiet.Where(e => e.muahang_chitiet.dutru_chitiet.dutru.type_id == 3).Select(e => e.thanhtien_vat).Sum()),
                    data_giantiep = group.Sum(d => d.muahang_chonmua.chitiet.Where(e => e.muahang_chitiet.dutru_chitiet.dutru.type_id == 2).Select(e => e.thanhtien_vat).Sum()),
                    group = group.ToList()
                }).OrderBy(d => d.sort).ToList();

            }
            else if (timetype == "Month")
            {
                list = data.GroupBy(d => new { year = d.pay_at.Value.Year, month = d.pay_at.Value.Month }).Select(group => new ChartDataBuy
                {
                    sort = group.Key.year + "-" + group.Key.month.ToString("d2"),
                    label = group.Key.month.ToString("d2") + "/" + group.Key.year,
                    data = group.Sum(d => d.muahang_chonmua.tonggiatri),
                    data_nvl = group.Sum(d => d.muahang_chonmua.chitiet.Where(e => e.muahang_chitiet.dutru_chitiet.dutru.type_id == 1).Select(e => e.thanhtien_vat).Sum()),
                    data_hoachat = group.Sum(d => d.muahang_chonmua.chitiet.Where(e => e.muahang_chitiet.dutru_chitiet.dutru.type_id == 3).Select(e => e.thanhtien_vat).Sum()),
                    data_giantiep = group.Sum(d => d.muahang_chonmua.chitiet.Where(e => e.muahang_chitiet.dutru_chitiet.dutru.type_id == 2).Select(e => e.thanhtien_vat).Sum()),
                    group = group.ToList()
                }).OrderBy(d => d.sort).ToList();
            }
            //else if (timetype == "Week")
            //{
            //    list = data.GroupBy(d => new { year = d.date_finish.Value.Year, month = d.date_finish.Value.Month }).Select(group => new ChartDataBuy
            //    {
            //        sort = group.Key.year + "-" + group.Key.month.ToString("d2"),
            //        label = group.Key.month.ToString("d2") + "/" + group.Key.year,
            //        data = group.Sum(d => d.muahang_chonmua.tonggiatri)
            //    }).OrderBy(d => d.sort).ToList();
            //}
            else
            {
                list = data.GroupBy(d => new { date = d.pay_at.Value.Date }).Select(group => new ChartDataBuy
                {
                    sort = group.Key.date.ToString("yyyy-MM-dd"),
                    label = group.Key.date.ToString("yyyy-MM-dd"),
                    data = group.Sum(d => d.muahang_chonmua.tonggiatri),
                    data_nvl = group.Sum(d => d.muahang_chonmua.chitiet.Where(e => e.muahang_chitiet.dutru_chitiet.dutru.type_id == 1).Select(e => e.thanhtien_vat).Sum()),
                    data_hoachat = group.Sum(d => d.muahang_chonmua.chitiet.Where(e => e.muahang_chitiet.dutru_chitiet.dutru.type_id == 3).Select(e => e.thanhtien_vat).Sum()),
                    data_giantiep = group.Sum(d => d.muahang_chonmua.chitiet.Where(e => e.muahang_chitiet.dutru_chitiet.dutru.type_id == 2).Select(e => e.thanhtien_vat).Sum()),
                    group = group.ToList()
                }).OrderBy(d => d.sort).ToList();
            }
            var dulieu = new
            {
                labels = list.Select(d => d.label).ToList(),
                datasets = new List<ChartBuy>()
                {
                    new ChartBuy { type="line",fill=false,borderWidth=2,label = "Tổng", data = list.Select(d => d.data).ToList() },

                    new ChartBuy { type="bar",label = "Nguyên liệu", data = list.Select(d => d.data_nvl).ToList() },


                    new ChartBuy { type="bar",label = "Hóa chất", data = list.Select(d => d.data_hoachat).ToList() },


                    new ChartBuy { type="bar",label = "Khác", data = list.Select(d => d.data_giantiep).ToList() }
                }
            };

            return Json(new
            {
                data = dulieu,
                //list = list
            }, new System.Text.Json.JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
            });


        }
        [HttpPost]
        public async Task<JsonResult> chiphibophan(DateTime? tungay, DateTime? denngay)
        {
            var cusdata = _buyContext.MuahangModel.Where(d => d.deleted_at == null && d.is_multiple_ncc != true && d.pay_at != null);
            if (tungay != null && tungay.HasValue)
            {
                if (tungay.Value.Kind == DateTimeKind.Utc)
                {
                    tungay = tungay.Value.ToLocalTime();
                }
                cusdata = cusdata.Where(d => d.pay_at >= tungay.Value);
            }
            if (denngay != null && denngay.HasValue)
            {
                if (denngay.Value.Kind == DateTimeKind.Utc)
                {
                    denngay = denngay.Value.ToLocalTime();
                }
                cusdata = cusdata.Where(d => d.pay_at <= denngay.Value);
            }
            var data = cusdata.Include(d => d.muahang_chonmua)
                .ThenInclude(d => d.chitiet)
                .ThenInclude(d => d.muahang_chitiet)
                .ThenInclude(d => d.dutru_chitiet).ThenInclude(d => d.dutru).ThenInclude(d => d.bophan)
                .ToList();
            data = data.Where(d => d.muahang_chonmua != null).ToList();
            var listbophan = new List<Chartbophan>();
            foreach (var d in data)
            {
                foreach (var e in d.muahang_chonmua.chitiet)
                {
                    var dutru = e.muahang_chitiet.dutru_chitiet.dutru;
                    if (dutru.type_id == 2)
                    {
                        var bophan = dutru.bophan;
                        var thanhtien = e.thanhtien_vat;
                        listbophan.Add(new Chartbophan()
                        {
                            bophan = bophan,
                            thanhtien = thanhtien
                        });
                    }
                }
            }
            var list = new List<ChartDataBuy>();

            list = listbophan.GroupBy(d => new { d.bophan }).Select(group => new ChartDataBuy
            {
                label = group.Key.bophan.name,
                data = group.Sum(d => d.thanhtien),
            }).OrderByDescending(d => d.data).ToList();

            var dulieu = new
            {
                labels = list.Select(d => d.label).ToList(),
                datasets = new List<ChartBuy>()
                {
                    new ChartBuy { label = "Chi phí gián tiếp", data = list.Select(d => d.data).ToList() },
                }
            };

            return Json(new
            {
                data = dulieu,
            }, new System.Text.Json.JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
            });
        }
        [HttpPost]
        public async Task<JsonResult> chiphibophantheo(DateTime? tungay, DateTime? denngay, string timetype = "Month", int bophan_id = 0)
        {
            var cusdata = _buyContext.MuahangModel.Where(d => d.deleted_at == null && d.is_multiple_ncc != true && d.pay_at != null);
            if (tungay != null && tungay.HasValue)
            {
                if (tungay.Value.Kind == DateTimeKind.Utc)
                {
                    tungay = tungay.Value.ToLocalTime();
                }
                cusdata = cusdata.Where(d => d.pay_at >= tungay.Value);
            }
            if (denngay != null && denngay.HasValue)
            {
                if (denngay.Value.Kind == DateTimeKind.Utc)
                {
                    denngay = denngay.Value.ToLocalTime();
                }
                cusdata = cusdata.Where(d => d.pay_at <= denngay.Value);
            }
            var data = cusdata.Include(d => d.muahang_chonmua)
                .ThenInclude(d => d.chitiet)
                .ThenInclude(d => d.muahang_chitiet)
                .ThenInclude(d => d.dutru_chitiet).ThenInclude(d => d.dutru).ThenInclude(d => d.bophan)
                .ToList();
            data = data.Where(d => d.muahang_chonmua != null).ToList();
            var listbophan = new List<Chartbophan>();
            foreach (var d in data)
            {
                foreach (var e in d.muahang_chonmua.chitiet)
                {
                    var dutru = e.muahang_chitiet.dutru_chitiet.dutru;
                    if (dutru.type_id == 2)
                    {
                        var bophan = dutru.bophan;
                        var thanhtien = e.thanhtien_vat;
                        listbophan.Add(new Chartbophan()
                        {
                            bophan = bophan,
                            thanhtien = thanhtien,
                            pay_at = d.pay_at.Value,
                        });
                    }
                }
            }
            if (bophan_id > 0)
            {
                listbophan = listbophan.Where(d => d.bophan != null && d.bophan.id == bophan_id).ToList();
            }
            var list = new List<ChartDataBuy>();
            if (timetype == "Year")
            {
                list = listbophan.GroupBy(d => new { d.pay_at.Year }).Select(group => new ChartDataBuy
                {
                    sort = group.Key.Year.ToString(),
                    label = group.Key.Year.ToString(),
                    data = group.Sum(d => d.thanhtien)
                }).OrderBy(d => d.sort).ToList();

            }
            else if (timetype == "Month")
            {
                list = listbophan.GroupBy(d => new { year = d.pay_at.Year, month = d.pay_at.Month }).Select(group => new ChartDataBuy
                {
                    sort = group.Key.year + "-" + group.Key.month.ToString("d2"),
                    label = group.Key.month.ToString("d2") + "/" + group.Key.year,
                    data = group.Sum(d => d.thanhtien)
                }).OrderBy(d => d.sort).ToList();
            }
            //else if (timetype == "Week")
            //{
            //    list = data.GroupBy(d => new { year = d.date_finish.Value.Year, month = d.date_finish.Value.Month }).Select(group => new ChartDataBuy
            //    {
            //        sort = group.Key.year + "-" + group.Key.month.ToString("d2"),
            //        label = group.Key.month.ToString("d2") + "/" + group.Key.year,
            //        data = group.Sum(d => d.muahang_chonmua.tonggiatri)
            //    }).OrderBy(d => d.sort).ToList();
            //}
            else
            {
                list = listbophan.GroupBy(d => new { date = d.pay_at.Date }).Select(group => new ChartDataBuy
                {
                    sort = group.Key.date.ToString("yyyy-MM-dd"),
                    label = group.Key.date.ToString("yyyy-MM-dd"),
                    data = group.Sum(d => d.thanhtien)
                }).OrderBy(d => d.sort).ToList();
            }
            var dulieu = new
            {
                labels = list.Select(d => d.label).ToList(),
                datasets = new List<ChartBuy>()
                {
                    new ChartBuy { type="line",fill=false,borderWidth=2,label = "Chi phí", data = list.Select(d => d.data).ToList() },
                }
            };

            return Json(new
            {
                data = dulieu,
                //list = list
            }, new System.Text.Json.JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
            });

        }
    }
    public class ChartBuy
    {
        public string type { get; set; }
        public string label { get; set; }
        public List<decimal?> data { get; set; }
        public bool fill { get; set; }
        public int borderWidth { get; set; }
        public string borderColor { get; set; }
        public string backgroundColor { get; set; }
    }
    public class ChartDataBuy
    {
        public string sort { get; set; }
        public string label { get; set; }
        public decimal? data { get; set; }
        public decimal? data_nvl { get; set; }
        public decimal? data_hoachat { get; set; }
        public decimal? data_giantiep { get; set; }
        public List<MuahangModel> group { get; set; }
    }
    public class Chartbophan
    {
        public string sort { get; set; }
        public DepartmentModel bophan { get; set; }
        public decimal? thanhtien { get; set; }

        public DateTime pay_at { get; set; }
    }
}
