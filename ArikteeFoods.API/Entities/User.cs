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

    [Column("password")]
    [StringLength(50)]
    public string Password { get; set; } = null!;

    [Column("passwordhash")]
    [StringLength(512)]
    public string Passwordhash { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
}
