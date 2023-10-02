using System.ComponentModel.DataAnnotations;

namespace ArikteeFoods.Web.Models
{
    public class SelectAddress
    {
        [Required(ErrorMessage = "Please select your delivery address.")]
        [DataType(DataType.Text)]
        public String DeliveryAddress { get; set; }
    }
}
