using IMS.DAO.ProductDao;
using log4net;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using ISession = NHibernate.ISession;
using IMS.Entity.Entities;
using IMS.CustomException;
using IMS.Entity.EntityViewModels;
using System.Text.RegularExpressions;

namespace IMS.Service
{
    public interface IProductTypeService
    {
        Task CreateAsync(CategoryTypeCreateViewModel productType);
        Task<List<ProductTypeViewModel>> GetAllAsync();
        Task<ProductTypeViewModel> GetByIdAsync(long id);
        Task UpdateAsync(long id, ProductTypeUpdateViewModel productType);
        Task DeleteAsync(long id);
    }
    public class ProductTypeService : IProductTypeService
    {
        private readonly IProductTypeDao _productTypeDao;
        private readonly ISession _session;
        private readonly ISessionFactory _sessionFactory;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ProductTypeService));
        public ProductTypeService(IProductTypeDao productTypeDao)
        {
            _productTypeDao = productTypeDao;
        }

        public ProductTypeService()
        {
            _sessionFactory = NHibernateConfig.GetSession();
            _session = _sessionFactory.OpenSession();
            _productTypeDao = new ProductTypeDao(_session);
        }

        public async Task<List<ProductTypeViewModel>> GetAllAsync()
        {
            try
            {
                var productTypeViewList = new List<ProductTypeViewModel>();
                var productType = await _productTypeDao.GetAll();

                if(productType.Count > 0)
                {
                    productTypeViewList = productType.Select(t => new ProductTypeViewModel
                    {
                        Id = t.Id,
                        TypeName = t.TypeName,
                        CreatedBy = t.CreatedBy,
                        CreatedDate = t.CreatedDate,
                        ModifyBy = t.ModifyBy,
                        ModifyDate = t.ModifyDate,
                    }).ToList();
                }

                return productTypeViewList;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw new InvalidNameException(ex.Message);
            }
        }

        public async Task<ProductTypeViewModel> GetByIdAsync(long id)
        {
            try
            {
                var productTypeViewModel = new ProductTypeViewModel();
                var individualProductType = await _productTypeDao.GetById(id);

                if (individualProductType == null)
                {
                    throw new Exception("The type is not found");
                }
                else
                {
                    productTypeViewModel.Id = individualProductType.Id;
                    productTypeViewModel.TypeName = individualProductType.TypeName;
                    productTypeViewModel.CreatedBy = individualProductType.CreatedBy;
                    productTypeViewModel.CreatedDate = individualProductType.CreatedDate;
                    productTypeViewModel.ModifyBy = individualProductType.ModifyBy;
                    productTypeViewModel.ModifyDate = individualProductType.ModifyDate; 
                }
                return productTypeViewModel;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task CreateAsync(CategoryTypeCreateViewModel categoryTypeCreateViewModel )
        {
            try
            {
                var productTypeMainEntity = new ProductType();
                var productTypeAll = await _productTypeDao.GetAll();

                ModelValidatorMethod(categoryTypeCreateViewModel);

                if (productTypeAll.Count > 0)
                {
                    foreach ( var productType in productTypeAll)
                    {
                        if (categoryTypeCreateViewModel.TypeName.Contains(productType.TypeName))
                        {
                            throw new DuplicateValueException("Product type can not be duplicate!");
                        }
                    }
                }



                using (var transaction = _session.BeginTransaction())
                {
                    try
                    {
                        productTypeMainEntity.TypeName = categoryTypeCreateViewModel.TypeName;
                        productTypeMainEntity.CreatedBy = categoryTypeCreateViewModel.CreatedBy;
                        productTypeMainEntity.CreatedDate = DateTime.Now;
                        productTypeMainEntity.ModifyBy = categoryTypeCreateViewModel.ModifyBy;
                        productTypeMainEntity.ModifyDate = DateTime.Now;

                        productTypeMainEntity.ProductCategory = new ProductCategory()
                        {
                            Id = categoryTypeCreateViewModel.CategoryId
                        };

                        await _productTypeDao.Create(productTypeMainEntity);
                        await transaction.CommitAsync();
                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _logger.Error(ex);
                        throw ex;
                    }

                }
            }
            catch (InvalidNameException ex)
            {
                throw ex;
            }
            catch (DuplicateValueException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                throw ex;
            }
        }

        public async Task UpdateAsync(long id, ProductTypeUpdateViewModel productTypeUpdateViewModel)
        {
            try
            {
                var individualProductTypeUpdate = await _productTypeDao.GetById(id);

                ModelValidatorMethod(productTypeUpdateViewModel);

                var productTypeAll = await _productTypeDao.GetAll();
                //var productTypeDuplicateCount = productTypeAll.Count(p => p.TypeName == productTypeUpdateViewModel.TypeName );

                foreach (var item in productTypeAll)
                {
                    if (item.TypeName == productTypeUpdateViewModel.TypeName && item.ProductCategory.Id != productTypeUpdateViewModel.CategoryId)
                    {
                        throw new DuplicateValueException("Product type can not be duplicate!");
                    }
                }


                using (var transaction = _session.BeginTransaction())
                {
                    individualProductTypeUpdate.TypeName = productTypeUpdateViewModel.TypeName;
                    individualProductTypeUpdate.ModifyBy = productTypeUpdateViewModel.ModifyBy;
                    individualProductTypeUpdate.ModifyDate = DateTime.Now;

                    individualProductTypeUpdate.ProductCategory = new ProductCategory()
                    {
                        Id = productTypeUpdateViewModel.CategoryId
                    };

                    await _productTypeDao.Update(individualProductTypeUpdate);
                    await transaction.CommitAsync();
                }
                
            }
            catch(DuplicateValueException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw ex;
            }
        }

        public async Task DeleteAsync(long id)
        {
            try
            {
                using (var transaction = _session.BeginTransaction())
                {
                    try
                    {
                        var individualTypeDelete = _productTypeDao.GetById(id);
                        if (individualTypeDelete != null)
                        {
                            await _productTypeDao.DeleteById(id);
                            await transaction.CommitAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw ex;
            }
        }

        private void ModelValidatorMethod(CategoryTypeCreateViewModel modelToValidate)
        {
            if (string.IsNullOrWhiteSpace(modelToValidate.TypeName))
            {
                throw new InvalidNameException("Name can not be null!");
            }
            if (modelToValidate.TypeName?.Trim().Length < 3 || modelToValidate.TypeName?.Trim().Length > 30)
            {
                throw new InvalidNameException("Name character should be in between 3 to 30!");
            }
            if (!Regex.IsMatch(modelToValidate.TypeName, @"^[a-zA-Z ]+$"))
            {
                throw new InvalidNameException("Name can not contain numbers or special characters! Please input alphabetic characters and space only!");
            }
            if (modelToValidate.CategoryId == 0)
            {
                throw new InvalidNameException("Please select a category!");
            }
        }

        private void ModelValidatorMethod(ProductTypeUpdateViewModel modelToValidate)
        {
            if (string.IsNullOrWhiteSpace(modelToValidate.TypeName))
            {
                throw new InvalidNameException("Name can not be null!");
            }
            if (modelToValidate.TypeName?.Trim().Length < 3 || modelToValidate.TypeName?.Trim().Length > 30)
            {
                throw new InvalidNameException("Name character should be in between 3 to 30!");
            }
            if (!Regex.IsMatch(modelToValidate.TypeName, @"^[a-zA-Z ]+$"))
            {
                throw new InvalidNameException("Name can not contain numbers or special characters! Please input alphabetic characters and space only!");
            }
            if (modelToValidate.CategoryId == 0)
            {
                throw new InvalidNameException("Please select a category!");
            }
        }
    }

}
