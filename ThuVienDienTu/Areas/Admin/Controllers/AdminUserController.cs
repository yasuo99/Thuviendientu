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
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ThuVienDienTu.Data;
using ThuVienDienTu.DesignPatterns.SingletonPatterns;
using ThuVienDienTu.Models;
using ThuVienDienTu.Models.ViewModels;
using ThuVienDienTu.Utility;

namespace ThuVienDienTu.Areas.Admin.Controllers
{
    //[Authorize(Roles = SD.ADMIN_ROLE)]
    [Area("Admin")]
    public class AdminUserController : Controller
    {

        private readonly ApplicationDbContext _db;
        private ISingleton _iSingleTon;
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
            _iSingleTon = Singleton.GetInstance;
        }
        public async Task<IActionResult> Index(int productPage = 1, string q = null, string userId = null)
        {
            try
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
                if (q == "Active")
                {
                    ApplicationUserVM.ApplicationUsers = ApplicationUserVM.ApplicationUsers.Where(u => u.LockoutEnd.HasValue == false).ToList();
                }
                if (q == "Deactive")
                {
                    ApplicationUserVM.ApplicationUsers = ApplicationUserVM.ApplicationUsers.Where(u => u.LockoutEnd.HasValue == true).ToList();
                }
                if (userId != null)
                {
                    ApplicationUserVM.ApplicationUser = await _db.ApplicationUsers.Where(u => u.Id == userId).FirstOrDefaultAsync();
                }
                return View(ApplicationUserVM);
            }
            catch (Exception e)
            {
                _iSingleTon.LogException(e.Message);
                throw;
            }
        }

        //Get Edit
        public async Task<IActionResult> Edit(string id)
        {
            try
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
            catch (Exception e)
            {
                _iSingleTon.LogException(e.Message);
                throw;
            }
           
        }

        //Post Edit
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(string id)
        {
            try
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
                        if (userFromDb.UserAvatar != null)
                        {
                            var oldPath = webRootPath + @"" + userFromDb.UserAvatar;
                            System.IO.File.Delete(oldPath);
                        }
                        var path = Path.Combine(webRootPath, SD.UserAvatar);
                        var extension = Path.GetExtension(files[0].FileName);
                        using (var fileStream = new FileStream(Path.Combine(path, id + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                        userFromDb.UserAvatar = @"\" + SD.UserAvatar + @"\" + id + extension;
                    }
                }
                return View();
            }
            catch (Exception e)
            {
                _iSingleTon.LogException(e.Message);
                throw;
            }
           
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
            try
            {
                ApplicationUser userFromDb = _db.ApplicationUsers.Where(u => u.Id == id).FirstOrDefault();
                userFromDb.LockoutEnd = DateTime.Now.AddYears(1000);

                _db.SaveChanges();
                _iSingleTon.LogException("Khóa tài khoản email: " + userFromDb.Email);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _iSingleTon.LogException(e.Message);
                return RedirectToAction("Error", "Log", new { area = "Admin" });
            }
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteForever(string id)
        {
            try
            {
                var user = await _db.ApplicationUsers.Where(u => u.Id == id).FirstOrDefaultAsync();
                _db.Remove(user);
                await _db.SaveChangesAsync();
                _iSingleTon.LogException("Xóa tài khoản email: " + user.Email);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                _iSingleTon.LogException(e.Message);
                return RedirectToAction("Error", "Log",new {area = "Admin"});
            }  
        }
    }
}