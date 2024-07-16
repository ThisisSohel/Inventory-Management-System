using IMS.DAO.CustomerDao;
using IMS.DAO.ProductDao;
using IMS.Entity.Entities;
using log4net;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using IMS.CustomException;

namespace IMS.Service
{
    public interface ICustomerService
    {
        Task CreateAsync(Customer customer);
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer> GetById(long id);
        Task UpdateAsync(long id, Customer customer);
        Task DeleteAsync(long id);
    }
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerDao _customerDao;
        private readonly ISession _session;
        private readonly ISessionFactory _sessionFactory;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(CustomerService));
        public CustomerService(ICustomerDao customerDao)
        {
            _customerDao = customerDao;
        }
        public CustomerService()
        {
            _sessionFactory = NHibernateConfig.GetSession();
            _session = _sessionFactory.OpenSession();
            _customerDao = new CustomerDao(_session);
        }
        private void CustomerValidator(Customer customerToValidate)
        {
            if (customerToValidate.CustomerName.Trim().Length == 0)
            {
                throw new InvalidNameException("sorry! your input feild is empty.");
            }
            if (String.IsNullOrWhiteSpace(customerToValidate.CustomerName))
            {
                throw new InvalidNameException("sorry! only white space is not allowed");
            }
            if (customerToValidate.CustomerName.Trim().Length < 5 || customerToValidate.CustomerName.Trim().Length > 30)
            {
                throw new InvalidNameException("Sorry! You have to input your name more than 5 character and less than 60 characters");
            }
        }
        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            try
            {
                return await _customerDao.GetAll();

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw new InvalidNameException(ex.Message);
            }
        }
        public async Task<Customer> GetById(long id)
        {
            try
            {
                var individualProductSku = await _customerDao.GetById(id);
                if (individualProductSku == null)
                {
                    throw new ObjectNotFoundException(individualProductSku, $"The Customer with the id {id} is not found");
                }
                return individualProductSku;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw new InvalidNameException(ex.Message);
            }
        }
        public async Task CreateAsync(Customer customer)
        {
            try
            {
                CustomerValidator(customer);
                var newCustomer = new Customer
                {
                    CustomerName = customer.CustomerName,
                    CustomerNumber = customer.CustomerNumber,
                    EmailAddress = customer.EmailAddress,
                    CustomerAddress = customer.CustomerAddress,
                    CreatedBy = 100.ToString(),
                    CreatedDate = DateTime.Now,
                    ModifyBy = 100.ToString(),
                    ModifyDate = DateTime.Now,
                };
                using (var transaction = _session.BeginTransaction())
                {
                    await _customerDao.Create(newCustomer);
                    await transaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateAsync(long id, Customer customer)
        {
            try
            {
                var individualCustomer = await _customerDao.GetById(id);
                if (individualCustomer != null)
                {
                    using (var transaction = _session.BeginTransaction())
                    {
                        individualCustomer.CustomerName = customer.CustomerName;
                        individualCustomer.CustomerNumber = customer.CustomerNumber;
                        individualCustomer.EmailAddress = customer.EmailAddress;
                        individualCustomer.CustomerAddress = customer.CustomerAddress;
                        //individualCustomer.ModifyBy = customer.ModifyBy;
                        individualCustomer.ModifyDate = DateTime.Now;
                        await _customerDao.Update(individualCustomer);
                        await transaction.CommitAsync();
                    }
                }
                else
                {
                    throw new ObjectNotFoundException(individualCustomer, "Customer Not Found!");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }
        public async Task DeleteAsync(long id)
        {
            try
            {
                var individualTypeDelete = _customerDao.GetById(id);
                if (individualTypeDelete != null)
                {
                    await _customerDao.DeleteById(id);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }
    }
}
