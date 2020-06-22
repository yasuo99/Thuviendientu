using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ThuVienDienTu.Models
{
    public class ReportAccount
    {
        public int Id { get; set; }
        public string ReportId { get; set; }
        [ForeignKey("ReportId")]
        public virtual ApplicationUser Report { get; set; }
        public string ReportedId { get; set; }
        [ForeignKey("ReportedId")]
        public virtual ApplicationUser Reported { get; set; }        
        public string Reason { get; set; }
        public DateTime ReportedDate { get; set; }
        public bool Confirm { get; set; }
    }
}
