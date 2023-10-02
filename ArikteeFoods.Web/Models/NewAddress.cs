using System.ComponentModel.DataAnnotations;

namespace ArikteeFoods.Web.Models
{
    public class NewAddress
    {
        [Required(ErrorMessage = "Please enter your house address.")]
        [DataType(DataType.Text)]
        public String StreetAddress { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter your city.")]
        public String City { get; set; }
    }
}
