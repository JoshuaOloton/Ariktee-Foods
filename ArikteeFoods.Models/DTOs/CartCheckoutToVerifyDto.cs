using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArikteeFoods.Models.DTOs
{
    public class CartCheckoutToVerifyDto
    {
        public int CartId { get; set; }
        public String Reference { get; set; }
    }
}
