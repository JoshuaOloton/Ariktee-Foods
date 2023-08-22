using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArikteeFoods.Models.DTOs
{
    public class ProductToAddDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public String ImageURL { get; set; }
        public int Price { get; set; }
    }
}
