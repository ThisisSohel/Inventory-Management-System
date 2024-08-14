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


        public async Task<ActionResult> Details(long id)
        {
            try
            {
                var individualType = await _productTypeService.GetById(id);
                return View(individualType);

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Update(long id)
        {
            var updateProductType = new ProductType();
            try
            {
                updateProductType = await _productTypeService.GetById(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return View(updateProductType);
        }

        [HttpPost]
        public async Task<ActionResult> Update(long id, ProductType productType)
        {
            var typeToUpdate = await _productTypeService.GetById(id);
            if (typeToUpdate == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                if (ModelState.IsValid == false)
                {
                    return View(typeToUpdate);
                }
                await _productTypeService.UpdateAsync(id, productType);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            TempData["AlertMessage"] = "Type is updated successfully!";
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> Delete(long id)
        {
            var individualTypeDelete = await _productTypeService.GetById(id);
            if (individualTypeDelete == null)
            {
                return RedirectToAction("Index");
            }
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
            return View(individualTypeDelete);
        }
    }
}