using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArikteeFoods.Models.DTOs
{
    public class ShoppingCartDto
    {
        public int UserId { get; set; }
        public DateTime TransDate { get; set; }
        public int TransStatus { get; set; }
        public String UserEmail { get; set; }
        public String UserFirstname { get; set; }
        public String UserSurname { get; set; }
    }
}
