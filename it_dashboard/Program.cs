﻿//using it.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Vue.Data;
using Vue.Models;
using Vue.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.FileProviders;
using System.Net;
//using it.Services;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.AspNetCore.Authentication;
//using Vue.Middleware;

namespace Vue
{
    public class Program
    {
        public static string description { get; set; } = "";
        private static string MyAllowSpecificOrigins = "tran";
        public static void Main(string[] args)
        {
            Startup(args);

        }
        public static WebApplicationBuilder CreateDefaultBuilder(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigurationManager configuration = builder.Configuration;

            var connectionString = builder.Configuration.GetConnectionString("ItConnection") ?? throw new InvalidOperationException("Connection string 'ItConnection' not found.");
            var KT = builder.Configuration.GetConnectionString("KT") ?? throw new InvalidOperationException("Connection string 'KT' not found.");
            var QLSX = builder.Configuration.GetConnectionString("QLSX") ?? throw new InvalidOperationException("Connection string 'QLSX' not found.");
            var NHANSU = builder.Configuration.GetConnectionString("NHANSU") ?? throw new InvalidOperationException("Connection string 'NHANSU' not found.");
            var Buy = builder.Configuration.GetConnectionString("Buy") ?? throw new InvalidOperationException("Connection string 'Buy' not found.");


            builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            builder.Services.AddControllersWithViews().AddJsonOptions(x =>
              x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            builder.Services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDefaultIdentity<UserModel>(options => options.SignIn.RequireConfirmedAccount = false).AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>(); ;



            builder.Services.AddDbContext<ItContext>(options =>
              options.UseSqlServer(connectionString)
              );

            builder.Services.AddDbContext<KT_Context>(options =>
             options.UseSqlServer(KT)
             );

            builder.Services.AddDbContext<Buy_Context>(options =>
             options.UseSqlServer(Buy)
             );
            builder.Services.AddDbContext<QLSX_Context>(options =>
             options.UseSqlServer(QLSX)
             );
            builder.Services.AddDbContext<NhansuContext>(options =>
             options.UseSqlServer(NHANSU)
             );
            //builder.Services.AddScoped<ViewRender, ViewRender>();

            builder.Services.AddScoped<LoginMailPyme, LoginMailPyme>();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = false;
                options.ExpireTimeSpan = TimeSpan.FromHours(Int64.Parse(configuration["JWT:Expire"]));

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });




            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
                                  });
            });
            builder.Services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "frontend/dist";

            });
            return builder;
        }
        public static void Startup(string[] args)
        {
            var builder = CreateDefaultBuilder(args);
            var app = builder.Build();

            //app.UseMiddleware<CheckTokenMiddleware>();
            app.UseDeveloperExceptionPage();
            // Configure the HTTP request pipeline.
            app.UseStaticFiles();
            if (!app.Environment.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }
            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseStaticFiles(new StaticFileOptions
            //{

            //    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "frontend", "src")),
            //    RequestPath = "/src",
            //    OnPrepareResponse = ctx =>
            //    {
            //    }
            //});
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(builder.Configuration["Source:Path_Private"]),
                RequestPath = "/private",
                OnPrepareResponse = ctx =>
                {
                    var token = builder.Configuration["Key_Access"];
                    var token_query = ctx.Context.Request.Query["token"].ToString();

                    if (!ctx.Context.User.Identity.IsAuthenticated && token_query != token)
                    {
                        ctx.Context.Response.Redirect("/Identity/Account/Login");
                    }
                }
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                   name: "areas",
                   pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                 );
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");

            });
            app.MapRazorPages();

            ////Xác thực đăng nhập khi dùng SPA
            app.Use(async (context, next) =>
            {
                var path = (string)context.Request.Path;
                var is_except = false;
                var except = new List<string>()
                {
                    "/v1/auth",
                    "/Identity/Account/AccessDenied",
                };
                foreach (var item in except)
                {
                    if (path.ToLower().StartsWith(item.ToLower()))
                    {
                        is_except = true;
                        break;
                    }
                }
                var roles = builder.Configuration.GetSection("Roles").Get<string[]>().ToList();
                foreach (var role in roles)
                {
                    if (context.User.IsInRole(role))
                    {
                        is_except = true;
                        break;
                    }
                }
                if (is_except)
                {
                    await next();
                }
                else if (!context.User.Identity.IsAuthenticated)
                {
                    context.Response.Redirect("/Identity/Account/Login");
                }
                else
                {
                    context.Response.Redirect("/Identity/Account/AccessDenied");
                }
            });
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "frontend";
                if (app.Environment.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:5173");
                    //spa.UseVueDevelopmentServer();
                }
            });
            app.Run();
        }
    }
}