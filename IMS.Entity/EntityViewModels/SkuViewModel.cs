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
        public  long Id { get; set; }
        [Required(ErrorMessage = "The name can not be blank")]
        //[RegularExpression(@"[A-Z]{3,50}$", ErrorMessage = "Only uppercase Characters are allowed.")]
        public  string SKUsName { get; set; }
        public  string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
