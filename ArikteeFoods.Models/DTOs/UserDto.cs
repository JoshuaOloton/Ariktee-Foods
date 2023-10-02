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
        public String? RefreshToken { get; set; }
        public int UserId { get; set; }
        public String Fullname { get; set; }
        public String Email { get; set; }
        public String PhoneNo { get; set; }
        public String PasswordHash { get; set; }
        public List<AddressDto> DeliveryAddresses { get; set; }
    }

    public class AddressDto
    {
        public String City { get; set; }
        public String HouseAddress { get; set; }
        public bool Recent { get; set; }
    }
}
