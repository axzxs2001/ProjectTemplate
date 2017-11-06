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

            //【2固定角色Cookie验证】 注入验证 2.0
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
