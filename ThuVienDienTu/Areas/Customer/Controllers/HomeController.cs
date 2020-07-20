using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ThuVienDienTu.Data;
using ThuVienDienTu.Extensions;
using ThuVienDienTu.Models;
using ThuVienDienTu.Models.ViewModels;
using ThuVienDienTu.Utility;

namespace ThuVienDienTu.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private int PageSize = 16;
        [BindProperty]
        public BooksViewModel BooksVM { get; set; }
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
            BooksVM = new BooksViewModel()
            {
                Book = new Book(),
                BooksSeen = new List<Book>(),
                Books = new List<Book>(),
                Authors = new List<Author>(),
                Publishers = new List<Publisher>(),
                Countries = new List<Country>(),
                Chapters = new List<Chapter>(),
                Reviews = new List<Review>(),
                Comments = new List<Comment>(),
                ReadingListsVM = new List<ReadingListViewModel>(),
                Error = null,
                AlreadyBought = 0
            };
        }
        public async Task<IActionResult> Index(int productPage = 1, string timkiem = null)
        {
            if (User.IsInRole(SD.ADMIN_ROLE) || User.IsInRole(SD.LIBRARIAN_ROLE) || User.IsInRole(SD.CENSOR_ROLE))
            {
                return RedirectToAction("Index", "CMS", new { area = "Admin" });
            }
            StringBuilder param = new StringBuilder();
            param.Append("Customer/Home?productPage=:");
            var books = await _db.Books.Include(u => u.Author).Include(u => u.Publisher).Where(u => u.Approved == true).ToListAsync();
            var authors = await _db.Authors.ToListAsync();
            var publishers = await _db.Publishers.ToListAsync();
            var countries = await _db.Countries.ToListAsync();
            if (timkiem != null)
            {
                books = books.Where(u => u.BookName.ToLower().Contains(timkiem.ToLower()) || u.Publisher.PublisherName.ToLower().Contains(timkiem.ToLower())
                || u.Author.Signed.ToLower().Contains(timkiem.ToLower()) || u.Author.FullName.ToLower().Contains(timkiem.ToLower())).ToList();
            }
            books = books.Skip((productPage - 1) * PageSize).Take(PageSize).ToList();
            BooksVM.Books = books;
            var count = books.Count;
            BooksVM.PagingInfo = new PagingInfo()
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItems = count,
                urlParam = param.ToString()
            };
            List<int> booksSeen = HttpContext.Session.Get<List<int>>("BookSeenSession");
            if (booksSeen == null)
            {
                booksSeen = new List<int>();
                HttpContext.Session.Set("BookSeenSession", booksSeen);

            }
            else
            {
                foreach (var item in booksSeen)
                {
                    BooksVM.BooksSeen.Add(await _db.Books.Where(u => u.Id == item).FirstOrDefaultAsync());
                }
            }
            BooksVM.Authors = authors;
            BooksVM.Publishers = publishers;
            BooksVM.Countries = countries;
            return View(BooksVM);
        }
        public async Task<IActionResult> Details(int id)
        {
            List<int> booksSeen = HttpContext.Session.Get<List<int>>("BookSeenSession");
            if (booksSeen == null)
            {
                booksSeen = new List<int>();
            }
            else
            {
                foreach (var item in booksSeen)
                {
                    BooksVM.BooksSeen.Add(await _db.Books.Where(u => u.Id == item).FirstOrDefaultAsync());
                }
            }
            var bookSeen = await _db.Books.Where(u => u.Id == id).FirstOrDefaultAsync();
            if (!booksSeen.Contains(bookSeen.Id))
            {
                booksSeen.Add(bookSeen.Id);
            }
            HttpContext.Session.Set("BookSeenSession", booksSeen);
            var bookFromDb = await _db.Books.Where(u => u.Id == id).Include(u => u.Publisher).Include(u => u.Author).FirstOrDefaultAsync();
            bookFromDb.Accesscount++;
            
            var claimIdentity = (ClaimsIdentity)User.Identity;
            if (claimIdentity.Claims.Count() > 0)
            {
                var claimUser = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var user = await _db.ApplicationUsers.Where(u => u.Id == claimUser.Value).FirstOrDefaultAsync();
                var chaptersPurchased = await (from a in _db.Purchaseds
                                               join b in _db.Chapters
                                               on a.ChapterId equals b.Id
                                               where a.ApplicationUserId == user.Id
                                               select b).ToListAsync();
                foreach (var chapter in chaptersPurchased)
                {
                    if (chapter.BookId == id)
                    {
                        BooksVM.AlreadyBought++;
                    }
                }
                var readingListOfUser = await _db.ReadingLists.Where(u => u.ApplicationUserId == user.Id).ToListAsync();
                foreach (var readingList in readingListOfUser)
                {
                    ReadingListViewModel readingListViewModel = new ReadingListViewModel();
                    readingListViewModel.ReadingList = readingList;
                    var bookAlreadyInList = await _db.BookInLists.Where(u => u.BookId == id && u.ReadingListId == readingList.Id).FirstOrDefaultAsync();
                    if (bookAlreadyInList != null)
                    {
                        readingListViewModel.AlreadyIn = true;
                    }
                    else
                    {
                        readingListViewModel.AlreadyIn = false;
                    }
                    BooksVM.ReadingListsVM.Add(readingListViewModel);
                }

            }
            var chapterOfBook = await _db.Chapters.Where(u => u.BookId == id && u.Approved == true).ToListAsync();
            var reviewOfBook = await _db.Reviews.Where(u => u.BookId == id).Include(u => u.ApplicationUser).ToListAsync();
            var commentOfBook = await _db.Comments.Where(u => u.BookId == id).Include(u => u.ApplicationUser).OrderByDescending(u => u.Date).ToListAsync();
            var authorOfBook = await _db.Authors.Where(u => u.Id == bookFromDb.AuthorId).Include(u => u.Country).FirstOrDefaultAsync();
            if(reviewOfBook.Count > 0)
            {
                BooksVM.Rating = (int)(reviewOfBook.Sum(u => u.Star) / reviewOfBook.Count);

            }
            BooksVM.Chapters = chapterOfBook;
            BooksVM.Book = bookFromDb;
            BooksVM.Reviews = reviewOfBook;
            BooksVM.Comments = commentOfBook;
            BooksVM.Author = authorOfBook;
            await _db.SaveChangesAsync();
            return View(BooksVM);
        }
        [ActionName("Details")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SD.READER_ROLE)]
        public async Task<IActionResult> DetailsPost(int id)
        {
            var book = await _db.Books.Where(u => u.Id == id).Include(u => u.Author).Include(u => u.Publisher).FirstOrDefaultAsync();
            var user = await _db.ApplicationUsers.Where(u => u.Email == User.Identity.Name).FirstOrDefaultAsync();
            var chapterOfBook = await _db.Chapters.Where(u => u.BookId == id).ToListAsync();
            var reviewOfBook = await _db.Reviews.Where(u => u.BookId == id).ToListAsync();
            var commentOfBook = await _db.Comments.Where(u => u.BookId == id).ToListAsync();
            var boughtChapters = await _db.Purchaseds.Where(u => u.Chapter.BookId == id && u.ApplicationUserId == user.Id).Select(u => u.Chapter).ToListAsync();
            var sellingChapters = await _db.Chapters.Where(u => u.BookId == id && u.Price > 0).ToListAsync();
            boughtChapters.ForEach(delegate (Chapter chapter)
            {
                sellingChapters.Remove(chapter);
            });
            if (user.Balance >= sellingChapters.Sum(u => u.Price))
            {


                foreach (var chapter in sellingChapters)
                {
                    Purchased purchased = new Purchased()
                    {
                        ChapterId = chapter.Id,
                        ApplicationUserId = user.Id,
                        PurchaseDate = DateTime.Now
                    };
                    _db.Purchaseds.Add(purchased);
                }
                user.Balance -= sellingChapters.Sum(u => u.Price);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", "Books", new { area = "Customer" });
            }
            else
            {
                BooksVM.Error = "Số dư không đủ vui lòng nạp thêm";
                BooksVM.Book = book;
                BooksVM.Chapters = chapterOfBook;
                BooksVM.Reviews = reviewOfBook;
                BooksVM.Comments = commentOfBook;
            }
            return View(nameof(Details), BooksVM);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
