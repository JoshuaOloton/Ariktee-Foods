using System.ComponentModel.DataAnnotations;

namespace ArikteeFoods.Web.Models
{
    public class UserRegister
    {
        [Required(ErrorMessage = "Your FullName is required.")]
        [DataType(DataType.Text)]
        public String FullName { get; set; }

        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }

        [Required(ErrorMessage = "Please enter your phone number.")]
        [DataType(DataType.PhoneNumber)]
        public String PhoneNo { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Your password should not be less 4 than characters long.")]
        public String Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public String ConfirmPassword { get; set; }
    }
}
