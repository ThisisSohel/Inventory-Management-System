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
        Task<IEnumerable<Supplier>> GetAll();
        Task<Supplier> GetById(long id);
        Task Create(Supplier supplier);
        Task Update(Supplier supplier);
        Task DeleteById(long id);
    }
    public class SupplierDao : ISupplierDao
    {
        private readonly ISession _session;

        public SupplierDao(ISession session)
        {
            _session = session;
        }
        public async Task<IEnumerable<Supplier>> GetAll()
        {
            try
            {
                return await _session.Query<Supplier>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Faild to retrieve the supplier", ex);
            }
        }
        public async Task<Supplier> GetById(long id)
        {
            try
            {
                return await _session.GetAsync<Supplier>(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Faild to retrieve supplier with the ID {id}", ex);
            }
        }
        public async Task Create(Supplier supplier)
        {
            try
            {
                using (var transaction = _session.BeginTransaction())
                {
                    await _session.SaveAsync(supplier);
                    await transaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create new supplier", ex);
            }
        }
        public async Task Update(Supplier supplier)
        {
            try
            {
                using (var transaction = _session.BeginTransaction())
                {
                    await _session.UpdateAsync(supplier);
                    await transaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Faild to update the Supplier", ex);
            }
        }
        public async Task DeleteById(long id)
        {
            try
            {
                var deleteIndividualSupplier = await _session.GetAsync<Supplier>(id);
                if (deleteIndividualSupplier != null)
                {
                    using (var transaction = _session.BeginTransaction())
                    {
                        await _session.DeleteAsync(deleteIndividualSupplier);
                        await transaction.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete Customer with ID {id}", ex);
            }
        }
    }


}
