﻿
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThuVienDienTu.Models;
using ThuVienDienTu.Utility;

namespace ThuVienDienTu.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task Initialize()
        {
            if (_db.Database.GetPendingMigrations().Count() > 0)
            {
                _db.Database.Migrate();
            }

            if (_db.Roles.Any(r => r.Name == SD.ADMIN_ROLE)) return;
            _roleManager.CreateAsync(new IdentityRole(SD.ADMIN_ROLE)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.LIBRARIAN_ROLE)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.CENSOR_ROLE)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.READER_ROLE)).GetAwaiter().GetResult();
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "yasuo120999@gmail.com",
                Email = "yasuo120999@gmail.com",
                DisplayName = "Thanh",
                PhoneNumber = "0983961054",
                Address = "Quận 9",
                Date = DateTime.Now,
                EmailConfirmed = true,
            }, "Thanhpro1@").GetAwaiter().GetResult();
            ApplicationUser usertodo = _db.ApplicationUsers.Where(u => u.Email == "yasuo120999@gmail.com").FirstOrDefault();
            _userManager.AddToRoleAsync(usertodo, SD.ADMIN_ROLE).GetAwaiter().GetResult();
        }


    }
}
