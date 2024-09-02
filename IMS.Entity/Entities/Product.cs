using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Entity.Entities;

namespace IMS.Entity.Entities
{
    public class Product
    {
        public virtual long Id { get; set; }
        public virtual string ProductName { get; set; }
        public virtual int Quantity { get; set; }
        public virtual decimal Price { get; set; }
        public virtual string Description { get; set; }
        public virtual string ProductImage { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string ModifyBy { get; set; }
        public virtual DateTime? ModifyDate { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
    }
}
