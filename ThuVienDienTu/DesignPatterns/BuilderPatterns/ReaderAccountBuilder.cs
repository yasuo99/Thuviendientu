using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PayPal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ThuVienDienTu.Data;
using ThuVienDienTu.Models;
using ThuVienDienTu.Utility;

namespace ThuVienDienTu.DesignPatterns.BuilderPatterns
{
    public class ReaderAccountBuilder : IAccountBuilder
    {
        private ApplicationUser userAccount = new ApplicationUser();
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        public ReaderAccountBuilder(ApplicationDbContext db, RoleManager<IdentityRole> roleManager,UserManager<IdentityUser> userManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task BuildAccount()
        {
            var numOfAccount = _db.ApplicationUsers.Count();
            var result = _userManager.CreateAsync(userAccount, "Reader" + numOfAccount.ToString() + "@").GetAwaiter().GetResult();
            if (result.Succeeded)
            {
               _userManager.AddToRoleAsync(userAccount, SD.READER_ROLE).GetAwaiter().GetResult();
            }

        }

        public void BuildInfor()
        {
            var numOfAccount = _db.ApplicationUsers.Count();
            userAccount.Email = "reader" + numOfAccount.ToString() + "@reader.com";
            userAccount.UserName = userAccount.Email;
            userAccount.UserAvatar = SD.DefaultUserAvatar;
            userAccount.DisplayName = "Độc giả " + numOfAccount.ToString();
            userAccount.Fullname = "Độc giả số " + numOfAccount;
            userAccount.Address = "Reader" + numOfAccount.ToString() + "@";
            userAccount.Balance = 0;
            userAccount.EmailConfirmed = true;
        }
        public void BuildDate() {
            userAccount.Date = DateTime.Now;
             }
        public ApplicationUser GetAccount()
        {
            return userAccount;
        }
    }
}
