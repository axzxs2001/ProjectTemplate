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
        public User Login(string userName, string password)
        {
            using (var con = new SqlConnection(_permissionConnectionString))
            {
                return con.QuerySingle<User>($@"select users.*,roles.RoleName from Users join UserRoles on Users.id = UserRoles.UserID join  Roles on roles.ID = UserRoles.RoleID where users.UserName=@username and users.password=@password", new { username = userName, password = password });
            }
        }
        /// <summary>
        /// 按用户ID获取角色
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public Role GetRole(int userID)
        {
            return null;
        }
        /// <summary>
        /// 添加用户角色表
        /// </summary>
        /// <param name="userID">用ID</param>
        /// <param name="roleID">角色ID</param>
        /// <returns></returns>
        public bool AddUserRole(int userID, int roleID)
        {
            using (var con = new SqlConnection(_permissionConnectionString))
            {
                return con.Execute(
                    $@"insert into userroles(roleid,userid) values(@roleid,@userid)",
                    new { roleid = roleID, userid = userID }
                    ) > 0;
            }
        }
    }
}
