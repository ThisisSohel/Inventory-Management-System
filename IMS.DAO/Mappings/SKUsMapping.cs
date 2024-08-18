using FluentNHibernate.Mapping;
using IMS.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.DAO
{
    public class SKUsMapping : ClassMap<SKU>
    {
        public SKUsMapping() 
        {
            Table("SKU");
            Id(x => x.Id);
            Map(x => x.SKUsName);
            Map(x => x.CreatedBy);
            Map(x => x.CreatedDate);
            Map(x => x.ModifyBy);
            Map(x => x.ModifyDate);

            References(x => x.ProductType)
                .Column("TypeId")
                .Not.Nullable();
        }
    }
}
