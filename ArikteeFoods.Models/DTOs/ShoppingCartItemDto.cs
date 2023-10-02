using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArikteeFoods.Models.DTOs
{
    public class ShoppingCartItemDto
    {
        public int Id { get; set; }

        public int CartId { get; set; }

        public int ProductId { get; set; }

        public int Qty { get; set; }

        public String ProductName { get; set; }

        public string ProductImageURL { get; set; }

        public String ProductUnit { get; set; }

        public int ProductUnitPrice { get; set; }

        public int TotalPrice { get; set; }

        public String UserEmail { get; set; }

        public String UserFullname { get; set; }
    }
}
