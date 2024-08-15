using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IMS.CustomException;
using IMS.Entity.Entities;
using IMS.Entity.EntityViewModels;
using IMS.Service;
using log4net;

namespace IMS.WEB.Controllers
{
    public class ProductTypeController : Controller
    {
        private readonly IProductTypeService _productTypeService;
        public static readonly ILog _logger = LogManager.GetLogger(typeof(HomeController));

        public ProductTypeController()
        {
            _productTypeService = new ProductTypeService();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CategoryTypeCreateViewModel productTypeViewModel)
        {
            bool isValid = false;
            string message = string.Empty;
            try
            {
                if (productTypeViewModel != null)
                {
                    productTypeViewModel.CreatedBy = User.Identity.Name;
                    productTypeViewModel.ModifyBy = User.Identity.Name;
                    await _productTypeService.CreateAsync(productTypeViewModel);

                    message = "Type is created successfully!";
                    isValid = true;
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
                _logger.Error(message, ex);
                message = "Something went wrong!";
            }

            return Json(new
            {
                Message = message,
                IsValid = isValid
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult LoadAll()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> LoadTypeData()
        {
            var typeViewList = new List<ProductTypeViewModel>();

            try
            {
                var categoryList = await _productTypeService.GetAllAsync();
                var categoryCount = categoryList.Count();

                typeViewList.AddRange(categoryList);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return Json(new
            {
                draw = 1,
                recordsTotal = typeViewList.Count,
                recordsFiltered = typeViewList.Count,
                data = typeViewList
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> DetailsAsync(long id)
        {
            bool isSuccess = false;
            string message = string.Empty;
            var productTypeDetails = new ProductTypeViewModel();
            try
            {
                productTypeDetails = await _productTypeService.GetByIdAsync(id);

                if (productTypeDetails != null)
                {
                    isSuccess = true;
                }
                else
                {
                    message = "Category Type is not found!";
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                message = ex.Message;
            }

            return Json(new
            {
                IsSuccess = isSuccess,
                Message = message,
                Details = new
                {
                    productTypeDetails.CreatedBy,
                    CreatedDate = productTypeDetails.CreatedDate?.ToString("yyyy-MM-dd HH:mm:ss tt"),
                    productTypeDetails.ModifyBy,
                    ModifyDate = productTypeDetails.ModifyDate?.ToString("yyyy-MM-dd HH:mm:ss tt")
                }
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> UpdateAsync(long id)
        {
            bool isSuccess = false;
            string message = string.Empty;
            var updateProductTypeViewModel = new ProductTypeViewModel();

            try
            {
                updateProductTypeViewModel = await _productTypeService.GetByIdAsync(id);

                if (updateProductTypeViewModel != null)
                {
                    isSuccess = true;
                }
                else
                {
                    message = "Product Type is not found to update!";
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                message = "Something went wrong!";
            }

            return Json(new
            {
                UpdateProductTypeData = updateProductTypeViewModel,
                IsSuccess = isSuccess,
                Message = message,
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateAsync(long id, ProductTypeUpdateViewModel productTypeUpdateViewModel)
        {
            string message = string.Empty;
            bool isSuccess = false;

            if(productTypeUpdateViewModel == null)
            {
                message = "ProductType is not found! Try again!";
            }
            else
            {
                try
                {
                    productTypeUpdateViewModel.ModifyBy = User.Identity.Name;
                    await _productTypeService.UpdateAsync(id, productTypeUpdateViewModel);
                    isSuccess = true;
                    message = "Product Category is updated successfully!";
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

        [HttpPost]
        public async Task<ActionResult> DeleteAsync(long id)
        {
            //var individualTypeDelete = await _productTypeService.GetById(id);
            //if (individualTypeDelete == null)
            //{
            //    return RedirectToAction("Index");
            //}
            try
            {
                await _productTypeService.DeleteAsync(id);
                TempData["AlertMessage"] = "Type is Deleted successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                //ViewBag.Error = ex.Message;
                //TempData["DeleteAlertMessage"] = "Brand is not found!";
            }
            return View();
        }
    }
}