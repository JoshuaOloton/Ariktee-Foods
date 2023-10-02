using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArikteeFoods.Models.DTOs
{
    public class LoggedInUserDto
    {
        public bool Status { get; set; }
        public String AccessToken { get; set; }
        public String RefreshToken { get; set; }
        public int Id { get; set; }
        public String Email { get; set; }
        public String Fullname { get; set; }
    }
}
