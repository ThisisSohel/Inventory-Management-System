using FluentNHibernate.Mapping;
using IMS.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.DAO.Mappings
{
    public class CustomerMapping : ClassMap<Customer>
    {
        public CustomerMapping() 
        {
            Table("Customers");
            Id(x => x.Id);
            Map(x => x.CustomerName);
            Map(x => x.CustomerNumber);
            Map(x => x.EmailAddress);
            Map(x => x.CustomerAddress);
            Map(x => x.CreatedBy);
            Map(x => x.CreatedDate);
            Map(x => x.ModifyBy);
            Map(x => x.ModifyDate);
        }
    }
}
