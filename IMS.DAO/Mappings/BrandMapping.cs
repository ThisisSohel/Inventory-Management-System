using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Entity.Entities;

namespace IMS.DAO.Mappings
{
    public class BrandMapping : ClassMap<Brand>
    {
         public BrandMapping()
        {
            Table("Brand");
            Id(x => x.Id);
            Map(x => x.BrandName);
            Map(x => x.CreatedBy);
            Map(x => x.CreatedDate);
            Map(x => x.ModifyBy);
            Map(x => x.ModifyDate);
        }
    }
}
