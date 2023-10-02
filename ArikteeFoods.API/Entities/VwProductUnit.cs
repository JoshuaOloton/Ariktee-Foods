using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ArikteeFoods.API.Entities;

[Keyless]
public partial class VwProductUnit
{
    [StringLength(50)]
    public string ProductName { get; set; } = null!;

    [Column("ProductImageURL")]
    [StringLength(50)]
    public string? ProductImageUrl { get; set; }

    [Column("unit")]
    [StringLength(50)]
    [Unicode(false)]
    public string Unit { get; set; } = null!;

    [Column("unitPrice")]
    public int UnitPrice { get; set; }
}
