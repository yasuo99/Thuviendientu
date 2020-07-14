using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThuVienDienTu.Models
{
    public class Chapter
    {
        public int Id { get; set; }
        [Display(Name = "Mã sách/ mã tác phẩm")]
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }
        [Display(Name = "Tên chap")]
        public string Chaptername { get; set; }
        [Display(Name = "Nội dung")]
        public string ChapterContent { get; set; }
        [Display(Name = "Giá")]
        public int Price { get; set; }
        [Display(Name = "Ngày chỉnh sửa")]
        public DateTime EditDate { get; set; }
        [Display(Name = "Người chỉnh sửa")]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        [Display(Name = "Trạng thái")]
        public bool Approved { get; set; }
        public virtual ICollection<Purchased> Purchases { get; set; }
        public virtual ICollection<ReadingHistory> ReadingHistories { get; set; }
    }
}