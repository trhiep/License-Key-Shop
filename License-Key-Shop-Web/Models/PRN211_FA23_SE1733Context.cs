using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace License_Key_Shop_Web.Models
{
    public partial class PRN211_FA23_SE1733Context : DbContext
    {
        public static PRN211_FA23_SE1733Context INSTANCE = new PRN211_FA23_SE1733Context();
        public PRN211_FA23_SE1733Context()
        {
            if (INSTANCE == null)
            {
                INSTANCE = this;
            }
        }

        public PRN211_FA23_SE1733Context(DbContextOptions<PRN211_FA23_SE1733Context> options)
            : base(options)
        {
        }

        public virtual DbSet<BalanceHistoryHe173252> BalanceHistoryHe173252s { get; set; } = null!;
        public virtual DbSet<CartHe173252> CartHe173252s { get; set; } = null!;
        public virtual DbSet<CartItemHe173252> CartItemHe173252s { get; set; } = null!;
        public virtual DbSet<CategoryHe173252> CategoryHe173252s { get; set; } = null!;
        public virtual DbSet<DepositHistoryHe173252> DepositHistoryHe173252s { get; set; } = null!;
        public virtual DbSet<OrderDetailHe173252> OrderDetailHe173252s { get; set; } = null!;
        public virtual DbSet<OrderHistoryHe173252> OrderHistoryHe173252s { get; set; } = null!;
        public virtual DbSet<ProductHe173252> ProductHe173252s { get; set; } = null!;
        public virtual DbSet<ProductKeyHe173252> ProductKeyHe173252s { get; set; } = null!;
        public virtual DbSet<RoleHe173252> RoleHe173252s { get; set; } = null!;
        public virtual DbSet<UserBalanceHe173252> UserBalanceHe173252s { get; set; } = null!;
        public virtual DbSet<UserHe173252> UserHe173252s { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder().
                    SetBasePath(Directory.GetCurrentDirectory()).

                AddJsonFile("appsettings.json", optional: false);

                IConfiguration con = builder.Build();

                optionsBuilder.UseSqlServer(con.GetConnectionString("DBContext"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BalanceHistoryHe173252>(entity =>
            {
                entity.ToTable("BalanceHistory_HE173252");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ChangeDate).HasColumnType("datetime");

                entity.Property(e => e.Reason)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserUsername)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("User_Username");

                entity.HasOne(d => d.UserUsernameNavigation)
                    .WithMany(p => p.BalanceHistoryHe173252s)
                    .HasForeignKey(d => d.UserUsername)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("BalanceHistory_HE173252_User_HE173252");
            });

            modelBuilder.Entity<CartHe173252>(entity =>
            {
                entity.HasKey(e => e.UserUsername)
                    .HasName("Cart_HE173252_pk");

                entity.ToTable("Cart_HE173252");

                entity.Property(e => e.UserUsername)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("User_Username");

                entity.HasOne(d => d.UserUsernameNavigation)
                    .WithOne(p => p.CartHe173252)
                    .HasForeignKey<CartHe173252>(d => d.UserUsername)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Cart_HE173252_User_HE173252");
            });

            modelBuilder.Entity<CartItemHe173252>(entity =>
            {
                entity.HasKey(e => e.ItemId)
                    .HasName("CartItem_HE173252_pk");

                entity.ToTable("CartItem_HE173252");

                entity.Property(e => e.ItemId).HasColumnName("ItemID");

                entity.Property(e => e.ProductProductId).HasColumnName("Product_ProductID");

                entity.Property(e => e.UserUsername)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("User_Username");

                entity.HasOne(d => d.ProductProduct)
                    .WithMany(p => p.CartItemHe173252s)
                    .HasForeignKey(d => d.ProductProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CartItem_HE173252_Product_HE173252");

                entity.HasOne(d => d.UserUsernameNavigation)
                    .WithMany(p => p.CartItemHe173252s)
                    .HasForeignKey(d => d.UserUsername)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CartItem_HE173252_Cart_HE173252");
            });

            modelBuilder.Entity<CategoryHe173252>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("Category_HE173252_pk");

                entity.ToTable("Category_HE173252");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryName).HasMaxLength(255);
            });

            modelBuilder.Entity<DepositHistoryHe173252>(entity =>
            {
                entity.HasKey(e => e.DepositId)
                    .HasName("DepositRequest_HE173252_pk");

                entity.ToTable("DepositHistory_HE173252");

                entity.Property(e => e.DepositId).HasColumnName("DepositID");

                entity.Property(e => e.ActionBy)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.ActionDate).HasColumnType("datetime");

                entity.Property(e => e.UserUsername)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("User_Username");

                entity.HasOne(d => d.UserUsernameNavigation)
                    .WithMany(p => p.DepositHistoryHe173252s)
                    .HasForeignKey(d => d.UserUsername)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("DepositRequest_HE173252_User_HE173252");
            });

            modelBuilder.Entity<OrderDetailHe173252>(entity =>
            {
                entity.HasKey(e => e.OrderDetailId)
                    .HasName("OrderDetail_HE173252_pk");

                entity.ToTable("OrderDetail_HE173252");

                entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");

                entity.Property(e => e.ExpirationDate).HasColumnType("date");

                entity.Property(e => e.OrderHistoryOrderId).HasColumnName("OrderHistory_OrderID");

                entity.Property(e => e.ProductKey)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ProductSoldName).HasMaxLength(255);

                entity.HasOne(d => d.OrderHistoryOrder)
                    .WithMany(p => p.OrderDetailHe173252s)
                    .HasForeignKey(d => d.OrderHistoryOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("OrderDetail_HE173252_OrderHistory_HE173252");
            });

            modelBuilder.Entity<OrderHistoryHe173252>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("OrderHistory_HE173252_pk");

                entity.ToTable("OrderHistory_HE173252");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.UserUsername)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("User_Username");

                entity.HasOne(d => d.UserUsernameNavigation)
                    .WithMany(p => p.OrderHistoryHe173252s)
                    .HasForeignKey(d => d.UserUsername)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("OrderHistory_HE173252_User_HE173252");
            });

            modelBuilder.Entity<ProductHe173252>(entity =>
            {
                entity.HasKey(e => e.ProductId)
                    .HasName("Product_HE173252_pk");

                entity.ToTable("Product_HE173252");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.CategoryCategoryId).HasColumnName("Category_CategoryID");

                entity.Property(e => e.ProductName).HasMaxLength(255);

                entity.HasOne(d => d.CategoryCategory)
                    .WithMany(p => p.ProductHe173252s)
                    .HasForeignKey(d => d.CategoryCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Product_Category");
            });

            modelBuilder.Entity<ProductKeyHe173252>(entity =>
            {
                entity.HasKey(e => e.KeyId)
                    .HasName("ProductKey_HE173252_pk");

                entity.ToTable("ProductKey_HE173252");

                entity.Property(e => e.KeyId).HasColumnName("KeyID");

                entity.Property(e => e.ExpirationDate)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.IsExpired).HasColumnName("isExpired");

                entity.Property(e => e.ProductKey)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ProductProductId).HasColumnName("Product_ProductID");

                entity.HasOne(d => d.ProductProduct)
                    .WithMany(p => p.ProductKeyHe173252s)
                    .HasForeignKey(d => d.ProductProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ProductKey_HE173252_Product_HE173252");
            });

            modelBuilder.Entity<RoleHe173252>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("Role_HE173252_pk");

                entity.ToTable("Role_HE173252");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            modelBuilder.Entity<UserBalanceHe173252>(entity =>
            {
                entity.HasKey(e => e.UserUsername)
                    .HasName("UserBalance_HE173252_pk");

                entity.ToTable("UserBalance_HE173252");

                entity.Property(e => e.UserUsername)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("User_Username");

                entity.HasOne(d => d.UserUsernameNavigation)
                    .WithOne(p => p.UserBalanceHe173252)
                    .HasForeignKey<UserBalanceHe173252>(d => d.UserUsername)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UserBalance_HE173252_User_HE173252");
            });

            modelBuilder.Entity<UserHe173252>(entity =>
            {
                entity.HasKey(e => e.Username)
                    .HasName("User_HE173252_pk");

                entity.ToTable("User_HE173252");

                entity.Property(e => e.Username)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName).HasMaxLength(75);

                entity.Property(e => e.LastName).HasMaxLength(75);

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RoleRoleId).HasColumnName("Role_RoleID");

                entity.HasOne(d => d.RoleRole)
                    .WithMany(p => p.UserHe173252s)
                    .HasForeignKey(d => d.RoleRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("User_Role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
