using IMS.Entity.Entities;
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
        Task<IEnumerable<ProductCategory>> GetAll();
        Task<ProductCategory> GetById(long id);
        Task CreateCategory(ProductCategory productCategory);
        Task Update(ProductCategory productCategory);
        Task DeleteById(long id);
    }
    public class CategoryDao : ICategoryDao
    {
            private readonly ISession _session;

            public CategoryDao(ISession session)
            {
                _session = session;
            }

            public async Task<IEnumerable<ProductCategory>> GetAll()
            {
                try
                {
                    return await _session.Query<ProductCategory>().ToListAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception("Faild to retrieve Categories", ex);
                }
            }

            public async Task<ProductCategory> GetById(long id)
            {
                try
                {
                    return await _session.GetAsync<ProductCategory>(id);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Faild to retrieve the product with the ID {id}", ex);
                }
            }

            public async Task CreateCategory(ProductCategory productCategory)
            {
                try
                {
                    using (var transaction = _session.BeginTransaction())
                    {
                        await _session.SaveAsync(productCategory);
                        await transaction.CommitAsync();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to create brand", ex);
                }
            }

            public async Task Update(ProductCategory productCategory)
            {
                try
                {
                    using (var transaction = _session.BeginTransaction())
                    {
                        await _session.UpdateAsync(productCategory);
                        await transaction.CommitAsync();
                    }
                }
                catch (Exception ex)
                {
                throw new Exception($"Faild to update the category", ex);
            }
            }

            public async Task DeleteById(long id)
            {
                try
                {
                    var deleteCategory = await _session.GetAsync<ProductCategory >(id);
                    if (deleteCategory != null)
                    {
                        using (var transaction = _session.BeginTransaction())
                        {
                            await _session.DeleteAsync(deleteCategory);
                            await transaction.CommitAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to delete category with ID {id}", ex);
                }
            }
    }
}
