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
using IMS.Entity.EntityViewModels;
using System.ServiceModel.Channels;
using ISession = NHibernate.ISession;

namespace IMS.Service
{
    public interface ISupplierService
    {
        Task CreateAsync(SupplierViewModel supplier);
        Task<List<SupplierViewModel>> GetAllAsync();
        Task<SupplierViewModel> GetById(long id);
        Task<SupplierViewModel> Details(long id);
        Task UpdateAsync(long id, SupplierViewModel supplier);
        Task DeleteAsync(long id);
    }
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierDao _supplierDao;
        private readonly ISession _session;
        private readonly ISessionFactory _sessionFactory;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(SupplierService));

        public SupplierService(ISupplierDao supplierDao)
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
                throw new InvalidNameException("sorry! your input feiled is empty.");
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

        public async Task<List<SupplierViewModel>> GetAllAsync()
        {
            var supplier = new List<Supplier>();
            var supplierView = new List<SupplierViewModel>();

            try
            {
                supplier = await _supplierDao.Load(); 

                supplierView = supplier.Select(s => new SupplierViewModel
                {
                    Id = s.Id,
                    SupplierName = s.SupplierName,
                    SupplierNumber = s.SupplierNumber,
                    EmailAddress = s.EmailAddress,
                    SupplierAddress = s.SupplierAddress,
                    CreatedBy = s.CreatedBy,
                    CreatedDate = s.CreatedDate,
                    ModifyBy = s.ModifyBy,
                    ModifyDate = s.ModifyDate
                }).ToList();
                return supplierView;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SupplierViewModel> GetById(long id)
        {
            var supplierView = new SupplierViewModel();
            try
            {
                var individualSupplierFind = await _supplierDao.Get(id);

                if (individualSupplierFind == null)
                {
                    throw new Exception($"The Supplier with the id {id} is not found");
                }
                else
                {
                    supplierView.Id = individualSupplierFind.Id;
                    supplierView.SupplierName = individualSupplierFind.SupplierName;
                    supplierView.SupplierNumber = individualSupplierFind.SupplierNumber;
                    supplierView.EmailAddress = individualSupplierFind.EmailAddress;
                    supplierView.SupplierAddress = individualSupplierFind.SupplierAddress;
                    supplierView.CreatedBy = individualSupplierFind.CreatedBy;
                    supplierView.CreatedDate = individualSupplierFind.CreatedDate;
                    supplierView.ModifyBy = individualSupplierFind.ModifyBy;
                    supplierView.ModifyDate = individualSupplierFind.ModifyDate;

                    return supplierView;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task CreateAsync(SupplierViewModel supplierViewModel)
        {
            var supplierMainEntity = new Supplier();
            try
            {
                supplierMainEntity.SupplierName = supplierViewModel.SupplierName;
                supplierMainEntity.SupplierNumber = supplierViewModel.SupplierNumber;
                supplierMainEntity.EmailAddress = supplierViewModel.EmailAddress;
                supplierMainEntity.SupplierAddress = supplierViewModel.SupplierAddress;
                supplierMainEntity.CreatedBy = 100;
                supplierMainEntity.CreatedDate = DateTime.Now;
                supplierMainEntity.ModifyBy = 100;
                supplierMainEntity.ModifyDate = DateTime.Now;

                await _supplierDao.Create(supplierMainEntity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateAsync(long id, SupplierViewModel supplierViewModel)
        {
            try
            {
                var individualSupplierUpdate = await _supplierDao.Get(id);

                if (individualSupplierUpdate != null)
                {
                    individualSupplierUpdate.SupplierName = supplierViewModel.SupplierName;
                    individualSupplierUpdate.SupplierNumber = supplierViewModel.SupplierNumber;
                    individualSupplierUpdate.EmailAddress = supplierViewModel.EmailAddress;
                    individualSupplierUpdate.SupplierAddress = supplierViewModel.SupplierAddress;
                    //individualSupplierUpdate.ModifyBy = supplier.ModifyBy;
                    individualSupplierUpdate.ModifyDate = DateTime.Now;
                    await _supplierDao.Update(individualSupplierUpdate);
                }
                else
                {
                    throw new Exception("Supplier Not Found!");
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
                var individualTypeDelete = _supplierDao.Get(id);
                if (individualTypeDelete != null)
                {
                    await _supplierDao.Delete(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SupplierViewModel> Details (long id)
        {
            var supplierDetails = new SupplierViewModel();

            try
            {
                var supplier = await _supplierDao.Get(id);

                if(supplier != null)
                {
                    supplierDetails.Id = supplier.Id;
                    supplierDetails.SupplierName = supplier.SupplierName;
                    supplierDetails.SupplierNumber = supplier.SupplierNumber;
                    supplierDetails.EmailAddress = supplier.EmailAddress;   
                    supplierDetails.SupplierAddress = supplier.SupplierAddress;
                    supplierDetails.CreatedBy = supplier.CreatedBy;
                    supplierDetails.CreatedDate = supplier.CreatedDate;
                    supplierDetails.ModifyBy = supplier.ModifyBy;
                    supplierDetails.ModifyDate = supplier.ModifyDate;
                }
                else
                {
                    throw new Exception("No supplier is found!");
                }
                return supplierDetails;
            }catch(Exception ex)
            {
                throw ex;
            }
        }

    }
}
