using IMS.DAO.ProductDao;
using IMS.Entity.Entities;
using log4net;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ISession = NHibernate.ISession;
using IMS.CustomException;

namespace IMS.Service
{
    public interface ISkuService
    {
        Task CreateAsync(SKU sKU);
        Task<IEnumerable<SKU>> GetAllAsync();
        Task<SKU> GetById(long id);
        Task UpdateAsync(long id, SKU sKU);
        Task DeleteAsync(long id);
    }
    public class SkuService : ISkuService
    {
        private readonly ISkuDao _skuDao;
        private readonly ISession _session;
        private readonly ISessionFactory _sessionFactory;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ProductTypeService));

        public SkuService(ISkuDao skuDao)
        {
            _skuDao = skuDao;
        }
        public SkuService()
        {
            _sessionFactory = NHibernateConfig.GetSession();
            _session = _sessionFactory.OpenSession();
            _skuDao = new SKUDao(_session);
        }

        private void SkuValidator(SKU sKuToValidate)
        {
            if (sKuToValidate.SKUsName.Trim().Length == 0)
            {
                throw new InvalidNameException("sorry! your input feild is empty.");
            }
            if (String.IsNullOrWhiteSpace(sKuToValidate.SKUsName))
            {
                throw new InvalidNameException("sorry! only white space is not allowed");
            }
            if (sKuToValidate.SKUsName.Trim().Length < 1 || sKuToValidate.SKUsName.Trim().Length > 4)
            {
                throw new InvalidNameException("Sorry! You have to input your name more than 5 character and less than 60 characters");
            }
        }
        public async Task<IEnumerable<SKU>> GetAllAsync()
        {
            try
            {
                return await _skuDao.GetAll();

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw new InvalidNameException(ex.Message);
            }
        }
        public async Task<SKU> GetById(long id)
        {
            try
            {
                var individualProductSku = await _skuDao.GetById(id);
                if (individualProductSku == null)
                {
                    throw new ObjectNotFoundException(individualProductSku, $"The product SKU with the id {id} is not found");
                }
                return individualProductSku;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw new InvalidNameException(ex.Message);
            }
        }
        public async Task CreateAsync(SKU sKU)
        {
            try
            {
                SkuValidator(sKU);
                var newSku = new SKU
                {
                    SKUsName = sKU.SKUsName,
                    CreatedBy = 100.ToString(),
                    CreatedDate = DateTime.Now,
                    ModifyBy = 100.ToString(),
                    ModifyDate = DateTime.Now,
                };
                using (var transaction = _session.BeginTransaction())
                {
                    await _skuDao.Create(newSku);
                    await transaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateAsync(long id, SKU sKU)
        {
            try
            {
                var individualProductSku = await _skuDao.GetById(id);
                if (individualProductSku != null)
                {
                    using (var transaction = _session.BeginTransaction())
                    {
                        individualProductSku.SKUsName = sKU.SKUsName;
                        //individualProductSku.ModifyBy = sKU.ModifyBy;
                        individualProductSku.ModifyDate = DateTime.Now;
                        await _skuDao.Update(individualProductSku);
                        await transaction.CommitAsync();
                    }
                }
                else
                {
                    throw new ObjectNotFoundException(individualProductSku, "ProductSKU Not Found!");
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
                var individualTypeDelete = _skuDao.GetById(id);
                if (individualTypeDelete != null)
                {
                    await _skuDao.DeleteById(id);
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
