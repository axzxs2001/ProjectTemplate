using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PermissionsTemplate.Models.Repository;

namespace PermissionsTemplate.Controllers
{
    [Authorize(Policy = "Permission")]
    public class PermissionController : Controller
    {
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
        /// <summary>
        /// 获取全部用户，角色，权限
        /// </summary>
        /// <returns></returns>
        public IActionResult GetAll()
        {
            try
            {
                var users = _userRepository.GetAllUser();
                var roles = _roleRespository.GetAllRole();
                var permissions = _permissionRepository.GetAllPermission();
                return new JsonResult(new { result = 1, data = new { users = users, roles = roles, permissions = permissions } }, new Newtonsoft.Json.JsonSerializerSettings());
            }
            catch (Exception exc)
            {
                return new JsonResult(new { result = 1, message = exc.Message }, new Newtonsoft.Json.JsonSerializerSettings());
            }
        }
       

        public IActionResult UserPermission()
        {
            return View();
        }
    }
}