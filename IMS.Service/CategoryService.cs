using IMS.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.DAO;
using IMS.DAO.ProductDao;
using System.ServiceModel.Channels;
using log4net;
using NHibernate;
using ISession = NHibernate.ISession;
using System.Runtime.Serialization;
using IMS.CustomException;

namespace IMS.Service
{
    public interface ICategoryService
    {
        Task CreateAsync (ProductCategory category);
        Task<IEnumerable<ProductCategory>> GetAllAsync();
        Task<ProductCategory> GetById(long id);
        Task UpdateAsync (long id, ProductCategory category);
        Task DeleteAsync (long id);

    }
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryDao _categoryDao;
        private readonly ISession _session;
        private readonly ISessionFactory _sessionFactory;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(BrandService));
        public CategoryService(ICategoryDao categoryDao) 
        { 
            _categoryDao = categoryDao;
        }
        
        public CategoryService()
        {
            _sessionFactory = NHibernateConfig.GetSession();
            _session = _sessionFactory.OpenSession();
            _categoryDao = new CategoryDao(_session);
        }
        private void CategoryValidator(ProductCategory categoryToValidate)
        {
            if (categoryToValidate.CategoryName.Trim().Length == 0)
            {
                throw new InvalidNameException("sorry! your input feild is empty.");
            }
            if (String.IsNullOrWhiteSpace(categoryToValidate.CategoryName))
            {
                throw new InvalidNameException("sorry! only white space is not allowed");
            }
            if (categoryToValidate.CategoryName.Trim().Length < 3 || categoryToValidate.CategoryName.Trim().Length > 20)
            {
                throw new InvalidNameException("Sorry! You have to input your name more than 3 character and less than 20 characters");
            }
        }

        public async Task<IEnumerable<ProductCategory>> GetAllAsync()
        {
            try
            {
                return await _categoryDao.GetAll();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProductCategory> GetById(long id)
        {
            try
            {
                var individualCategory = await _categoryDao.GetById(id);
                if (individualCategory == null)
                {
                    throw new ObjectNotFoundException(individualCategory, $"The category with the id {id} is not found");
                }
                return individualCategory;
            }catch (Exception ex)
            {
                _logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }
        public async Task CreateAsync (ProductCategory category)
        {
            try
            {
                CategoryValidator(category);
                var newCategory = new ProductCategory
                {
                    CategoryName = category.CategoryName,
                    CategoryDescription = category.CategoryDescription,
                    CreatedBy = 100.ToString(),
                    CreatedDate = DateTime.Now,
                    ModifyBy = 100.ToString(),
                    ModifyDate = DateTime.Now,
                };
                using (var transaction = _session.BeginTransaction())
                {
                    await _categoryDao.CreateCategory(newCategory);
                    await transaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateAsync (long id, ProductCategory category)
        {
            try
            {
                var updateProductCategory = await _categoryDao.GetById(id);
                if (updateProductCategory != null)
                {
                    using (var transaction = _session.BeginTransaction())
                    {
                        updateProductCategory.CategoryName = category.CategoryName;
                        updateProductCategory.CategoryDescription = category.CategoryDescription;
                        //updateProductCategory.ModifyBy = category.ModifyBy;
                        updateProductCategory.ModifyDate = DateTime.Now;
                        await _categoryDao.Update(updateProductCategory);
                        await transaction.CommitAsync();
                    }
                }else
                {
                    throw new ObjectNotFoundException(updateProductCategory, "category");
                }
            }catch (Exception ex)
            {
                _logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }
        public async Task DeleteAsync(long id)
        {
            try
            {
                var individualCategoryDelete = await _categoryDao.GetById(id);
                if (individualCategoryDelete != null)
                {
                    await _categoryDao.DeleteById(id);
                }
            }catch(Exception ex) 
            { 
                _logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }
    }

}
