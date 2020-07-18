using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ThuVienDienTu.DesignPatterns.SingletonPatterns;

namespace ThuVienDienTu.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LogController : Controller
    {
        public LogController()
        {
        }
        public IActionResult Index()
        {
            List<string> log = new List<string>();
            string logFilePath = string.Format(@"{0}\{1}", AppDomain.CurrentDomain.BaseDirectory, "exception.log");
            var file = System.IO.File.ReadAllText(logFilePath);
            foreach(var row in file.Split("\n"))
            {
                log.Add(row);
            }    
            return View(log);
        }
        public IActionResult Error()
        {
            return View();
        }
    }
}
