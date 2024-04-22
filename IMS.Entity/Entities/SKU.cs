using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entity.Entities
{
    public class SKU
    {
        public virtual long Id { get; set; }
        public virtual string SKUsName { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedAt { get; set; }
        public virtual string UpdatedBy { get; set; }
        public virtual DateTime? UpdatedAt { get; set; }
        public virtual Product Product { get; set; }
        public virtual IList<Product> Products { get; set; }
    }
}
