using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThuVienDienTu.Data;
using ThuVienDienTu.Models;
using ThuVienDienTu.Models.ViewModels;

namespace ThuVienDienTu.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ChaptersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private int PageSize = 10;
        [BindProperty]
        public BooksViewModel BooksVM { get; set; }
        [BindProperty]
        public ChaptersViewModel ChaptersVM { get; set; }     
        public ChaptersController(ApplicationDbContext context)
        {
            _context = context;
            ChaptersVM = new ChaptersViewModel()
            {
                Chapter = new Chapter(),
                Book = new Book(),
                Censor = new ApplicationUser(),
                Chapters = new List<Chapter>()
            };
            BooksVM = new BooksViewModel()
            {
                Chapter = new Chapter()
            };
        }

        // GET: Admin/Chapters
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Chapters.Include(c => c.ApplicationUser).Include(c => c.Book);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Chapters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chapter = await _context.Chapters
                .Include(c => c.ApplicationUser)
                .Include(c => c.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chapter == null)
            {
                return NotFound();
            }

            return View(chapter);
        }

        // GET: Admin/Chapters/Create
        //Tạo chương mới cho sách
        public IActionResult Create(int? id)
        {
            var bookFromDb = _context.Books.Where(u => u.Id == id).FirstOrDefault(); //Lấy ra cuốn sách để thêm chương
            ChaptersVM.Book = bookFromDb;
            var userFromDb = _context.ApplicationUsers.Where(u => u.Email == User.Identity.Name).FirstOrDefault(); //Lấy ra user đang làm thao tác thêm chương
            var chapters = _context.Chapters.Where(u => u.BookId == id).ToList();
            ChaptersVM.Censor = userFromDb;
            ChaptersVM.Chapters = chapters;
            return View(ChaptersVM);
        }

        // POST: Admin/Chapters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id)
        {
            if (ModelState.IsValid)
            {
                var chapter = ChaptersVM.Chapter;
                chapter.EditDate = DateTime.Now;
                chapter.ApplicationUserId = ChaptersVM.Censor.Id;
                chapter.BookId = ChaptersVM.Book.Id;
                chapter.Approved = false;
                _context.Add(chapter);
                var book = await _context.Books.Where(u => u.Id == ChaptersVM.Book.Id).FirstOrDefaultAsync();
                var sum = book.BookPrice + chapter.Price;
                book.BookPrice = (int)Math.Round(sum * 0.90,MidpointRounding.AwayFromZero);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "Chapters", new { id = id });
            }
            return View(ChaptersVM);
        }

        // GET: Admin/Chapters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chapter = await _context.Chapters.FindAsync(id);
            if (chapter == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", chapter.ApplicationUserId);
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id", chapter.BookId);
            return View(chapter);
        }

        // POST: Admin/Chapters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookId,Chaptername,ChapterContent,Price,EditDate,ApplicationUserId")] Chapter chapter)
        {
            if (id != chapter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    chapter.Approved = false;
                    _context.Update(chapter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChapterExists(chapter.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", chapter.ApplicationUserId);
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id", chapter.BookId);
            return View(chapter);
        }
        [HttpPost, ActionName("CensorEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CensorEdit()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _context.ApplicationUsers.Where(u => u.Email == User.Identity.Name).FirstOrDefaultAsync();
                    var chapter = await _context.Chapters.Where(u => u.Id == BooksVM.Chapter.Id).FirstOrDefaultAsync();
                    chapter.ChapterContent = BooksVM.Chapter.ChapterContent;
                    chapter.ApplicationUserId = user.Id;
                    chapter.EditDate = DateTime.Now;
                    _context.Update(chapter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChapterExists(BooksVM.Chapter.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Read", new { area = "Customer", id = BooksVM.Chapter.BookId });
            }
            return RedirectToAction("Index", "Read", new { area = "Customer", id = BooksVM.Chapter.BookId });
        }
        // GET: Admin/Chapters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chapter = await _context.Chapters
                .Include(c => c.ApplicationUser)
                .Include(c => c.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chapter == null)
            {
                return NotFound();
            }

            return View(chapter);
        }

        // POST: Admin/Chapters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chapter = await _context.Chapters.FindAsync(id);
            _context.Chapters.Remove(chapter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChapterExists(int id)
        {
            return _context.Chapters.Any(e => e.Id == id);
        }
    }
}
