using FluentNHibernate.Mapping;
using IMS.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.DAO.Mappings
{
	internal class ProductMapping: ClassMap<Product>
	{
		public ProductMapping()
		{
			Table("Product");

			Id(x => x.Id);
			Map(x => x.ProductName);
			Map(x => x.Quantity);
			Map(x => x.Price);
			Map(x => x.Description);
			Map(x => x.ProductImage);
			Map(x => x.CreatedBy);
			Map(x => x.CreatedDate);
			Map(x => x.ModifyBy);
			Map(x => x.ModifyDate);

			References(x => x.Brand)
				.Column("BrandId")
				.Not.Nullable();

			References(x => x.ProductCategory)
				.Column("CategoryId")
				.Not.Nullable();
		}
	}
}
