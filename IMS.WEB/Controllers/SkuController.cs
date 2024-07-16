using IMS.Entity.Entities;
using IMS.Service;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IMS.WEB.Controllers
{
    public class SkuController : Controller
    {
        private readonly ISkuService _skuService;
        public static readonly ILog _logger = LogManager.GetLogger(typeof(HomeController));
        public SkuController()
        {
            _skuService = new SkuService();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SKU sKU)
        {
            try
            {
                if (ModelState.IsValid == false)
                {
                    return View(sKU);
                }
                await _skuService.CreateAsync(sKU);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                ViewBag.Error = ex.Message;
            }
            TempData["AlertMessage"] = "New SKU is Created successfully!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> DataTableView()
        {
            var SkuList = await _skuService.GetAllAsync();
            return Json(new
            {
                data = SkuList
            }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Details(long id)
        {
            try
            {
                var individualSku = await _skuService.GetById(id);
                return View(individualSku);

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
            var updateProductSku = new SKU();
            try
            {
                updateProductSku = await _skuService.GetById(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return View(updateProductSku);
        }

        [HttpPost]
        public async Task<ActionResult> Update(long id, SKU sKU )
        {
            var sKuToUpdate = await _skuService.GetById(id);
            if (sKuToUpdate == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                if (ModelState.IsValid == false)
                {
                    return View(sKuToUpdate);
                }
                await _skuService.UpdateAsync(id, sKU);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            TempData["AlertMessage"] = "SKU is updated successfully!";
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(long id)
        {
            var individualSkuDelete = await _skuService.GetById(id);
            if (individualSkuDelete == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                await _skuService.DeleteAsync(id);
                TempData["AlertMessage"] = "SKU is Deleted successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                //ViewBag.Error = ex.Message;
                //TempData["DeleteAlertMessage"] = "Brand is not found!";
            }
            return View(individualSkuDelete);
        }
    }
}