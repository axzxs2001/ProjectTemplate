using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AuthorizePolicy.JWT;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace AspNetCoreWebApiTemplate
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
            #region 【添加JWT验证】
            AddAuthorization(services);
            #endregion

            services.AddMvc();

            #region 【1 Swagger注入】
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "AspNetCoreWebApiTemplate测试Web API",
                    Version = "v1",
                    Description = "AspNetCoreWebApiTemplate测试Web API ",
                    TermsOfService = "None",
                    Contact = new Swashbuckle.AspNetCore.Swagger.Contact
                    {
                        Name = "桂素伟",
                        Email = "axzxs2001@163.com"
                    },
                });
                var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "AspNetCoreWebApiTemplate.xml");//需要找开项目属性-生成-XML文档文件选上
                c.IncludeXmlComments(filePath);
                c.CustomSchemaIds((type) => type.FullName);
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            #region 【1 Swagger注入】
            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) => swaggerDoc.Host = httpReq.Host.Value);
                c.RouteTemplate = "docs/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "docs";
                c.SwaggerEndpoint("/docs/v1/swagger.json", "AspNetCoreWebApiTemplate测试Web API");
                c.InjectStylesheet("/swagger-ui/custom.css");
                c.InjectOnCompleteJavaScript("/swagger-ui/custom.js");
            });
            #endregion
        }

        /// <summary>
        /// 添加JWT自定义策略
        /// </summary>
        /// <param name="services"></param>
        void AddAuthorization(IServiceCollection services)
        {
            //读取配置文件
            var audienceConfig = Configuration.GetSection("Audience");
            var symmetricKeyAsBase64 = audienceConfig["Secret"];
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = audienceConfig["Issuer"],//发行人
                ValidateAudience = true,
                ValidAudience = audienceConfig["Audience"],//订阅人
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,

            };
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            //这个集合模拟用户权限表,可从数据库中查询出来
            var permission = new List<Permission> {
                              new Permission {  Url="/", Name="admin"},
                              new Permission {  Url="/api/values", Name="admin"},
                              new Permission {  Url="/", Name="system"},
                              new Permission {  Url="/api/values1", Name="system"}
                          };
            //如果第三个参数，是ClaimTypes.Role，上面集合的每个元素的Name为角色名称，如果ClaimTypes.Name，即上面集合的每个元素的Name为用户名
            var permissionRequirement = new PermissionRequirement(
                "/api/denied", permission,
                ClaimTypes.Role,
                audienceConfig["Issuer"],
                audienceConfig["Audience"],
                signingCredentials,
                expiration: TimeSpan.FromSeconds(10)
                );
            services.AddAuthorization(options =>
            {

                options.AddPolicy("Permission",
                          policy => policy.Requirements.Add(permissionRequirement));

            }).AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                //不使用https
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = tokenValidationParameters;
                o.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        if (context.Request.Path.Value.ToString() == "/api/logout")
                        {
                            var token = ((context as TokenValidatedContext).SecurityToken as JwtSecurityToken).RawData;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            //注入授权Handler
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            services.AddSingleton(permissionRequirement);
        }
    }
}
