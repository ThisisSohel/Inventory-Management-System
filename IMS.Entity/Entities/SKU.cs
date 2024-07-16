using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IMS.Entity.Entities
{
    public class SKU
    {
        public virtual long Id { get; set; }
        [Required(ErrorMessage = "The name can not be blank")]
        //[RegularExpression(@"[A-Z]{3,50}$", ErrorMessage = "Only uppercase Characters are allowed.")]
        public virtual string SKUsName { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string ModifyBy { get; set; }
        public virtual DateTime? ModifyDate { get; set; }
        public virtual Product Product { get; set; }
        public virtual IList<Product> Products { get; set; }
    }
}
