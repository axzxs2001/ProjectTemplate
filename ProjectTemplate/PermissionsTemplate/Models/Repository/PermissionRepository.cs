using Microsoft.EntityFrameworkCore;
using PermissionsTemplate.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace PermissionsTemplate.Models.Repository
{
    public class PermissionRepository:IPermissionRepository
    {
        string _permissionConnectionString;
        public PermissionRepository(IConfiguration configuration)
        {
            _permissionConnectionString = configuration.GetConnectionString("PermissionConnectionString");
        }
        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <returns></returns>
        public List<Permission> GetRolePermissons()
        {
            using (var con = new SqlConnection(_permissionConnectionString))
            {
                return con.Query<Permission>($@"select RoleName,Method,Action 
                from  dbo.Permissions INNER JOIN
                dbo.RolePermissions ON dbo.Permissions.ID =dbo.RolePermissions.PermissionID INNER JOIN
                dbo.Roles ON dbo.RolePermissions.RoleID = dbo.Roles.ID").ToList();    
            }
        }
    }
}
