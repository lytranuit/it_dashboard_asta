
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using Vue.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DiagnosticAdapter;
using System.Data.Common;

namespace Vue.Data
{
    public class Buy_Context : DbContext
    {
        private IActionContextAccessor actionAccessor;
        private UserManager<UserModel> UserManager;
        public Buy_Context(DbContextOptions<Buy_Context> options) : base(options)
        {

        }



        public DbSet<DutruModel> DutruModel { get; set; }
        public DbSet<DutruChitietModel> DutruChitietModel { get; set; }

        public virtual DbSet<MuahangModel> MuahangModel { get; set; }
        public DbSet<MuahangChitietModel> MuahangChitietModel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MuahangModel>()
              .HasOne(s => s.muahang_chonmua)
              .WithMany()
              .HasForeignKey(e => e.muahang_chonmua_id);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
        }

    }
    public class CommandInterceptor
    {
        [DiagnosticName("Microsoft.EntityFrameworkCore.Database.Command.CommandExecuting")]
        public void OnCommandExecuting(DbCommand command, DbCommandMethod executeMethod, Guid commandId, Guid connectionId, bool async, DateTimeOffset startTime)
        {

            var list_talbe = new List<string>()
            {
                "AspNetUsers","AspNetUserRoles","emails","Token","user_manager","department","user_department","queue",
                "document","document_event","document_attachment","document_file","document_user_receive","document_signature","document_type","document_type_receive","related_esign"
            };
            //var tableName = "AspNetUsers";
            foreach (var tableName in list_talbe)
            {
                var secondaryDatabaseName = "OrgData";
                var schemaName = "dbo";
                command.CommandText = command.CommandText.Replace($" [{tableName}]", $" [{schemaName}].[{tableName}]")
                                                     .Replace($" [{schemaName}].[{tableName}]", $" [{secondaryDatabaseName}].[{schemaName}].[{tableName}]");
            }
            var list_talbe2 = new List<string>()
            {
                "dm_hanghoa","TBL_DANHMUCNHACC","TBL_DANHMUCHANGHOA_MUAHANG","TBL_DANHMUCHANGHOA_DINHKEM","TBL_DANHMUCNHASX","TBL_DANHMUCNHOMHANG","TBL_DANHMUCTHANHPHAM","TBL_DANHMUCKHUVUC","TBL_DANHMUCBOM","TBL_DANHMUCBOM_THAYTHE","TBL_DANHMUCKIENHANG"
            };
            //var tableName = "AspNetUsers";
            foreach (var tableName in list_talbe2)
            {
                var secondaryDatabaseName = "QLSX";
                var schemaName = "dbo";
                command.CommandText = command.CommandText.Replace($" [{tableName}]", $" [{schemaName}].[{tableName}]")
                                                     .Replace($" [{schemaName}].[{tableName}]", $" [{secondaryDatabaseName}].[{schemaName}].[{tableName}]");
            }

        }
    }
}
