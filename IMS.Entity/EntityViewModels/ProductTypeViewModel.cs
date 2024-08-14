using IMS.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entity.EntityViewModels
{
    public class ProductTypeViewModel
    {
        public long Id { get; set; }
        public string TypeName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public IList<ProductCategoryTypeViewModel> Category { get; set; } = new List<ProductCategoryTypeViewModel>();
    }
}
