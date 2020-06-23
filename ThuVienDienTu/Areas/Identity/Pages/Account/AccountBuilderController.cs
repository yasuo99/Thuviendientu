﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ThuVienDienTu.Data;
using ThuVienDienTu.DesignPatterns.BuilderPatterns;

namespace ThuVienDienTu.Areas.Identity.Pages.Account
{
    [Area("Identity")]
    public class AccountBuilderController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountBuilderController(ApplicationDbContext dbContext,UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult CreateAdminAccount()
        {
            return RedirectToAction("Index", "ApplicationUser", new { area = "Admin" });
        }
        public IActionResult CreateAccount()
        {
            var readerAccountBuilder = new AccountDirector(new ReaderAccountBuilder(_db,_roleManager,_userManager));
            readerAccountBuilder.Construct();
            var account = readerAccountBuilder.GetAccount();
            return RedirectToAction("InformAccount", "Account", new { area = "Customer", id = account.Id });
        }
    }
}
