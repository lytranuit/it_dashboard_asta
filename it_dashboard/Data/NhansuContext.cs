
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using Vue.Models;

namespace Vue.Data
{
    public class NhansuContext : DbContext
    {
        private IActionContextAccessor actionAccessor;
        private UserManager<UserModel> UserManager;
        public NhansuContext(DbContextOptions<NhansuContext> options) : base(options)
        {

        }



        public DbSet<Info.Models.PersonnelModel> PersonnelModel { get; set; }
        public DbSet<Info.Models.SalaryModel> SalaryModel { get; set; }
        public DbSet<Info.Models.SalaryUserModel> SalaryUserModel { get; set; }

        public DbSet<Info.Models.PhongModel> DepartmentModel { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
        }

    }

}
