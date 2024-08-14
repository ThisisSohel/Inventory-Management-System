using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using IMS.CustomException;
using IMS.Entity.Entities;
using IMS.Entity.EntityViewModels;
using IMS.Service;
using log4net;

namespace IMS.WEB.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public static readonly ILog _logger = LogManager.GetLogger(typeof(CategoryController));

        public CategoryController()
        {
            _categoryService = new CategoryService();
        }

        [Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateAsync(ProductCategoryViewModel productCategoryViewModel)
        {
            string message = string.Empty;
            bool isValid = false;

            try
            {
                if (productCategoryViewModel != null)
                {
                    productCategoryViewModel.CreatedBy = User.Identity.Name;
                    productCategoryViewModel.ModifyBy = User.Identity.Name;
                    await _categoryService.CreateAsync(productCategoryViewModel);
                    isValid = true;
                    message = "Category is created successfully!";
                }
                else
                {
                    message = "Something is wrong! Please try again!";
                }
            }
            catch (DuplicateValueException ex)
            {
                message = ex.Message;
            }
            catch (InvalidNameException ex)
            {
                message = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                message = "Something went wrong!";
            }

            return Json(new
            {
                IsValid = isValid,
                Message = message
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpGet]
        public ActionResult LoadAll()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> LoadCategoryData()
        {
            var categoryViewList = new List<ProductCategoryViewModel>();
            string message = string.Empty;

            try
            {
                var categoryList = await _categoryService.LoadAllAsync();
                categoryViewList.AddRange(categoryList);

            }
            catch (Exception ex)
            {
                message = "No Product-Category is not available! Please add your new product category!";
                _logger.Error(ex);

            }

            return Json(new
            {
                draw = 1,
                recordsTotal = categoryViewList.Count,
                recordsFiltered = categoryViewList.Count,
                data = categoryViewList,
                Message = message
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> DetailsAsync(long id)
        {
            bool isSuccess = false;
            string message = string.Empty;
            var categoryDetails = new ProductCategoryViewModel();

            try
            {
                categoryDetails = await _categoryService.GetByIdAsync(id);

                if (categoryDetails != null)
                {
                    isSuccess = true;
                }
                else
                {
                    message = "Category is not found!";
                }
            }
            catch (Exception ex)
            {
                message = "Something went wrong!";
                _logger.Error(message, ex);
            }

            return Json(new
            {
                Details = new
                {
                    categoryDetails.CreatedBy,
                    CreatedDate = categoryDetails.CreatedDate?.ToString("yyyy-MM-dd HH:mm:ss tt"),
                    categoryDetails.ModifyBy,
                    ModifyDate = categoryDetails.ModifyDate?.ToString("yyyy-MM-dd HH:mm:ss tt")
                },
                IsSuccess = isSuccess,
                Message = message,
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> UpdateAsync(long id)
        {
            bool isSuccess = false;
            string message = string.Empty;
            var updateCategory = new ProductCategoryViewModel();

            try
            {
                updateCategory = await _categoryService.GetByIdAsync(id);
                if (updateCategory != null)
                {
                    isSuccess = true;
                }
                else
                {
                    message = "Category not found to update!";
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                message = "Something went wrong!";
            }

            return Json(new
            {
                UpdateCategoryData = updateCategory,
                IsSuccess = isSuccess,
                Message = message,
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> UpdateAsync(long id, ProductCategoryViewModel productCategoryViewModel)
        {
            string message = string.Empty;
            bool isSuccess = false;

            if (productCategoryViewModel == null)
            {
                message = "Category is not found! Try again";
            }
            else
            {
                try
                {
                    productCategoryViewModel.ModifyBy = User.Identity.Name;
                    await _categoryService.UpdateAsync(id, productCategoryViewModel);
                    isSuccess = true;
                    message = "Category is updated successfully!";
                }
                catch (InvalidNameException ex)
                {
                    message = ex.Message;
                }
                catch (Exception ex)
                {
                    message = "Something went wrong!";
                    _logger.Error(ex.Message);
                }
            }

            return Json(new
            {
                IsSuccess = isSuccess,
                Message = message,
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> DeleteAsync(long id)
        {
            string message = string.Empty;
            bool isSuccess = false;

            try
            {
                var individualCategoryDelete = await _categoryService.GetByIdAsync(id);

                if (individualCategoryDelete != null)
                {
                    await _categoryService.DeleteAsync(id);
                    message = "Category is deleted successfully!";
                    isSuccess = true;
                }
                else
                {
                    message = "Category is not found to delete!";
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                message = "Something went wrong!";
            }

            return Json(new
            {
                IsSuccess = isSuccess,
                Message = message,
            }, JsonRequestBehavior.AllowGet);
        }

    }

}
