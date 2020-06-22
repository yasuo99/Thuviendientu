using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThuVienDienTu.Models.ViewModels
{
    public class ReadingListViewModel
    {
        public ReadingList ReadingList { get; set; }    
        public bool AlreadyIn { get; set; }
        public List<Book> Books { get; set; }
        public string Error { get; set; }
    }
}
