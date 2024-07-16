using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Entity.Entities;

namespace IMS.DAO.Mappings
{
    public class ProductCategoryMapping : ClassMap<ProductCategory>
    {
        public ProductCategoryMapping() 
        {
            Table("ProductCategory");
            Id(x => x.Id);
            Map(x => x.CategoryName);
            Map(x => x.CategoryDescription);
            Map(x => x.CreatedBy);
            Map(x => x.CreatedDate);
            Map(x => x.ModifyBy);
            Map(x => x.ModifyDate);
        }
    }
}
