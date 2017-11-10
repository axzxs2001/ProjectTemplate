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
    /// <summary>
    /// 权限仓储接口
    /// </summary>
    public interface IPermissionRepository
    {
        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <returns></returns>
        List<AuthorizePermission> GetRolePermissons();

    }
}
