using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using AspNetCoreTemplate.Models.Repository;

namespace AspNetCoreTemplate.Controllers
{
    public class LoginController : Controller
    {
        /// <summary>
        /// 用户仓储接口
        /// </summary>
        IUserRespository _userRepository;
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="userRepository">用户仓储</param>
        public LoginController(IUserRespository userRepository)
        {
            //用户仓储
            _userRepository = userRepository;
        }
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
            dynamic user = _userRepository.Login(userName, password);
            if (user != null)
            {
                //查询角色名称
                dynamic roleName = _userRepository.GetRole(user.RoleID).RoleName;

                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.UserData,user.UserName),
                    new Claim(ClaimTypes.Role,roleName),
                    new Claim(ClaimTypes.Name,user.Name),
                    new Claim(ClaimTypes.PrimarySid,user.ID.ToString())
                 };
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(claims)));
                return new RedirectResult(returnUrl == null ? "/home/index" : returnUrl);

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