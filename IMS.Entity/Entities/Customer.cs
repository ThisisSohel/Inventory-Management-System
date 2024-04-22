using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entity.Entities
{
    public class Customer
    {
        public virtual long Id { get; set; }
        public virtual string CustomerName { get; set; }
        public virtual string CustomerNumber { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual string CustomerAddress {  get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedAt { get; set; }
        public virtual string UpdatedBy { get; set; }
        public virtual DateTime? UpdatedAt { get; set; }

    }
}
