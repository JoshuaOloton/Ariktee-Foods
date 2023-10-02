using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ArikteeFoods.API.Entities;

[Keyless]
public partial class VwCart
{
    public int Id { get; set; }

    public int UserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime TransDate { get; set; }

    public int TransStatus { get; set; }

    [Column(TypeName = "date")]
    public DateTime? PaymentDate { get; set; }

    [Column("fullname")]
    [StringLength(127)]
    [Unicode(false)]
    public string Fullname { get; set; } = null!;

    [Column("email")]
    [StringLength(50)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Column("phoneNo")]
    [StringLength(50)]
    [Unicode(false)]
    public string PhoneNo { get; set; } = null!;
}
