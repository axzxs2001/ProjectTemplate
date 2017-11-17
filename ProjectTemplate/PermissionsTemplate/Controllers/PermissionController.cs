using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PermissionsTemplate.Models.Repository;
using PermissionsTemplate.Models;
using PermissionsTemplate.Models.Common;
using PermissionsTemplate.Models.DataModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace PermissionsTemplate.Controllers
{
    /// <summary>
    /// 权限管理Controller
    /// </summary>
    [Authorize(Policy = "Permission")]
    public class PermissionController : BaseController
    {
        #region 初始化 字段，构造
        /// <summary>
        /// 权限仓储
        /// </summary>
        IPermissionRepository _permissionRepository;
        /// <summary>
        /// 用户仓储
        /// </summary>
        IUserRespository _userRepository;
        /// <summary>
        /// 角色仓储
        /// </summary>
        IRoleRespository _roleRespository;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="permissionRepository">权限</param>
        /// <param name="userRepository">用户</param>
        /// <param name="roleRespository">角色</param>
        public PermissionController(IPermissionRepository permissionRepository, IUserRespository userRepository, IRoleRespository roleRespository)
        {
            _permissionRepository = permissionRepository;
            _userRepository = userRepository;
            _roleRespository = roleRespository;
        }
        #endregion
        #region 用户角色
        public IActionResult UserRole()
        {
            return View();
        }
        /// <summary>
        /// 获取全部用户，角色
        /// </summary>
        /// <returns></returns>
        [HttpGet("getuserrole")]
        public IActionResult GetUserRole()
        {
            try
            {
                var users = _userRepository.GetAllUser();
                var roles = _roleRespository.GetAllRole();
                return ToJson(HttpResult.Success, new { users = users, roles = roles });     
            }
            catch (Exception exc)
            {
                return ToJson(HttpResult.Exception,message:exc.Message);               
            }
        }
        /// <summary>
        /// 保存用户角色
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="roleIDs">角色ID组</param>
        /// <returns></returns>
        [HttpPost("savauserrole")]
        public IActionResult SavaUserRole(int userID, int[] roleIDs)
        {
            try
            {
                var result = _userRepository.AddUserRole(userID, roleIDs);
                return ToJson(result?HttpResult.Success:HttpResult.Fail,result?"保存成功":"保存失败");            
            }
            catch (Exception exc)
            {
                return ToJson(HttpResult.Exception, message: exc.Message);
            }
        }
        /// <summary>
        /// 按照userid获取用户id
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        [HttpGet("getrolebyuserid")]
        public IActionResult GetUserRole(int userID)
        {
            try
            {
                var userroles = _userRepository.GetUserRole(userID);
                return ToJson(HttpResult.Success,userroles);              
            }
            catch (Exception exc)
            {
                return ToJson(HttpResult.Exception, message: exc.Message);
            }
        }
        #endregion
        #region 角色权限
        public IActionResult RolePermission()
        {
            return View();
        }
        /// <summary>
        /// 获取全部角色，权限
        /// </summary>
        /// <returns></returns>
        [HttpGet("getrolepermission")]
        public IActionResult GetRolePermission()
        {
            try
            {
                var roles = _roleRespository.GetAllRole();
                var permissions = _permissionRepository.GetAllPermission().Select(s => new { id = s.ID, name = $"{s.PermissionName}[{s.Method}]", pid = s.Pid });
                return ToJson(HttpResult.Success, new { roles = roles, permissions = permissions });              
            }
            catch (Exception exc)
            {
                return ToJson(HttpResult.Exception, message: exc.Message);
            }
        }
        /// <summary>
        /// 保存角色权限
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <param name="permissionIDs">权限ID组</param>
        /// <returns></returns>
        [HttpPost("savarolepermission")]
        public IActionResult SavaRolePermission(int roleID, int[] permissionIDs)
        {
            try
            {
                var result = _roleRespository.AddRolePermission(roleID, permissionIDs);
                return ToJson(result ? HttpResult.Success : HttpResult.Fail, result ? "保存成功" : "保存失败");
            }
            catch (Exception exc)
            {
                return ToJson(HttpResult.Exception, message: exc.Message);
            }
        }

        /// <summary>
        /// 按照roleid获取权限id
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <returns></returns>
        [HttpGet("getpermissionbyroleid")]
        public IActionResult GetRolePermission(int roleID)
        {
            try
            {
                var rolepermissions = _roleRespository.GetRolePermissionsByRoleID(roleID);
                return ToJson(HttpResult.Success, rolepermissions);
            }
            catch (Exception exc)
            {
                return ToJson(HttpResult.Exception, message: exc.Message);
            }
        }



        #endregion

        #region 用户
        public IActionResult UserIndex()
        {
            return View();
        }

        /// <summary>
        /// 获取全部用户
        /// </summary>
        /// <returns></returns>
        [HttpGet("getusers")]
        public IActionResult GetUsers()
        {
            try
            {
                var users = _userRepository.GetAllUser();
                return ToJson(HttpResult.Success, users);
            }
            catch (Exception exc)
            {
                return ToJson(HttpResult.Exception, message: exc.Message);
            }
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        [HttpPost("adduser")]
        public IActionResult AddUser(User user)
        {
            try
            {
                var result = _userRepository.AddUser(user);
                return ToJson(result?HttpResult.Success:HttpResult.Fail,message:result?"添加成功":"添加失败");
            }
            catch (Exception exc)
            {
                return new JsonResult(new { result = 0, message = exc.Message }, new Newtonsoft.Json.JsonSerializerSettings()
                {
                    ContractResolver = new LowercaseContractResolver()
                });
            }
        }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        [HttpPut("modifyuser")]
        public IActionResult ModifyUser(User user)
        {
            try
            {
                var result = _userRepository.ModifyUser(user);
                return ToJson(result ? HttpResult.Success : HttpResult.Fail, message: result ? "修改成功" : "修改失败");
            }
            catch (Exception exc)
            {
                return ToJson(HttpResult.Exception, message: exc.Message);
            }
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        [HttpDelete("removeuser")]
        public IActionResult RemoveUser(int userID)
        {
            try
            {
                var result = _userRepository.RemoveUser(userID);
                return ToJson(result ? HttpResult.Success : HttpResult.Fail, message: result ? "删除成功" : "删除失败");
            }
            catch (Exception exc)
            {
                return ToJson(HttpResult.Exception, message: exc.Message);
            }
        }
        #endregion
        #region 角色
        public IActionResult RoleIndex()
        {
            return View();
        }
        /// <summary>
        /// 获取全部角色
        /// </summary>
        /// <returns></returns>
        [HttpGet("getroles")]
        public IActionResult GetRoles()
        {
            try
            {
                var roles = _roleRespository.GetAllRole();
                return ToJson(HttpResult.Success, roles);
            }
            catch (Exception exc)
            {
                return ToJson(HttpResult.Exception, message: exc.Message);
            }
        }
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="role">角色</param>
        /// <returns></returns>
        [HttpPost("addrole")]
        public IActionResult AddRole(string roleName)
        {
            try
            {
                var result = _roleRespository.AddRole(roleName);
                return ToJson(result ? HttpResult.Success : HttpResult.Fail, message: result ? "添加成功" : "添加失败");
            }
            catch (Exception exc)
            {
                return new JsonResult(new { result = 0, message = exc.Message }, new Newtonsoft.Json.JsonSerializerSettings()
                {
                    ContractResolver = new LowercaseContractResolver()
                });
            }
        }
        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="role">角色</param>
        /// <returns></returns>
        [HttpPut("modifyrole")]
        public IActionResult ModifyRole(Role role)
        {
            try
            {
                var result = _roleRespository.ModifyRole(role);
                return ToJson(result ? HttpResult.Success : HttpResult.Fail, message: result ? "修改成功" : "修改失败");
            }
            catch (Exception exc)
            {
                return ToJson(HttpResult.Exception, message: exc.Message);
            }
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <returns></returns>
        [HttpDelete("removerole")]
        public IActionResult RemoveRole(int roleID)
        {
            try
            {
                var result = _roleRespository.RemoveRole(roleID);
                return ToJson(result ? HttpResult.Success : HttpResult.Fail, message: result ? "删除成功" : "删除失败");
            }
            catch (Exception exc)
            {
                return ToJson(HttpResult.Exception, message: exc.Message);
            }
        }
        #endregion
        #region 权限
        public IActionResult PermissionIndex()
        {
            return View();
        }
        /// <summary>
        /// 获取全部权限
        /// </summary>
        /// <returns></returns>
        [HttpGet("getpermissions")]
        public IActionResult GetPermissions()
        {
            try
            {
                var permissions = _permissionRepository.GetAllPermission();
                return ToJson(HttpResult.Success, permissions);
            }
            catch (Exception exc)
            {
                return ToJson(HttpResult.Exception, message: exc.Message);
            }
        }
        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="role">权限</param>
        /// <returns></returns>
        [HttpPost("addpermission")]
        public IActionResult AddPermission(Permission permission)
        {
            try
            {
                var result = _permissionRepository.AddPermission(permission);
                return ToJson(result ? HttpResult.Success : HttpResult.Fail, message: result ? "添加成功" : "添加失败");
            }
            catch (Exception exc)
            {
                return new JsonResult(new { result = 0, message = exc.Message }, new Newtonsoft.Json.JsonSerializerSettings()
                {
                    ContractResolver = new LowercaseContractResolver()
                });
            }
        }
        /// <summary>
        /// 修改权限
        /// </summary>
        /// <param name="role">权限</param>
        /// <returns></returns>
        [HttpPut("modifypermission")]
        public IActionResult ModifyPermission(Permission permission)
        {
            try
            {
                var result = _permissionRepository.ModifyPermission(permission);
                return ToJson(result ? HttpResult.Success : HttpResult.Fail, message: result ? "修改成功" : "修改失败");
            }
            catch (Exception exc)
            {
                return ToJson(HttpResult.Exception, message: exc.Message);
            }
        }
        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="roleID">权限ID</param>
        /// <returns></returns>
        [HttpDelete("removepermission")]
        public IActionResult RemovePermission(int permissionID)
        {
            try
            {
                var result = _permissionRepository.RemovePermission(permissionID);
                return ToJson(result ? HttpResult.Success : HttpResult.Fail, message: result ? "删除成功" : "删除失败");
            }
            catch (Exception exc)
            {
                return ToJson(HttpResult.Exception, message: exc.Message);
            }
        }
        #endregion


        #region 登录，登出
        #region Lgoin View
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

            var user = _userRepository.Login(userName, password);
            if (user != null)
            {
                //用户标识
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

                identity.AddClaim(new Claim(ClaimTypes.Name, user.Name));
                identity.AddClaim(new Claim(ClaimTypes.Role, user.RoleName));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Sid, user.UserID.ToString()));
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

        [AllowAnonymous]
        [HttpGet("denied")]
        public IActionResult Denied()
        {
            return View();
        }
        #endregion
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}