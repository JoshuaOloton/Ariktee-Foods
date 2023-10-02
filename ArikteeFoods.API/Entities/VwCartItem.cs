using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ArikteeFoods.API.Entities;

[Keyless]
public partial class VwCartItem
{
    public int Id { get; set; }

    public int CartId { get; set; }

    public int ProductId { get; set; }

    public int? UnitId { get; set; }

    public int Qty { get; set; }

    [StringLength(50)]
    public string ProductName { get; set; } = null!;

    [Column("ProductImageURL")]
    [StringLength(50)]
    public string? ProductImageUrl { get; set; }

    public int UserId { get; set; }

    [Column("fullname")]
    [StringLength(127)]
    [Unicode(false)]
    public string Fullname { get; set; } = null!;

    [Column("email")]
    [StringLength(50)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Column("unit")]
    [StringLength(50)]
    [Unicode(false)]
    public string Unit { get; set; } = null!;

    [Column("unitPrice")]
    public int UnitPrice { get; set; }
}
