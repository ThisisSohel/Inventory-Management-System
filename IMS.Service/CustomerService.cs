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
using IMS.Entity.EntityViewModels;

namespace IMS.Service
{
    public interface ICustomerService
    {
        Task CreateAsync(CustomerViewModel customer);
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer> GetById(long id);
        Task<CustomerViewModel> CustomerDetails(long id);
        Task UpdateAsync(long id, CustomerViewModel customer);
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
                throw new InvalidNameException("sorry! your input field is empty.");
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
                return await _customerDao.Load();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Customer> GetById(long id)
        {
            try
            {
                var individualCustomer = await _customerDao.Get(id);
                if (individualCustomer == null)
                {
                    throw new Exception($"The Customer with the id {id} is not found");
                }
                return individualCustomer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CreateAsync(CustomerViewModel customerViewModel)
        {
            var customerMainEntity = new Customer();
            try
            {
                customerMainEntity.CustomerName = customerViewModel.CustomerName;
                customerMainEntity.CustomerNumber = customerViewModel.CustomerNumber;
                customerMainEntity.EmailAddress = customerViewModel.EmailAddress;
                customerMainEntity.CustomerAddress = customerViewModel.CustomerAddress;
                customerMainEntity.CreatedBy = customerViewModel.CreatedBy;
                customerMainEntity.CreatedDate = DateTime.Now;
                customerMainEntity.ModifyBy = customerViewModel.ModifyBy;
                customerMainEntity.ModifyDate = DateTime.Now;

                await _customerDao.Create(customerMainEntity);  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateAsync(long id, CustomerViewModel customerViewModel)
        {
            try
            {
                var individualCustomer = await _customerDao.Get(id);

                if (individualCustomer != null)
                {

                        individualCustomer.CustomerName = customerViewModel.CustomerName;
                        individualCustomer.CustomerNumber = customerViewModel.CustomerNumber;
                        individualCustomer.EmailAddress = customerViewModel.EmailAddress;
                        individualCustomer.CustomerAddress = customerViewModel.CustomerAddress;
                        individualCustomer.ModifyBy = customerViewModel.ModifyBy;
                        individualCustomer.ModifyDate = DateTime.Now;

                        await _customerDao.Update(individualCustomer);
                }
                else
                {
                    throw new Exception("Customer Not Found!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteAsync(long id)
        {
            try
            {
                var individualCustomerDelete = _customerDao.Get(id);
                if (individualCustomerDelete != null)
                {
                    await _customerDao.Delete(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CustomerViewModel> CustomerDetails(long id)
        {
            var customerDetails = new CustomerViewModel();

            try
            {
                var customer = await _customerDao.Get(id);
                if (customer != null)
                {
                    customerDetails.Id = customer.Id;
                    customerDetails.CustomerName = customer.CustomerName;
                    customerDetails.CustomerNumber = customer.CustomerNumber;
                    customerDetails.EmailAddress = customer.EmailAddress;
                    customerDetails.CustomerAddress = customer.CustomerAddress;
                    customerDetails.CreatedBy = customer.CreatedBy;
                    customerDetails.CreatedDate = customer.CreatedDate;
                    customerDetails.ModifyBy = customer.ModifyBy;
                    customerDetails.ModifyDate = customer.ModifyDate;
                }
                else
                {
                    throw new Exception("No customer is found!");
                }
                return customerDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
