using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ArikteeFoods.API.Entities;

[Table("DeliveryAddress")]
public partial class DeliveryAddress
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("city")]
    [StringLength(50)]
    public string City { get; set; } = null!;

    [Column("houseAddress")]
    [StringLength(1024)]
    public string HouseAddress { get; set; } = null!;

    [Column("userId")]
    public int UserId { get; set; }

    [Column("recent")]
    public bool? Recent { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("DeliveryAddresses")]
    public virtual User User { get; set; } = null!;
}
