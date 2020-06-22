using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThuVienDienTu.Models
{
    public class Genres
    {
        public int Id { get; set; }
        [Display(Name = "Thể loại")]
        public string GenresName { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        public virtual ICollection<BookGenres> BookGenres { get; set; }
    }
}
