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

        private void TypeValidator(ProductType productTypeToValidate)
        {
            if (productTypeToValidate.TypeName.Trim().Length == 0)
            {
                throw new InvalidNameException("sorry! your input feild is empty.");
            }
            if (String.IsNullOrWhiteSpace(productTypeToValidate.TypeName))
            {
                throw new InvalidNameException("sorry! only white space is not allowed");
            }
            if (productTypeToValidate.TypeName.Trim().Length < 5 || productTypeToValidate.TypeName.Trim().Length > 60)
            {
                throw new InvalidNameException("Sorry! You have to input your name more than 5 character and less than 60 characters");
            }
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

        public async Task CreateAsync(CategoryTypeCreateViewModel categoryTypeCreateView )
        {
            try
            {
                var productTypeMainEntity = new ProductType();

                productTypeMainEntity.TypeName = categoryTypeCreateView.TypeName;
                productTypeMainEntity.CreatedBy = categoryTypeCreateView.CreatedBy;
                productTypeMainEntity.CreatedDate = DateTime.Now;
                productTypeMainEntity.ModifyBy = categoryTypeCreateView.ModifyBy;
                productTypeMainEntity.ModifyDate = DateTime.Now;

                productTypeMainEntity.ProductCategory = new ProductCategory()
                {
                    Id = categoryTypeCreateView.CategoryId
                };

                using (var transaction = _session.BeginTransaction())
                {
                    try
                    {
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
                if (individualProductTypeUpdate == null)
                {

                }
                else
                {
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
                var individualTypeDelete = _productTypeDao.GetById(id);
                if (individualTypeDelete != null)
                {
                    await _productTypeDao.DeleteById(id);
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
