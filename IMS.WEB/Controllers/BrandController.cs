using FluentNHibernate.Conventions.Inspections;
using IMS.Entity.Entities;
using IMS.Service;
using log4net;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IMS.WEB.Controllers
{
    public class BrandController : Controller
    {

        private readonly IBrandService _brandService;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(BrandController));

        public BrandController()
        {
            _brandService = new BrandService();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Brand brand)
        {
            try
            {
                if (ModelState.IsValid == false)
                {
                    return View(brand);
                }
                await _brandService.CreateAsync(brand);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Index()
        {
            try
            {
                var result = await _brandService.GetAll();
                return View(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return View();
        }

        public async Task<ActionResult> Details(long id)
        {
            try
            {
                var barnd = await _brandService.GetById(id);
                return View(barnd);

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return View();
        }

        [HttpPost, ActionName("Update")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(long id, Brand brand)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            try
            {
                var existingBrand = await _brandService.GetById(id);
                if (existingBrand == null)
                {
                    return RedirectToAction("Index");
                }
                await _brandService.Update(existingBrand);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return View(id);
        }

        public async Task<ActionResult> Delete(long id)
        {
            var brand = await _brandService.GetById(id);
            if (brand == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                await _brandService.DeleteAsync(id);
                return RedirectToAction("Index");


            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return View(brand);

        }
    }
}