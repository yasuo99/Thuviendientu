using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ThuVienDienTu.Models
{
    public class BookInList
    {
        public int ReadingListId { get; set; }
        [ForeignKey("ReadingListId")]
        public virtual ReadingList ReadingList { get; set; }
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }
    }
}
