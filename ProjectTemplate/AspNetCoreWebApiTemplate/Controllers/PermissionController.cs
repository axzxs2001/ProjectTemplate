
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using AuthorizePolicy.JWT;
using Microsoft.AspNetCore.Cors;
using System.Threading.Tasks;
using System;

namespace AspNetCoreWebApiTemplate.Controllers
{
    /// <summary>
    /// 权限Controller
    /// </summary>
    [Authorize("Permission")]
    [EnableCors("MyDomain")]
    public class PermissionController : Controller
    {
        /// <summary>
        /// 自定义策略参数
        /// </summary>
        PermissionRequirement _requirement;
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="requirement">自定义策略参数</param>
        public PermissionController(PermissionRequirement requirement)
        {
            _requirement = requirement;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="role">角色</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("/api/login")]
        public IActionResult Login(string username, string password, string role)
        {
            var isValidated = username == "gsw" && password == "111111";
            if (!isValidated)
            {
                return new JsonResult(new
                {
                    Status = false,
                    Message = "认证失败"
                });
            }
            else
            {               
                //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
                var claims = new Claim[] { new Claim(ClaimTypes.Name, username), new Claim(ClaimTypes.Role, role), new Claim(ClaimTypes.Expiration ,DateTime.Now.AddSeconds(_requirement.Expiration.TotalSeconds).ToString())};
                //用户标识
                var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
                identity.AddClaims(claims);
             
                var token = JwtToken.BuildJwtToken(claims, _requirement);
                return new JsonResult(token);
            }
        }
        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [HttpPost("/api/logout")]
        public  IActionResult Logout()
        {
            return Ok();
        }
        /// <summary>
        /// 拒绝
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("/api/denied")]
        public IActionResult Denied()
        {
            return new JsonResult(new
            {
                Status = false,
                Message = "你无权限访问"
            });
        }
    }
}
