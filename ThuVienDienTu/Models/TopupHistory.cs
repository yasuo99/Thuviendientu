using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ThuVienDienTu.Models
{
    public class TopupHistory
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        public int TopupAmount { get; set; }
        public DateTime TopupDate { get; set; }       
        public virtual Report Report { get; set; }
    }
}
