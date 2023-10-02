using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ArikteeFoods.API.Entities;

[Table("User")]
public partial class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

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

    [Column("password")]
    [StringLength(50)]
    public string Password { get; set; } = null!;

    [Column("passwordhash")]
    [StringLength(512)]
    public string Passwordhash { get; set; } = null!;

    [Column("refreshToken")]
    [StringLength(512)]
    public string? RefreshToken { get; set; }

    [Column("tokenCreated", TypeName = "datetime")]
    public DateTime? TokenCreated { get; set; }

    [Column("tokenExpires", TypeName = "datetime")]
    public DateTime? TokenExpires { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    [InverseProperty("User")]
    public virtual ICollection<DeliveryAddress> DeliveryAddresses { get; set; } = new List<DeliveryAddress>();
}
