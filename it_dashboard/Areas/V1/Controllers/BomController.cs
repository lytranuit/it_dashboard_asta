using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Spire.Xls;
using System;
using System.Collections;
using System.Data;
using System.Text.Json.Serialization;
using Vue.Data;
using Vue.Models;

namespace it_template.Areas.V1.Controllers
{

    public class BomController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<UserModel> UserManager;
        private readonly Buy_Context _Buy_Context;
        private readonly QLSX_Context _QLSX_Context;
        public BomController(ItContext context, UserManager<UserModel> UserMgr, QLSX_Context QLSX_Context, Buy_Context Buy_Context, IConfiguration configuration) : base(context)
        {
            _QLSX_Context = QLSX_Context;
            _Buy_Context = Buy_Context;
            _configuration = configuration;
            UserManager = UserMgr;
        }
        [HttpPost]
        public async Task<JsonResult> Table()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            var mahh = Request.Form["filters[mahh]"].FirstOrDefault();
            var tenhh = Request.Form["filters[tenhh]"].FirstOrDefault();
            var mahh_goc = Request.Form["filters[mahh_goc]"].FirstOrDefault();
            var tenhh_goc = Request.Form["filters[tenhh_goc]"].FirstOrDefault();
            var colo = Request.Form["filters[colo]"].FirstOrDefault();
            var mapl = Request.Form["filters[mapl]"].FirstOrDefault();
            var nvl = Request.Form["filters[nvl]"].FirstOrDefault();
            var status_string = Request.Form["filters[status]"].FirstOrDefault();
            int status = status_string != null ? Convert.ToInt32(status_string) : 0;
            var date_from = Request.Form["filters[dates][0]"].FirstOrDefault();
            var date_to = Request.Form["filters[dates][1]"].FirstOrDefault();
            var date0 = DateTime.Now;
            var date1 = DateTime.Now;

            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var user = await UserManager.GetUserAsync(currentUser);
            var is_RD = await UserManager.IsInRoleAsync(user, "RD");

            if (date_from != null)
            {

                date0 = DateTime.Parse(date_from);
                if (date0.Kind == DateTimeKind.Utc)
                {
                    date0 = date0.ToLocalTime();
                }
            }
            if (date_to != null)
            {

                date1 = DateTime.Parse(date_to);
                if (date1.Kind == DateTimeKind.Utc)
                {
                    date1 = date1.ToLocalTime();
                }
            }

            int skip = start != null ? Convert.ToInt32(start) : 0;
            decimal? colo_d = colo != null ? Convert.ToDecimal(colo) : null;

            var list_tonkho = _QLSX_Context.PackageModel.Where(d => d.soluong != 0 && d.deleted_at == null && d.tinhtrang != "Chờ xử lý" && d.tinhtrang != "Loại bỏ").GroupBy(d => new { d.mahh }).Select(d => new PackageInventory1()
            {
                mahh = d.Key.mahh,
                soluong_chapnhan = (decimal)d.Where(d => d.tinhtrang == "Chấp nhận" || d.tinhtrang == "Chấp nhận tạm").Sum(e => e.soluong),
                //soluong_ = (decimal)d.Where(d => d.tinhtrang == "Chấp nhận" || d.tinhtrang == "Chấp nhận tạm").Sum(e => e.soluong),
                soluong = (decimal)d.Sum(e => e.soluong)
            }).ToList();
            var query = _QLSX_Context.BomModel.Where(d => 1 == 1);
            if (nvl != null)
            {
                query = query.Where(d => d.manvl == nvl);
            }
            var customerData = query.GroupBy(d => new { d.mahh, d.colo }).Select(d => new BOM()
            {
                mahh = d.Key.mahh,
                colo = d.Key.colo.Value,
                tenhh = d.FirstOrDefault().tenhh,
                dvt = d.FirstOrDefault().dvt,
                mahh_goc = d.FirstOrDefault().mahh_goc,
                tenhh_goc = d.FirstOrDefault().tenhh_goc,
                quicach = _QLSX_Context.ProductModel.Where(e => e.mahh == d.Key.mahh).FirstOrDefault().quicachdonggoi,
                details = d.Select(e => new BOM_DETAILS()
                {
                    id = e.id,
                    stt = e.stt.Value,
                    manvl = e.manvl,
                    tennvl = e.tennvl,
                    dvtnvl = e.dvtnvl,
                    me = e.me.Value,
                    soluong = e.soluong.Value,
                    thaythe = new List<BOM_DETAILS>()
                }).ToList()
            }).ToList();


            if (is_RD)
            {
                var list_spRD = _QLSX_Context.ProductModel.Where(d => d.mapl == "13").Select(d => d.mahh).ToList();
                customerData = customerData.Where(d => list_spRD.Contains(d.mahh)).ToList();
            }
            int recordsTotal = customerData.Count();
            if (mahh != null && mahh != "")
            {
                customerData = customerData.Where(d => d.mahh.Contains(mahh)).ToList();
            }

            if (tenhh != null && tenhh != "")
            {
                customerData = customerData.Where(d => d.tenhh.ToLower().Contains(tenhh.ToLower())).ToList();
            }
            if (mahh_goc != null && mahh_goc != "")
            {
                customerData = customerData.Where(d => d.mahh_goc.Contains(mahh_goc)).ToList();
            }
            if (tenhh_goc != null && tenhh_goc != "")
            {
                customerData = customerData.Where(d => d.tenhh_goc.ToLower().Contains(tenhh_goc.ToLower())).ToList();
            }
            if (colo_d != null)
            {
                customerData = customerData.Where(d => d.colo == colo_d).ToList();
            }
            if (mapl != null && mapl != "")
            {
                var find_mahh = _QLSX_Context.ProductModel.Where(d => d.mapl == mapl).Select(d => d.mahh).ToList();

                customerData = customerData.Where(d => find_mahh.Contains(d.mahh)).ToList();
            }

            var datapost = customerData.OrderBy(d => d.mahh).ToList();
            int recordsFiltered = datapost.Count();
            if (status == 0)
            {
                datapost = datapost.Skip(skip).Take(pageSize).ToList();
            }


            var dutru_all = _Buy_Context.DutruChitietModel.Where(d => d.mahh != null).Include(d => d.dutru).Where(d => d.dutru.deleted_at == null).Select(d => new
            {
                mahh = d.mahh,
                created_at = d.dutru.created_at,
                id = d.dutru.id,
                code = d.dutru.code,
                status_id = d.dutru.status_id
            }).ToList();
            var muahang_all = _Buy_Context.MuahangChitietModel.Where(d => d.mahh != null).Include(d => d.muahang).Where(d => d.muahang.deleted_at == null).Select(d => new
            {
                mahh = d.mahh,
                created_at = d.muahang.created_at,
                id = d.muahang.id,
                code = d.muahang.code,
                status_id = d.muahang.status_id,
                is_dathang = d.muahang.is_dathang,
                is_nhanhang = d.muahang.is_nhanhang,
                is_thanhtoan = d.muahang.is_thanhtoan,
                date_finish = d.muahang.date_finish,
                loaithanhtoan = d.muahang.loaithanhtoan
            }).ToList();

            //var data = new ArrayList();
            foreach (var record in datapost)
            {
                foreach (var item in record.details)
                {
                    ///Ton
                    var ton_mahh = list_tonkho.Where(d => d.mahh == item.manvl).FirstOrDefault();
                    if (ton_mahh != null)
                    {
                        item.tonkho = ton_mahh.soluong;
                        item.tonkho_chapnhan = ton_mahh.soluong_chapnhan;
                    }
                    ///
                    item.status = 1; /// Không đủ tồn
                    if (item.manvl != "N/A")
                    {
                        if (item.tonkho_chapnhan >= item.soluong)
                            item.status = 2; /// Đủ tồn
                        else if (item.tonkho >= item.soluong)
                        {
                            item.status = 3; /// Đủ tồn + Đang biệt trữ ... 
                        }
                    }
                    else
                    {
                        item.status = 2; /// Đủ tồn + Đang biệt trữ ... 
                    }




                    ///Thay the
                    item.thaythe = _QLSX_Context.BomThaytheModel.Where(d => d.manvl == item.manvl && d.mahh == record.mahh && d.colo == record.colo).Select(e => new BOM_DETAILS()
                    {
                        id = e.id,
                        stt = e.stt_thaythe,
                        manvl = e.manvl_thaythe,
                        tennvl = e.tennvl_thaythe,
                        dvtnvl = e.dvtnvl_thaythe,
                        me = e.me_thaythe.Value,
                        soluong = e.soluong_thaythe.Value
                    }).ToList();

                    foreach (var item1 in item.thaythe)
                    {
                        var ton_mahh_thaythe = list_tonkho.Where(d => d.mahh == item1.manvl).FirstOrDefault();
                        if (ton_mahh_thaythe != null)
                        {
                            item1.tonkho = ton_mahh_thaythe.soluong;
                            item1.tonkho_chapnhan = ton_mahh_thaythe.soluong_chapnhan;
                        }
                        if (item.status == 1)
                        {
                            if (item1.tonkho_chapnhan >= item1.soluong)
                                item.status = 2; /// Đủ tồn
                            else if (item1.tonkho >= item1.soluong)
                            {
                                item.status = 3; /// Đủ tồn + Đang biệt trữ ... 
                            }
                        }
                    }


                    ////Du tru
                    var dutru = dutru_all.Where(d => d.mahh == item.manvl && d.created_at >= date0 && d.created_at <= date1).OrderBy(d => d.created_at).Select(d => new DUTRU_DETAILS()
                    {
                        id = d.id,
                        code = d.code,
                        status_id = d.status_id.Value
                    }).ToList();

                    item.dutru = dutru;


                    ////Muahang
                    var muahang = muahang_all.Where(d => d.mahh == item.manvl && d.created_at >= date0 && d.created_at <= date1).OrderBy(d => d.created_at).Select(d => new MUAHANG_DETAILS()
                    {
                        id = d.id,
                        code = d.code,
                        status_id = d.status_id.Value,
                        is_dathang = d.is_dathang,
                        is_nhanhang = d.is_nhanhang,
                        is_thanhtoan = d.is_thanhtoan,
                        date_finish = d.date_finish,
                        loaithanhtoan = d.loaithanhtoan
                    }).ToList();

                    item.muahang = muahang;
                }
                var count_status_1 = record.details.Where(d => d.status == 1).Count();
                var count_status_3 = record.details.Where(d => d.status == 3).Count();
                if (count_status_1 > 0)
                {
                    record.status = 1; /// Không đủ tồn
                }
                else if (count_status_3 > 0)
                {
                    record.status = 2;  /// Không thiếu hồ sơ
                }
                else
                {
                    record.status = 3; /// Đã sẵn sàng
                }

            }
            if (status > 0)
            {
                datapost = datapost.Where(d => d.status == status).ToList();
            }

            var jsonData = new { draw = draw, recordsFiltered = recordsFiltered, recordsTotal = recordsTotal, data = datapost };
            return Json(jsonData, new System.Text.Json.JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
            });
        }

    }
    public class BOM
    {
        public string id { get; set; }
        public string label { get; set; }
        public string mahh { get; set; }
        public decimal colo { get; set; }
        public string tenhh { get; set; }
        public string dvt { get; set; }
        public string mahh_goc { get; set; }
        public string tenhh_goc { get; set; }
        public string quicach { get; set; }
        public string dangbaoche { get; set; }

        public List<BOM_DETAILS> details { get; set; }
        public int status { get; set; }
    }
    public class BOM_DETAILS
    {
        public int id { get; set; }
        public int stt { get; set; }
        public string manvl { get; set; }
        public string tennvl { get; set; }
        public string dvtnvl { get; set; }
        public double me { get; set; }
        public decimal? soluong { get; set; }
        public decimal tonkho { get; set; }
        public decimal tonkho_chapnhan { get; set; }
        public int status { get; set; }
        public string? manhom { get; set; }
        public List<BOM_DETAILS> thaythe { get; set; }

        public List<DUTRU_DETAILS> dutru { get; set; }

        public List<MUAHANG_DETAILS> muahang { get; set; }
    }

    public class PackageInventory1
    {
        public string mahh { get; set; }
        public decimal soluong { get; set; }
        public decimal soluong_chapnhan { get; set; }
    }


    public class DUTRU_DETAILS
    {
        public int id { get; set; }
        public string code { get; set; }

        public int status_id { get; set; }
    }
    public class MUAHANG_DETAILS
    {
        public int id { get; set; }
        public string code { get; set; }

        public int status_id { get; set; }

        public DateTime? date_finish { get; set; }

        public bool? is_dathang { get; set; }

        public bool? is_thanhtoan { get; set; }
        public bool? is_nhanhang { get; set; }
        public string? loaithanhtoan { get; set; }
    }
}
