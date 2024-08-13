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
        Task CreateAsync(ProductTypeViewModel productType);
        Task<List<ProductTypeViewModel>> GetAllAsync();
        Task<ProductType> GetById(long id);
        Task UpdateAsync(long id, ProductType productType);
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

        public async Task<ProductType> GetById(long id)
        {
            try
            {
                var individualProductType = await _productTypeDao.GetById(id);
                if (individualProductType == null)
                {
                    throw new ObjectNotFoundException(individualProductType, $"The product type with the id {id} is not found");
                }
                return individualProductType;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw new InvalidNameException(ex.Message);
            }
        }

        public async Task CreateAsync(ProductTypeViewModel productTypeViewModel)
        {
            try
            {
                var productTypeMainEntity = new ProductType();

                productTypeMainEntity.TypeName = productTypeViewModel.TypeName;
                productTypeMainEntity.CreatedBy = productTypeViewModel.CreatedBy;
                productTypeMainEntity.CreatedDate = DateTime.Now;
                productTypeMainEntity.ModifyBy = productTypeViewModel.ModifyBy;
                productTypeMainEntity.ModifyDate = DateTime.Now;

                //foreach (var categoryView in productTypeViewModel.Category)
                //{
                //    var category = new ProductCategory
                //    {

                //    };
                //}

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
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateAsync(long id, ProductType productType)
        {
            try
            {
                var individualProductUpdate = await _productTypeDao.GetById(id);
                if (individualProductUpdate != null)
                {
                    using (var transaction = _session.BeginTransaction())
                    {
                        individualProductUpdate.TypeName = productType.TypeName;
                        //individualProductUpdate.ModifyBy = productType.ModifyBy;
                        individualProductUpdate.ModifyDate = DateTime.Now;
                        await _productTypeDao.Update(individualProductUpdate);
                        await transaction.CommitAsync();
                    }
                }
                else
                {
                    throw new ObjectNotFoundException(individualProductUpdate, "ProductType Not Found!");
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
