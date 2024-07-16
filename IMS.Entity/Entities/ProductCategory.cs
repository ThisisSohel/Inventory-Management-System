using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entity.Entities
{
    public class ProductCategory
    {
        public virtual long Id { get; set; }

        [Required(ErrorMessage = "Name is required!")]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Please enter upper and lower case only!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "The name field must be a minimum length of 3 and a maximum lenght of 20!")]
        public virtual string CategoryName { get; set; }

        [Required(ErrorMessage = "Description is required!")]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Please enter upper and lower case only!")]
        [StringLength(120, MinimumLength = 12, ErrorMessage = "The name field must be a minimum length of 12 and a maximum lenght of 60!")]
        public virtual string CategoryDescription { get; set; }
        public virtual string CreatedBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string ModifyBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public virtual DateTime? ModifyDate { get; set; }
        public virtual Product Product { get; set; }
        public virtual IList<Product> Products { get; set; }
    }
}
