﻿// <auto-generated />
using System;
using MapraBookPlayer.Domain.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MapraBookPlayer.Domain.Migrations
{
    [DbContext(typeof(BookContext))]
    partial class BookContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

            modelBuilder.Entity("MapraBookPlayer.Domain.AudioBook", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Author")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cover")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("MapraBookPlayer.Domain.Bookmark", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ChapterId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Header")
                        .HasColumnType("TEXT");

                    b.Property<double>("Position")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("ChapterId");

                    b.ToTable("Bookmarks");
                });

            modelBuilder.Entity("MapraBookPlayer.Domain.Chapter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AudioBookId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ChapterName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AudioBookId");

                    b.ToTable("Chapters");
                });

            modelBuilder.Entity("MapraBookPlayer.Domain.Bookmark", b =>
                {
                    b.HasOne("MapraBookPlayer.Domain.Chapter", "Chapter")
                        .WithMany("Bookmarks")
                        .HasForeignKey("ChapterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chapter");
                });

            modelBuilder.Entity("MapraBookPlayer.Domain.Chapter", b =>
                {
                    b.HasOne("MapraBookPlayer.Domain.AudioBook", null)
                        .WithMany("Chapters")
                        .HasForeignKey("AudioBookId");
                });

            modelBuilder.Entity("MapraBookPlayer.Domain.AudioBook", b =>
                {
                    b.Navigation("Chapters");
                });

            modelBuilder.Entity("MapraBookPlayer.Domain.Chapter", b =>
                {
                    b.Navigation("Bookmarks");
                });
#pragma warning restore 612, 618
        }
    }
}