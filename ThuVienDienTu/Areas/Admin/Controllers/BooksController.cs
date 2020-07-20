using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
    [AllowAnonymous]
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private int PageSize = 10;
        private IWebHostEnvironment _hostEnvironment;
        private ISingleton _iSingleton;
        [BindProperty]
        public BooksViewModel BooksVM { get; set; }
        public BooksController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            BooksVM = new BooksViewModel()
            {
                Books = new List<Book>(),
                Book = new Book(),
                GenresViewModels = new List<GenresViewModel>()
            };
            _iSingleton = Singleton.GetInstance;
        }

        // GET: Admin/Books
        public async Task<IActionResult> Index(int productPage = 1,string q = null)
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Signed");
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "PublisherName");
            StringBuilder param = new StringBuilder();
            param.Append("/Admin/Books?productPage=:");
            var books = await _context.Books.Include(b => b.Author).Include(b => b.Publisher).ToListAsync();
            var count = books.Count;
            books = books.Skip((productPage - 1) * PageSize).Take(PageSize).ToList();
            BooksVM.PagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItems = count,
                urlParam = param.ToString()
            };
            BooksVM.Books = books;
            if (q == "Selling")
            {
                BooksVM.Books = books.Where(u => u.Approved == true).ToList();
            }
            if(q == "Waiting")
            {
                BooksVM.Books = books.Where(u => u.Approved == false).ToList();
            }
            return View(BooksVM);
        }
        // GET: Admin/Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            BooksVM.Book = book;
            return View(BooksVM);
        }
        // GET: Admin/Books/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Signed");
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "PublisherName");
            var genres = _context.Genres.ToList();
            var authorsFromDb = _context.Authors.ToList();
            var publishersFromDb = _context.Publishers.ToList();
            if (authorsFromDb.Count == 0)
            {
                return RedirectToAction("Create", "Authors", new { area = "Admin" });
            }
            if (publishersFromDb.Count == 0)
            {
                return RedirectToAction("Create", "Publishers", new { area = "Admin" });
            }
            foreach (var genr in genres)
            {
                GenresViewModel genresView = new GenresViewModel()
                {
                    Genres = genr,
                    Selected = false

                };
                BooksVM.GenresViewModels.Add(genresView);
            }
            return View(BooksVM);
        }

        // POST: Admin/Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var book = BooksVM.Book;
                    if (SameBookExist(book))
                    {
                        ModelState.AddModelError("SameBook", "Sách đã có trong hệ thống");
                        ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Signed", BooksVM.Book.AuthorId);
                        ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "PublisherName", BooksVM.Book.PublisherId);
                        return View(BooksVM);
                    }
                    _context.Add(BooksVM.Book);
                    await _context.SaveChangesAsync();
                    if (BooksVM.BareTag != null)
                    {
                        List<string> tags = HandingBareTag(BooksVM.BareTag);
                        foreach (var tag in tags)
                        {
                            int id;
                            Tag bTag = new Tag()
                            {
                                Tagname = tag
                            };
                            var getTag = await _context.Tag.Where(u => u.Tagname == bTag.Tagname).FirstOrDefaultAsync();
                            if (getTag == null)
                            {
                                _context.Tag.Add(bTag);
                                await _context.SaveChangesAsync();
                                id = bTag.Id;
                            }
                            else
                            {
                                id = getTag.Id;
                            }
                            BookTag bookTag = new BookTag()
                            {
                                BookId = BooksVM.Book.Id,
                                TagId = id
                            };
                            _context.BookTags.Add(bookTag);
                            await _context.SaveChangesAsync();
                        }
                    }
                    var bookFromDb = _context.Books.Find(BooksVM.Book.Id);
                    var webRootPath = _hostEnvironment.WebRootPath;
                    var files = HttpContext.Request.Form.Files;
                    if (files.Count != 0)
                    {
                        var uploads = Path.Combine(webRootPath, SD.BookImageFolder);
                        var extension = Path.GetExtension(files[0].FileName);
                        using (var fileStream = new FileStream(Path.Combine(uploads, BooksVM.Book.Id + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                        bookFromDb.BookImage = @"\" + SD.BookImageFolder + @"\" + BooksVM.Book.Id + extension;
                    }
                    else
                    {
                        var uploads = Path.Combine(webRootPath, SD.BookDefaultImage);
                        System.IO.File.Copy(uploads, webRootPath + @"\" + SD.BookImageFolder + @"\" + BooksVM.Book.Id + ".png");
                        bookFromDb.BookImage = @"\" + SD.BookImageFolder + @"\" + BooksVM.Book.Id + ".png";
                    }
                    if (BooksVM.GenresViewModels != null)
                    {
                        foreach (var genresView in BooksVM.GenresViewModels)
                        {
                            if (genresView.Selected)
                            {
                                BookGenres bookGenres = new BookGenres()
                                {
                                    BookId = bookFromDb.Id,
                                    GenresId = genresView.Genres.Id
                                };
                                _context.BookGenres.Add(bookGenres);
                            }
                        }
                    }
                    bookFromDb.Approved = false;
                    await _context.SaveChangesAsync();
                    _iSingleton.LogException("Thêm sách Id: " + bookFromDb.Id + " " + bookFromDb.BookName);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Id", BooksVM.Book.AuthorId);
                ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Id", BooksVM.Book.PublisherId);
                return View(BooksVM.Book);
            }
            catch (Exception e)
            {
                _iSingleton.LogException(e.Message);
                return RedirectToAction("Error", "Log");
            }    
        }
        // GET: Admin/Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            var currentGenres = await (from a in _context.Genres
                                       join b in _context.BookGenres
                                       on a.Id equals b.GenresId
                                       where b.BookId == id
                                       select a).ToListAsync();
            var genres = await _context.Genres.ToListAsync();
            foreach (var genr in genres)
            {
                GenresViewModel genresViewModel = new GenresViewModel()
                {
                    Genres = genr
                };
                if (currentGenres.Contains(genr))
                {
                    genresViewModel.Selected = true;
                }
                else
                {
                    genresViewModel.Selected = false;
                }
                BooksVM.GenresViewModels.Add(genresViewModel);
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Id", book.AuthorId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Id", book.PublisherId);
            BooksVM.Book = book;
            return View(BooksVM);
        }

        // POST: Admin/Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                if (id != BooksVM.Book.Id)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        var webRootPath = _hostEnvironment.WebRootPath;
                        var files = HttpContext.Request.Form.Files;
                        if (files.Count > 0)
                        {
                            if (BooksVM.Book.BookImage != null)
                            {
                                var removeUpload = webRootPath + @"" + BooksVM.Book.BookImage;
                                System.IO.File.Delete(removeUpload);
                            }
                            var uploads = Path.Combine(webRootPath, SD.BookImageFolder);
                            var extension = Path.GetExtension(files[0].FileName);
                            using (var fileStream = new FileStream(Path.Combine(uploads, BooksVM.Book.Id + extension), FileMode.Create))
                            {
                                files[0].CopyTo(fileStream);
                            }
                            BooksVM.Book.BookImage = @"\" + SD.BookImageFolder + @"\" + BooksVM.Book.Id + extension;
                        }
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BookExists(BooksVM.Book.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    _context.Update(BooksVM.Book);
                    _iSingleton.LogException("Chỉnh sửa sách Id: " + BooksVM.Book.Id);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Signed", BooksVM.Book.AuthorId);
                ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "PublisherName", BooksVM.Book.PublisherId);
                return View(BooksVM.Book);
            }
            catch (Exception e)
            {
                _iSingleton.LogException(e.Message);
                return RedirectToAction("Error", "Log");
            }
            

            
        }
        //[Authorize(Roles = SD.LIBRARIAN_ROLE)]
        // GET: Admin/Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }
     //   [Authorize(Roles = SD.LIBRARIAN_ROLE + "," + SD.ADMIN_ROLE)]
        // POST: Admin/Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int bookid)
        {
            try
            {
                var book = await _context.Books.FindAsync(bookid);
                if (book.BookImage != null)
                {
                    var webRootPath = _hostEnvironment.WebRootPath;
                    var removeUploaded = webRootPath + @"" + book.BookImage;
                    System.IO.File.Delete(removeUploaded);
                }
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                _iSingleton.LogException("Xóa sách Id: " + bookid + " " + book.BookName);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _iSingleton.LogException(e.Message);
                return RedirectToAction("Error", "Log");
            }
            
        }

        private bool SameBookExist(Book book)
        {
            var bookFromDb = _context.Books.Where(e => e.BookName == book.BookName && e.AuthorId == book.AuthorId && e.PublisherId == book.PublisherId).FirstOrDefault();
            if (bookFromDb != null)
            {
                return true;
            }
            return false;
        }
        private bool BookExists(int id)
        {
            return _context.Books.Any(u => u.Id == id);
        }
        public List<string> HandingBareTag(string bareTag)
        {
            List<string> tag = new List<string>();
            int start = 0;
            int end = 0;
            while (start < bareTag.Length)
            {
                string temp = "";
                for (int j = start; j < bareTag.Length; j++)
                {
                    if (bareTag[j] == ',')
                    {
                        end = j;
                        break;
                    }
                    if (j == bareTag.Length - 1)
                    {
                        end = j + 1;
                        break;
                    }
                }
                for (int k = start; k < end; k++)
                {
                    temp += bareTag[k];
                }
                start = end + 1;
                tag.Add(temp.Trim());
            }
            return tag;
        }
    }
}
