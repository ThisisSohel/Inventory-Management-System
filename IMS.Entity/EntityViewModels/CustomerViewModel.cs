using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entity.EntityViewModels
{
    public class CustomerViewModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Name can not be blank!")]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Please enter upper and lower case only & no special character/digit is acceptable!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "The name field must be a minimum lenght of 3 and a maximum lenght of 20!")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Mobile number is required!")]
        [Phone]
        [RegularExpression("^([0-9]{11})$", ErrorMessage = "Invalid Mobile Number.")]
        public string CustomerNumber { get; set; }

        [Required(ErrorMessage = "Email address is required!")]
        [RegularExpression(@"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$", ErrorMessage = "Invalid email address!")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Customer address is required!")]
        public string CustomerAddress { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
