using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThuVienDienTu.Data;
using ThuVienDienTu.Models;
using ThuVienDienTu.Models.ViewModels;

namespace ThuVienDienTu.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ReadingListsController : Controller
    {
        private readonly ApplicationDbContext _context;
        [BindProperty]
        public ReadingListViewModel ReadingListsVM { get; set; } 

        public ReadingListsController(ApplicationDbContext context)
        {
            _context = context;
            ReadingListsVM = new ReadingListViewModel()
            {
                Books = new List<Book>(),
                ReadingList = new ReadingList(),
                Error = null
            };
        }

        // GET: Customer/ReadingLists
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ReadingLists.Include(r => r.ApplicationUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Customer/ReadingLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var readingList = await _context.ReadingLists
                .Include(r => r.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (readingList == null)
            {
                return NotFound();
            }
            return View(readingList);
        }

        // GET: Customer/ReadingLists/Create
        public IActionResult Create()
        {
            return View(ReadingListsVM);
        }

        // POST: Customer/ReadingLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claimsUser = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var user = _context.ApplicationUsers.Where(u => u.Id == claimsUser.Value).FirstOrDefault();
                ReadingListsVM.ReadingList.ApplicationUserId = user.Id;
                if (AlreadyHas(ReadingListsVM.ReadingList.ReadingListName) == false)
                {
                    _context.Add(ReadingListsVM.ReadingList);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ReadingListsVM.Error = "Danh sách đọc đã tồn tại";
                    return View(ReadingListsVM);
                }
            }
            return View(ReadingListsVM);
        }

        // GET: Customer/ReadingLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var readingList = await _context.ReadingLists.FindAsync(id);
            readingList.BookInLists = await _context.BookInLists.Where(u => u.ReadingListId == readingList.Id).Include(u => u.Book).Include(u => u.ReadingList).ToListAsync();
            if (readingList == null)
            {
                return NotFound();
            }
            return View(readingList);
        }

        // POST: Customer/ReadingLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ApplicationUserId,ReadingListName")] ReadingList readingList)
        {
            if (id != readingList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _context.ApplicationUsers.Where(u => u.Email == User.Identity.Name).FirstOrDefaultAsync();
                    readingList.ApplicationUserId = user.Id;
                    _context.Update(readingList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReadingListExists(readingList.Id))
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
            return View(readingList);
        }

        // GET: Customer/ReadingLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var readingList = await _context.ReadingLists
                .Include(r => r.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            var bookInList = await _context.BookInLists.Where(u => u.ReadingListId == id).Include(u => u.Book).Include(u => u.ReadingList).ToListAsync();
            if (readingList == null)
            {
                return NotFound();
            }
            readingList.BookInLists = bookInList;
            return View(readingList);
        }

        // POST: Customer/ReadingLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var readingList = await _context.ReadingLists.FindAsync(id);
            _context.ReadingLists.Remove(readingList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReadingListExists(int id)
        {
            return _context.ReadingLists.Any(e => e.Id == id);
        }
        //Add book to list
        public async Task<IActionResult> AddToList(int bookId, int readingListId)
        {
            BookInList bookInList = new BookInList()
            {
                BookId = bookId,
                ReadingListId = readingListId
            };
            _context.BookInLists.Add(bookInList);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details","Home",new {area = "Customer", id = bookId });
        }
        public async Task<IActionResult> RemoveFromList(int bookId,int readingListId)
        {
            var bookInList = await _context.BookInLists.Where(u => u.BookId == bookId && u.ReadingListId == readingListId).FirstOrDefaultAsync();
            _context.Remove(bookInList);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Home", new { area = "Customer", id = bookId });
        }
        public bool AlreadyHas(string readingListName)
        {
            var readingListInDb = _context.ReadingLists.Where(u => u.ReadingListName.ToLower().Trim() == readingListName.ToLower().Trim()).FirstOrDefault();
            if(readingListInDb == null)
            {
                return false;
            }
            return true;
        }
    }
}
