using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreTemplate.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using AspNetCoreTemplate.Models.Repository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace AspNetCoreTemplate.Controllers
{

    [Authorize(Policy = "Permission")]//【4自定义策略Cookie验证】与固定角色Cookie验证二选一
    public class HomeController : Controller
    {
        /// <summary>
        /// 【3NLog】日志对象
        /// </summary>
        private readonly ILogger<HomeController> _logger;
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="logger">日志对象</param>

        public HomeController(ILogger<HomeController> logger)
        {
            //【3NLog】日志对象
            _logger = logger;

        }

        public IActionResult Index()
        {
            _logger.LogInformation("Home下的Index");
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region 【4自定义策略Cookie验证】与固定角色Cookie验证二选一
        [AllowAnonymous]
        [HttpGet("login")]
        public IActionResult Login(string returnUrl = null)
        {
            TempData["returnUrl"] = returnUrl;
            return View();
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(string userName, string password, string returnUrl = null)
        {
            //这个集合模拟用户表，可从数据库中查询出来
            var list = new List<dynamic> {
                new { UserName = "gsw", Password = "111111", Role = "admin",Name="桂素伟"},
                new { UserName = "aaa", Password = "222222", Role = "system",Name="测试A"}
            };
            var user = list.SingleOrDefault(s => s.UserName == userName && s.Password == password);
            if (user != null)
            {
                //用户标识
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                //如果是基于角色的授权策略，这里要添加用户
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                //如果是基于角色的授权策略，这里要添加角色
                identity.AddClaim(new Claim(ClaimTypes.Role, user.Role));
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                if (returnUrl == null)
                {
                    returnUrl = TempData["returnUrl"]?.ToString();
                }
                if (returnUrl != null)
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            else
            {
                const string badUserNameOrPasswordMessage = "用户名或密码错误！";
                return BadRequest(badUserNameOrPasswordMessage);
            }
        }
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        [HttpGet("denied")]
        public IActionResult Denied()
        {
            return View();
        }
        #endregion
    }
}
