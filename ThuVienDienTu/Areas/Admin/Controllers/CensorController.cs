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
            var bookOnQueue = await _db.Books.Where(u => u.Approved == false).ToListAsync();
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
            BooksVM.Book = book;
            BooksVM.Chapters = chapterOfBook;
            return View(BooksVM);

        }
        public async Task<IActionResult> Approve(int id)
        {
            var book = _db.Books.Where(u => u.Id == id).FirstOrDefault();
            book.Approved = true;
            _db.Books.Update(book);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", new { productPage = 1 });
        }
        public async Task<IActionResult> Decline(int id)
        {
            var book = _db.Books.Where(u => u.Id == id).FirstOrDefault();
            _db.Books.Remove(book);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", new { productPage = 1 });
        }
    }
}