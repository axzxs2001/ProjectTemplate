using Microsoft.Extensions.Configuration;
using PermissionsTemplate.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;

namespace PermissionsTemplate.Models.Repository
{
    public class RoleRespository : IRoleRespository
    {
        string _permissionConnectionString;
        public RoleRespository(IConfiguration configuration)
        {
            _permissionConnectionString = configuration.GetConnectionString("PermissionConnectionString");
        }
        /// <summary>
        /// 添加角色要权限表
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <param name="permissionID">权限ID</param>
        /// <returns></returns>
        public bool AddRolePermission(int roleID, int permissionID)
        {
            using (var con = new SqlConnection(_permissionConnectionString))
            {
                return con.Execute(
                    $@"insert into rolepermissions(roleid,permissionid) values(@roleid,@permissionid)",
                    new { roleid = roleID, permissionid = permissionID }
                    ) > 0;
            }
        }
        /// <summary>
        /// 获取全部角色权限
        /// </summary>
        /// <returns></returns>
        public List<RolePermission> GetRolePermissions()
        {
            using (var con = new SqlConnection(_permissionConnectionString))
            {
                return con.Query<RolePermission>(
                    $@"select * from roles join rolepermissions
                    on roles.id=rolepermissions.roleid 
                    join permissions on permissions.id=rolepermissions.permissionid").ToList();
            }
        }

        /// <summary>
        /// 获取全部角色
        /// </summary>
        /// <returns></returns>
        public List<Role> GetAllRole()
        {
            using (var con = new SqlConnection(_permissionConnectionString))
            {
                return con.Query<Role>(
                    $@"select * from roles").ToList();
            }
        }


        /// <summary>
        /// 根据角色ID获取角色权限
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <returns></returns>
        public List<RolePermission> GetRolePermissionsByRoleID(int roleID)
        {
            using (var con = new SqlConnection(_permissionConnectionString))
            {
                return con.Query<RolePermission>(
                    $@"select * from roles join rolepermissions
                    on roles.id=rolepermissions.roleid 
                    join permissions on permissions.id=rolepermissions.permissionid where roleid=@roleid",new { roleid=roleID}).ToList();
            }
        }
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <returns></returns>
        public bool AddRole(string roleName)
        {
            using (var con = new SqlConnection(_permissionConnectionString))
            {
                return con.Execute(
                    $@"insert into roles(rolename) values(@rolename)",
                    new { rolename = roleName }
                    ) > 0;
            }
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="role">角色</param>
        /// <returns></returns>
        public bool ModifyRole(Role role)
        {
            using (var con = new SqlConnection(_permissionConnectionString))
            {
                return con.Execute(
                    $@"update roles set rolename=@rolename where id=@id",
                    new { rolename = role.RoleName, id = role.ID }
                    ) > 0;
            }
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <returns></returns>
        public bool RemoveRole(int roleID)
        {
            using (var con = new SqlConnection(_permissionConnectionString))
            {
                return con.Execute(
                    $@"delete roles where id=@id",
                    new { id = roleID }
                    ) > 0;
            }
        }

        /// <summary>
        /// 添加角色权限ID
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <param name="permissionIDs">权限ID</param>
        /// <returns></returns>
        public bool AddRolePermission(int roleID, int[] permissionIDs)
        {
            using (var con = new SqlConnection(_permissionConnectionString))
            {
                con.Open();
                var tran = con.BeginTransaction();
                try
                {
                    con.Execute($"delete rolepermissions where roleid=@roleid", new { roleid = roleID }, tran);
                    var list = new List<dynamic>();
                    foreach (var permissionID in permissionIDs)
                    {
                        list.Add(new { roleid = roleID, permissionid = permissionID });
                    }
                    con.Execute($@"insert into rolepermissions(roleid,permissionid) values(@roleid,@permissionid)", list, tran);

                    tran.Commit();
                    return true;
                }
                catch (Exception exc)
                {
                    tran.Rollback();
                    throw exc;
                }
            }
        }
    }
}
