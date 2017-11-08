using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using AuthorizePolicy;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreTemplate
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //【1数据库连接字符串】数据库连接字符串读取方式
            var connectionString1 = Configuration.GetConnectionString("ConnectionString1");
            var connectionString2 = Configuration.GetConnectionString("ConnectionString2");


            #region 【2固定角色Cookie验证】 注入验证 2.0
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, m =>
            {
                m.LoginPath = new PathString("/login");
                m.AccessDeniedPath = new PathString("/home/error");
                m.LogoutPath = new PathString("/logout");
                m.Cookie.Path = "/";
            });
            #endregion

            #region 【4自定义策略Cookie验证】与固定角色Cookie验证二选一
            services.AddAuthorization(options =>
            {
                //这个集合模拟用户权限表,可从数据库中查询出来
                var permission = new List<Permission> {
                              new Permission {  Url="/", Name="admin"},
                              new Permission {  Url="/home/permissionadd", Name="admin"},
                              new Permission {  Url="/", Name="system"},
                              new Permission {  Url="/home/contact", Name="system"}
                          };
                //如果第三个参数，是ClaimTypes.Role，上面集合的每个元素的Name为角色名称，如果ClaimTypes.Name，即上面集合的每个元素的Name为用户名
                var permissionRequirement = new PermissionRequirement("/denied", permission, ClaimTypes.Role);
                options.AddPolicy("Permission",
                          policy => policy.Requirements.Add(permissionRequirement));
            }).AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = new PathString("/login");
                options.AccessDeniedPath = new PathString("/denied");
                options.ExpireTimeSpan = TimeSpan.FromSeconds(20);
            });
            //注入授权Handler
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            #endregion

            services.AddMvc();
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            //【2固定角色Cookie验证】
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
