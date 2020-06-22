using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Threading.Tasks;
using ThuVienDienTu.Data;

namespace ThuVienDienTu.UsernameViewComponents
{
    public class UsernameViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public UsernameViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimIdentity = (ClaimsIdentity)this.User.Identity;
            var claimUser = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userFromDb = await _db.ApplicationUsers.Where(u => u.Id == claimUser.Value).FirstOrDefaultAsync();
            return View(userFromDb);
        }
    }
}
