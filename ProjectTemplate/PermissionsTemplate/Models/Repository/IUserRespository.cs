using PermissionsTemplate.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PermissionsTemplate.Models.Repository
{
    /// <summary>
    /// 用户仓储接口
    /// </summary>
    public interface IUserRespository
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        Users Login(string userName, string password);
        /// <summary>
        /// 按用户ID获取角色
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        Roles GetRole(int userID);
    }
}
