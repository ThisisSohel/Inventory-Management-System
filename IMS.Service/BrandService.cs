using IMS.CustomException;
using IMS.DAO;
using IMS.Entity.Entities;
using log4net;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ISession = NHibernate.ISession;
using IMS.Entity.EntityViewModels;


namespace IMS.Service
{
    public interface IBrandService
    {
        Task CreateBrandService(BrandViewModel brandViewModel);
        Task<IEnumerable<Brand>> GetAll();
        Task<Brand> GetById(long id);
        Task<BrandViewModel> BrandDetailsService(long  id);
        Task Update(long id, BrandViewModel brand);
        Task DeleteAsync(long id);
    }

    public class BrandService : IBrandService
    {
        private readonly IBrandDao _brandDao;
        private readonly ISession _session;
        private readonly ISessionFactory _sessionFactory;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(BrandService));
        public BrandService(IBrandDao brandDao)
        {
            _brandDao = brandDao;
        }
        public BrandService()
        {
            _sessionFactory = NHibernateConfig.GetSession();
            _session = _sessionFactory.OpenSession();
            _brandDao = new BrandDao(_session);
        }

        private void BrandValidator(Brand brandToValidate)
        {
            if (String.IsNullOrWhiteSpace(brandToValidate.BrandName))
            {
                throw new InvalidNameException("sorry! only white space is not allowed");
            }
            if (brandToValidate.BrandName.Length == 0)
            {
                //ModelState.AddModelError("sorry! your input feild is empty.");
                throw new InvalidNameException("sorry! your input feild is empty.");
            }
            if (brandToValidate.BrandName.Trim().Length < 3 || brandToValidate.BrandName.Trim().Length > 20)
            {
                throw new InvalidNameException("Sorry! You have to input your brand name more than 3 character and less than 20 characters");
            }
        }

        public async Task<IEnumerable<Brand>> GetAll()
        {
            try
            {
                return await _brandDao.Load();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Brand> GetById(long id)
        {
            try
            {
                var brand = await _brandDao.Get(id);
                if (brand == null)
                {
                    throw new ObjectNotFoundException(brand, "Brand is not found");
                }
                return brand;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Update(long id, BrandViewModel brand)
        {
            try
            {
                var valueForUpdate = await _brandDao.Get(brand.Id);
                if (valueForUpdate != null)
                {
                        valueForUpdate.BrandName = brand.BrandName;
                        valueForUpdate.ModifyBy = brand.ModifyBy;
                        valueForUpdate.ModifyDate = DateTime.Now;
                        await _brandDao.BrandUpdate(valueForUpdate);                  
                }
                else
                {
                    throw new ObjectNotFoundException(valueForUpdate, "brand");
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
                var individualBrand = await _brandDao.Get(id);
                if (individualBrand != null)
                {
                    await _brandDao.BrandDelete(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CreateBrandService(BrandViewModel brandViewModelEntity)
        {
            var brandMainEntity = new Brand();
            try
            {
                    brandMainEntity.BrandName = brandViewModelEntity.BrandName;
                    brandMainEntity.CreatedBy = 100;
                    brandMainEntity.CreatedDate = DateTime.Now;
                    brandMainEntity.ModifyBy = 100;
                    brandMainEntity.ModifyDate = DateTime.Now;
                    await _brandDao.BrandCreate(brandMainEntity);

            }catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BrandViewModel> BrandDetailsService(long id)
        {
            var brandDetails = new BrandViewModel();
            try
            {
                var brand = await _brandDao.Get(id);

                if (brand != null)
                {
                    brandDetails.Id = brand.Id;
                    brandDetails.BrandName = brand.BrandName;
                    brandDetails.CreatedBy = brand.CreatedBy;
                    brandDetails.CreatedDate = brand.CreatedDate;
                    brandDetails.ModifyBy = brand.ModifyBy;
                    brandDetails.ModifyDate = brand.ModifyDate;
                    
                }
                else
                {
                    throw new Exception("Brand is null!");
                }
                return brandDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
