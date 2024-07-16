using FluentNHibernate.Mapping;
using IMS.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.DAO.Mappings
{
    public class SupplierMapping : ClassMap<Supplier>
    {
        public SupplierMapping() 
        {
            Table("Suppliers");
            Id(x => x.Id);
            Map(x => x.SupplierName);
            Map(x => x.SupplierNumber);
            Map(x => x.EmailAddress);
            Map(x => x.SupplierAddress);
            Map(x => x.CreatedBy);
            Map(x => x.CreatedDate);
            Map(x => x.ModifyBy);
            Map(x => x.ModifyDate);
        }
    }
}
