using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ThuVienDienTu.Data;
using ThuVienDienTu.Models;
using ThuVienDienTu.Models.ViewModels;
using ThuVienDienTu.Utility;

namespace ThuVienDienTu.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.ADMIN_ROLE)]
    [Area("Admin")]
    public class AdminUserController : Controller
    {

        private readonly ApplicationDbContext _db;
        private int PageSize = 10;
        public IWebHostEnvironment _hostEnvironment { get; set; }
        [BindProperty]
        public ApplicationUserViewModel UserVM { get; set; }
        public AdminUserController(ApplicationDbContext db,IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;
            UserVM = new ApplicationUserViewModel()
            {
                ApplicationUser = new ApplicationUser()
            };
        }
        public IActionResult Index(int productPage = 1)
        {
            ApplicationUserViewModel ApplicationUserVM = new ApplicationUserViewModel()
            {
                ApplicationUsers = new List<Models.ApplicationUser>()
            };
            StringBuilder param = new StringBuilder();

            param.Append("/Admin/AdminUser?productPage=:");
            ApplicationUserVM.ApplicationUsers = _db.ApplicationUsers.Where(u => u.Email != User.Identity.Name).ToList();

            var count = ApplicationUserVM.ApplicationUsers.Count;
            ApplicationUserVM.ApplicationUsers = ApplicationUserVM.ApplicationUsers.Skip((productPage - 1) * PageSize).Take(PageSize).ToList();
            ApplicationUserVM.PagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItems = count,
                urlParam = param.ToString()
            };

            return View(ApplicationUserVM);
        }

        //Get Edit
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || id.Trim().Length == 0)
            {
                return NotFound();
            }

            var userFromDb = await _db.ApplicationUsers.FindAsync(id);

            if (userFromDb == null)
            {
                return NotFound();
            }          
            UserVM.ApplicationUser = userFromDb;          
            return View(UserVM);
        }


        //Post Edit
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(string id)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser userFromDb = _db.ApplicationUsers.Where(u => u.Id == id).FirstOrDefault();
                userFromDb.DisplayName = UserVM.ApplicationUser.DisplayName;
                userFromDb.PhoneNumber = UserVM.ApplicationUser.PhoneNumber;
                userFromDb.Address = UserVM.ApplicationUser.Address;
                var webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files != null)
                {
                    if(userFromDb.UserAvatar != null)
                    {
                        var oldPath = webRootPath + @"" + userFromDb.UserAvatar;
                        System.IO.File.Delete(oldPath);
                    }
                    var path = Path.Combine(webRootPath, SD.UserAvatar);
                    var extension = Path.GetExtension(files[0].FileName);
                    using(var fileStream = new FileStream(Path.Combine(path,id + extension),FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    userFromDb.UserAvatar = @"\" + SD.UserAvatar + @"\" + id + extension;
                }
            } 
            return View();
        }


        //Get Delete
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || id.Trim().Length == 0)
            {
                return NotFound();
            }

            var userFromDb = await _db.ApplicationUsers.FindAsync(id);
            if (userFromDb == null)
            {
                return NotFound();
            }

            return View(userFromDb);
        }


        //Post Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(string id)
        {
            ApplicationUser userFromDb = _db.ApplicationUsers.Where(u => u.Id == id).FirstOrDefault();
            userFromDb.LockoutEnd = DateTime.Now.AddYears(1000);

            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}