using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ArikteeFoods.API.Entities;

[Table("Cart")]
public partial class Cart
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime TransDate { get; set; }

    public int TransStatus { get; set; }

    [StringLength(255)]
    public string? AuthorizationUrl { get; set; }

    [StringLength(50)]
    public string? TransReference { get; set; }

    [Column(TypeName = "date")]
    public DateTime? PaymentDate { get; set; }

    [Column("deliveryAddress")]
    [StringLength(512)]
    public string? DeliveryAddress { get; set; }

    [InverseProperty("Cart")]
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    [ForeignKey("UserId")]
    [InverseProperty("Carts")]
    public virtual User User { get; set; } = null!;
}
