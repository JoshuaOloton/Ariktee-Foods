using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArikteeFoods.Models.DTOs
{
    public class InitializeCheckoutDto
    {
        public String Authorization_url { get; set; }
        public String Reference { get; set; }
    }
}
