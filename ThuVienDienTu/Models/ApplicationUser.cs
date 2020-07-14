using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ThuVienDienTu.Models
{
    public class ApplicationUser : IdentityUser
    {       
        [Display(Name = "Họ và tên")]
        public string Fullname { get; set; }
        [Display(Name ="Ảnh đại diện")]
        public string UserAvatar { get; set; }
        [Display(Name = "SĐT")]
        public string Phone { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Ngày tham gia")]
        public DateTime Date { get; set; }
        [Display(Name ="Số dư tài khoản")]
        public int Balance { get; set; }
        public int ReportCount { get; set; }
        public string DisplayName{ get; set; }
        [NotMapped]
        public bool IsSuperAdmin { get; set; }
        [NotMapped]
        public bool IsAuthor { get; set; }
        [NotMapped]
        public bool IsVerify { get; set;}
        public virtual ICollection<TopupHistory> TopupHistories { get; set; }
        public virtual ICollection<Purchased> Purchases { get; set; }
        public virtual ICollection<ReadingList> Lists { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<Chapter> Chapters { get; set; }
        public virtual ICollection<ReportAccount> ReportAccounts { get; set; }
        public virtual ICollection<ReportAccount> ReportedAccounts { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<ReadingHistory> ReadingHistories { get; set; }
    }
}
