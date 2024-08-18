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
        Task<List<ProductType>> GetAll();
        Task<ProductType> GetById(long id);
        Task Create(ProductType productType);
        Task Update(ProductType productType);
        Task DeleteById(long id);
    }
    public class ProductTypeDao : IProductTypeDao
    {
        private readonly ISession _session;

        public ProductTypeDao(ISession session)
        {
            _session = session;
        }

        public async Task<List<ProductType>> GetAll()
        {
            try
            {
                return await _session.Query<ProductType>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Faild to retrieve Product Type", ex);
            }
        }
        public async Task<ProductType> GetById(long id)
        {
            return await _session.GetAsync<ProductType>(id);
        }

        public async Task Create(ProductType productType)
        {
            await _session.SaveAsync(productType);
        }

        public async Task Update(ProductType productType)
        {
            await _session.UpdateAsync(productType);
        }

        public async Task DeleteById(long id)
        {

            var deleteIndividualType = await _session.GetAsync<ProductType>(id);

            if (deleteIndividualType != null)
            {
                await _session.DeleteAsync(deleteIndividualType);
            }
        }
    }
}
