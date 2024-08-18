using IMS.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entity.EntityViewModels.SKUViewModel
{
    public class SkuCreateViewModel
    {
        public virtual long TypeId { get; set; }
        public virtual string SKUsName { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual string ModifyBy { get; set; }
        public virtual DateTime? ModifyDate { get; set; }
    }
}
