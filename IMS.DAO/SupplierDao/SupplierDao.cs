using IMS.Entity.Entities;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using ISession = NHibernate.ISession;


namespace IMS.DAO.SupplierDao
{
    public interface ISupplierDao
    {
        Task<List<Supplier>> Load();
        Task<Supplier> Get(long id);
        Task Create(Supplier supplier);
        Task Update(Supplier supplier);
        Task Delete(long id);
    }
    public class SupplierDao : ISupplierDao
    {
        private readonly ISession _session;

        public SupplierDao(ISession session)
        {
            _session = session;
        }

        public async Task<List<Supplier>> Load()
        {
            try
            {
                return await _session.Query<Supplier>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve the supplier", ex);
            }
        }

        public async Task<Supplier> Get(long id)
        {
            try
            {
                return await _session.GetAsync<Supplier>(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve supplier with the ID {id}", ex);
            }
        }

        public async Task Create(Supplier supplier)
        {
            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    await _session.SaveAsync(supplier);
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Supplier is not created! Internal issue!", ex);
                }
            }
        }

        public async Task Update(Supplier supplier)
        {
            try
            {
                using (var transaction = _session.BeginTransaction())
                {
                    try
                    {
                        await _session.UpdateAsync(supplier);
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Internal Error! Please try again!", ex);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update the Supplier", ex);
            }
        }

        public async Task Delete(long id)
        {
            try
            {
                var supplier = await _session.GetAsync<Supplier>(id);
                if (supplier != null)
                {
                    using (var transaction = _session.BeginTransaction())
                    {
                        try
                        {
                            await _session.DeleteAsync(supplier);
                            await transaction.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Internal Error! Please try again!", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete supplier with ID {id}", ex);
            }
        }
    }


}
