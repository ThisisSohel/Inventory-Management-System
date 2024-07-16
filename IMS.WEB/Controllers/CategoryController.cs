using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
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

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductCategory category)
        {
            var createNewCategory = new CategoryViewModel();
            try
            {
                if (ModelState.IsValid == false)
                {
                    return View(category);
                }
                await _categoryService.CreateAsync(category);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            TempData["AlertMessage"] = "Category is Created successfully!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> DataTableView()
        {
            try
            {
                var categoryList = await _categoryService.GetAllAsync();
                var categoryCount = categoryList.Count();

                return Json(new
                {
                    draw = 1,
                    recordsTotal = categoryCount,
                    recordsFiltered = categoryCount,
                    data = categoryList
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return View();
        }

        public async Task<ActionResult> Details(long id)
        {
            try
            {
                var individualCategory = await _categoryService.GetById(id);
                return View(individualCategory);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Update(long id)
        {
            var updateCategory = new ProductCategory();
            try
            {
                 updateCategory = await _categoryService.GetById(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return View(updateCategory);
        }

        [HttpPost]
        public async Task<ActionResult> Update(long id, ProductCategory productCategory)
        {
            var categoryToUpdate = await _categoryService.GetById(id);
            if (categoryToUpdate == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                if(ModelState.IsValid == false) 
                {
                    return View(categoryToUpdate);
                }
                await _categoryService.UpdateAsync(id, productCategory);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            TempData["AlertMessage"] = "Category is updated successfully!";
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(long id)
        {
            var individualCategoryDelete = await _categoryService.GetById(id);
            if (individualCategoryDelete == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                await _categoryService.DeleteAsync(id);
                TempData["AlertMessage"] = "Category is Deleted successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                //ViewBag.Error = ex.Message;
                //TempData["DeleteAlertMessage"] = "Brand is not found!";
            }
            return View(individualCategoryDelete);
        }

    }

}
