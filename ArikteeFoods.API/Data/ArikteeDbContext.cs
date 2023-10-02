using System;
using System.Collections.Generic;
using ArikteeFoods.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArikteeFoods.API.Data;

public partial class ArikteeDbContext : DbContext
{
    public ArikteeDbContext()
    {
    }

    public ArikteeDbContext(DbContextOptions<ArikteeDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<DeliveryAddress> DeliveryAddresses { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductUnit> ProductUnits { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VwCart> VwCarts { get; set; }

    public virtual DbSet<VwCartItem> VwCartItems { get; set; }

    public virtual DbSet<VwProductUnit> VwProductUnits { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("<Connection_String>");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasOne(d => d.User).WithMany(p => p.Carts).HasConstraintName("FK_Cart_User");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems).HasConstraintName("FK_CartItem_Cart");

            entity.HasOne(d => d.Product).WithMany(p => p.CartItems).HasConstraintName("FK_CartItem_Product");

            entity.HasOne(d => d.Unit).WithMany(p => p.CartItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItem_ProductUnit");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_State");
        });

        modelBuilder.Entity<DeliveryAddress>(entity =>
        {
            entity.HasOne(d => d.User).WithMany(p => p.DeliveryAddresses).HasConstraintName("FK_DeliveryAddress_User");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<ProductUnit>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Product).WithMany(p => p.ProductUnits).HasConstraintName("FK_ProductUnit_Product");
        });

        modelBuilder.Entity<VwCart>(entity =>
        {
            entity.ToView("vw_Carts");
        });

        modelBuilder.Entity<VwCartItem>(entity =>
        {
            entity.ToView("vw_CartItems");
        });

        modelBuilder.Entity<VwProductUnit>(entity =>
        {
            entity.ToView("vw_ProductUnits");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
