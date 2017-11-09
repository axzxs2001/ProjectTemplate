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
    public class UserRespository : IUserRespository
    {
        string _permissionConnectionString;
        public UserRespository(IConfiguration configuration)
        {
            _permissionConnectionString = configuration.GetConnectionString("PermissionConnectionString");
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public Users Login(string userName, string password)
        {
            using (var con = new SqlConnection(_permissionConnectionString))
            {
                return con.QuerySingle<Users>($@"select users.*,roles.RoleName from Users join UserRoles on Users.id = UserRoles.UserID join  Roles on roles.ID = UserRoles.RoleID where users.UserName=@username and users.password=@password", new { username = userName, password = password });
            }
        }
        /// <summary>
        /// 按用户ID获取角色
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public Roles GetRole(int userID)
        {
            return null;
        }
    }
}
