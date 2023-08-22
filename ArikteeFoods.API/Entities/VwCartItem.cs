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

    public int Qty { get; set; }

    [Column(TypeName = "text")]
    public string? ProductDescription { get; set; }

    [StringLength(50)]
    public string ProductName { get; set; } = null!;

    [Column("ProductImageURL")]
    [StringLength(50)]
    public string? ProductImageUrl { get; set; }

    public int ProductPrice { get; set; }

    public int UserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime TransDate { get; set; }

    public int TransStatus { get; set; }

    [Column("surname")]
    [StringLength(50)]
    [Unicode(false)]
    public string Surname { get; set; } = null!;

    [Column("firstname")]
    [StringLength(50)]
    [Unicode(false)]
    public string Firstname { get; set; } = null!;

    [Column("email")]
    [StringLength(50)]
    [Unicode(false)]
    public string Email { get; set; } = null!;
}
