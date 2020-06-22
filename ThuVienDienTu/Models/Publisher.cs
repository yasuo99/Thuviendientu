using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThuVienDienTu.Models
{
    public class Publisher
    {
        public int Id { get; set; }
        [Display(Name = "Tên NXB")]
        public string PublisherName { get; set; }
        [Display(Name = "Logo NXB")]
        public string PublisherLogo { get; set; }
        [Display(Name ="Mô tả về NXB")]
        public string Description { get; set; }
        public virtual List<Book> Books { get; set; }
        [Display(Name = "Quốc gia")]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
    }
}
