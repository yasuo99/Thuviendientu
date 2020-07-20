using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThuVienDienTu.Models.ViewModels
{
    public class ChaptersViewModel
    {
        public Book Book { get; set; }
        public List<Book> Books { get; set; }
        public Chapter Chapter { get; set;}
        public List<Chapter> Chapters { get; set;}
        public ApplicationUser Censor { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
