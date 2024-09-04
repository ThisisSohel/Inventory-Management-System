using IMS.DAO.ProductDao;
using IMS.Entity.EntityViewModels.ProductViewModels;
using log4net;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISession = NHibernate.ISession;

namespace IMS.Service
{
	public interface IProductService
	{
		Task CreateAsync(ProductCreateViewModel model);
		Task<List<ProductViewModel>> GetAllAsync();
		Task<ProductViewModel> GetByIdAsync(long id);
		Task UpdateAsync(ProductUpdateViewModel model);
		Task DeleteAsync(long id);
	}

	public class ProductService : IProductService
	{
		private readonly IProductsDao _productDao;
		private readonly ISession _session;
		private readonly ISessionFactory _sessionFactory;
		private static readonly ILog _logger = LogManager.GetLogger(typeof(ProductService));

		public ProductService(IProductsDao productsDao)
		{
			_productDao = productsDao;
		}
		public ProductService()
		{
			_sessionFactory = NHibernateConfig.GetSession();
			_session = _sessionFactory.OpenSession();
			_productDao = new ProductsDao(_session);
		}

		public Task CreateAsync(ProductCreateViewModel model)
		{
			throw new System.NotImplementedException();
		}

		public async Task<List<ProductViewModel>> GetAllAsync()
		{
			try
			{
				var productListView = new List<ProductViewModel>();
				var products = await _productDao.GetAll();

				if(products.Count > 0)
				{
					productListView = products.Select(p => new ProductViewModel
					{
						Id = p.Id,
						BrandId = p.Brand.Id,
						CategoryId = p.ProductCategory.Id,
						ProductName = p.ProductName,
						Quantity = p.Quantity,
						Description = p.Description,
						ProductImage = p.ProductImage,
						CreatedBy = p.CreatedBy,
						CreatedDate = p.CreatedDate,
						ModifyBy = p.ModifyBy,
						ModifyDate = p.ModifyDate,
					}).ToList();
				}
				return productListView;	
			}
			catch(Exception ex)
			{
				_logger.Error(ex);	
				throw ex;
			}
		}

		public Task<ProductViewModel> GetByIdAsync(long id)
		{
			throw new System.NotImplementedException();
		}

		public Task UpdateAsync(ProductUpdateViewModel model)
		{
			throw new System.NotImplementedException();
		}

		public Task DeleteAsync(long id)
		{
			throw new System.NotImplementedException();
		}
	}
}
