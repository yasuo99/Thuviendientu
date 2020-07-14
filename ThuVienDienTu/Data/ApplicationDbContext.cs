using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ThuVienDienTu.Models;

namespace ThuVienDienTu.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Genres> Genres { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Purchased> Purchaseds { get; set; }
        public DbSet<Report> Report { get; set; }
        public DbSet<ReadingList> ReadingLists { get; set; }
        public DbSet<BookInList> BookInLists { get; set; }     
        public DbSet<BookGenres> BookGenres { get; set; }
        public DbSet<TopupHistory> TopupHistories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<BookTag> BookTags { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<ReportAccount> ReportAccounts { get; set; }
        public DbSet<ReadingHistory> ReadingHitories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BookInList>().HasKey(key => new { key.ReadingListId, key.BookId });
            modelBuilder.Entity<BookGenres>().HasKey(sc => new { sc.BookId, sc.GenresId });
            modelBuilder.Entity<BookTag>().HasKey(bt => new { bt.BookId, bt.TagId });
            modelBuilder.Entity<ReportAccount>().HasOne(ra => ra.Report)
                .WithMany(ra => ra.ReportAccounts)
                .HasForeignKey(ra => ra.ReportId)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<ReportAccount>().HasOne(ra => ra.Reported)
                .WithMany(ra => ra.ReportedAccounts)
                .HasForeignKey(ra => ra.ReportedId)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<Book>().HasOne(b => b.Publisher)
                .WithMany(b => b.Books)
                .HasForeignKey(b => b.PublisherId)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<Book>().HasOne(b => b.Author)
                .WithMany(b => b.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<ReadingHistory>().HasOne(key => key.Chapter).WithMany(m => m.ReadingHistories).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ReadingHistory>().HasKey(key => new { key.ApplicationUserId, key.BookId });

        }
        public DbSet<ThuVienDienTu.Models.Tag> Tag { get; set; }
    } 
}
