using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThuVienDienTu.Data;
using ThuVienDienTu.Models;
using ThuVienDienTu.Utility;

namespace ThuVienDienTu.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ReviewAndComment : Controller
    {
        private readonly ApplicationDbContext _db;
        public ReviewAndComment(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }
        public IActionResult Index(int id)
        {
            var review = _db.Reviews.Where(u => u.BookId == id).Include(u => u.Book).FirstOrDefault();
            return View(review);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SD.READER_ROLE + "," + SD.CENSOR_ROLE)]
        public async Task<IActionResult> AddComment(int bookId, string comment)
        {
            var user = await _db.ApplicationUsers.Where(u => u.Email == User.Identity.Name).FirstOrDefaultAsync();
            Comment cmt = new Comment()
            {
                ApplicationUserId = user.Id,
                BookId = bookId,
                Date = DateTime.Now,
                UserComment = comment
            };
            _db.Comments.Add(cmt);
            await _db.SaveChangesAsync();
            return RedirectToAction("Details", "Home", new { area = "Customer", id = bookId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SD.READER_ROLE)]
        public async Task<IActionResult> AddReview(int bookId, [Bind("Id,BookId,ApplicationUserId,Star,UserReview")] Review review)
        {
            var user = await _db.ApplicationUsers.Where(u => u.Email == User.Identity.Name).FirstOrDefaultAsync();
            var userReview = await _db.Reviews.Where(u => u.ApplicationUserId == user.Id && u.BookId == bookId).FirstOrDefaultAsync();
            if(userReview != null)
            {
                userReview.Star = review.Star;
                userReview.UserReview = review.UserReview;
                _db.Update(userReview);
            }
            else
            {
                review.ApplicationUserId = user.Id;
                review.BookId = bookId;
                _db.Add(review);
            }
            
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Books", new { area = "Customer" });
        }
    }
}
