using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ThuVienDienTu.Data;
using ThuVienDienTu.Models;

namespace ThuVienDienTu.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signinManager;
        private readonly ApplicationDbContext _context;
        public AccountController(ApplicationDbContext context,SignInManager<IdentityUser> signinManager) 
        {
            _context = context;
            _signinManager = signinManager;
        }
        public async Task<IActionResult> InformAccount(string id)
        {
            var user = await _context.ApplicationUsers.FindAsync(id);
            return View("Successful",user);
        }
    }
}
