using IMS.Entity.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entity.EntityViewModels
{
    public class SkuViewModel
    {
        public virtual long Id { get; set; }
        [Required(ErrorMessage = "The name can not be blank")]
        //[RegularExpression(@"[A-Z]{3,50}$", ErrorMessage = "Only uppercase Characters are allowed.")]
        public virtual string SKUsName { get; set; }
        public virtual long CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual long ModifyBy { get; set; }
        public virtual DateTime? ModifyDate { get; set; }
    }
}
