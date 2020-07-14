using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ThuVienDienTu.Areas.Admin.Controllers;

namespace ThuVienDienTu.Models
{
    public class ReadingHistory
    {
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }
        public int ChapterId { get; set; }
        [ForeignKey("ChapterId")]
        public Chapter Chapter { get; set; }
    }
}
