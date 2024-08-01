using IMS.Entity.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entity.EntityViewModels
{
    public class ProductCategoryViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Name is required!")]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Please enter upper and lower case only!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "The name field must be a minimum length of 3 and a maximum lenght of 20!")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Description is required!")]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Please enter upper and lower case only!")]
        [StringLength(120, MinimumLength = 12, ErrorMessage = "The name field must be a minimum length of 12 and a maximum lenght of 60!")]
        public string CategoryDescription { get; set; }
        public long CreatedBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedDate { get; set; }
        public long ModifyBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? ModifyDate { get; set; }

    }
}
