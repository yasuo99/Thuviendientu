using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThuVienDienTu.Data;
using ThuVienDienTu.Models;
using ThuVienDienTu.Models.ViewModels;

namespace ThuVienDienTu.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public BooksViewModel BooksVM { get; set; }
        public BooksController(ApplicationDbContext db)
        {
            _db = db;
            BooksVM = new BooksViewModel()
            {
                Books = new List<Book>()
            };
        }
        public async Task<IActionResult> Index()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claimUser = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var user = await _db.ApplicationUsers.Where(u => u.Id == claimUser.Value).FirstOrDefaultAsync();
            var chaptersPurchased = await (from a in _db.Purchaseds
                                           join b in _db.Chapters
                                           on a.ChapterId equals b.Id
                                           where a.ApplicationUserId == user.Id
                                           select b).ToListAsync();
            List<Book> books = new List<Book>();
            foreach (var chapter in chaptersPurchased)
            {
                var book = await _db.Books.Where(u => u.Id == chapter.BookId).Include(u => u.Publisher).Include(u => u.Author).FirstOrDefaultAsync();
                if (!books.Contains(book))
                {
                    books.Add(book);
                }
            }
            BooksVM.Books = books;
            return View(BooksVM);
        }
    }
}