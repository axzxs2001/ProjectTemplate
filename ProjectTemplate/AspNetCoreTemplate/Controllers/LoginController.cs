using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AspNetCoreTemplate.Controllers
{
    public class LoginController : Controller
    {
        [Authorize("admin")]
        public IActionResult ABC()
        {
            return new JsonResult(new { ID = 1, Name = "姓名" });
        }

        #region 登录页
        /// <summary>
        /// 登录页
        /// </summary>
        /// <param name="returnUrl">跳转Url</param>
        /// <returns></returns> 
        [AllowAnonymous]
        [HttpGet("login")]
        public IActionResult Login(string returnUrl)
        {
            //判断是否验证
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                //把返回地址保存在前台的hide表单中
                ViewBag.returnUrl = returnUrl;
            }
            ViewBag.error = null;
            HttpContext.SignOutAsync("loginvalidate");
            return View();
        }
        /// <summary>
        /// 实现登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="returnUrl">跳转Url</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(string userName, string password, string returnUrl)
        {
            //查询users
            dynamic user = null;// _userResitory.Login(userName, password);
            if (user != null)
            {
                //查询角色名称
                dynamic roleName = "admin";// _roleResitory.GetRole(user.RoleID).RoleName;

                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.UserData,user.UserName),
                    new Claim(ClaimTypes.Role,roleName),
                    new Claim(ClaimTypes.Name,user.Name),
                    new Claim(ClaimTypes.PrimarySid,user.ID.ToString())
                 };
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(claims)));
                //雇员直接导航到我的工作，节省一点点击跳转
                if (roleName == "Employee")
                {
                    return new RedirectResult("myworks");
                }
                else
                {
                    return new RedirectResult(returnUrl == null ? "/home/index" : returnUrl);
                }
            }
            else
            {
                ViewBag.error = "用户名或密码错误！";
                return View();
            }
        }
        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        #endregion

    }
}