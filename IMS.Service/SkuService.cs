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
using System.Text.RegularExpressions;
using IMS.Entity.EntityViewModels.SKUViewModel;

namespace IMS.Service
{
    public interface ISkuService
    {
        Task CreateSkuService(SkuCreateViewModel skuViewModel);
        Task<List<SkuViewModel>> GetAll();
        Task<SkuViewModel> GetById(long id);
        Task<SkuViewModel> SkuDetailsService(long id);
        Task UpdateAsync(long id, SkuUpdateViewModel sKU);
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

        public async Task<List<SkuViewModel>> GetAll()
        {
            try
            {
                var skuViewList = new List<SkuViewModel>();
                var skuList = new List<SKU>();

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

            try
            {
                var skuView = new SkuViewModel();
                var individualProductSku = await _skuDao.Get(id);

                if (individualProductSku == null)
                {
                    throw new Exception("The product SKU is not found");
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

        public async Task CreateSkuService(SkuCreateViewModel  skuViewModelEntity)
        {
            try
            {
                var skuMainEntity = new SKU();
                var sku = await _skuDao.Load();

                //ModelValidatorMethod(skuViewModelEntity);

                if (sku.Count != 0)
                {
                    foreach (var item in sku)
                    {
                        if (skuViewModelEntity.SKUsName == item.SKUsName)
                        {
                            throw new DuplicateValueException("SKU name can not be duplicate!");
                        }
                    }
                }



                using (var transaction = _session.BeginTransaction())
                {
                    skuMainEntity.SKUsName = skuViewModelEntity.SKUsName.Trim();
                    skuMainEntity.CreatedBy = skuViewModelEntity.CreatedBy;
                    skuMainEntity.CreatedDate = DateTime.Now;
                    skuMainEntity.ModifyBy = skuViewModelEntity.ModifyBy;
                    skuMainEntity.ModifyDate = DateTime.Now;

                    skuMainEntity.ProductType = new ProductType()
                    {
                        Id = skuViewModelEntity.TypeId
                    };
                    await _skuDao.SkuCreate(skuMainEntity);
                    await transaction.CommitAsync();
                }
            }
            catch (DuplicateValueException ex)
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

        public async Task UpdateAsync(long id, SkuUpdateViewModel skuUpdateViewModel )
        {
            try
            {
                ModelValidatorMethod(skuUpdateViewModel);
                var updateSku = await _skuDao.Get(id);

                var typeAllToCheckDuplicate = await _skuDao.Load();


                foreach (var item in typeAllToCheckDuplicate)
                {
                    if(item.SKUsName == skuUpdateViewModel.SKUsName && item.ProductType.Id != skuUpdateViewModel.TypeId)
                    {
                        throw new DuplicateValueException("SKU can not be duplicate!");
                    }
                }

                using(var transaction = _session.BeginTransaction())
                {
                    updateSku.SKUsName = skuUpdateViewModel.SKUsName;
                    updateSku.ModifyBy = skuUpdateViewModel.ModifyBy;
                    updateSku.ModifyDate = DateTime.Now;

                    updateSku.ProductType = new ProductType
                    {
                        Id = skuUpdateViewModel.TypeId
                    };

                    await _skuDao.SkuUpdate(updateSku);
                    await transaction.CommitAsync();

                }
            }
            catch (DuplicateValueException ex)
            {
                throw ex;
            }
            catch (InvalidNameException ex)
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
            try
            {
                var skuDetails = new SkuViewModel();
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

        private void ModelValidatorMethod(SkuViewModel modelToValidate)
        {
            if (String.IsNullOrWhiteSpace(modelToValidate.SKUsName))
            {
                throw new InvalidNameException("Name can not be null!");
            }
            if (modelToValidate.SKUsName?.Trim().Length < 1 || modelToValidate.SKUsName?.Trim().Length > 4)
            {
                //ModelState.AddModelError("sorry! your input field is empty.");
                throw new InvalidNameException("Name character should be in between 3 to 30!");
            }
            if (!Regex.IsMatch(modelToValidate.SKUsName, @"^[a-zA-Z ]+$"))
            {
                throw new InvalidNameException("Name can not contain numbers or special characters! Please input alphabetic characters and space only!");
            }
        }

        private void ModelValidatorMethod(SkuUpdateViewModel modelToValidate)
        {
            if (String.IsNullOrWhiteSpace(modelToValidate.SKUsName))
            {
                throw new InvalidNameException("Name can not be null!");
            }
            if (modelToValidate.SKUsName?.Trim().Length < 1 || modelToValidate.SKUsName?.Trim().Length > 4)
            {
                //ModelState.AddModelError("sorry! your input field is empty.");
                throw new InvalidNameException("Name character should be in between 3 to 30!");
            }
            if (!Regex.IsMatch(modelToValidate.SKUsName, @"^[a-zA-Z ]+$"))
            {
                throw new InvalidNameException("Name can not contain numbers or special characters! Please input alphabetic characters and space only!");
            }
        }

    }
}
