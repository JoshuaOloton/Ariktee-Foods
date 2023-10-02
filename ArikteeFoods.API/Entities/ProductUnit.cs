using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ArikteeFoods.API.Entities;

[Table("ProductUnit")]
public partial class ProductUnit
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("unit")]
    [StringLength(50)]
    [Unicode(false)]
    public string Unit { get; set; } = null!;

    [Column("unitPrice")]
    public int UnitPrice { get; set; }

    [Column("productID")]
    public int ProductId { get; set; }

    [InverseProperty("Unit")]
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    [ForeignKey("ProductId")]
    [InverseProperty("ProductUnits")]
    public virtual Product Product { get; set; } = null!;
}
