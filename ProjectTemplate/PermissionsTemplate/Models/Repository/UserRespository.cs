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
    /// <summary>
    /// 用户仓储类
    /// </summary>
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
        public UserRole Login(string userName, string password)
        {
            using (var con = new SqlConnection(_permissionConnectionString))
            {
                return con.QuerySingle<UserRole>($@"select UserRoles.userid,UserRoles.roleid,roles.RoleName,users.[name],users.username from Users join UserRoles on Users.id = UserRoles.UserID join  Roles on roles.ID = UserRoles.RoleID where users.UserName=@username and users.password=@password", new { username = userName, password = password });
            }
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
        /// <summary>
        /// 按用户用户ID获取权限
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<UserRolePermission> GetUserPermissionByUserID(int userID)
        {
            using (var con = new SqlConnection(_permissionConnectionString))
            {
                return con.Query<UserRolePermission>($@"select UserRoles.userid,UserRoles.roleid,roles.RoleName,user.name,user.username from Users join UserRoles on Users.id = UserRoles.UserID join  Roles on roles.ID = UserRoles.RoleID join rolepermissions on roles.id=rolepermissions.roleid join permissions rolepermissions.permissionid=permissions.id where UserRoles.userid=@userid", new { userid = userID }).ToList();
            }
        }
        /// <summary>
        /// 查询全部用户
        /// </summary>
        /// <returns></returns>
        public List<User> GetAllUser()
        {
            using (var con = new SqlConnection(_permissionConnectionString))
            {
                return con.Query<User>($@"select * from users").ToList();
            }
        }

        /// <summary>
        /// 按用户ID查询角色
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public List<UserRole> GetUserRole(int userID)
        {
            using (var con = new SqlConnection(_permissionConnectionString))
            {
                return con.Query<UserRole>($@"select UserRoles.userid,UserRoles.roleid,roles.RoleName,users.name,users.username from Users join UserRoles on Users.id = UserRoles.UserID join  Roles on roles.ID = UserRoles.RoleID where UserRoles.userid=@userid", new { userid = userID }).ToList();
            }
        }


        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public bool AddUser(User user)
        {
            using (var con = new SqlConnection(_permissionConnectionString))
            {
                return con.Execute(
                    $@"insert into users([name],username,password) values(@name,@username,@password)",
                    new { name=user.Name, username=user.UserName, password=user.Password}
                    ) > 0;
            }
        }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public bool ModifyUser(User user)
        {
            using (var con = new SqlConnection(_permissionConnectionString))
            {
                return con.Execute(
                    $@"update users set [name]=@name,username=@username,password=@password where id=@id",
                    new {id=user.ID,name = user.Name, username = user.UserName, password = user.Password }
                    ) > 0;
            }
        }
        /// <summary>
        /// 移除用户
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public bool RemoveUser(int userID)
        {
            using (var con = new SqlConnection(_permissionConnectionString))
            {
                return con.Execute(
                    $@"delete users where id=@id",
                    new { id = userID}
                    ) > 0;
            }
        }
        /// <summary>
        /// 添加用户角色ID
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="roleIDs">角色ID</param>
        /// <returns></returns>
        public bool AddUserRole(int userID, int[] roleIDs)
        {
            using (var con = new SqlConnection(_permissionConnectionString))
            {
                con.Open();
                var tran = con.BeginTransaction();
                try
                {
                    con.Execute($"delete userroles where userid=@userid",new { userid=userID},tran);

                    var list = new List<dynamic>();
                    foreach (var roleID in roleIDs)
                    {
                        list.Add(new { userid = userID, roleid = roleID });
                    }
                    con.Execute($@"insert into userroles(userid,roleid) values(@userid,@roleid)", list, tran) ;

                    tran.Commit();
                    return true;
                }
                catch(Exception exc)
                {
                    tran.Rollback();
                    throw exc;
                }
            }
        }
    }
}
