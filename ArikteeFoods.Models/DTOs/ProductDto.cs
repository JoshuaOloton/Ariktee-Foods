using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArikteeFoods.Models.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string ProductName { get; set; } = null!;

        public string? ProductDescription { get; set; }

        public string? ProductImageUrl { get; set; }

        public List<ProductUnitDto> ProductUnits { get; set; }
    }

    public class ProductUnitDto
    {
        public int Id { get; set; }
        public string Unit { get; set; }
        public int Price { get; set;
        }
    }
}
