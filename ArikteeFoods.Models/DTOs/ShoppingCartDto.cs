using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArikteeFoods.Models.DTOs
{
    public class ShoppingCartDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime TransDate { get; set; }
        public int TransStatus { get; set; }
        public String? AuthorizationUrl { get; set; }
        public String? TransReference { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? SubTotal { get; set; }
        public String UserEmail { get; set; }
        public String UserFullname { get; set; }
    }
}
