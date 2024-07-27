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
        Task<IEnumerable<SKU>> Load();
        Task<SKU> Get(long id);
        Task SkuCreate(SKU sKU);
        Task SkuUpdate(SKU sKU);
        Task SkuDelete(long id);
    }
    public class SKUDao : ISkuDao
    {
        private readonly ISession _session;
        public SKUDao(ISession session)
        {
            _session = session;
        }
        public async Task<IEnumerable<SKU>> Load()
        {
            try
            {
                return await _session.Query<SKU>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve Product SKU", ex);
            }
        }


        public async Task<SKU> Get(long id)
        {
            try
            {
                return await _session.GetAsync<SKU>(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve the product SKU with the ID {id}", ex);
            }
        }


        public async Task SkuCreate(SKU sKU)
        {
            try
            {
                using (var transaction = _session.BeginTransaction())
                {
                    try
                    {
                        await _session.SaveAsync(sKU);
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("SKU is not created! Internal issue!", ex);

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create brand", ex);

            }
        }


        public async Task SkuUpdate(SKU sKU)
        {
            try
            {
                using (var transaction = _session.BeginTransaction())
                {
                    try
                    {
                        await _session.UpdateAsync(sKU);
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("SKU is not updated! Internal Error!", ex);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update the SKU", ex);
            }
        }


        public async Task SkuDelete(long id)
        {
            try
            {
                var sku = await _session.GetAsync<SKU>(id);
                if (sku != null)
                {
                    using (var transaction = _session.BeginTransaction())
                    {
                        try
                        {
                            await _session.DeleteAsync(sku);
                            await transaction.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Rolled Back! Please try again!", ex);
                        }
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
