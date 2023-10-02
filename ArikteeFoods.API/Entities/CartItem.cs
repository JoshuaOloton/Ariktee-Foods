using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ArikteeFoods.API.Entities;

[Table("CartItem")]
public partial class CartItem
{
    [Key]
    public int Id { get; set; }

    public int CartId { get; set; }

    public int ProductId { get; set; }

    public int UnitId { get; set; }

    public int Qty { get; set; }

    public int UnitAmount { get; set; }

    [ForeignKey("CartId")]
    [InverseProperty("CartItems")]
    public virtual Cart Cart { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("CartItems")]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("UnitId")]
    [InverseProperty("CartItems")]
    public virtual ProductUnit Unit { get; set; } = null!;
}
