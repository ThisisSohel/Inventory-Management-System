﻿using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Entity;
using IMS.Entity.Entities;

namespace IMS.DAO.Mappings
{
    public class ProductTypeMapping : ClassMap<ProductType>
    {
        public ProductTypeMapping() 
        {
            Table("ProductType");
            Id(x => x.Id);
            Map(x => x.TypeName);
            Map(x => x.CreatedBy);
            Map(x => x.CreatedDate);
            Map(x => x.ModifyBy);
            Map(x => x.ModifyDate);

            References(x => x.ProductCategory)
                .Column("CategoryId")
                .Not.Nullable();
        }
    }
}
