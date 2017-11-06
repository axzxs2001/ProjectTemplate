using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreTemplate.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace AspNetCoreTemplate.Controllers
{
    public class HomeController : Controller
    {

      

        /// <summary>
        /// 日志对象
        /// </summary>
        private readonly ILogger<HomeController> _logger;
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="logger">日志对象</param>
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            //日志对象
            _logger = logger;
            //连接字符串
            var connectionString1 = configuration.GetConnectionString("ConnectionString1");
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Home下的Index");
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
