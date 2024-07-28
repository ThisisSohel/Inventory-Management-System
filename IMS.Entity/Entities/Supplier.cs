using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public virtual long CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual long ModifyBy { get; set; }   
        public virtual DateTime? ModifyDate { get;set; }
    }
}
