using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ArikteeFoods.API.Entities;

[Table("Product")]
public partial class Product
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string ProductName { get; set; } = null!;

    [Column(TypeName = "text")]
    public string? ProductDescription { get; set; }

    [Column("ProductImageURL")]
    [StringLength(50)]
    public string? ProductImageUrl { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    [InverseProperty("Product")]
    public virtual ICollection<ProductUnit> ProductUnits { get; set; } = new List<ProductUnit>();
}
