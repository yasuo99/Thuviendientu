using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ThuVienDienTu.Data;

namespace ThuVienDienTu.ViewComponents
{
    public class CreditViewComponent: ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public CreditViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claimUser = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var user = await _db.ApplicationUsers.Where(u => u.Id == claimUser.Value).FirstOrDefaultAsync();
            return View(user);
        }
    }
}
