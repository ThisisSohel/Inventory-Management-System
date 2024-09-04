using IMS.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Linq;
using ISession = NHibernate.ISession;

namespace IMS.DAO.ProductDao
{
	public interface IProductsDao
	{
		Task<List<Product>> GetAll();
		Task<Product> GetById(long id);
		Task Create(Product product);
		Task Update(Product product);
		Task Delete(long id);
	}
	public class ProductsDao : IProductsDao
	{
		private readonly ISession _session;

		public ProductsDao(ISession session)
		{
			_session = session;
		}

		public async Task Create(Product product)
		{
			await _session.SaveAsync(product);
		}

		public async Task<List<Product>> GetAll()
		{
			return await _session.Query<Product>().ToListAsync();
		}

		public async Task<Product> GetById(long id)
		{
			return await _session.GetAsync<Product>(id);
		}

		public async Task Update(Product product)
		{
			await _session.UpdateAsync(product);
		}

		public async Task Delete(long id)
		{
			var productToDelete = await _session.GetAsync<Product>(id);

			if (productToDelete != null)
			{
				await _session.DeleteAsync(productToDelete);
			}
		}
	}
}
