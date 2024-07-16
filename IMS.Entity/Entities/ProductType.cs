using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IMS.Entity.Entities
{
    public class ProductType
    {
        public virtual long Id { get; set; }

        [Required(ErrorMessage = "The name can not be Blank")]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Please enter upper and lower case only!")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "The name field must be a minimum lenght of 3 and a maximum lenght of 60!")]
        [DisplayName("Product TypeName: ")]
        public virtual string TypeName { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string ModifyBy { get; set; }
        public virtual DateTime? ModifyDate { get; set; }
        public virtual Product Product { get; set; }
        public virtual IList<Product> Products { get; set; }
    }
}
