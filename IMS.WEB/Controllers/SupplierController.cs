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
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierService;
        public static readonly ILog _logger = LogManager.GetLogger(typeof(SupplierController));

        public SupplierController()
        {
            _supplierService = new SupplierService();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Supplier supplier)
        {
            try
            {
                if (ModelState.IsValid == false)
                {
                    return View(supplier);
                }
                await _supplierService.CreateAsync(supplier);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                ViewBag.Error = ex.Message;
            }
            TempData["AlertMessage"] = "New Supplier is Created successfully!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> DataTableView()
        {
            try
            {
                var supplierList = await _supplierService.GetAllAsync();
                return Json(new
                {
                    data = supplierList
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                ViewBag.Error = ex.Message;
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Details(long id)
        {
            try
            {
                var individualSupplier = await _supplierService.GetById(id);
                return View(individualSupplier);
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
            var individualSupplierGet = new Supplier();
            try
            {
                individualSupplierGet = await _supplierService.GetById(id);


            }catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return View(individualSupplierGet);
        }

        [HttpPost]
        public async Task<ActionResult> Update(long id, Supplier supplier)
        {
            var individualSupplierUpdate = await _supplierService.GetById(id);
            if (individualSupplierUpdate == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                if(ModelState.IsValid == false)
                {
                    return View(individualSupplierUpdate);
                }
                await _supplierService.UpdateAsync(id, supplier);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            TempData["AlertMessage"] = "Supplier details are updated !";
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(long id)
        {
            var individualSupplierDelete = await _supplierService.GetById(id);  
            if (individualSupplierDelete == null)
            {
                TempData["AlertMessage"] = "Sorry! This supplier is not found! please try with valid Id";
                return RedirectToAction("Index");
            }
            try
            {
                await _supplierService.DeleteAsync(id);
                TempData["AlertMessage"] = "Customer is Deleted successfully!";
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return View(individualSupplierDelete);
        }

    }
}