using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PermissionsTemplate.Models.Repository;
using PermissionsTemplate.Models;

namespace PermissionsTemplate.Controllers
{
    /// <summary>
    /// 权限管理Controller
    /// </summary>
    [Authorize(Policy = "Permission")]
    public class PermissionController : Controller
    {
        #region 初始化
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

        #region UserPermission View

        #region 获取全部用户，角色，权限
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
                return new JsonResult(new { result = 1, data = new { users = users, roles = roles } }, new Newtonsoft.Json.JsonSerializerSettings()
                {
                    ContractResolver = new LowercaseContractResolver()
                });
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
                return new JsonResult(new { result = 1, data = new { roles = roles, permissions = permissions } }, new Newtonsoft.Json.JsonSerializerSettings()
                {
                    ContractResolver = new LowercaseContractResolver()
                });
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
                if (result)
                {
                    return new JsonResult(new { result = 1, message = "保存成功！" }, new Newtonsoft.Json.JsonSerializerSettings()
                    {
                        ContractResolver = new LowercaseContractResolver()
                    });
                }
                else
                {
                    return new JsonResult(new { result = 0, message = "保存失败！" }, new Newtonsoft.Json.JsonSerializerSettings()
                    {
                        ContractResolver = new LowercaseContractResolver()
                    });
                }
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
                return new JsonResult(new { result = 1, data = userroles.Select(s=>new {id=s.RoleID }) }, new Newtonsoft.Json.JsonSerializerSettings()
                {
                    ContractResolver = new LowercaseContractResolver()
                });
            }
            catch (Exception exc)
            {
                return new JsonResult(new { result = 0, message = exc.Message }, new Newtonsoft.Json.JsonSerializerSettings()
                {
                    ContractResolver = new LowercaseContractResolver()
                });
            }
        }
        #endregion

        public IActionResult UserRole()
        {
            return View();
        }
        public IActionResult RolePermission()
        {
            return View();
        }

        #endregion

    }
}