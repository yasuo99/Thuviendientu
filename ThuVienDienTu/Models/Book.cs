using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ThuVienDienTu.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Display(Name ="Tên sách")]
        public string BookName { get; set; }
        [Display(Name ="Ảnh bìa")]
        public string BookImage { get; set; }
        [Display(Name ="Mô tả sách")]
        public string Description { get; set; }
        [Display(Name ="Giá")]
        public int BookPrice { get; set; }
        public int AuthorId { get; set; }
        [ForeignKey("AuthorId"),Display(Name = "Tác giả")]
        public virtual Author Author { get; set; }
        public int PublisherId { get; set; }
        [ForeignKey("PublisherId"), Display(Name = "NXB")] 
        public virtual Publisher Publisher { get; set; }
        [Display(Name = "Lượt truy cập")]
        public int Accesscount { get; set; }
        public bool Approved { get; set; }
        [ForeignKey("Lượt thích")]
        public int Like { get; set; }
        [ForeignKey("Lượt dislike")]
        public int Dislike { get; set; }
        public virtual ICollection<BookInList> BookInLists { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<BookGenres> BookGenres { get; set; }
        public virtual ICollection<Chapter> Chapters { get; set; }
        public virtual ICollection<BookTag> BookTags { get; set; }
    }
}
