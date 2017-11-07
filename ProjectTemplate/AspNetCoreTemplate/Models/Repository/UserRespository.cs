using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreTemplate.Models.Repository
{
    public class UserRespository: IUserRespository
    {
        public UserRespository(IConfiguration configuration)
        {
            //【1数据库连接字符串】连接字符串
            var connectionString1 = configuration.GetConnectionString("ConnectionString1");
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public User Login(string userName,string password)
        {
            return null;
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
    }
}
