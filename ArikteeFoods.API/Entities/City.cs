using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ArikteeFoods.API.Entities;

[Table("City")]
public partial class City
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("city")]
    [StringLength(50)]
    public string City1 { get; set; } = null!;
}
