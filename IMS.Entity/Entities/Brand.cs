using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entity.Entities
{
    public class Brand
    {
        public virtual long Id { get; set; }
        [Required]
        [StringLength(60)]
        public virtual string BrandName { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string ModifyBy { get; set; }
        public virtual DateTime? ModifyDate { get; set; }
        public virtual Product Product { get; set; }
        public virtual IList<Product> Products { get; set; }
    }
}
