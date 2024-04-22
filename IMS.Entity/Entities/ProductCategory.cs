using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entity.Entities
{
    public class ProductCategory
    {
        public virtual long Id { get; set; }
        [Required]
        [StringLength(60)]
        public virtual string CategoryName { get; set; }
        [Required]
        [StringLength(60)]
        [Display(Name = "Write the Description of Product Category")]
        public virtual string CategoryDescription { get; set; }
        public virtual string CreatedBy { get; set; }
        [DataType(DataType.Date)]
        public virtual DateTime? CreatedAt { get; set; }
        public virtual string UpdatedBy { get; set; }
        public virtual DateTime? UpdatedAt { get; set; }
        public virtual Product Product { get; set; }
        public virtual IList<Product> Products { get; set; }
    }
}
