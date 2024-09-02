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
        public virtual string CategoryName { get; set; }
        public virtual string CategoryDescription { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string ModifyBy { get; set; }
        public virtual DateTime? ModifyDate { get; set; }
        public virtual IList<ProductType> ProductType { get; set; } = new List<ProductType>();
        public virtual IList<Product> Products { get; set; }
    }
}
