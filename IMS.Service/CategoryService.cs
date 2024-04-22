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

namespace IMS.Service
{
    public interface ICategoryService
    {
        Task CreateAsync (ProductCategory category);
        Task<IEnumerable<ProductCategory>> GetAllAsync();
        Task<ProductCategory> GetById(long id);
        Task UpdateAsync (ProductCategory category);
        Task DeleteAsync (long id);

    }
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryDao _categoryDao;
        private readonly ISession session;
        private readonly ISessionFactory _sessionFactory;
        private readonly ISession _session;
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
        protected void BrandValidator(ProductCategory category)
        {
            if (category.CategoryName.Trim().Length == 0)
            {
                throw new InvalidNameException("sorry! your input feild is empty.");
            }
            if (String.IsNullOrWhiteSpace(category.CategoryName))
            {
                throw new InvalidNameException("sorry! only white space is not allowed");
            }
            if (category.CategoryName.Trim().Length < 5 || category.CategoryName.Trim().Length > 60)
            {
                throw new InvalidNameException("Sorry! You have to input your name more than 5 character and less than 60 characters");
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
                await _categoryDao.CreateCategory(category);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateAsync (ProductCategory category)
        {
            try
            {
                await _categoryDao.Update(category);
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
                await _categoryDao.DeleteById(id);
            }catch(Exception ex) 
            { 
                _logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }
    }
}
