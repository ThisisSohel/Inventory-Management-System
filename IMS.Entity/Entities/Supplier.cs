using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entity.Entities
{
    public class Supplier
    {
        public virtual long Id { get; set; }

        [Required(ErrorMessage ="Name is required!")]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Please enter upper and lower case only & no special character/digit is acceptable!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "The name field must be a minimum lenght of 3 and a maximum lenght of 20!")]
        public virtual string SupplierName { get; set; }

        [Required(ErrorMessage = "Mobile number is required!")]
        [Phone]
        [RegularExpression("^([0-9]{11})$", ErrorMessage = "Invalid Mobile Number.")]
        public virtual string SupplierNumber { get; set; }

        [Required(ErrorMessage = "Email address is required!")]
        [RegularExpression(@"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$", ErrorMessage = "Invalid email address!")]
        public virtual string EmailAddress { get; set; }

        [Required(ErrorMessage = "Customer address is required!")]
        public virtual string SupplierAddress { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string ModifyBy { get; set; }   
        public virtual DateTime? ModifyDate { get;set; }
    }
}
