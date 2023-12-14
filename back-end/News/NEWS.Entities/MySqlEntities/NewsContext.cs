using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NEWS.Entities.MySqlEntities;

public partial class NewsContext : DbContext
{
    public NewsContext()
    {
    }

    public NewsContext(DbContextOptions<NewsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<FileManagement> FileManagements { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<PostCategory> PostCategories { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_vietnamese_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Category");

            entity.Property(e => e.IsActive).HasDefaultValueSql("'1'");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .UseCollation("utf8mb4_0900_ai_ci");
        });

        modelBuilder.Entity<FileManagement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("FileManagement");

            entity.Property(e => e.Extension)
                .HasMaxLength(20)
                .HasDefaultValueSql("''");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Type).HasDefaultValueSql("'1'");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Post");

            entity.HasIndex(e => e.ThumbnailId, "ThumbnailId");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.Content)
                .HasColumnType("text")
                .UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.IntroText)
                .HasMaxLength(2000)
                .HasDefaultValueSql("''")
                .UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
            entity.Property(e => e.Status).HasDefaultValueSql("'1'");
            entity.Property(e => e.Title)
                .HasMaxLength(2000)
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasOne(d => d.Thumbnail).WithMany(p => p.Posts)
                .HasForeignKey(d => d.ThumbnailId)
                .HasConstraintName("post_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.Posts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("post_ibfk_1");
        });

        modelBuilder.Entity<PostCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("PostCategory");

            entity.HasIndex(e => e.CategoryId, "CategoryId");

            entity.HasIndex(e => new { e.PostId, e.CategoryId }, "post_category_id").IsUnique();

            entity.HasOne(d => d.Category).WithMany(p => p.PostCategories)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("postcategory_ibfk_2");

            entity.HasOne(d => d.Post).WithMany(p => p.PostCategories)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("postcategory_ibfk_1");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Role");

            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("User");

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.PhoneNumber).HasMaxLength(255);
            entity.Property(e => e.Salt).HasMaxLength(255);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("UserRole");

            entity.HasIndex(e => e.RoleId, "RoleId");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("userrole_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("userrole_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
