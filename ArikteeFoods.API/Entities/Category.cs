using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ArikteeFoods.API.Entities;

[Keyless]
[Table("Category")]
public partial class Category
{
    public int Id { get; set; }

    [StringLength(50)]
    public string? CategoryName { get; set; }
}
