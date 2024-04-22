using IMS.DAO;
using IMS.Entity.Entities;
using log4net;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using ISession = NHibernate.ISession;

namespace IMS.Service
{
    public interface IBrandService
    {
        Task CreateAsync(Brand brand);
        Task<IEnumerable<Brand>> GetAll();
        Task<Brand> GetById(long id);
        Task Update(Brand brand);
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

        protected void BrandValidator(Brand brand)
        {
            if (brand.BrandName.Trim().Length == 0)
            {
                throw new InvalidNameException("sorry! your input feild is empty.");
            }
            if (String.IsNullOrWhiteSpace(brand.BrandName))
            {
                throw new InvalidNameException("sorry! only white space is not allowed");
            }
            if (brand.BrandName.Trim().Length < 5 || brand.BrandName.Trim().Length > 60)
            {
                throw new InvalidNameException("Sorry! You have to input your name more than 5 character and less than 60 characters");
            }
        }

        public async Task<IEnumerable<Brand>> GetAll()
        {
            try
            {
                return await _brandDao.GetAll();
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
                var brand = await _brandDao.GetById(id);
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
                var brandDao = new Brand
                {
                    BrandName = brand.BrandName,
                    CreatedBy = 1000.ToString(),
                    CreatedDate = DateTime.Now,
                    ModifyBy = 1000.ToString(),
                    ModifyDate = DateTime.Now,
                };
                using (var transaction = _session.BeginTransaction())
                {
                    await _brandDao.CreateBrand(brandDao);
                    await transaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                throw;
            }
        }

        public async Task Update(Brand brand)
        {
            try
            {
                var valueForUpdate = await _brandDao.GetById(brand.Id);
                if (valueForUpdate != null)
                {
                    using (var transaction = _session.BeginTransaction())
                    {
                        valueForUpdate.BrandName = brand.BrandName;
                        valueForUpdate.CreatedBy = 10000.ToString();
                        valueForUpdate.CreatedDate = DateTime.Now;
                        valueForUpdate.ModifyBy = 1000.ToString();
                        valueForUpdate.ModifyDate = DateTime.Now;
                        await _brandDao.CreateBrand(valueForUpdate);
                        await transaction.CommitAsync();
                    }
                }
                else
                {
                    throw new ObjectNotFoundException(valueForUpdate, "brand");                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message,ex);
                throw;
            }
        }

        public async Task DeleteAsync(long id)
        {
            try
            {
                var individualBrand = await _brandDao.GetById(id);
                if (individualBrand != null)
                {
                    await _brandDao.DeleteById(id);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                throw;
            }
        }
    }

    [Serializable]
    internal class InvalidNameException : Exception
    {
        public InvalidNameException()
        {
        }

        public InvalidNameException(string message) : base(message)
        {
        }

        public InvalidNameException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidNameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
