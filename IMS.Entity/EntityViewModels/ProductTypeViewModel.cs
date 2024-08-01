using IMS.Entity.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entity.EntityViewModels
{
    public class ProductTypeViewModel
    {
        public  long Id { get; set; }

        [Required(ErrorMessage = "The name can not be Blank")]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Please enter upper and lower case only!")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "The name field must be a minimum length of 3 and a maximum length of 60!")]
        public string TypeName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
