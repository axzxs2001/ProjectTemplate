using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreTemplate.Models
{
    /// <summary>
    /// 用户实体
    /// </summary>
    public class User
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public string Role
        { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        { get; set; }
    }
}
