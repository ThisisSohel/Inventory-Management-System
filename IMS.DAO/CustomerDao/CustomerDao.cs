using IMS.Entity.Entities;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IMS.DAO.CustomerDao
{
    public interface ICustomerDao
    {
        Task<IEnumerable<Customer>> GetAll();
        Task<Customer> GetById(long id);
        Task Create(Customer customer);
        Task Update(Customer customer);
        Task DeleteById(long id);
    }
    public class CustomerDao : ICustomerDao
    {
        private readonly ISession _session;
        public CustomerDao(ISession session)
        {
            _session = session;
        }
        public async Task<IEnumerable<Customer>> GetAll()
        {
            try
            {
                return await _session.Query<Customer>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Faild to retrieve the customer", ex);
            }
        }
        public async Task<Customer> GetById(long id)
        {
            try
            {
                return await _session.GetAsync<Customer>(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Faild to retrieve customer with the ID {id}", ex);
            }
        }
        public async Task Create(Customer customer)
        {
            try
            {
                using (var transaction = _session.BeginTransaction())
                {
                    await _session.SaveAsync(customer);
                    await transaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create new customer", ex);
            }
        }
        public async Task Update(Customer customer)
        {
            try
            {
                using (var transaction = _session.BeginTransaction())
                {
                    await _session.UpdateAsync(customer);
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
                var deleteIndividualSKU = await _session.GetAsync<Customer>(id);
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
                throw new Exception($"Failed to delete Customer with ID {id}", ex);
            }
        }
    }
}
