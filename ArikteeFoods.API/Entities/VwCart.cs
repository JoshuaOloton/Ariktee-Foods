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

    [Column("phoneNo")]
    [StringLength(50)]
    [Unicode(false)]
    public string PhoneNo { get; set; } = null!;

    [Column("deliveryAddress")]
    [StringLength(255)]
    [Unicode(false)]
    public string DeliveryAddress { get; set; } = null!;
}
