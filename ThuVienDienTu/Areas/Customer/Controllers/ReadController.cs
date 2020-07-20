using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThuVienDienTu.Data;
using ThuVienDienTu.Helper;
using ThuVienDienTu.Models;
using ThuVienDienTu.Models.ViewModels;
using ThuVienDienTu.Utility;

namespace ThuVienDienTu.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ReadController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly SignInManager<IdentityUser> _signinManager;
        [BindProperty]
        public BooksViewModel BooksVM { get; set; }
        public ReadController(ApplicationDbContext db, SignInManager<IdentityUser> signinManager)
        {
            _db = db;
            BooksVM = new BooksViewModel()
            {
                Chapters = new List<Chapter>(),
                Book = new Book(),
                Genres = new List<Genres>(),
                Chapter = new Chapter(),
                BuyRequest = null
            };
            _signinManager = signinManager;
        }
        public async Task<IActionResult> Index(int id)
        {
            var genres = (from a in _db.Books
                          join b in _db.BookGenres
                          on a.Id equals b.BookId
                          join c in _db.Genres
                          on b.GenresId equals c.Id
                          where a.Id == id
                          select c).ToList();
            var book = await _db.Books.Where(u => u.Id == id).FirstOrDefaultAsync();
            if (_signinManager.IsSignedIn(User))
            {
                var user = await _db.ApplicationUsers.Where(u => u.Email == User.Identity.Name).FirstOrDefaultAsync();
                var freeChapters = await _db.Chapters.Where(u => u.BookId == id && u.Price == 0).ToListAsync();
                var alreadyBought = await _db.Purchaseds.Where(u => u.Chapter.BookId == id && u.ApplicationUserId == user.Id).Select(u => u.Chapter).ToListAsync();
                freeChapters.AddRange(alreadyBought);
                var readingHistory = await _db.ReadingHitories.Where(u => u.ApplicationUserId == user.Id && u.Chapter.BookId == id).FirstOrDefaultAsync();
                if (User.IsInRole(SD.CENSOR_ROLE) || User.IsInRole(SD.ADMIN_ROLE) || User.IsInRole(SD.LIBRARIAN_ROLE))
                {
                    var chaptersOfBook = await _db.Chapters.Where(u => u.BookId == id).Include(u => u.Book).ToListAsync();
                    if (chaptersOfBook.Count > 0)
                    {
                        if (readingHistory != null)
                        {
                            BooksVM.Chapter = await _db.Chapters.Where(u => u.Id == readingHistory.ChapterId).FirstOrDefaultAsync();
                        }
                        else
                        {

                            ReadingHistory history = new ReadingHistory()
                            {
                                ApplicationUserId = user.Id,
                                BookId = id,
                                ChapterId = chaptersOfBook[0].Id
                            };
                            _db.ReadingHitories.Add(history);
                            await _db.SaveChangesAsync();
                            BooksVM.Chapter = chaptersOfBook[0];
                        }
                    }

                    BooksVM.Chapters = chaptersOfBook;
                    if (!User.IsInRole(SD.CENSOR_ROLE) && !User.IsInRole(SD.ADMIN_ROLE))
                    {
                        BooksVM.Chapters = chaptersOfBook.Where(u => u.Approved == true).ToList();
                    }
                    BooksVM.Genres = genres;
                }
                if (User.IsInRole(SD.READER_ROLE))
                {
                    if (readingHistory != null)
                    {
                        BooksVM.Chapter = readingHistory.Chapter;
                        BooksVM.Chapters = freeChapters;
                    }
                    else
                    {
                        if (freeChapters.Count > 0)
                        {

                            ReadingHistory history = new ReadingHistory()
                            {
                                ApplicationUserId = user.Id,
                                BookId = id,
                                ChapterId = freeChapters[0].Id
                            };
                            _db.Add(history);
                            await _db.SaveChangesAsync();
                            BooksVM.Chapters = freeChapters;
                        }
                        else
                        {
                            BooksVM.BuyRequest = "Sách này tính phí vui lòng mua";
                            BooksVM.Book = book;
                        }
                    }
                }
            }
            else
            {

                var chaptersOfBook = await _db.Chapters.Where(u => u.BookId == id && u.Price == 0 && u.Approved == true).Include(u => u.Book).ToListAsync();
                if (chaptersOfBook.Count > 0)
                {

                    var index = HttpContext.Session.GetInt32(book.Id.ToString() + "Index");
                    if (index.HasValue)
                    {
                        BooksVM.Chapter = chaptersOfBook[index.Value];
                    }
                    else
                    {
                        HttpContext.Session.SetInt32(book.Id.ToString() + "Index", 0);
                        BooksVM.Chapter = chaptersOfBook[0];
                    }
                    BooksVM.Chapters = chaptersOfBook;
                }
                else
                {
                    BooksVM.BuyRequest = "Sách này tính phí vui lòng mua";
                    BooksVM.Book = book;
                }
            }
            return View(BooksVM);
        }
        public async Task<IActionResult> TrialRead(int id)
        {
            var chapters = await _db.Chapters.Where(u => u.BookId == id && u.Price == 0).ToListAsync();
            var book = await _db.Books.Where(u => u.Id == id).FirstOrDefaultAsync();
            var user = await _db.ApplicationUsers.Where(u => u.Email == User.Identity.Name).FirstOrDefaultAsync();
            if (user != null)
            {
                var readingHistory = await _db.ReadingHitories.Where(u => u.ApplicationUserId == user.Id && u.BookId == id).Include(u => u.Chapter).FirstOrDefaultAsync();
                if (readingHistory != null)
                {
                    BooksVM.Chapter = readingHistory.Chapter;
                }
                else
                {
                    ReadingHistory history = new ReadingHistory()
                    {
                        ApplicationUserId = user.Id,
                        BookId = id,
                        ChapterId = chapters[0].Id
                    };
                    _db.Add(history);
                    await _db.SaveChangesAsync();
                }
            }
            else
            {
                var index = HttpContext.Session.GetInt32(book.Id.ToString() + "Index");
                if (index.HasValue)
                {
                    BooksVM.Chapter = chapters[index.Value];
                }
                else
                {
                    HttpContext.Session.SetInt32(book.Id.ToString() + "Index", 0);
                    BooksVM.Chapter = chapters[0];
                }
            }
            BooksVM.Chapters = chapters;
            BooksVM.Book = book;
            return View(nameof(Index), BooksVM);
        }
        [HttpGet]
        public async Task<IActionResult> ReadingChapter(int id)
        { 
            var chapter = await _db.Chapters.Where(u => u.Id == id).Include(u => u.Book).FirstOrDefaultAsync();
            var book = await _db.Books.Where(u => u.Id == chapter.BookId).FirstOrDefaultAsync();
            if (_signinManager.IsSignedIn(User))
            {
                var user = await _db.ApplicationUsers.Where(u => u.Email == User.Identity.Name).FirstOrDefaultAsync();
                var readingHistory = await _db.ReadingHitories.Where(u => u.Chapter.BookId == chapter.BookId).FirstOrDefaultAsync();

                if (User.IsInRole(SD.CENSOR_ROLE) || User.IsInRole(SD.ADMIN_ROLE) || User.IsInRole(SD.LIBRARIAN_ROLE))
                {
                    if (readingHistory != null)
                    {
                        readingHistory.ChapterId = id;
                    }
                    else
                    {
                        ReadingHistory history = new ReadingHistory()
                        {
                            ApplicationUserId = user.Id,
                            BookId = book.Id,
                            ChapterId = chapter.Id
                        };
                        _db.Add(history);
                    }
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index", new { id = book.Id });
                }
                else
                {

                    if (chapter.Price == 0)
                    {
                        if (readingHistory != null)
                        {
                            readingHistory.ChapterId = id;
                        }
                        else
                        {
                            ReadingHistory history = new ReadingHistory()
                            {
                                ApplicationUserId = user.Id,
                                BookId = book.Id,
                                ChapterId = id
                            };
                            _db.Add(history);     
                        }
                        await _db.SaveChangesAsync();
                        return RedirectToAction("Index", new { id = book.Id });
                    }
                    else
                    {
                        var alreadyBought = await _db.Purchaseds.Where(u => u.ApplicationUserId == user.Id && u.ChapterId == id).FirstOrDefaultAsync();
                        if (alreadyBought != null)
                        {
                            if (readingHistory != null)
                            {
                                readingHistory.ChapterId = id;
                            }
                            else
                            {
                                ReadingHistory history = new ReadingHistory()
                                {
                                    ApplicationUserId = user.Id,
                                    BookId = book.Id,
                                    ChapterId = id
                                };
                                _db.Add(history);
                                
                            }
                            await _db.SaveChangesAsync();
                            return RedirectToAction("Index", new { id = book.Id });
                        }
                        else
                        {
                            var freeChapters = await _db.Chapters.Where(u => u.BookId == book.Id && u.Price == 0 && u.Approved == true).ToListAsync();
                            var boughtChapters = await _db.Purchaseds.Where(u => u.Chapter.BookId == book.Id && u.ApplicationUserId == user.Id).Select(u => u.Chapter).ToListAsync();
                            freeChapters.AddRange(boughtChapters);
                            BooksVM.Chapter = chapter;
                            BooksVM.BuyRequest = "Để đọc toàn bộ chương vui lòng mua";
                            BooksVM.Chapters = freeChapters;
                            return View(nameof(Index), BooksVM);
                        }
                    }
                }
            }
            else
            {
                if (chapter.Price == 0)
                {
                    var freeChapters = await _db.Chapters.Where(u => u.BookId == book.Id && u.Price == 0 && u.Approved == true).ToListAsync();
                    var index = freeChapters.IndexOf(chapter);
                    HttpContext.Session.SetInt32(book.Id.ToString() + "Index", index);
                    return RedirectToAction("Index", new { id = book.Id });
                }
                else
                {
                    BooksVM.BuyRequest = "Để đọc toàn bộ chương vui lòng mua";
                    BooksVM.Book = book;
                    BooksVM.Chapter = chapter;
                    return View(nameof(Index), BooksVM);
                }
            }
        }
        public async Task<IActionResult> DecreaseIndex(int id)
        {
            var user = await _db.ApplicationUsers.Where(u => u.Email == User.Identity.Name).FirstOrDefaultAsync();

            if (user != null)
            {
                var chaptersOfBook = await _db.Chapters.Where(u => u.BookId == id && u.Approved == true).ToListAsync();
                if (User.IsInRole(SD.READER_ROLE))
                {
                    var freeChapter = chaptersOfBook.Where(u => u.Price == 0).ToList();
                    var boughtChapter = await _db.Purchaseds.Where(u => u.ApplicationUserId == user.Id && u.Chapter.BookId == id).Select(u => u.Chapter).ToListAsync();
                    freeChapter.AddRange(boughtChapter);
                    var readingHistory = await _db.ReadingHitories.Where(u => u.ApplicationUserId == user.Id && u.BookId == id).FirstOrDefaultAsync();
                    if (readingHistory != null)
                    {
                        var index = freeChapter.IndexOf(readingHistory.Chapter);
                        if (index > 0)
                        {
                            readingHistory.ChapterId = freeChapter[index - 1].Id;
                            await _db.SaveChangesAsync();
                        }
                    }
                }
                else
                {
                    chaptersOfBook = await _db.Chapters.Where(u => u.BookId == id).ToListAsync();
                    var readingHitory = await _db.ReadingHitories.Where(u => u.BookId == id && u.ApplicationUserId == user.Id).FirstOrDefaultAsync();

                    var index = chaptersOfBook.IndexOf(readingHitory.Chapter);
                    if (index > 0)
                    {
                        readingHitory.ChapterId = chaptersOfBook[index - 1].Id;
                    }
                }

            }
            else
            {
                var index = HttpContext.Session.GetInt32(id.ToString() + "Index");
                if (index.HasValue && index.Value > 0)
                {
                    index -= 1;
                    HttpContext.Session.SetInt32(id.ToString() + "Index", index.Value);
                }
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", new { id = id });
        }
        public async Task<IActionResult> IncreaseIndex(int id)
        {

            var user = await _db.ApplicationUsers.Where(u => u.Email == User.Identity.Name).FirstOrDefaultAsync();
            if (user != null)
            {
                var chaptersOfBook = await _db.Chapters.Where(u => u.BookId == id).ToListAsync();
                var readingHitory = await _db.ReadingHitories.Where(u => u.BookId == id && u.ApplicationUserId == user.Id).FirstOrDefaultAsync();

                if (User.IsInRole(SD.READER_ROLE))
                {
                    var freeChapter = chaptersOfBook.Where(u => u.Price == 0).ToList();
                    var boughtChapter = await _db.Purchaseds.Where(u => u.ApplicationUserId == user.Id && u.Chapter.BookId == id).Select(u => u.Chapter).ToListAsync();
                    freeChapter.AddRange(boughtChapter);
                    var index = freeChapter.IndexOf(readingHitory.Chapter);
                    if (index < freeChapter.Count - 1)
                    {
                        readingHitory.ChapterId = freeChapter[index + 1].Id;
                        await _db.SaveChangesAsync();
                    }
                }
                else
                {
                    var index = chaptersOfBook.IndexOf(readingHitory.Chapter);

                    if (index < chaptersOfBook.Count - 1)
                    {
                        readingHitory.ChapterId = chaptersOfBook[index + 1].Id;
                    }
                }

            }
            else
            {
                var chaptersOfBook = await _db.Chapters.Where(u => u.BookId == id && u.Approved == true && u.Price == 0).ToListAsync();
                var index = HttpContext.Session.GetInt32(id.ToString() + "Index");
                if (index.HasValue && index.Value < chaptersOfBook.Count - 1)
                {
                    index++;
                    HttpContext.Session.SetInt32(id.ToString() + "Index", index.Value);
                }
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", new { id = id });
        }
        [Authorize(Roles = SD.READER_ROLE)]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> BuyChapter(int chapterId)
        {
            var user = await _db.ApplicationUsers.Where(u => u.Email == User.Identity.Name).FirstOrDefaultAsync();
            var chapter = await _db.Chapters.Where(u => u.Id == chapterId).FirstOrDefaultAsync();
            var readingHistory = await _db.ReadingHitories.Where(u => u.ApplicationUserId == user.Id && u.BookId == chapter.BookId).FirstOrDefaultAsync();

            Purchased purchased = new Purchased()
            {
                ApplicationUserId = user.Id,
                ChapterId = chapterId,
                PurchaseDate = DateTime.Now,
            };
            if (user.Balance >= chapter.Price)
            {
                _db.Purchaseds.Add(purchased);
                user.Balance -= chapter.Price;
                if(readingHistory != null)
                {
                    readingHistory.ChapterId = chapterId;
                }
                else
                {
                    ReadingHistory history = new ReadingHistory()
                    {
                        ApplicationUserId = user.Id,
                        BookId = chapter.BookId,
                        ChapterId = chapter.Id
                    };
                    _db.Add(history);
                }
              
                await _db.SaveChangesAsync();
                return RedirectToAction("ReadingChapter", "Read", new { area = "Customer", id = chapterId });
            }
            else
            {
                return RedirectToAction("Index", "Topup");
            }
        }
    }
}