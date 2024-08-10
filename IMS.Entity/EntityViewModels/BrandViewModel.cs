using IMS.Entity.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entity.EntityViewModels
{
    public class BrandViewModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Name is required!")]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Please enter upper and lower case only!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "The name field must be a minimum length of 3 and a maximum length of 20!")]
        public string BrandName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; } = DateTime.Now;
    }
}
