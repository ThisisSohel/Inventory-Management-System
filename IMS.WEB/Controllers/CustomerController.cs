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
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        public static readonly ILog _logger = LogManager.GetLogger(typeof(CustomerController));
        public CustomerController()
        {
            _customerService = new CustomerService();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Customer customer)
        {
            try
            {
                if (ModelState.IsValid == false)
                {
                    return View(customer);
                }
                await _customerService.CreateAsync(customer);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                ViewBag.Error = ex.Message;
            }
            TempData["AlertMessage"] = "New Customer is Created successfully!";
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
            var customerList = await _customerService.GetAllAsync();
            return Json(new
            {
                data = customerList
            }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Details(long id)
        {
            try
            {
                var individualSku = await _customerService.GetById(id);
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
            var updateIndividualCustomer = new Customer();
            try
            {
                updateIndividualCustomer = await _customerService.GetById(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return View(updateIndividualCustomer);
        }

        [HttpPost]
        public async Task<ActionResult> Update(long id, Customer customer)
        {
            var updateIndividualCustomer = await _customerService.GetById(id);
            if (updateIndividualCustomer == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                if (ModelState.IsValid == false)
                {
                    return View(updateIndividualCustomer);
                }
                await _customerService.UpdateAsync(id, customer);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            TempData["AlertMessage"] = "Customer details are updated successfully!";
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(long id)
        {
            var individualSkuDelete = await _customerService.GetById(id);
            if (individualSkuDelete == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                await _customerService.DeleteAsync(id);
                TempData["AlertMessage"] = "Customer is Deleted successfully!";
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