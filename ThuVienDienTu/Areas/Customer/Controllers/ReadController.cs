﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        [BindProperty]
        public BooksViewModel BooksVM { get; set; }
        public ReadController(ApplicationDbContext db)
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
        }
        public async Task<IActionResult> Index(int id)
        {
            
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claimsUser = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var user = await _db.ApplicationUsers.Where(u => u.Id == claimsUser.Value).FirstOrDefaultAsync();
            var alreadyBought = await _db.Purchaseds.Where(u => u.Chapter.BookId == id && u.ApplicationUserId == user.Id).FirstOrDefaultAsync();
            var book = await _db.Books.Where(u => u.Id == id).FirstOrDefaultAsync();
            if (alreadyBought != null || book.BookPrice == 0 || User.IsInRole(SD.CENSOR_ROLE) || User.IsInRole(SD.ADMIN_ROLE) || User.IsInRole(SD.LIBRARIAN_ROLE))
            {
                Chapter chapter = new Chapter();
                var genres = (from a in _db.Books
                              join b in _db.BookGenres
                              on a.Id equals b.BookId
                              join c in _db.Genres
                              on b.GenresId equals c.Id
                              where a.Id == id
                              select c).ToList();
                var chaptersOfBook = await _db.Chapters.Where(u => u.BookId == id).Include(u => u.Book).ToListAsync();
                var currentIndex = HttpContext.Session.GetInt32("Index");
                if (chaptersOfBook.Count > 0)
                {
                    if (currentIndex == null)
                    {
                        HttpContext.Session.SetInt32("Index", 0);
                    }
                    else
                    {
                        chapter = chaptersOfBook[currentIndex.Value];
                    }
                }
                HttpContext.Session.SetInt32("TotalChapter", chaptersOfBook.Count);
                BooksVM.Chapters = chaptersOfBook;
                BooksVM.Chapter = chapter;
                BooksVM.Genres = genres;
                
            }
            else
            {
                BooksVM.BuyRequest = "Sách này tính phí vui lòng mua";
                BooksVM.Book = book;
            }
            return View(BooksVM);
        }
        public async Task<IActionResult> TrialRead(int id)
        {
            var chapters = await _db.Chapters.Where(u => u.BookId == id && u.Price == 0).ToListAsync();
            var book = await _db.Books.Where(u => u.Id == id).FirstOrDefaultAsync();
            BooksVM.Chapters = chapters;
            BooksVM.Book = book;
            return View(nameof(Index), BooksVM);
        }
        public async Task<IActionResult> ReadingChapter(int id)
        {
            var chapter = await _db.Chapters.Where(u => u.Id == id).Include(u => u.Book).FirstOrDefaultAsync();
            Purchased alreadyBought = new Purchased();
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            if(claimsIdentity.Claims.Count() > 0)
            {
                var claimsUser = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var user = await _db.ApplicationUsers.Where(u => u.Id == claimsUser.Value).FirstOrDefaultAsync();
                
                alreadyBought = await _db.Purchaseds.Where(u => u.ChapterId == chapter.Id && u.ApplicationUserId == user.Id).FirstOrDefaultAsync();
            }
            if (alreadyBought != null || chapter.Price == 0 || User.IsInRole(SD.ADMIN_ROLE) || User.IsInRole(SD.CENSOR_ROLE) || User.IsInRole(SD.LIBRARIAN_ROLE))
            {
                var chaptersOfBook = await _db.Chapters.Where(u => u.BookId == chapter.Book.Id).Include(u => u.Book).ToListAsync();
                var index = chaptersOfBook.IndexOf(chapter);
                HttpContext.Session.SetInt32("Index", index);
                HttpContext.Session.SetInt32("TotalChapter", chaptersOfBook.Count);
                BooksVM.Chapter = chapter;
                BooksVM.Chapters = chaptersOfBook;
                return View(nameof(Index), BooksVM);
            }
            else
            {
                var book = await _db.Books.Where(u => u.Id == chapter.BookId).FirstOrDefaultAsync();
                BooksVM.BuyRequest = "Chương này tính phí vui lòng mua";
                BooksVM.Book = book;
                BooksVM.Chapter = chapter;
                return View(nameof(Index), BooksVM);
            }
        }
        public IActionResult DecreaseIndex(int id)
        {
            var currentIndex = HttpContext.Session.GetInt32("Index");
            if (currentIndex != null && currentIndex >= 1 && currentIndex.HasValue)
            {
                HttpContext.Session.SetInt32("Index", currentIndex.Value - 1);
            }
            return RedirectToAction("Index", new { id = id });
        }
        public IActionResult IncreaseIndex(int id)
        {
            var currentIndex = HttpContext.Session.GetInt32("Index");
            var totalChapter = HttpContext.Session.GetInt32("TotalChapter");
            if (currentIndex.HasValue && currentIndex.Value < totalChapter.Value - 1)
            {
                HttpContext.Session.SetInt32("Index", currentIndex.Value + 1);
            }
            return RedirectToAction("Index", new { id = id });
        }
        public async Task<IActionResult> BuyChapter(int chapterId)
        {
            var user = await _db.ApplicationUsers.Where(u => u.Email == User.Identity.Name).FirstOrDefaultAsync();
            var chapter = await _db.Chapters.Where(u => u.Id == chapterId).FirstOrDefaultAsync();
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