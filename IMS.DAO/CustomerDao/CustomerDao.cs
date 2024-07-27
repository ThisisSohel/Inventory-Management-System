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
        Task<IEnumerable<Customer>> Load();
        Task<Customer> Get(long id);
        Task Create(Customer customer);
        Task Update(Customer customer);
        Task Delete(long id);
    }
    public class CustomerDao : ICustomerDao
    {
        private readonly ISession _session;
        public CustomerDao(ISession session)
        {
            _session = session;
        }
        public async Task<IEnumerable<Customer>> Load()
        {
            try
            {
                return await _session.Query<Customer>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve the customer", ex);
            }
        }

        public async Task<Customer> Get(long id)
        {
            try
            {
                return await _session.GetAsync<Customer>(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve customer with the ID {id}", ex);
            }
        }

        public async Task Create(Customer customer)
        {
            try
            {
                using (var transaction = _session.BeginTransaction())
                {
                    try
                    {
                        await _session.SaveAsync(customer);
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Customer is not created! Internal issue!", ex);

                    }
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
                    try
                    {
                        await _session.UpdateAsync(customer);
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Customer is not updated! Internal Error!", ex);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update the Customer", ex);
            }
        }

        public async Task Delete(long id)
        {
            try
            {
                var customer = await _session.GetAsync<Customer>(id);
                if (customer != null)
                {
                    using (var transaction = _session.BeginTransaction())
                    {
                        try
                        {
                            await _session.DeleteAsync(customer);
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
                throw new Exception($"Failed to delete Customer with ID {id}", ex);
            }
        }
    }
}
