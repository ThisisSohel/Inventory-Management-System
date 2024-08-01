using IMS.Entity.Entities;
using IMS.Entity.EntityViewModels;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.DAO.ProductDao
{
    public interface ICategoryDao
    {
        Task<List<ProductCategory>> LoadAll();
        Task<ProductCategory> GetByIdAsync(long id);
        Task CreateAsync(ProductCategory productCategory);
        Task UpdateAsync(ProductCategory productCategory);
        Task DeleteAsync(long id);
    }
    public class CategoryDao : ICategoryDao
    {
        private readonly ISession _session;

        public CategoryDao(ISession session)
        {
            _session = session;
        }

        public async Task<List<ProductCategory>> LoadAll()
        {

            return await _session.Query<ProductCategory>().ToListAsync();

        }

        public async Task<ProductCategory> GetByIdAsync(long id)
        {

             return await _session.GetAsync<ProductCategory>(id);

        }

        public async Task CreateAsync(ProductCategory productCategory)
        {
              await _session.SaveAsync(productCategory);
        }

        public async Task UpdateAsync(ProductCategory productCategory)
        {
             await _session.UpdateAsync(productCategory);
        }

        public async Task DeleteAsync(long id)
        {
            var deleteCategory = await _session.GetAsync<ProductCategory>(id);
            await _session.DeleteAsync(deleteCategory);
        }
    }
}
