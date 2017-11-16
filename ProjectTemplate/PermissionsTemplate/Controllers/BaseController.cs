using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PermissionsTemplate.Models;
using PermissionsTemplate.Models.Common;

namespace PermissionsTemplate.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 转JsonResult对象
        /// </summary>
        /// <param name="result">结果</param>
        /// <param name="data">返回数据</param>
        /// <param name="message">信息</param>
        /// <param name="dateFormatString">日期格式</param>
        /// <returns></returns>
        protected JsonResult ToJson(HttpResult result, dynamic data=null, string message=null, string dateFormatString = "yyyy年MM月dd日")
        {
            return new JsonResult(new { result = (int)result, data = data, message = message }, new Newtonsoft.Json.JsonSerializerSettings()
            {
                DateFormatString = dateFormatString,
                ContractResolver = new LowercaseContractResolver()
            });
        }
    }
}