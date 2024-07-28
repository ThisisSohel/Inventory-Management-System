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
using System.Linq;

namespace IMS.Service
{
    public interface ICustomerService
    {
        Task CreateAsync(CustomerViewModel customer);
        Task<List<CustomerViewModel>> GetAllAsync();
        Task<CustomerViewModel> GetById(long id);
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

        public async Task<List<CustomerViewModel>> GetAllAsync()
        {
            var customer = new List<Customer>();
            var customerView = new List<CustomerViewModel>();

            try
            {
                customer =  await _customerDao.Load();

                customerView = customer.Select(c => new CustomerViewModel
                {
                    Id = c.Id,
                    CustomerName = c.CustomerName,
                    CustomerNumber = c.CustomerNumber,
                    EmailAddress = c.EmailAddress,
                    CustomerAddress = c.CustomerAddress,
                    CreatedBy = c.CreatedBy,
                    CreatedDate = c.CreatedDate,
                    ModifyBy = c.ModifyBy,
                }).ToList();
                return customerView;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<CustomerViewModel> GetById(long id)
        {    
            var customerView = new CustomerViewModel();
            try
            {
                var individualCustomer = await _customerDao.Get(id);
                if (individualCustomer == null)
                {
                    throw new Exception($"The Customer with the id {id} is not found");
                }

                customerView.Id = individualCustomer.Id;
                customerView.CustomerName = individualCustomer.CustomerName;
                customerView.CustomerNumber = individualCustomer.CustomerNumber;
                customerView.EmailAddress = individualCustomer.EmailAddress;
                customerView.CustomerAddress = individualCustomer.CustomerAddress;
                customerView.CreatedBy = individualCustomer.CreatedBy;
                customerView.CreatedDate = individualCustomer.CreatedDate;
                customerView.ModifyBy = individualCustomer.ModifyBy;    
                customerView.ModifyDate = individualCustomer.ModifyDate;

                return customerView;
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
                var individualCustomerUpdate = await _customerDao.Get(id);

                if (individualCustomerUpdate != null)
                {

                        individualCustomerUpdate.CustomerName = customerViewModel.CustomerName;
                        individualCustomerUpdate.CustomerNumber = customerViewModel.CustomerNumber;
                        individualCustomerUpdate.EmailAddress = customerViewModel.EmailAddress;
                        individualCustomerUpdate.CustomerAddress = customerViewModel.CustomerAddress;
                        individualCustomerUpdate.ModifyBy = customerViewModel.ModifyBy;
                        individualCustomerUpdate.ModifyDate = DateTime.Now;

                        await _customerDao.Update(individualCustomerUpdate);
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
