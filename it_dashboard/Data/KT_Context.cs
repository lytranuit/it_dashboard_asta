
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using Vue.Models;

namespace Vue.Data
{
    public class KT_Context : DbContext
    {
        private IActionContextAccessor actionAccessor;
        private UserManager<UserModel> UserManager;
        public KT_Context(DbContextOptions<KT_Context> options) : base(options)
        {

        }


        public DbSet<TBL_DANHMUCKHO> TBL_DANHMUCKHO { get; set; }
        public DbSet<MaNhomModel> MaNhomModel { get; set; }
        public DbSet<TinhModel> TinhModel { get; set; }
        public DbSet<SanphamModel> SanphamModel { get; set; }
        public DbSet<TBL_DANHMUCHANGHOAModel> TBL_DANHMUCHANGHOAModel { get; set; }
        public DbSet<KhachhangModel> KhachhangModel { get; set; }
        public DbSet<RawDataModel> RawDataModel { get; set; }
        public DbSet<TonkhoModel> TonkhoModel { get; set; }
        public DbSet<TBL_DANHMUCPHANLOAIKHACHHANGModel> TBL_DANHMUCPHANLOAIKHACHHANGModel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RawDataModel>()
        .HasKey(p => new { p.MAHH, p.MAKH, p.MATDV, p.NHOM, p.MACH, p.MATINH, p.PHANLOAI, p.PHANLOAIKHACHHANG, p.NgayLapHD, p.TENDVBC });
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
        }

    }

}
