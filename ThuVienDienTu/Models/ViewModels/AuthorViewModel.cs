using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThuVienDienTu.Models.ViewModels
{
    public class AuthorViewModel
    {
        public Author Author { get; set; }
        public List<Author> Authors { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
