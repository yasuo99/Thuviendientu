using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThuVienDienTu.Data;
using ThuVienDienTu.Models.ViewModels;

namespace ThuVienDienTu.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CMSController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public RevenueViewModel RevenueVM { get; set; }
        public CMSController(ApplicationDbContext db)
        {
            _db = db;
            RevenueVM = new RevenueViewModel()
            {
                Register = new Dictionary<string, int>(),
                Revenue = new Dictionary<string, double>(),
            };

        }
        public async Task<IActionResult> Index()
        {
            foreach(var topup in _db.TopupHistories.OrderBy(u => u.TopupDate).ToList())
            {
                if(RevenueVM.Revenue.ContainsKey(topup.TopupDate.Date.ToString("dd/MM/yyyy")))
                {
                    RevenueVM.Revenue[topup.TopupDate.Date.ToString("dd/MM/yyyy")] += topup.TopupAmount;
                }
                else
                {
                    RevenueVM.Revenue.Add(topup.TopupDate.Date.ToString("dd/MM/yyyy"), topup.TopupAmount);
                }
            }    
            foreach(var account in _db.ApplicationUsers.OrderBy(u => u.Date).ToList())
            {
                if (RevenueVM.Register.ContainsKey(account.Date.ToString("dd/MM/yyyy")))
                {
                    RevenueVM.Register[account.Date.ToString("dd/MM/yyyy")] += 1;
                }
                else
                {
                    RevenueVM.Register.Add(account.Date.ToString("dd/MM/yyyy"), 1);
                }
            }
            RevenueVM.OnSelling = await _db.Books.Where(u => u.Approved == true).CountAsync();
            RevenueVM.OnWaitingForCensor = await _db.Books.Where(u => u.Approved == false).CountAsync();
            RevenueVM.NumOfAccount = await _db.ApplicationUsers.Where(u => u.LockoutEnd.Value == null).CountAsync();
            RevenueVM.NumOfBanned = await _db.ApplicationUsers.Where(u => u.LockoutEnd > DateTime.Now).CountAsync();
            return View(RevenueVM);
        }
        public async Task<IActionResult> Search(string q = null)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            var controlleractionlist = asm.GetTypes()
                    .Where(type => typeof(Controller).IsAssignableFrom(type))
                    .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    .Select(x => new
                    {
                        Controller = x.DeclaringType.Name,
                        Action = x.Name,
                        Area = x.DeclaringType.CustomAttributes.Where(c => c.AttributeType == typeof(AreaAttribute))
                    }).ToList();
            List<string> controller = new List<string>();
            foreach (var item in controlleractionlist)
            {
                var area = item.Area.Select(v => v.ConstructorArguments[0].Value.ToString()).FirstOrDefault();
                if (item.Area.Select(v => v.ConstructorArguments[0].Value.ToString()).FirstOrDefault() == "Admin" && !controller.Contains(item.Controller))
                {
                    controller.Add(item.Controller);
                }
            }
            var desController = controller.Where(u => u.ToLower().Trim().Contains(q.ToLower().Trim())).FirstOrDefault();
            if (desController != null)
            {
                return RedirectToAction("Index", desController.Replace("Controller", ""), new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Index","CMS");
            }
        }
    }
}
