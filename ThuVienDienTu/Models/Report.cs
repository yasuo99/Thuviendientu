using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ThuVienDienTu.Models
{
    public class Report
    {
        public int Id { get; set; }
        [Display(Name = "Ngày tạo")]
        public DateTime CreateDate { get; set; }
        public int Total { get; set; }
        public string CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public virtual ApplicationUser Creator { get; set; }
        public virtual ICollection<TopupHistory> TopupHistories { get; set; }
    }
}
