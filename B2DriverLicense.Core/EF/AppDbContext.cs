using B2DriverLicense.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace B2DriverLicense.Core.EF
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chapter>().HasData(
                new Chapter { Id = 1, Title = "Khái niệm và quy tắc giao thông đường bộ" },
                new Chapter { Id = 2, Title = "Nghiệp vụ vận tải" },
                new Chapter { Id = 3, Title = "Văn hóa, đạo đức người lái xe" },
                new Chapter { Id = 4, Title = "Kỹ thuật lái xe" },
                new Chapter { Id = 5, Title = "Cấu tạo và sửa chữa xe" },
                new Chapter { Id = 6, Title = "Biển báo hiệu đường bộ" },
                new Chapter { Id = 7, Title = "Giải các thế sa hình và kỹ năng xử lý tình huống giao thông" },
                new Chapter { Id = 8, Title = "Câu hỏi điểm liệt" }
                );
        }

        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Hint> Hints { get; set; }
    }
}
