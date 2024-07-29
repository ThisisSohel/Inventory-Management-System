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
using System.Text.RegularExpressions;

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
                ModelValidatorMethod(customerViewModel);

                customerMainEntity.CustomerName = customerViewModel.CustomerName.Trim();
                customerMainEntity.CustomerNumber = customerViewModel.CustomerNumber.Trim();
                customerMainEntity.EmailAddress = customerViewModel.EmailAddress.Trim();
                customerMainEntity.CustomerAddress = customerViewModel.CustomerAddress;
                customerMainEntity.CreatedBy = customerViewModel.CreatedBy;
                customerMainEntity.CreatedDate = DateTime.Now;
                customerMainEntity.ModifyBy = customerViewModel.ModifyBy;
                customerMainEntity.ModifyDate = DateTime.Now;

                await _customerDao.Create(customerMainEntity);  
            }
            catch(InvalidNameException ex)
            {
                throw ex;
            }
            catch(InvalidExpressionException ex)
            {
                throw ex;
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
                ModelValidatorMethod(customerViewModel);
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
            catch(InvalidExpressionException ex)
            {
                throw ex;
            }
            catch(InvalidNameException ex)
            {
                throw ex;
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

        private void ModelValidatorMethod(CustomerViewModel modelToValidate)
        {
            if (String.IsNullOrWhiteSpace(modelToValidate.CustomerName))
            {
                throw new InvalidNameException("Name can not be null!");
            }
            if (modelToValidate.CustomerName?.Trim().Length < 3 || modelToValidate.CustomerName.Trim().Length > 30)
            {
                throw new InvalidNameException("Name character should be in between 3 to 30!");
            }
            if (!Regex.IsMatch(modelToValidate.CustomerName, @"^[a-zA-Z ]+$"))
            {
                throw new InvalidExpressionException("Name can not contain numbers or special characters! Please input alphabetic characters and space only!");
            }
            if (!Regex.IsMatch(modelToValidate.CustomerNumber, @"^([0-9\(\)\/\+ \-]*)$"))
            {
                throw new InvalidExpressionException("Invalid number! Please input correct format number!");
            }
            if (modelToValidate.CustomerNumber == null)
            {
                throw new InvalidNameException("Number can not be null");
            }
            if (modelToValidate.CustomerNumber.Length < 11 || modelToValidate.CustomerNumber.Length > 18)
            {
                throw new InvalidNameException("Number length should be between 11 to 18");
            }
            if(modelToValidate.EmailAddress?.Trim() == null)
            {
                throw new InvalidNameException("Email can not be null");
            }
            if (!Regex.IsMatch(modelToValidate.EmailAddress, @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$"))
            {
                throw new InvalidExpressionException("Please enter valid email");
            }
            if(modelToValidate.CustomerAddress?.Trim() == null)
            {
                throw new InvalidNameException("Address can not be null");
            }
            if (modelToValidate.CustomerAddress?.Trim().Length > 250 || modelToValidate.CustomerAddress?.Trim().Length < 10)
            {
                throw new InvalidNameException("Address length should be in between 20 to 250");
            }
        }
    }
}
