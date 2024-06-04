
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using Vue.Models;

namespace Vue.Data
{
    public class QLSX_Context : DbContext
    {
        private IActionContextAccessor actionAccessor;
        private UserManager<UserModel> UserManager;
        public QLSX_Context(DbContextOptions<QLSX_Context> options) : base(options)
        {

        }


        public DbSet<Tinhtrangsanpham> Tinhtrangsanpham { get; set; }
        public DbSet<Bomchecklist> Bomchecklist { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
        }

    }

}
