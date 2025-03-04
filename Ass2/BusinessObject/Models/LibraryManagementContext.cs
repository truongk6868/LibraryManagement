using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BusinessObject.Models;

public partial class LibraryManagementContext : DbContext
{
    public LibraryManagementContext()
    {
    }

    public LibraryManagementContext(DbContextOptions<LibraryManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BorrowingRecord> BorrowingRecords { get; set; }

    public virtual DbSet<Reader> Readers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server =(local); database = LibraryManagement;uid=sa;pwd=123;TrustServerCertificate=True;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__Books__3DE0C22733452174");

            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.Author).HasMaxLength(255);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<BorrowingRecord>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__Borrowin__FBDF78C97DF373AF");

            entity.Property(e => e.RecordId).HasColumnName("RecordID");
            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.BorrowDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ReaderId).HasColumnName("ReaderID");
            entity.Property(e => e.ReturnDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Book).WithMany(p => p.BorrowingRecords)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK__Borrowing__BookI__46E78A0C");

            entity.HasOne(d => d.Reader).WithMany(p => p.BorrowingRecords)
                .HasForeignKey(d => d.ReaderId)
                .HasConstraintName("FK__Borrowing__Reade__45F365D3");
        });

        modelBuilder.Entity<Reader>(entity =>
        {
            entity.HasKey(e => e.ReaderId).HasName("PK__Readers__8E67A58195BCCC42");

            entity.HasIndex(e => e.PhoneNumber, "UQ__Readers__85FB4E38F586836E").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Readers__A9D105348877E8FB").IsUnique();

            entity.Property(e => e.ReaderId).HasColumnName("ReaderID");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC00837D5E");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E49D8C6AC5").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.ReaderId).HasColumnName("ReaderID");
            entity.Property(e => e.Role).HasMaxLength(20);
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.Reader).WithMany(p => p.Users)
                .HasForeignKey(d => d.ReaderId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Users__ReaderID__412EB0B6");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
