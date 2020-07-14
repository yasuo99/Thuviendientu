using System;
using System.Collections.Generic;
using System.Linq;
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
            return View();
        }
    }
}
