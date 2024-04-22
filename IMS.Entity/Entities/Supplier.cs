using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entity.Entities
{
    public class Supplier
    {
        public virtual long Id { get; set; }
        public virtual string SupplierName { get; set; }
        public virtual string SupplierNumber { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual string SupplierAddress { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedAt { get; set; }
        public virtual string UpdatedBy { get; set; }   
        public virtual DateTime? UpdatedAt { get;set; }
    }
}
