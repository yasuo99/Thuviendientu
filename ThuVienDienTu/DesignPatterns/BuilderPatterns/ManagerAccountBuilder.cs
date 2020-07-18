using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThuVienDienTu.Data;
using ThuVienDienTu.Models;
using ThuVienDienTu.Utility;

namespace ThuVienDienTu.DesignPatterns.BuilderPatterns
{
    public class ManagerAccountBuilder : IAccountBuilder
    {
        private ApplicationUser user;
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        public ManagerAccountBuilder(ApplicationDbContext db, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
            user = new ApplicationUser();
        }
        public async Task BuildAccount()
        {
            var numOfAccount = _db.ApplicationUsers.Count();
            var result = _userManager.CreateAsync(user, "Librarian" + numOfAccount.ToString() + "@").GetAwaiter().GetResult();
            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(user, SD.LIBRARIAN_ROLE).GetAwaiter().GetResult();
            }
        }

        public void BuildDate()
        {
            user.Date = DateTime.Now;
        }

        public void BuildInfor()
        {
            var numOfAccount = _db.ApplicationUsers.Count();
            user.Email = "librarian" + numOfAccount.ToString() + "@tlth.com";
            user.UserName = user.Email;
            user.UserAvatar = SD.DefaultUserAvatar;
            user.DisplayName = "Thủ thư " + numOfAccount.ToString();
            user.Fullname = "Thủ thư số " + numOfAccount;
            user.Address = "Librarian" + numOfAccount.ToString() + "@";
            user.Balance = 0;
            user.EmailConfirmed = true;
        }

        public ApplicationUser GetAccount()
        {
            return user;
        }
    }
}
