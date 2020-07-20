using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThuVienDienTu.Data;
using ThuVienDienTu.DesignPatterns.SingletonPatterns;
using ThuVienDienTu.Models;
using ThuVienDienTu.Models.ViewModels;
using ThuVienDienTu.Utility;

namespace ThuVienDienTu.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ChaptersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private ISingleton _iSingleton;
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
            _iSingleton = Singleton.GetInstance;
        }

        // GET: Admin/Chapters
        public async Task<IActionResult> Index(int productPage = 1,int bookId = 0)
        {
            StringBuilder param = new StringBuilder();
            param.Append("/Admin/Chapters?productPage=:");
            var applicationDbContext = _context.Chapters.Include(c => c.ApplicationUser).Include(c => c.Book).ToList();
            var count = applicationDbContext.Count();
            applicationDbContext = applicationDbContext.Skip((productPage - 1) * PageSize).Take(PageSize).ToList();
            PagingInfo pagingInfo = new PagingInfo()
            {
                TotalItems = count,
                urlParam = param.ToString(),
                CurrentPage = productPage,
                ItemsPerPage = PageSize
            };
            ChaptersVM.Chapters = applicationDbContext;
            ChaptersVM.Books = applicationDbContext.Select(u => u.Book).Distinct().ToList();
            if(bookId > 0)
            {
                ChaptersVM.Chapters = applicationDbContext.Where(u => u.BookId == bookId).ToList();
                ChaptersVM.Book = await _context.Books.Where(u => u.Id == bookId).FirstOrDefaultAsync();
            }
            ChaptersVM.PagingInfo = pagingInfo;
            return View(ChaptersVM);
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
            try
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
                    book.BookPrice = (int)Math.Round(sum * 0.90, MidpointRounding.AwayFromZero);
                    await _context.SaveChangesAsync();
                    _iSingleton.LogException("Thêm chương mới của sách Id: " + book.Id);
                    return RedirectToAction("Create", "Chapters", new { id = id });
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                _iSingleton.LogException(e.Message);
                return RedirectToAction("Error", "Log");
            }
           
            
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
                    if (User.IsInRole(SD.LIBRARIAN_ROLE))
                    {
                        chapter.Approved = false;
                    }
                    _context.Update(chapter);
                    _iSingleton.LogException("Chỉnh sửa chương Id: " + id + " của sách Id: " + chapter.BookId);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    if (!ChapterExists(chapter.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _iSingleton.LogException(e.Message);
                        return RedirectToAction("Error", "Log");
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
                    _iSingleton.LogException("Cập nhật chương Id: " + chapter.Id + " bởi kiểm duyệt viên email: " + user.Email);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    if (!ChapterExists(BooksVM.Chapter.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _iSingleton.LogException(e.Message);
                        return RedirectToAction("Error", "Log");
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
            try
            {
                var chapter = await _context.Chapters.FindAsync(id);
                _context.Chapters.Remove(chapter);
                await _context.SaveChangesAsync();
                _iSingleton.LogException("Xóa chương Id: " + id + " của sách Id: " + chapter.BookId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _iSingleton.LogException(e.Message);
                return RedirectToAction("Error", "Log");
            }
           
        }

        private bool ChapterExists(int id)
        {
            return _context.Chapters.Any(e => e.Id == id);
        }
    }
}
