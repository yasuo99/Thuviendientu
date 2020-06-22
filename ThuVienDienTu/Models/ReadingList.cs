using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThuVienDienTu.Models
{
    public class ReadingList
    {
        public int Id { get; set; }
        [Display(Name = "Mã độc giả")]
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        [Display(Name = "Tên danh sách")]
        public string ReadingListName { get; set; }
        public virtual ICollection<BookInList> BookInLists { get; set; }
    }
}