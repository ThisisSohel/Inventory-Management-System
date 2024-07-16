using IMS.DAO.CustomerDao;
using IMS.DAO.SupplierDao;
using IMS.Entity.Entities;
using log4net;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.CustomException;

namespace IMS.Service
{
    public interface ISupplierService
    {
        Task CreateAsync(Supplier supplier);
        Task<IEnumerable<Supplier>> GetAllAsync();
        Task<Supplier> GetById(long id);
        Task UpdateAsync(long id, Supplier supplier );
        Task DeleteAsync(long id);
    }
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierDao _supplierDao;
        private readonly ISession _session;
        private readonly ISessionFactory _sessionFactory;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(SupplierService));

        public SupplierService( ISupplierDao supplierDao)
        {
            _supplierDao = supplierDao;
        }

        public SupplierService()
        {
            _sessionFactory = NHibernateConfig.GetSession();
            _session = _sessionFactory.OpenSession();
            _supplierDao = new SupplierDao(_session);
        }
        private void SupplierValidator(Supplier supplierToValidate)
        {
            if (supplierToValidate.SupplierName.Trim().Length == 0)
            {
                throw new InvalidNameException("sorry! your input feild is empty.");
            }
            if (String.IsNullOrWhiteSpace(supplierToValidate.SupplierName))
            {
                throw new InvalidNameException("sorry! only white space is not allowed");
            }
            if (supplierToValidate.SupplierName.Trim().Length < 5 || supplierToValidate.SupplierName.Trim().Length > 30)
            {
                throw new InvalidNameException("Sorry! You have to input your name more than 5 character and less than 60 characters");
            }
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            try
            {
                return await _supplierDao.GetAll();

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw new InvalidNameException(ex.Message);
            }
        }
        public async Task<Supplier> GetById(long id)
        {
            try
            {
                var individualSupplierFind = await _supplierDao.GetById(id);
                if (individualSupplierFind == null)
                {
                    throw new ObjectNotFoundException(individualSupplierFind, $"The Supplier with the id {id} is not found");
                }
                return individualSupplierFind;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw new InvalidNameException(ex.Message);
            }
        }
        public async Task CreateAsync(Supplier supplier)
        {
            try
            {
                SupplierValidator(supplier);
                var newSupplier = new Supplier
                {
                    SupplierName = supplier.SupplierName,
                    SupplierNumber = supplier.SupplierNumber,
                    EmailAddress = supplier.EmailAddress,
                    SupplierAddress = supplier.SupplierAddress,
                    CreatedBy = supplier.CreatedBy,
                    CreatedDate = DateTime.Now,
                    ModifyBy = supplier.ModifyBy,
                    ModifyDate = DateTime.Now,
                };
                using (var transaction = _session.BeginTransaction())
                {
                    await _supplierDao.Create(newSupplier);
                    await transaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateAsync(long id, Supplier supplier)
        {
            try
            {
                var individualSupplierUpdate = await _supplierDao.GetById(id);
                if (individualSupplierUpdate != null)
                {
                    using (var transaction = _session.BeginTransaction())
                    {
                        individualSupplierUpdate.SupplierName = supplier.SupplierName;
                        individualSupplierUpdate.SupplierNumber = supplier.SupplierNumber;
                        individualSupplierUpdate.EmailAddress = supplier.EmailAddress;
                        individualSupplierUpdate.SupplierAddress = supplier.SupplierAddress;
                        //individualSupplierUpdate.ModifyBy = supplier.ModifyBy;
                        individualSupplierUpdate.ModifyDate = DateTime.Now;
                        await _supplierDao.Update(individualSupplierUpdate);
                        await transaction.CommitAsync();
                    }
                }
                else
                {
                    throw new ObjectNotFoundException(individualSupplierUpdate, "Supplier Not Found!");
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
                var individualTypeDelete = _supplierDao.GetById(id);
                if (individualTypeDelete != null)
                {
                    await _supplierDao.DeleteById(id);
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
