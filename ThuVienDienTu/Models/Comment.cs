using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ThuVienDienTu.Data;

namespace ThuVienDienTu.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Display(Name ="Mã sách")]
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }
        [Display(Name = "Ngày")]
        public DateTime Date { get; set; }
        [Display(Name ="Bình luận")]
        public string UserComment { get; set; }
        [Display(Name = "Mã độc giả")]
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; } 
    }
}
