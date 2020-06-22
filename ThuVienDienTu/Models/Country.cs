using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThuVienDienTu.Models
{
    public class Country
    {
        public int Id { get; set;}
        public string CountryName { get; set; }
        public virtual ICollection<Author> Authors { get; set; }
        public virtual ICollection<Publisher> Publishers { get; set; }
    }
}
