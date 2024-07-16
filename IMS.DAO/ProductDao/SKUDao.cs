using IMS.Entity.Entities;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IMS.DAO.ProductDao
{
    public interface ISkuDao
    {
        Task<IEnumerable<SKU>> GetAll();
        Task<SKU> GetById(long id);
        Task Create(SKU sKU);
        Task Update(SKU sKU);
        Task DeleteById(long id);
    }
    public class SKUDao : ISkuDao
    {
        private readonly ISession _session;
        public SKUDao(ISession session)
        {
            _session = session;
        }
        public async Task<IEnumerable<SKU>> GetAll()
        {
            try
            {
                return await _session.Query<SKU>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Faild to retrieve Product SKU", ex);
            }
        }
        public async Task<SKU> GetById(long id)
        {
            try
            {
                return await _session.GetAsync<SKU>(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Faild to retrieve the product SKU with the ID {id}", ex);
            }
        }
        public async Task Create(SKU sKU)
        {
            try
            {
                using (var transaction = _session.BeginTransaction())
                {
                    await _session.SaveAsync(sKU);
                    await transaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create ProductType", ex);
            }
        }
        public async Task Update(SKU sKU)
        {
            try
            {
                using (var transaction = _session.BeginTransaction())
                {
                    await _session.UpdateAsync(sKU);
                    await transaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Faild to update the Type", ex);
            }
        }
        public async Task DeleteById(long id)
        {
            try
            {
                var deleteIndividualSKU = await _session.GetAsync<SKU>(id);
                if (deleteIndividualSKU != null)
                {
                    using (var transaction = _session.BeginTransaction())
                    {
                        await _session.DeleteAsync(deleteIndividualSKU);
                        await transaction.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete SKU with ID {id}", ex);
            }
        }
    }
}
