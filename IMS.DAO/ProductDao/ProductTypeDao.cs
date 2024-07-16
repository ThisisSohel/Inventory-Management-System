using IMS.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Linq;

namespace IMS.DAO.ProductDao
{
    public interface IProductTypeDao
    {
        Task<IEnumerable<ProductType>> GetAll();
        Task<ProductType>GetById(long id);
        Task Create(ProductType productType);
        Task Update(ProductType productType);
        Task DeleteById(long id);
    }
    public class ProductTypeDao : IProductTypeDao
    {
        private readonly ISession _session;

        public ProductTypeDao( ISession session)
        {
            _session = session;
        }

        public async Task<IEnumerable<ProductType>> GetAll()
        {
            try
            {
                return await _session.Query<ProductType>().ToListAsync();
            }catch (Exception ex)
            {
                throw new Exception("Faild to retrieve Product Type", ex);
            }
        }
        public async Task<ProductType> GetById(long id)
        {
            try
            {
                return await _session.GetAsync<ProductType>(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Faild to retrieve the product with the ID {id}", ex);
            }
        }
        public async Task Create(ProductType productType)
        {
            try
            {
                using (var transaction = _session.BeginTransaction())
                {
                    await _session.SaveAsync(productType);
                    await transaction.CommitAsync();
                }
            }catch (Exception ex)
            {
                throw new Exception("Failed to create ProductType", ex);
            }
        }
        public async Task Update(ProductType productType)
        {
            try
            {
                using(var transaction = _session.BeginTransaction())
                {
                    await _session.UpdateAsync(productType);
                    await transaction.CommitAsync();
                }
            }catch(Exception ex)
            {
                throw new Exception($"Faild to update the Type", ex);
            }
        }
        public async Task DeleteById(long id)
        {
            try
            {
                var deleteIndividualType = await _session.GetAsync<ProductType>(id);
                if (deleteIndividualType != null)
                {
                    using (var transaction = _session.BeginTransaction())
                    {
                        await _session.DeleteAsync(deleteIndividualType);
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
