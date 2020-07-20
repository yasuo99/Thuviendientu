using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThuVienDienTu.Data;
using ThuVienDienTu.Models;
using ThuVienDienTu.Models.ViewModels;
using ThuVienDienTu.Utility;

namespace ThuVienDienTu.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.ADMIN_ROLE + "," + SD.CENSOR_ROLE)]
    [Area("Admin")]
    public class CensorController : Controller
    {
        private readonly ApplicationDbContext _db;
        private int PageSize = 10;
        [BindProperty]
        public BooksViewModel BooksVM { get; set; }
        public CensorController(ApplicationDbContext db)
        {
            _db = db;
            BooksVM = new BooksViewModel()
            {
                Books = new List<Book>(),
                Book = new Book(),
                PagingInfo = new PagingInfo()
            };
        }
        public async Task<IActionResult> Index(int productPage = 1)
        {
            StringBuilder param = new StringBuilder();
            param.Append("/Admin/Censor?productPage=:");
            var bookOnQueue = await _db.Books.ToListAsync();
            var count = bookOnQueue.Count;
            bookOnQueue.Skip((productPage - 1) * PageSize).Take(PageSize).ToList();
            BooksVM.Books = bookOnQueue;
            BooksVM.PagingInfo = new PagingInfo()
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItems = count,
                urlParam = param.ToString()
            };
            return View(BooksVM);
        }
        public async Task<IActionResult> Details(int id)
        {
            var book = await _db.Books.Where(u => u.Id == id).Include(a => a.Author).Include(b => b.Publisher).FirstOrDefaultAsync();
            var chapterOfBook = await _db.Chapters.Where(u => u.BookId == id).ToListAsync();
            book.Chapters = chapterOfBook;
            return View(book);

        }
        public async Task<IActionResult> ChapterApprove(int id)
        {
            var chapter = await  _db.Chapters.Where(u => u.Id == id).FirstOrDefaultAsync();
            chapter.Approved = true;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Read", new {area = "Customer", id = chapter.BookId});
        }
        public async Task<IActionResult> ChapterDecline(int id)
        {
            var chapter = await _db.Chapters.Where(u => u.Id == id).FirstOrDefaultAsync();
            chapter.Approved = true;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Read", new { area = "Customer", id = chapter.BookId });
        }
        public async Task<IActionResult> Publish(int id)
        {
            var book = await _db.Books.Where(u => u.Id == id).FirstOrDefaultAsync();
            if(book.Approved)
            {
                book.Approved = false;
            }
            else
            {
                book.Approved = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", new { productPage = 1 });
        }
    }
}