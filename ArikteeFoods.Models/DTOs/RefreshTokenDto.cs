using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArikteeFoods.Models.DTOs
{
    public class RefreshTokenDto
    {
        // create 3 properties for access token, refresh token, user id
        public int UserId { get; set; }
        public string AccessToken { get; set; }
        public string RefeshToken { get; set; }
    }
}
