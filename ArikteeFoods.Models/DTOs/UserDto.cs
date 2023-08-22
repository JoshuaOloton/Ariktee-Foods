using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArikteeFoods.Models.DTOs
{
    public class UserDto
    {
        public String? Token { get; set; }
        public int UserId { get; set; }
        public String Surname { get; set; }
        public String Firstname { get; set; }
        public String Email { get; set; }
        public String PhoneNo { get; set; }
        public String DeliveryAddress { get; set; }
        public String PasswordHash { get; set; }
    }
}
