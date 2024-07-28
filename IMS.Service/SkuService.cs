using IMS.DAO.ProductDao;
using IMS.Entity.Entities;
using log4net;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ISession = NHibernate.ISession;
using IMS.CustomException;
using IMS.Entity.EntityViewModels;
using System.Linq;
using System.Web.UI;

namespace IMS.Service
{
    public interface ISkuService
    {
        Task CreateSkuService(SkuViewModel skuViewModel);
        Task<List<SkuViewModel>> GetAll();
        Task<SkuViewModel> GetById(long id);
        Task<SkuViewModel> SkuDetailsService(long id);
        Task UpdateAsync(long id, SkuViewModel sKU);
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

        public async Task<List<SkuViewModel>> GetAll()
        {
            var skuViewList = new List<SkuViewModel>();
            var skuList = new List<SKU>();
            try
            {
                skuList =  await _skuDao.Load();

                if(skuList.Count > 0)
                {
                    skuViewList = skuList.Select(s => new SkuViewModel
                    {
                        Id = s.Id,
                        SKUsName = s.SKUsName,
                        CreatedBy = s.CreatedBy,
                        CreatedDate = s.CreatedDate,
                        ModifyBy = s.ModifyBy,
                        ModifyDate = s.ModifyDate,
                    }).ToList();
                    return skuViewList;
                }
                else
                {
                    throw new Exception("SKU is not found!");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SkuViewModel> GetById(long id)
        {
            var skuView = new SkuViewModel();
            try
            {
                var individualProductSku = await _skuDao.Get(id);
                if (individualProductSku == null)
                {
                    throw new ObjectNotFoundException(individualProductSku, $"The product SKU with the id {id} is not found");
                }
                else
                {
                    skuView.Id = individualProductSku.Id;
                    skuView.SKUsName = individualProductSku.SKUsName;
                    skuView.CreatedBy = individualProductSku.CreatedBy;
                    skuView.CreatedDate = individualProductSku.CreatedDate;
                    skuView.ModifyBy = individualProductSku.ModifyBy;
                    skuView.ModifyDate = individualProductSku.ModifyDate; 
                    
                    return skuView;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task CreateSkuService(SkuViewModel skuViewModelEntity)
        {
            var skuMainEntity = new SKU();
            try
            {

                skuMainEntity.SKUsName = skuViewModelEntity.SKUsName;
                skuMainEntity.CreatedBy = 100;
                skuMainEntity.CreatedDate = DateTime.Now;
                skuMainEntity.ModifyBy = 100;
                skuMainEntity.ModifyDate = DateTime.Now;

                await _skuDao.SkuCreate(skuMainEntity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateAsync(long id, SkuViewModel sKU)
        {
            try
            {
                var updateSku = await _skuDao.Get(id);

                if (updateSku != null)
                {
                    updateSku.SKUsName = sKU.SKUsName;
                    updateSku.ModifyBy = sKU.ModifyBy;
                    updateSku.ModifyDate = DateTime.Now;
                    await _skuDao.SkuUpdate(updateSku);

                }
                else
                {
                    throw new ObjectNotFoundException(updateSku, "skU");
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
                var individualSkuDelete = _skuDao.Get(id);
                if (individualSkuDelete != null)
                {
                    await _skuDao.SkuDelete(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SkuViewModel> SkuDetailsService(long id)
        {
            var skuDetails = new SkuViewModel();

            try
            {
                var sku = await _skuDao.Get(id);
                if(sku != null)
                {
                    skuDetails.Id = sku.Id;
                    skuDetails.SKUsName = sku.SKUsName;
                    skuDetails.CreatedBy = sku.CreatedBy;
                    skuDetails.CreatedDate = sku.CreatedDate;
                    skuDetails.ModifyBy = sku.ModifyBy;
                    skuDetails.ModifyDate = sku.ModifyDate;
                }
                else
                {
                    throw new Exception("Brand is null!");
                }
                return skuDetails;
            }catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
