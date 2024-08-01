﻿using IMS.Entity.Entities;
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
using IMS.Entity.EntityViewModels;
using System.Text.RegularExpressions;

namespace IMS.Service
{
    public interface ICategoryService
    {
        Task CreateAsync (ProductCategoryViewModel productCategoryViewModel);
        Task<List<ProductCategoryViewModel>> LoadAllAsync();
        Task<ProductCategoryViewModel> GetByIdAsync(long id);
        Task UpdateAsync (long id, ProductCategoryViewModel productCategoryViewModel);
        Task DeleteAsync (long id);

    }

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryDao _categoryDao;
        private readonly ISession _session;
        private readonly ISessionFactory _sessionFactory;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(CategoryService));
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

        public async Task<List<ProductCategoryViewModel>> LoadAllAsync()
        {
            try
            {
                var categoryViewList = new List<ProductCategoryViewModel>();

                var categoryList =  await _categoryDao.LoadAll();

                if(categoryList.Count > 0)
                {
                    categoryViewList = categoryList.Select(c => new ProductCategoryViewModel
                    {
                        Id = c.Id,
                        CategoryName = c.CategoryName,
                        CategoryDescription = c.CategoryDescription,
                        CreatedBy = c.CreatedBy,
                        CreatedDate = c.CreatedDate,
                        ModifyBy = c.ModifyBy,
                        ModifyDate = c.ModifyDate,
                    }).ToList();

                    return categoryViewList;
                }
                else
                {
                    throw new Exception("Product category is not found!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProductCategoryViewModel> GetByIdAsync(long id)
        {
            try
            {
                var categoryViewModel = new ProductCategoryViewModel();
                var category = await _categoryDao.GetByIdAsync(id);

                if (category == null)
                {
                    throw new Exception("The category is not found");
                }
                else
                {
                    categoryViewModel.Id = category.Id;
                    categoryViewModel.CategoryDescription = category.CategoryDescription;
                    categoryViewModel.CategoryName = category.CategoryName;
                    categoryViewModel.CreatedBy = category.CreatedBy;
                    categoryViewModel.CreatedDate = category.CreatedDate;
                    categoryViewModel.ModifyBy = category.ModifyBy;
                    categoryViewModel.ModifyDate = category.ModifyDate; 
                }

                return categoryViewModel;

            }catch (Exception ex)
            {
                _logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task CreateAsync (ProductCategoryViewModel productCategoryViewModel)
        {
            try
            {
                var categoryMainEntity = new ProductCategory();
                var category = await _categoryDao.LoadAll();
                
                if(category.Count > 0)
                {
                    foreach (var item in category)
                    {
                        if (productCategoryViewModel.CategoryName.Contains(item.CategoryName))
                        {
                            throw new DuplicateValueException("Category can not be duplicate");
                        }
                    }
                }

                ModelValidatorMethod(productCategoryViewModel);

                using (var transaction = _session.BeginTransaction())
                {
                    try
                    {
                        categoryMainEntity.CategoryName = productCategoryViewModel.CategoryName;
                        categoryMainEntity.CategoryDescription = productCategoryViewModel.CategoryDescription;
                        categoryMainEntity.CreatedBy = 100;
                        categoryMainEntity.CreatedDate = productCategoryViewModel.CreatedDate;
                        categoryMainEntity.ModifyBy = 100;
                        categoryMainEntity.ModifyDate = productCategoryViewModel.ModifyDate;

                        await _categoryDao.CreateAsync(categoryMainEntity);
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            catch(DuplicateValueException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateAsync (long id, ProductCategoryViewModel productCategoryViewModel)
        {
            try
            {
                ModelValidatorMethod (productCategoryViewModel);

                var productCategoryToUpdate = await _categoryDao.GetByIdAsync(id);

                if (productCategoryToUpdate != null)
                {
                    using (var transaction = _session.BeginTransaction())
                    {
                        try
                        {
                            productCategoryToUpdate.CategoryName = productCategoryViewModel.CategoryName;
                            productCategoryToUpdate.CategoryDescription = productCategoryViewModel.CategoryDescription;
                            productCategoryToUpdate.ModifyBy = 200;
                            productCategoryToUpdate.ModifyDate = DateTime.Now;
                            await _categoryDao.UpdateAsync(productCategoryToUpdate);
                            await transaction.CommitAsync();
                        }
                        catch(Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                }else
                {
                    throw new Exception("Category not found to Update!");
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
                using(var transaction = _session.BeginTransaction()) 
                {
                    try
                    {
                        var individualCategoryDelete = await _categoryDao.GetByIdAsync(id);
                        if (individualCategoryDelete != null)
                        {
                            await _categoryDao.DeleteAsync(id);
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }catch(Exception ex) 
            { 
                _logger.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        private void ModelValidatorMethod(ProductCategoryViewModel modelToValidate)
        {
            if (String.IsNullOrWhiteSpace(modelToValidate.CategoryName))
            {
                throw new InvalidNameException("Name can not be null!");
            }
            if (modelToValidate.CategoryName?.Trim().Length < 1 || modelToValidate.CategoryName?.Trim().Length > 4)
            {
                //ModelState.AddModelError("sorry! your input field is empty.");
                throw new InvalidNameException("Name character should be in between 3 to 30!");
            }
            if (!Regex.IsMatch(modelToValidate.CategoryName, @"^[a-zA-Z ]+$"))
            {
                throw new InvalidNameException("Name can not contain numbers or special characters! Please input alphabetic characters and space only!");
            }
            if (modelToValidate.CategoryDescription?.Trim().Length == 0)
            {
                throw new InvalidNameException("Description can not be empty!");
            }
            if (modelToValidate.CategoryDescription?.Trim().Length > 10 || modelToValidate.CategoryDescription?.Trim().Length >= 250)
            {
                throw new InvalidNameException("Description character length should be between 10 to 250");
            }
        }
    }

}
