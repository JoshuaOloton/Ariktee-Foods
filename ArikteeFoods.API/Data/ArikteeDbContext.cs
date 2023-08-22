﻿using System;
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

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VwCart> VwCarts { get; set; }

    public virtual DbSet<VwCartItem> VwCartItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("<ConnectionString>");

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
        });

        modelBuilder.Entity<VwCart>(entity =>
        {
            entity.ToView("vw_Carts");
        });

        modelBuilder.Entity<VwCartItem>(entity =>
        {
            entity.ToView("vw_CartItems");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
