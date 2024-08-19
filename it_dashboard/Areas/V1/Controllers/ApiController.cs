

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

    public class ApiController : BaseController
    {
        private readonly IConfiguration _configuration;

        private readonly KT_Context _ktcontext;
        private readonly QLSX_Context _qlsxcontext;
        private readonly string sql = "SELECT TBL_DANHMUCCUAHANG.MACH," +
            "TBL_MIEN.MIEN ," +
            "TBL_DANHMUCTIEUDEBAOCAO.TENDVBC ," +
            "TBL_DANHMUCHANGHOA.MAHH," +
            "TBL_DANHMUCHANGHOA.TENHH," +
            "TBL_DANHMUCHANGHOA.NHOM," +
            "TBL_DANHMUCNHOMHANG.TENNHOM," +
            "TBL_DANHMUCHANGHOA.DVT," +
            "SUM(ROUND(CAST(DTA_CT_HOADON_XUAT.SOLUONG AS MONEY)*CAST(DTA_CT_HOADON_XUAT.DONGIA AS MONEY),0)) AS TIEN ," +
            "SUM(ROUND(CAST(DTA_CT_HOADON_XUAT.SOLUONG AS MONEY)*CAST(DTA_CT_HOADON_XUAT.DONGIA AS MONEY),0))- SUM(ROUND(ROUND(CAST(DTA_CT_HOADON_XUAT.SOLUONG AS MONEY)*CAST(DTA_CT_HOADON_XUAT.DONGIA AS MONEY),0)*DTA_CT_HOADON_XUAT.TYLECK/100,0))  AS DOANHTHU ," +
            "SUM(CAST(DTA_CT_HOADON_XUAT.SOLUONG AS MONEY)) AS SOLUONG," +
            "DTA_HOADON_XUAT.NgayLapHD," +
            "DTA_HOADON_XUAT.MAKH," +
            "TBL_DANHMUCKHACHHANG.DONVI ," +
            "TBL_DANHMUCKHACHHANG.MATINH," +
            "TBL_DANHMUCDONVI.TenTinh," +
            "TBL_DANHMUCKHACHHANG.MATDV ," +
            "TBL_DANHMUCTDV.TENTDV ," +
            "TBL_DANHMUCKHACHHANG.PHANLOAI," +
            " TBL_DANHMUCKHACHHANG.PHANLOAIKHACHHANG " +

            "FROM DTA_CT_HOADON_XUAT LEFT JOIN TBL_DANHMUCHANGHOA ON DTA_CT_HOADON_XUAT.maHH=TBL_DANHMUCHANGHOA.maHH LEFT JOIN TBL_DANHMUCNHOMHANG ON TBL_DANHMUCHANGHOA.NHOM = TBL_DANHMUCNHOMHANG.MANHOM," +
            "DTA_HOADON_XUAT LEFT JOIN   TBL_DANHMUCKHACHHANG ON DTA_HOADON_XUAT.makh=TBL_DANHMUCKHACHHANG.makh," +
            "TBL_DANHMUCTIEUDEBAOCAO, TBL_MIEN,TBL_DANHMUCCUAHANG ,TBL_DANHMUCKHACHHANG TBL_DANHMUCKHACHHANG_1 LEFT JOIN TBL_DANHMUCDONVI ON TBL_DANHMUCKHACHHANG_1.matinh=TBL_DANHMUCDONVI.MaTinh," +
            "TBL_DANHMUCKHACHHANG TBL_DANHMUCKHACHHANG_2  LEFT JOIN TBL_DANHMUCTDV ON TBL_DANHMUCKHACHHANG_2.MATDV=TBL_DANHMUCTDV.MaTDV " +
            "WHERE DTA_HOADON_XUAT.SOHD=DTA_CT_HOADON_XUAT.SOHD " +
            "AND DTA_HOADON_XUAT.NGAYLAPHD=DTA_CT_HOADON_XUAT.NGAYLAPHD " +
            "AND DTA_HOADON_XUAT.MACH=DTA_CT_HOADON_XUAT.MACH " +
            "and DTA_CT_HOADON_XUAT.kt=1 " +
            "AND TBL_DANHMUCKHACHHANG_1.MAKH=TBL_DANHMUCKHACHHANG.makh " +
            "AND TBL_DANHMUCKHACHHANG_2.MAKH=TBL_DANHMUCKHACHHANG.makh " +
            "and DTA_HOADON_XUAT.MaPL = 'BAN' " +
            "and DTA_HOADON_XUAT.SOHD_DT is not null " +

            "group by TBL_DANHMUCCUAHANG.MACH,TBL_DANHMUCHANGHOA.MAHH," +
            "TBL_DANHMUCHANGHOA.TENHH," +
            "TBL_DANHMUCHANGHOA.NHOM," +
            "TBL_DANHMUCNHOMHANG.TENNHOM," +
            "TBL_DANHMUCHANGHOA.DVT," +
            "TBL_DANHMUCTIEUDEBAOCAO.TENDVBC ," +
            "TBL_MIEN.MIEN ," +
            "DTA_HOADON_XUAT.MAKH,TBL_DANHMUCKHACHHANG.DONVI," +
            "DTA_HOADON_XUAT.NgayLapHD," +
            "TBL_DANHMUCTINH.TENTINH," +
            "TBL_DANHMUCKHACHHANG.MATDV," +
            "TBL_DANHMUCTDV.TENTDV," +
            "TBL_DANHMUCKHACHHANG.MATDV," +
            "TBL_DANHMUCKHACHHANG.PHANLOAI," +
            "TBL_DANHMUCKHACHHANG.PHANLOAIKHACHHANG," +
            "TBL_DANHMUCKHACHHANG.MATINH";

        private readonly string sql1 = "SELECT TBL_DANHMUCCUAHANG.MACH," +
            "TBL_MIEN.MIEN ," +
            "TBL_DANHMUCTIEUDEBAOCAO.TENDVBC ," +
            "TBL_DANHMUCHANGHOA.MAHH," +
            "TBL_DANHMUCHANGHOA.TENHH," +
            "TBL_DANHMUCHANGHOA.NHOM," +
            "TBL_DANHMUCNHOMHANG.TENNHOM," +
            "TBL_DANHMUCHANGHOA.DVT," +
            "SUM(ROUND(CAST(DTA_CT_HOADON_XUAT.SOLUONG AS MONEY)*CAST(DTA_CT_HOADON_XUAT.DONGIA AS MONEY),0)) AS TIEN ," +
            "SUM(ROUND(CAST(DTA_CT_HOADON_XUAT.SOLUONG AS MONEY)*CAST(DTA_CT_HOADON_XUAT.DONGIA AS MONEY),0))- SUM(ROUND(ROUND(CAST(DTA_CT_HOADON_XUAT.SOLUONG AS MONEY)*CAST(DTA_CT_HOADON_XUAT.DONGIA AS MONEY),0)*DTA_CT_HOADON_XUAT.TYLECK/100,0))  AS DOANHTHU ," +
            "SUM(CAST(DTA_CT_HOADON_XUAT.SOLUONG AS MONEY)) AS SOLUONG," +
            "DTA_HOADON_XUAT.NgayLapHD," +
            "DTA_HOADON_XUAT.MAKH," +
            "TBL_DANHMUCKHACHHANG.DONVI ," +
            "TBL_DANHMUCKHACHHANG.MATINH," +
            "TBL_DANHMUCDONVI.TenTinh," +
            "TBL_DANHMUCKHACHHANG.MATDV ," +
            "TBL_DANHMUCTDV.TENTDV ," +
            "TBL_DANHMUCKHACHHANG.PHANLOAI," +
            " TBL_DANHMUCKHACHHANG.PHANLOAIKHACHHANG " +

            "FROM DTA_CT_HOADON_XUAT LEFT JOIN TBL_DANHMUCHANGHOA ON DTA_CT_HOADON_XUAT.maHH=TBL_DANHMUCHANGHOA.maHH LEFT JOIN TBL_DANHMUCNHOMHANG ON TBL_DANHMUCHANGHOA.NHOM = TBL_DANHMUCNHOMHANG.MANHOM," +
            "DTA_HOADON_XUAT LEFT JOIN   TBL_DANHMUCKHACHHANG ON DTA_HOADON_XUAT.makh=TBL_DANHMUCKHACHHANG.makh," +
            "TBL_DANHMUCTIEUDEBAOCAO, TBL_MIEN,TBL_DANHMUCCUAHANG ,TBL_DANHMUCKHACHHANG TBL_DANHMUCKHACHHANG_1 LEFT JOIN TBL_DANHMUCDONVI ON TBL_DANHMUCKHACHHANG_1.matinh=TBL_DANHMUCDONVI.MaTinh," +
            "TBL_DANHMUCKHACHHANG TBL_DANHMUCKHACHHANG_2  LEFT JOIN TBL_DANHMUCTDV ON TBL_DANHMUCKHACHHANG_2.MATDV=TBL_DANHMUCTDV.MaTDV " +
            "WHERE DTA_HOADON_XUAT.SOHD=DTA_CT_HOADON_XUAT.SOHD " +
            "AND DTA_HOADON_XUAT.NGAYLAPHD=DTA_CT_HOADON_XUAT.NGAYLAPHD " +
            "AND DTA_HOADON_XUAT.MACH=DTA_CT_HOADON_XUAT.MACH " +
            "and DTA_CT_HOADON_XUAT.kt=1 " +
            "AND TBL_DANHMUCKHACHHANG_1.MAKH=TBL_DANHMUCKHACHHANG.makh " +
            "and DTA_HOADON_XUAT.MaPL = 'BAN' " +
            "and DTA_HOADON_XUAT.SOHD_DT is not null " +
            "AND TBL_DANHMUCKHACHHANG_2.MAKH=TBL_DANHMUCKHACHHANG.makh ";

        private readonly string groupby = " group by TBL_DANHMUCCUAHANG.MACH,TBL_DANHMUCHANGHOA.MAHH," +
            "TBL_DANHMUCHANGHOA.TENHH," +
            "TBL_DANHMUCHANGHOA.NHOM," +
            "TBL_DANHMUCNHOMHANG.TENNHOM," +
            "TBL_DANHMUCHANGHOA.DVT," +
            "TBL_DANHMUCTIEUDEBAOCAO.TENDVBC ," +
            "TBL_MIEN.MIEN ," +
            "DTA_HOADON_XUAT.MAKH,TBL_DANHMUCKHACHHANG.DONVI," +
            "DTA_HOADON_XUAT.NgayLapHD," +
            "TBL_DANHMUCDONVI.TenTinh," +
            "TBL_DANHMUCKHACHHANG.MATDV," +
            "TBL_DANHMUCTDV.TENTDV," +
            "TBL_DANHMUCKHACHHANG.MATDV," +
            "TBL_DANHMUCKHACHHANG.PHANLOAI," +
            "TBL_DANHMUCKHACHHANG.PHANLOAIKHACHHANG," +
            "TBL_DANHMUCKHACHHANG.MATINH";
        public ApiController(ItContext context, IConfiguration configuration, KT_Context ktcontext, QLSX_Context qlsxcontext) : base(context)
        {
            _configuration = configuration;
            _ktcontext = ktcontext;
            _qlsxcontext = qlsxcontext;

        }



        public async Task<JsonResult> list_sanpham()
        {
            var list = _ktcontext.SanphamModel.FromSqlRaw($"SELECT MAHH,TENHH FROM DTA_CT_HOADON_XUAT where MAHH != '' group by MAHH,TENHH").OrderBy(d => d.MAHH).ToList();

            return Json(list);
        }
        public async Task<JsonResult> list_khachhang()
        {
            var list = _ktcontext.KhachhangModel.FromSqlRaw($"SELECT MAKH,DONVI FROM DTA_HOADON_XUAT where MAKH != '' group by MAKH,DONVI").OrderBy(d => d.MAKH).ToList();

            return Json(list);
        }
        public async Task<JsonResult> list_tinh()
        {
            var list = _ktcontext.TinhModel.FromSqlRaw($"select DTA_HOADON_XUAT.MaTinh,TBL_DANHMUCDONVI.TenTinh from DTA_HOADON_XUAT LEFT JOIN TBL_DANHMUCDONVI ON DTA_HOADON_XUAT.MaTinh = TBL_DANHMUCDONVI.matinh where DTA_HOADON_XUAT.MaTinh != '' group by DTA_HOADON_XUAT.MaTinh,TBL_DANHMUCDONVI.TenTinh").Distinct().OrderBy(d => d.MATINH).ToList();

            return Json(list);
        }
        public async Task<JsonResult> list_nhom()
        {
            var list = _ktcontext.MaNhomModel.FromSqlRaw($"select TBL_DANHMUCHANGHOA.NHOM,TBL_DANHMUCNHOMHANG.TENNHOM from DTA_CT_HOADON_XUAT LEFT JOIN TBL_DANHMUCHANGHOA ON DTA_CT_HOADON_XUAT.maHH=TBL_DANHMUCHANGHOA.maHH LEFT JOIN TBL_DANHMUCNHOMHANG ON TBL_DANHMUCHANGHOA.NHOM = TBL_DANHMUCNHOMHANG.MANHOM where TBL_DANHMUCHANGHOA.NHOM != '' group by TBL_DANHMUCHANGHOA.NHOM,TBL_DANHMUCNHOMHANG.TENNHOM").OrderBy(d => d.Nhom).ToList();
            return Json(list);
        }

        public async Task<JsonResult> list_PLKH()
        {
            var list = _ktcontext.TBL_DANHMUCPHANLOAIKHACHHANGModel.OrderBy(d => d.tenphanloai).ToList();
            return Json(list);
        }
        [HttpPost]
        public async Task<JsonResult> homebadge(List<string>? kh_exception, List<string>? list_tinh, List<string>? list_nhom, DateTime? tungay, DateTime? denngay)
        {
            var sqla = sql1;

            if (kh_exception != null && kh_exception.Count > 0)
            {
                var kh_exception1 = string.Join("','", kh_exception.ToArray());
                sqla += $" AND DTA_HOADON_XUAT.MAKH NOT IN('{kh_exception1}')";
            }
            if (list_tinh != null && list_tinh.Count > 0)
            {
                var list_tinh1 = string.Join("','", list_tinh.ToArray());
                sqla += $" AND TBL_DANHMUCKHACHHANG.MATINH IN('{list_tinh1}')";
            }
            if (list_nhom != null && list_nhom.Count > 0)
            {
                var list_nhom1 = string.Join("','", list_nhom.ToArray());
                sqla += $" AND TBL_DANHMUCHANGHOA.NHOM IN('{list_nhom1}')";
            }
            if (tungay != null && tungay.HasValue)
            {
                if (tungay.Value.Kind == DateTimeKind.Utc)
                {
                    tungay = tungay.Value.ToLocalTime();
                }
                sqla += $" AND DTA_HOADON_XUAT.NGAYLAPHD >= '{tungay.Value.ToString("yyyy-MM-dd")}'";
            }
            if (denngay != null && denngay.HasValue)
            {
                if (denngay.Value.Kind == DateTimeKind.Utc)
                {
                    denngay = denngay.Value.ToLocalTime();
                }

                sqla += $" AND DTA_HOADON_XUAT.NGAYLAPHD <= '{denngay.Value.ToString("yyyy-MM-dd")}'";
            }
            sqla += groupby;
            var list = _ktcontext.RawDataModel.FromSqlRaw($"{sqla}").Distinct().ToList();

            var thanhtien = list.Sum(d => d.DOANHTHU);
            var soluong = list.Sum(d => d.SOLUONG);
            var soluongsp = list.Select(d => d.MAHH).Distinct().Count();

            //var labels_phanloaikh = list.Select(d => d.PHANLOAIKHACHHANG).Distinct().ToList();
            var data_phanloaikh = list.GroupBy(d => d.PHANLOAIKHACHHANG).Select(group => new
            {
                label = group.Key,
                data = group.Sum(d => d.DOANHTHU)
            }).ToList();
            var phanloaikh = new { labels = data_phanloaikh.Select(d => d.label).ToList(), datasets = new List<Chart>() { new Chart { label = "Doanh thu", data = data_phanloaikh.Select(d => d.data).ToList() } } };

            var data_phanloai = list.GroupBy(d => d.PHANLOAI).Select(group => new
            {
                label = group.Key,
                data = group.Sum(d => d.DOANHTHU)
            }).ToList();
            var phanloai = new { labels = data_phanloai.Select(d => d.label).ToList(), datasets = new List<Chart>() { new Chart { label = "Doanh thu", data = data_phanloai.Select(d => d.data).ToList() } } };

            var data_phanloaidonvi = list.GroupBy(d => d.TENDVBC).Select(group => new
            {
                label = group.Key,
                data = group.Sum(d => d.DOANHTHU)
            }).ToList();
            var phanloaidonvi = new { labels = data_phanloaidonvi.Select(d => d.label).ToList(), datasets = new List<Chart>() { new Chart { label = "Doanh thu", data = data_phanloaidonvi.Select(d => d.data).ToList() } } };

            return Json(new { thanhtien = thanhtien, soluong = soluong, soluongsp = soluongsp, phanloaikh = phanloaikh, phanloai = phanloai, phanloaidonvi = phanloaidonvi });
        }

        [HttpPost]
        public async Task<JsonResult> topsp(List<string>? kh_exception, List<string>? list_tinh, List<string>? list_nhom, DateTime? tungay, DateTime? denngay, int limit = 5)
        {
            var sqla = sql1;
            if (kh_exception != null && kh_exception.Count > 0)
            {
                var kh_exception1 = string.Join("','", kh_exception.ToArray());
                sqla += $" AND DTA_HOADON_XUAT.MAKH NOT IN('{kh_exception1}')";
            }
            if (list_tinh != null && list_tinh.Count > 0)
            {
                var list_tinh1 = string.Join("','", list_tinh.ToArray());
                sqla += $" AND TBL_DANHMUCKHACHHANG.MATINH IN('{list_tinh1}')";
            }
            if (list_nhom != null && list_nhom.Count > 0)
            {
                var list_nhom1 = string.Join("','", list_nhom.ToArray());
                sqla += $" AND TBL_DANHMUCHANGHOA.NHOM IN('{list_nhom1}')";
            }
            if (tungay != null && tungay.HasValue)
            {
                if (tungay.Value.Kind == DateTimeKind.Utc)
                {
                    tungay = tungay.Value.ToLocalTime();
                }
                sqla += $" AND DTA_HOADON_XUAT.NGAYLAPHD >= '{tungay.Value.ToString("yyyy-MM-dd")}'";
            }
            if (denngay != null && denngay.HasValue)
            {
                if (denngay.Value.Kind == DateTimeKind.Utc)
                {
                    denngay = denngay.Value.ToLocalTime();
                }

                sqla += $" AND DTA_HOADON_XUAT.NGAYLAPHD <= '{denngay.Value.ToString("yyyy-MM-dd")}'";
            }
            sqla += groupby;
            var list = _ktcontext.RawDataModel.FromSqlRaw($"{sqla}").Distinct().ToList();


            var data = list.GroupBy(d => new { d.MAHH }).Select(group => new
            {
                MAHH = group.Key.MAHH,
                TENHH = group.First().TENHH,
                doanhthu = group.Sum(d => d.DOANHTHU),
                soluong = group.Sum(d => d.SOLUONG)
            }).OrderByDescending(d => d.doanhthu).Take(limit).ToList();

            return Json(new { data = data });
        }

        [HttpPost]
        public async Task<JsonResult> topnhom(List<string>? kh_exception, List<string>? list_tinh, List<string>? list_nhom, DateTime? tungay, DateTime? denngay, int limit = 5)
        {

            var sqla = sql1;
            if (kh_exception != null && kh_exception.Count > 0)
            {
                var kh_exception1 = string.Join("','", kh_exception.ToArray());
                sqla += $" AND DTA_HOADON_XUAT.MAKH NOT IN('{kh_exception1}')";
            }
            if (list_tinh != null && list_tinh.Count > 0)
            {
                var list_tinh1 = string.Join("','", list_tinh.ToArray());
                sqla += $" AND TBL_DANHMUCKHACHHANG.MATINH IN('{list_tinh1}')";
            }
            if (list_nhom != null && list_nhom.Count > 0)
            {
                var list_nhom1 = string.Join("','", list_nhom.ToArray());
                sqla += $" AND TBL_DANHMUCHANGHOA.NHOM IN('{list_nhom1}')";
            }
            if (tungay != null && tungay.HasValue)
            {
                if (tungay.Value.Kind == DateTimeKind.Utc)
                {
                    tungay = tungay.Value.ToLocalTime();
                }
                sqla += $" AND DTA_HOADON_XUAT.NGAYLAPHD >= '{tungay.Value.ToString("yyyy-MM-dd")}'";
            }
            if (denngay != null && denngay.HasValue)
            {
                if (denngay.Value.Kind == DateTimeKind.Utc)
                {
                    denngay = denngay.Value.ToLocalTime();
                }

                sqla += $" AND DTA_HOADON_XUAT.NGAYLAPHD <= '{denngay.Value.ToString("yyyy-MM-dd")}'";
            }
            sqla += groupby;
            var list = _ktcontext.RawDataModel.FromSqlRaw($"{sqla}").Distinct().ToList();

            var data = list.GroupBy(d => new { d.NHOM, d.TENNHOM }).Select(group => new
            {
                NHOM = group.Key.NHOM,
                TENNHOM = group.Key.TENNHOM,
                data = group.Sum(d => d.DOANHTHU)
            }).OrderByDescending(d => d.data).Take(limit).ToList();

            return Json(new { data = data });
        }

        [HttpPost]
        public async Task<JsonResult> toptdv(List<string>? kh_exception, List<string>? list_tinh, List<string>? list_nhom, DateTime? tungay, DateTime? denngay, int limit = 5)
        {

            var sqla = sql1;
            if (kh_exception != null && kh_exception.Count > 0)
            {
                var kh_exception1 = string.Join("','", kh_exception.ToArray());
                sqla += $" AND DTA_HOADON_XUAT.MAKH NOT IN('{kh_exception1}')";
            }
            if (list_tinh != null && list_tinh.Count > 0)
            {
                var list_tinh1 = string.Join("','", list_tinh.ToArray());
                sqla += $" AND TBL_DANHMUCKHACHHANG.MATINH IN('{list_tinh1}')";
            }
            if (list_nhom != null && list_nhom.Count > 0)
            {
                var list_nhom1 = string.Join("','", list_nhom.ToArray());
                sqla += $" AND TBL_DANHMUCHANGHOA.NHOM IN('{list_nhom1}')";
            }
            if (tungay != null && tungay.HasValue)
            {
                if (tungay.Value.Kind == DateTimeKind.Utc)
                {
                    tungay = tungay.Value.ToLocalTime();
                }
                sqla += $" AND DTA_HOADON_XUAT.NGAYLAPHD >= '{tungay.Value.ToString("yyyy-MM-dd")}'";
            }
            if (denngay != null && denngay.HasValue)
            {
                if (denngay.Value.Kind == DateTimeKind.Utc)
                {
                    denngay = denngay.Value.ToLocalTime();
                }

                sqla += $" AND DTA_HOADON_XUAT.NGAYLAPHD <= '{denngay.Value.ToString("yyyy-MM-dd")}'";
            }
            sqla += groupby;
            var list = _ktcontext.RawDataModel.FromSqlRaw($"{sqla}").Distinct().ToList();
            var data = list.GroupBy(d => new { d.MATDV, d.TENTDV }).Select(group => new
            {
                MATDV = group.Key.MATDV,
                TENTDV = group.Key.TENTDV,
                data = group.Sum(d => d.DOANHTHU)
            }).OrderByDescending(d => d.data).Take(limit).ToList();

            return Json(new { data = data });
        }

        [HttpPost]
        public async Task<JsonResult> toptinh(List<string>? kh_exception, List<string>? list_tinh, List<string>? list_nhom, DateTime? tungay, DateTime? denngay, int limit = 5)
        {

            var sqla = sql1;
            if (kh_exception != null && kh_exception.Count > 0)
            {
                var kh_exception1 = string.Join("','", kh_exception.ToArray());
                sqla += $" AND DTA_HOADON_XUAT.MAKH NOT IN('{kh_exception1}')";
            }
            if (list_tinh != null && list_tinh.Count > 0)
            {
                var list_tinh1 = string.Join("','", list_tinh.ToArray());
                sqla += $" AND TBL_DANHMUCKHACHHANG.MATINH IN('{list_tinh1}')";
            }
            if (list_nhom != null && list_nhom.Count > 0)
            {
                var list_nhom1 = string.Join("','", list_nhom.ToArray());
                sqla += $" AND TBL_DANHMUCHANGHOA.NHOM IN('{list_nhom1}')";
            }
            if (tungay != null && tungay.HasValue)
            {
                if (tungay.Value.Kind == DateTimeKind.Utc)
                {
                    tungay = tungay.Value.ToLocalTime();
                }
                sqla += $" AND DTA_HOADON_XUAT.NGAYLAPHD >= '{tungay.Value.ToString("yyyy-MM-dd")}'";
            }
            if (denngay != null && denngay.HasValue)
            {
                if (denngay.Value.Kind == DateTimeKind.Utc)
                {
                    denngay = denngay.Value.ToLocalTime();
                }

                sqla += $" AND DTA_HOADON_XUAT.NGAYLAPHD <= '{denngay.Value.ToString("yyyy-MM-dd")}'";
            }
            sqla += groupby;
            var list = _ktcontext.RawDataModel.FromSqlRaw($"{sqla}").Distinct().ToList();

            var data = list.GroupBy(d => new { d.MATINH, d.TENTINH }).Select(group => new
            {
                MATINH = group.Key.MATINH,
                TENTINH = group.Key.TENTINH,
                data = group.Sum(d => d.DOANHTHU)
            }).OrderByDescending(d => d.data).Take(limit).ToList();

            return Json(new { data = data });
        }


        [HttpPost]
        public async Task<JsonResult> topkh(List<string>? kh_exception, List<string>? list_tinh, List<string>? list_nhom, DateTime? tungay, DateTime? denngay, int limit = 5)
        {

            var sqla = sql1;
            if (kh_exception != null && kh_exception.Count > 0)
            {
                var kh_exception1 = string.Join("','", kh_exception.ToArray());
                sqla += $" AND DTA_HOADON_XUAT.MAKH NOT IN('{kh_exception1}')";
            }
            if (list_tinh != null && list_tinh.Count > 0)
            {
                var list_tinh1 = string.Join("','", list_tinh.ToArray());
                sqla += $" AND TBL_DANHMUCKHACHHANG.MATINH IN('{list_tinh1}')";
            }
            if (list_nhom != null && list_nhom.Count > 0)
            {
                var list_nhom1 = string.Join("','", list_nhom.ToArray());
                sqla += $" AND TBL_DANHMUCHANGHOA.NHOM IN('{list_nhom1}')";
            }
            if (tungay != null && tungay.HasValue)
            {
                if (tungay.Value.Kind == DateTimeKind.Utc)
                {
                    tungay = tungay.Value.ToLocalTime();
                }
                sqla += $" AND DTA_HOADON_XUAT.NGAYLAPHD >= '{tungay.Value.ToString("yyyy-MM-dd")}'";
            }
            if (denngay != null && denngay.HasValue)
            {
                if (denngay.Value.Kind == DateTimeKind.Utc)
                {
                    denngay = denngay.Value.ToLocalTime();
                }

                sqla += $" AND DTA_HOADON_XUAT.NGAYLAPHD <= '{denngay.Value.ToString("yyyy-MM-dd")}'";
            }
            sqla += groupby;
            var list = _ktcontext.RawDataModel.FromSqlRaw($"{sqla}").Distinct().ToList();

            var data = list.GroupBy(d => new { d.MAKH, d.DONVI }).Select(group => new
            {
                MAKH = group.Key.MAKH,
                DONVI = group.Key.DONVI,
                data = group.Sum(d => d.DOANHTHU)
            }).OrderByDescending(d => d.data).Take(limit).ToList();

            return Json(new { data = data });
        }


        [HttpPost]
        public async Task<JsonResult> doanhthu(List<string>? kh_exception, List<string>? list_tinh, List<string>? list_nhom, DateTime? tungay, DateTime? denngay, string timetype = "Month")
        {

            var sqla = sql1;
            if (kh_exception != null && kh_exception.Count > 0)
            {
                var kh_exception1 = string.Join("','", kh_exception.ToArray());
                sqla += $" AND DTA_HOADON_XUAT.MAKH NOT IN('{kh_exception1}')";
            }
            if (list_tinh != null && list_tinh.Count > 0)
            {
                var list_tinh1 = string.Join("','", list_tinh.ToArray());
                sqla += $" AND TBL_DANHMUCKHACHHANG.MATINH IN('{list_tinh1}')";
            }
            if (list_nhom != null && list_nhom.Count > 0)
            {
                var list_nhom1 = string.Join("','", list_nhom.ToArray());
                sqla += $" AND TBL_DANHMUCHANGHOA.NHOM IN('{list_nhom1}')";
            }
            if (tungay != null && tungay.HasValue)
            {
                if (tungay.Value.Kind == DateTimeKind.Utc)
                {
                    tungay = tungay.Value.ToLocalTime();
                }
                sqla += $" AND DTA_HOADON_XUAT.NGAYLAPHD >= '{tungay.Value.ToString("yyyy-MM-dd")}'";
            }
            if (denngay != null && denngay.HasValue)
            {
                if (denngay.Value.Kind == DateTimeKind.Utc)
                {
                    denngay = denngay.Value.ToLocalTime();
                }

                sqla += $" AND DTA_HOADON_XUAT.NGAYLAPHD <= '{denngay.Value.ToString("yyyy-MM-dd")}'";
            }
            sqla += groupby;
            var list = _ktcontext.RawDataModel.FromSqlRaw($"{sqla}").Distinct().ToList();

            var data = new List<Chart1>();
            if (timetype == "Year")
            {
                data = list.GroupBy(d => new { d.NgayLapHD.Year }).Select(group => new Chart1
                {
                    sort = group.Key.Year.ToString(),
                    label = group.Key.Year.ToString(),
                    data = group.Sum(d => d.DOANHTHU)
                }).OrderBy(d => d.sort).ToList();

            }
            else
            {
                data = list.GroupBy(d => new { year = d.NgayLapHD.Year, month = d.NgayLapHD.Month }).Select(group => new Chart1
                {
                    sort = group.Key.year + "-" + group.Key.month.ToString("d2"),
                    label = group.Key.month.ToString("d2") + "/" + group.Key.year,
                    data = group.Sum(d => d.DOANHTHU)
                }).OrderBy(d => d.sort).ToList();
            }
            var doanhthu = new { labels = data.Select(d => d.label).ToList(), datasets = new List<Chart>() { new Chart { label = "Doanh thu", data = data.Select(d => d.data).ToList() } } };

            return Json(new
            {
                doanhthu = doanhthu
            });
        }
        [HttpPost]
        public async Task<JsonResult> chitiet(List<string>? list_sanpham, List<string>? list_khachhang, List<string>? list_tinh, List<string>? list_nhom, DateTime? tungay, DateTime? denngay)
        {

            var sqla = sql1;
            if (list_sanpham != null && list_sanpham.Count > 0)
            {
                var list_sanpham1 = string.Join("','", list_sanpham.ToArray());
                sqla += $" AND TBL_DANHMUCHANGHOA.MAHH IN('{list_sanpham1}')";
            }
            if (list_khachhang != null && list_khachhang.Count > 0)
            {
                var list_khachhang1 = string.Join("','", list_khachhang.ToArray());
                sqla += $" AND DTA_HOADON_XUAT.MAKH IN('{list_khachhang1}')";
            }
            if (list_tinh != null && list_tinh.Count > 0)
            {
                var list_tinh1 = string.Join("','", list_tinh.ToArray());
                sqla += $" AND TBL_DANHMUCKHACHHANG.MATINH IN('{list_tinh1}')";
            }
            if (list_nhom != null && list_nhom.Count > 0)
            {
                var list_nhom1 = string.Join("','", list_nhom.ToArray());
                sqla += $" AND TBL_DANHMUCHANGHOA.NHOM IN('{list_nhom1}')";
            }
            if (tungay != null && tungay.HasValue)
            {
                if (tungay.Value.Kind == DateTimeKind.Utc)
                {
                    tungay = tungay.Value.ToLocalTime();
                }
                sqla += $" AND DTA_HOADON_XUAT.NGAYLAPHD >= '{tungay.Value.ToString("yyyy-MM-dd")}'";
            }
            if (denngay != null && denngay.HasValue)
            {
                if (denngay.Value.Kind == DateTimeKind.Utc)
                {
                    denngay = denngay.Value.ToLocalTime();
                }

                sqla += $" AND DTA_HOADON_XUAT.NGAYLAPHD <= '{denngay.Value.ToString("yyyy-MM-dd")}'";
            }
            sqla += groupby;
            var list = _ktcontext.RawDataModel.FromSqlRaw($"{sqla}").Distinct().ToList();

            var data = list.GroupBy(d => new { d.MAHH, d.TENHH, d.DVT, d.MAKH, d.DONVI }).Select(group => new
            {
                MAHH = group.Key.MAHH,
                TENHH = group.Key.TENHH,
                DVT = group.Key.DVT,
                MAKH = group.Key.MAKH,
                DONVI = group.Key.DONVI,
                doanhthu = group.Sum(d => d.DOANHTHU),
                soluong = group.Sum(d => d.SOLUONG)
            }).OrderByDescending(d => d.doanhthu).ToList();

            return Json(new { data = data });
        }


        public async Task<JsonResult> tonkho(int limit = 5)
        {
            var current = DateTime.Now;
            var thang = current.Month;
            var nam = current.Year;
            var tungay = current.ToString("yyyy-MM-dd");
            var sqla = $"EXECUTE laythongtintonkho_tatca_tatca @thang = {thang}, @nam = {nam}, @MAHH = '',@tungay='{tungay}',@soluong=0 ";
            Console.WriteLine(sqla);
            var data = _ktcontext.TonkhoModel.FromSqlRaw($"{sqla}").ToList().OrderByDescending(d => d.soluong_ton).Take(limit).ToList();
            return Json(new { data = data });
        }
        public async Task<JsonResult> tinhtrangsanpham(List<DateTime> dates, string search)
        {
            var current = DateTime.Now;
            if (dates == null)
            {
                return Json(new { success = false });
            }
            if (dates[0] != null)
            {
                if (dates[0].Kind == DateTimeKind.Utc)
                {
                    dates[0] = dates[0].ToLocalTime();
                }
            }
            if (dates[1] != null)
            {
                if (dates[1].Kind == DateTimeKind.Utc)
                {
                    dates[1] = dates[1].ToLocalTime();
                }
            }
            var tungay = dates[0].ToString("yyyy-MM-dd");
            var denngay = dates[1].ToString("yyyy-MM-dd");
            var sqla = $"EXECUTE BI_DODANG_HOANTHANH @tungay='{tungay}',@dengay='{denngay}' ";
            //Console.WriteLine(sqla);
            var data = _qlsxcontext.Tinhtrangsanpham.FromSqlRaw($"{sqla}").ToList().OrderByDescending(d => d.HOANTHANH).ToList();
            if (search != null && search != "")
            {
                data = data.Where(d => d.MAHH_GOC_1.Contains(search) || d.TENHH.Contains(search) || d.MALO_GOC.Contains(search)).ToList();
            }
            var tong_colo = data.Sum(d => d.COLO_GOC);
            return Json(new { success = true, data = data, tong_colo }, new System.Text.Json.JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
            });
        }
        public async Task<JsonResult> bomchecklist(int qui, int nam)
        {
            var current = DateTime.Now;

            var sqla = $"EXECUTE BOM_CHECKLIST_LOC @qui='{qui}',@nam='{nam}' ";
            Console.WriteLine(sqla);
            var data = _qlsxcontext.Bomchecklist.FromSqlRaw($"{sqla}").ToList().OrderByDescending(d => d.masp_1).ToList();
            return Json(new { success = true, data = data }, new System.Text.Json.JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
            });
        }
    }
    public class Chart
    {
        public string label { get; set; }
        public List<double> data { get; set; }
    }
    public class Chart1
    {
        public string sort { get; set; }
        public string label { get; set; }
        public double data { get; set; }
    }
}