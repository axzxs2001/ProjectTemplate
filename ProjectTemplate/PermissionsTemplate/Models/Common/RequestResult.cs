using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PermissionsTemplate.Models.Common
{
    /// <summary>
    /// 客户请求返回标识
    /// </summary>
    public enum HttpResult
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,
        /// <summary>
        /// 失败
        /// </summary>
        Fail = 0,
        /// <summary>
        /// 错误
        /// </summary>
        Error = -1,
        /// <summary>
        /// 异常
        /// </summary>
        Exception = -2
    }
}
