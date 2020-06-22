using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThuVienDienTu.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Tagname { get; set; }
        public virtual ICollection<BookTag> BookTags { get; set; }
    }
}
