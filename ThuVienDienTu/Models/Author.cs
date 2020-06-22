using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThuVienDienTu.Models
{
    public class Author
    {
        public int Id { get; set; }
        [Display(Name ="Hình đại diện")]
        public string Avatar { get; set; }
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; }
        [Display(Name = "Ký danh")]
        public string Signed { get; set; }
        [Display(Name = "Quốc gia")]
        public int CountryId { get; set; }
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}