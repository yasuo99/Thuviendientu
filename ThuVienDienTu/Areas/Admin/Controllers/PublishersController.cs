using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThuVienDienTu.Data;
using ThuVienDienTu.DesignPatterns.SingletonPatterns;
using ThuVienDienTu.Models;
using ThuVienDienTu.Utility;

namespace ThuVienDienTu.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class PublishersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private ISingleton _iSingleton;
        private IWebHostEnvironment _hostEnvironment;
        public PublishersController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _iSingleton = Singleton.GetInstance;
        }

        // GET: Admin/Publishers
        //[Authorize(Roles = SD.ADMIN_ROLE + "," + SD.LIBRARIAN_ROLE)]
        public async Task<IActionResult> Index()
        {
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "CountryName");
            var applicationDbContext = _context.Publishers.Include(p => p.Country);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Publishers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publisher = await _context.Publishers
                .Include(p => p.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (publisher == null)
            {
                return NotFound();
            }

            return View(publisher);
        }

        // GET: Admin/Publishers/Create
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

        // POST: Admin/Publishers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = SD.ADMIN_ROLE + "," + SD.LIBRARIAN_ROLE)]
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST(string publisherName, string description, int countryId)
        {
            try
            {
                Publisher publisher = new Publisher()
                {
                    PublisherName = publisherName,
                    Description = description,
                    CountryId = countryId
                };
                if (ModelState.IsValid)
                {
                    _context.Add(publisher);
                    await _context.SaveChangesAsync();
                    var webRootPath = _hostEnvironment.WebRootPath;
                    var files = HttpContext.Request.Form.Files;
                    if (files.Count != 0)
                    {
                        var uploads = Path.Combine(webRootPath, SD.PublisherImageFolder);
                        var extension = Path.GetExtension(files[0].FileName);
                        using (var fileStream = new FileStream(Path.Combine(uploads, publisher.Id + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        };
                        publisher.PublisherLogo = @"\" + SD.PublisherImageFolder + @"\" + publisher.Id + extension;
                    }
                    else
                    {
                        var uploads = Path.Combine(webRootPath, SD.DefaultPublisherLogo);
                        System.IO.File.Copy(uploads, webRootPath + @"\" + SD.PublisherImageFolder + @"\" + publisher.Id + ".png");
                        publisher.PublisherLogo = @"\" + SD.PublisherImageFolder + @"\" + publisher.Id + ".png";
                    }
                    await _context.SaveChangesAsync();
                    _iSingleton.LogException("Thêm NXB Id: " + publisher.Id + " " + publisher.PublisherName);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "CountryName", publisher.CountryId);
                return View(publisher);
            }
            catch (Exception e)
            {
                _iSingleton.LogException(e.Message);
                return RedirectToAction("Error", "Log");
            }

        }

        // GET: Admin/Publishers/Edit/5
        //[Authorize(Roles = SD.ADMIN_ROLE + "," + SD.LIBRARIAN_ROLE)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "CountryName", publisher.CountryId);
            return View(publisher);
        }

        // POST: Admin/Publishers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = SD.ADMIN_ROLE + "," + SD.LIBRARIAN_ROLE)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PublisherName,PublisherLogo,Description,CountryId")] Publisher publisher)
        {
            if (id != publisher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(publisher);
                    var webRootPath = _hostEnvironment.WebRootPath;
                    var files = HttpContext.Request.Form.Files;
                    if (files.Count > 0)
                    {
                        if (publisher.PublisherLogo != null)
                        {
                            var removeUpload = webRootPath + @"" + publisher.PublisherLogo;
                            System.IO.File.Delete(removeUpload);
                        }
                        var uploads = Path.Combine(webRootPath, SD.PublisherImageFolder);
                        var extension = Path.GetExtension(files[0].FileName);
                        using (var fileStream = new FileStream(Path.Combine(uploads, publisher.Id + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                        publisher.PublisherLogo = @"\" + SD.PublisherImageFolder + @"\" + publisher.Id + extension;
                    }
                }
                catch (DbUpdateConcurrencyException e)
                {
                    if (!PublisherExists(publisher.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _iSingleton.LogException(e.Message);
                        return RedirectToAction("Error", "Log");
                    }
                }
                _iSingleton.LogException("Cập nhật NXB Id: " + id);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "CountryName", publisher.CountryId);
            return View(publisher);

        }

        // GET: Admin/Publishers/Delete/5
        //[Authorize(Roles = SD.ADMIN_ROLE + "," + SD.LIBRARIAN_ROLE)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publisher = await _context.Publishers
                .Include(p => p.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (publisher == null)
            {
                return NotFound();
            }

            return View(publisher);
        }

        // POST: Admin/Publishers/Delete/5
        //[Authorize(Roles = SD.ADMIN_ROLE + "," + SD.LIBRARIAN_ROLE)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int publisherid)
        {
            var publisher = await _context.Publishers.FindAsync(publisherid);
            try
            {
                if (publisher.PublisherLogo != null)
                {
                    var webRootPath = _hostEnvironment.WebRootPath;
                    var removeUploaded = webRootPath + @"" + publisher.PublisherLogo;
                    System.IO.File.Delete(removeUploaded);
                }
                _context.Publishers.Remove(publisher);
                await _context.SaveChangesAsync();
                _iSingleton.LogException("Xóa NXB : " + publisher.PublisherName);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _iSingleton.LogException(e.Message);
                return RedirectToAction("Error", "Log");
            }
           
        }

        private bool PublisherExists(int id)
        {
            return _context.Publishers.Any(e => e.Id == id);
        }
    }
}
