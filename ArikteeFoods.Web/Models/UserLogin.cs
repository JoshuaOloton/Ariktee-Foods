using System.ComponentModel.DataAnnotations;

namespace ArikteeFoods.Web.Models
{
    public class UserLogin
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public String Password { get; set; }
    }
}
