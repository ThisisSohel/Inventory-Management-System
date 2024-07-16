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
        Task CreateAsync(Brand brand);
        Task CreateBrandService(BrandViewModel brandViewModel);
        Task<IEnumerable<Brand>> GetAll();
        Task<Brand> GetById(long id);
        Task Update(long id, Brand brand);
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
                _logger.Error(ex.Message);
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
                _logger.Error(ex.Message, ex);
                throw;
            }
        }

        public async Task CreateAsync(Brand brand)
        {
            try
            {

                BrandValidator(brand);
                var brandDao = new Brand
                {
                    BrandName = brand.BrandName,
                    CreatedBy = brand.CreatedBy,
                    CreatedDate = DateTime.Now,
                    ModifyBy = brand.CreatedBy,
                    ModifyDate = DateTime.Now,
                };
                using (var transaction = _session.BeginTransaction())
                {
                    await _brandDao.BrandCreate(brandDao);
                    await transaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                throw;
            }
        }

        public async Task Update(long id, Brand brand)
        {
            try
            {
                var valueForUpdate = await _brandDao.Get(brand.Id);
                if (valueForUpdate != null)
                {
                    using (var transaction = _session.BeginTransaction())
                    {
                        valueForUpdate.BrandName = brand.BrandName;
                        valueForUpdate.ModifyBy = brand.ModifyBy;
                        valueForUpdate.ModifyDate = DateTime.Now;
                        await _brandDao.BrandUpdate(valueForUpdate);
                        await transaction.CommitAsync();
                    }
                }
                else
                {
                    throw new ObjectNotFoundException(valueForUpdate, "brand");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                throw;
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
                _logger.Error(ex.Message, ex);
                throw;
            }
        }

        public async Task CreateBrandService(BrandViewModel brandViewModel)
        {
            var brandEntity = new Brand();

            try
            {
                using(var transaction = _session.BeginTransaction())
                {
                    brandEntity.BrandName = brandViewModel.BrandName;
                    brandEntity.CreatedBy = 100;
                    brandEntity.CreatedDate = DateTime.Now;
                    brandEntity.ModifyBy = 100;
                    brandEntity.ModifyDate = DateTime.Now;
                    await _brandDao.BrandCreate(brandEntity);
                    transaction.Commit();
                }

            }catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }

    //[Serializable]
    //public class InvalidNameException : Exception
    //{
    //    public InvalidNameException()
    //    {
    //    }

    //    public InvalidNameException(string message) : base(message)
    //    {
    //    }

    //    public InvalidNameException(string message, Exception innerException) : base(message, innerException)
    //    {
    //    }

    //    protected InvalidNameException(SerializationInfo info, StreamingContext context) : base(info, context)
    //    {
    //    }
    //}
}
