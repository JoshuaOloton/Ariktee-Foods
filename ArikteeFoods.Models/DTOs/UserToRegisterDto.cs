using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArikteeFoods.Models.DTOs
{
    public class UserToRegisterDto
    {
        public String FullName { get; set; }
        public String Email { get; set; }
        public String PhoneNo { get; set; }
        public String Password { get; set; }
    }
}
