using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ThuVienDienTu.Data;
using ThuVienDienTu.DesignPatterns.SingletonPatterns;
using ThuVienDienTu.Models;
using ThuVienDienTu.Models.ViewModels;
using ThuVienDienTu.Utility;

namespace ThuVienDienTu.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private ISingleton _iSingleTon;
        private int PageSize = 10;
        private IWebHostEnvironment _hostEnvironment;
        [BindProperty]
        public AuthorViewModel AuthorsVM { get; set; }
        public AuthorsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            AuthorsVM = new AuthorViewModel()
            {
                Author = new Author(),
                Authors = new List<Author>(),
                PagingInfo = new PagingInfo()
            };
            _iSingleTon = Singleton.GetInstance;
        }

        // GET: Admin/Authors
        //[Authorize(Roles = SD.ADMIN_ROLE + "," + SD.LIBRARIAN_ROLE)]
        public async Task<IActionResult> Index(int productPage = 1)
        {
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "CountryName");
            var authors = await _context.Authors.Include(a => a.Country).ToListAsync();
            StringBuilder param = new StringBuilder();
            param.Append("/Admin/Authors?productPage=:");
            var count = authors.Count;
            authors = authors.Skip((productPage - 1) * PageSize).Take(PageSize).ToList();
            AuthorsVM.PagingInfo = new PagingInfo()
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItems = count,
                urlParam = param.ToString()
            };
            AuthorsVM.Authors = authors;
            return View(AuthorsVM);
        }

        // GET: Admin/Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .Include(a => a.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // GET: Admin/Authors/Create
        //[Authorize(Roles = SD.ADMIN_ROLE + "," + SD.LIBRARIAN_ROLE)]
        public IActionResult Create()
        {
            var countriesFromDb = _context.Countries.ToList();
            if (countriesFromDb == null)
            {
                return RedirectToAction("Create", "Countries");
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "CountryName");
            return View();
        }

        // POST: Admin/Authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = SD.ADMIN_ROLE + "," + SD.LIBRARIAN_ROLE)]
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!SameName(AuthorsVM.Author.Signed, AuthorsVM.Author.CountryId))
                    {
                        _context.Add(AuthorsVM.Author);
                        await _context.SaveChangesAsync();
                        var webRootPath = _hostEnvironment.WebRootPath;
                        var files = HttpContext.Request.Form.Files;
                        if (files.Count > 0)
                        {
                            var uploads = Path.Combine(webRootPath, SD.AuthorImageFolder);
                            var extension = Path.GetExtension(files[0].FileName);
                            using (var fileStream = new FileStream(Path.Combine(uploads, AuthorsVM.Author.Id + extension), FileMode.Create))
                            {
                                files[0].CopyTo(fileStream);
                            }
                            AuthorsVM.Author.Avatar = @"\" + SD.AuthorImageFolder + @"\" + AuthorsVM.Author.Id + extension;
                        }
                        else
                        {
                            var upload = Path.Combine(webRootPath, SD.DefaultUserAvatar);
                            System.IO.File.Copy(upload, webRootPath + @"\" + SD.AuthorImageFolder + @"\" + AuthorsVM.Author.Id + ".png");
                            AuthorsVM.Author.Avatar = @"\" + SD.AuthorImageFolder + @"\" + AuthorsVM.Author.Id + ".png";
                        }
                        _iSingleTon.LogException("Thêm tác giả Id:" + AuthorsVM.Author.Id);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        AuthorsVM.Authors = await _context.Authors.ToListAsync();
                        ModelState.AddModelError("SameName", "Trùng kí danh tác giả");
                        return View(nameof(Index), AuthorsVM);
                    }


                }
                ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "CountryName", AuthorsVM.Author.CountryId);
                return View(nameof(Index), AuthorsVM);
            }
            catch (Exception e)
            {
                _iSingleTon.LogException(e.Message);
                return RedirectToAction("Error", "Log");
            }

        }

        // GET: Admin/Authors/Edit/5
        //[Authorize(Roles = SD.ADMIN_ROLE + "," + SD.LIBRARIAN_ROLE)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Id", author.CountryId);
            return View(author);
        }

        // POST: Admin/Authors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = SD.ADMIN_ROLE + "," + SD.LIBRARIAN_ROLE)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Avatar,FullName,Signed,CountryId,Description")] Author author)
        {
            if (id != author.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(author);
                    await _context.SaveChangesAsync();
                    var webRootPath = _hostEnvironment.WebRootPath;
                    var files = HttpContext.Request.Form.Files;
                    if (files.Count > 0)
                    {
                        if (author.Avatar != null)
                        {
                            var removeUploaded = webRootPath + @"" + author.Avatar;
                            System.IO.File.Delete(removeUploaded);
                        }
                        var uploads = Path.Combine(webRootPath, SD.AuthorImageFolder);
                        var extension = Path.GetExtension(files[0].FileName);
                        using (var fileStream = new FileStream(Path.Combine(uploads, author.Id + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                        author.Avatar = @"\" + SD.AuthorImageFolder + @"\" + author.Id + extension;
                    }
                    _iSingleTon.LogException("Chỉnh sửa tác giả id: " + author.Id);

                }
                catch (DbUpdateConcurrencyException e)
                {
                    if (!AuthorExists(author.Id))
                    {
                        _iSingleTon.LogException(e.Message);
                        return NotFound();
                    }
                    else
                    {
                        return RedirectToAction("Error", "Log");
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "CountryName", author.CountryId);
            return View(author);

        }
        //[Authorize(Roles = SD.ADMIN_ROLE + "," + SD.LIBRARIAN_ROLE)]
        // GET: Admin/Authors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .Include(a => a.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: Admin/Authors/Delete/5
        //[Authorize(Roles = SD.ADMIN_ROLE + "," + SD.LIBRARIAN_ROLE)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int authorid)
        {
            try
            {
                var author = await _context.Authors.FindAsync(authorid);
                if (author.Avatar != null)
                {
                    var webRootPath = _hostEnvironment.WebRootPath;
                    var removeUploaded = webRootPath + @"" + author.Avatar;
                    System.IO.File.Delete(removeUploaded);
                }
                _context.Authors.Remove(author);
                _iSingleTon.LogException("Xóa tác giả Id: " + authorid + " " + author.Signed);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _iSingleTon.LogException(e.Message);
                return RedirectToAction("Error", "Log");
            }
           
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }
        private bool SameName(string authorName, int countryId)
        {
            return _context.Authors.Any(u => u.Signed == authorName && u.CountryId == countryId);
        }
    }
}
