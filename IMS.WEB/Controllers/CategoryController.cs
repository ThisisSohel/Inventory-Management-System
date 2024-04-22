using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IMS.Entity.Entities;
using IMS.Service;
using log4net;

namespace IMS.WEB.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public static readonly ILog _logger = LogManager.GetLogger(typeof(CategoryController));

        public CategoryController() //Need to take a feedback from my mentors 
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
                _logger.Error(ex);
            }
            return View(category);
        }

        public async Task<ActionResult> Index()
        {
            try
            {
                var getAllCategory = await _categoryService.GetAllAsync();
                return View(getAllCategory);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(long id, ProductCategory category)
        {

            if (id == null)
            {
                return RedirectToAction("Index");
            }

            try
            {
                var existingCategory = await _categoryService.GetById(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return View(category);
        }

        public async Task<ActionResult> Delete(long id)
        {
            var categoryToDelete = await _categoryService.GetById(id);
            if (categoryToDelete == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                await _categoryService.DeleteAsync(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return View();
        }

    }

}
