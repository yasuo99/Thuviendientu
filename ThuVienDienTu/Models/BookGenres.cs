using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ThuVienDienTu.Models
{
    public class BookGenres
    {
        [Display(Name = "Mã sách")]
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }
        [Display(Name = "Mã thể loại")]
        public int GenresId { get; set; }
        [ForeignKey("GenresId")]
        public virtual Genres Genres { get; set; }
    }
}
