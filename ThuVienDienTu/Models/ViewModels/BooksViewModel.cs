using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThuVienDienTu.Areas.Admin.Controllers;
using ThuVienDienTu.Helper;

namespace ThuVienDienTu.Models.ViewModels
{
    public class BooksViewModel
    {
        public Book Book { get; set; }
        public List<Book> Books { get; set; }
        public List<Book> BooksSeen { get; set; }
        public string BareTag { get; set; }
        public Author Author { get; set; }
        public List<GenresViewModel> GenresViewModels { get; set; }
        public List<Genres> Genres { get; set; }
        public Genres TopGenres { get; set; }
        public List<Author> Authors { get; set; }
        public List<Country> Countries { get; set; }
        public List<Publisher> Publishers { get; set; }
        public Chapter Chapter { get; set; }
        public List<Chapter> Chapters { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Review> Reviews { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public List<ReadingListViewModel> ReadingListsVM { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string Error { get; set; }
        public string BuyRequest { get; set; }
        public int AlreadyBought { get; set; }
        public int Rating { get; set; }
    }
}
