﻿// <auto-generated />
using System;
using B2DriverLicense.Core.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace B2DriverLicense.Core.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20200824232634_ChapterEdited")]
    partial class ChapterEdited
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("B2DriverLicense.Core.Entities.Answer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Key")
                        .HasColumnType("int");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("B2DriverLicense.Core.Entities.Chapter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Chapters");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Title = "Khái niệm và quy tắc giao thông đường bộ"
                        },
                        new
                        {
                            Id = 2,
                            Title = "Nghiệp vụ vận tải"
                        },
                        new
                        {
                            Id = 3,
                            Title = "Văn hóa, đạo đức người lái xe"
                        },
                        new
                        {
                            Id = 4,
                            Title = "Kỹ thuật lái xe"
                        },
                        new
                        {
                            Id = 5,
                            Title = "Cấu tạo và sửa chữa xe"
                        },
                        new
                        {
                            Id = 6,
                            Title = "Biển báo hiệu đường bộ"
                        },
                        new
                        {
                            Id = 7,
                            Title = "Giải các thế sa hình và kỹ năng xử lý tình huống giao thông"
                        },
                        new
                        {
                            Id = 8,
                            Title = "Câu hỏi điểm liệt"
                        });
                });

            modelBuilder.Entity("B2DriverLicense.Core.Entities.Hint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId")
                        .IsUnique();

                    b.ToTable("Hints");
                });

            modelBuilder.Entity("B2DriverLicense.Core.Entities.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ChapterId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CorrectAnswer")
                        .HasColumnType("int");

                    b.Property<byte[]>("ImageData")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("ImageTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ChapterId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("B2DriverLicense.Core.Entities.Answer", b =>
                {
                    b.HasOne("B2DriverLicense.Core.Entities.Question", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("B2DriverLicense.Core.Entities.Hint", b =>
                {
                    b.HasOne("B2DriverLicense.Core.Entities.Question", "Question")
                        .WithOne("Hint")
                        .HasForeignKey("B2DriverLicense.Core.Entities.Hint", "QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("B2DriverLicense.Core.Entities.Question", b =>
                {
                    b.HasOne("B2DriverLicense.Core.Entities.Chapter", "Chapter")
                        .WithMany("Question")
                        .HasForeignKey("ChapterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}