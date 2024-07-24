using FluentNHibernate.Testing.Values;
using IMS.CustomException;
using IMS.Entity.Entities;
using IMS.Service;
using IMS.WEB.Database;
using log4net;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using IMS.Entity.EntityViewModels;

namespace IMS.WEB.Controllers
{
    public class BrandController : Controller
    {

        private readonly IBrandService _brandService;
        private readonly ApplicationUserManager _applicatinUserManager;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(BrandController));

        public BrandController()
        {
            _brandService = new BrandService();
            _applicatinUserManager = new ApplicationUserManager(new CustomUserStore(new ApplicationDbContext()));
        }

        public ActionResult CreateBrand()
        {
            return View(new BrandViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> CreateBrand(BrandViewModel brand)
        
        {
            string message = string.Empty;
            bool isValid = false;

            try
            {

                if (ModelState.IsValid)
                {
                    await _brandService.CreateBrandService(brand);
                    isValid = true;
                }
                else
                {
                    message = "Model is not bind correctly!";
                }
                
            }catch (Exception ex)
            {
                message = "Internal server error!";
            }
            return Json(new {Message = message, IsValid = isValid}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Load()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> LoadBrandData()
        {
            var brandViewModelList = new List<BrandViewModel>();
            string message = string.Empty;
            bool isValid = false;
            try
            {

                var brand = await _brandService.GetAll();

                if (brand != null)
                {
                    //foreach (var item in brand)
                    //{
                    //    new BrandViewModel
                    //    {
                    //        Id = item.Id,
                    //        BrandName = item.BrandName,
                    //        CreatedDate = DateTime.Now,
                    //        ModifyBy = item.ModifyBy,
                    //    };
                    //}
                    brandViewModelList = brand.Select(b => new BrandViewModel
                    {
                        Id = b.Id,
                        BrandName = b.BrandName,
                        CreatedDate = b.CreatedDate,
                        ModifyDate = b.ModifyDate
                    }).ToList();
                    isValid = true;
                }
                else
                {
                    message = "No brand is available!";
                }
            }
            catch (Exception ex)
            {
                message = "Internal Server Error!";
            }

            return Json(new 
            { 
                BrandList = brandViewModelList, 
                IsValid = isValid, 
                Message = message 
            }, 
            JsonRequestBehavior.AllowGet);
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
                brand.CreatedBy = long.Parse(User.Identity.GetUserId());
                brand.ModifyBy = long.Parse(User.Identity.GetUserId());
                await _brandService.CreateAsync(brand);
            }
            catch (InvalidNameException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(brand);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                ViewBag.Error = "Something went wrong!";
            }
            TempData["AlertMessage"] = "Brand is Created successfully!";
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
            var brandList = await _brandService.GetAll();
            return Json(new
            {
                data = brandList
            }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Details(long id)
        {
            try
            {
                var brand = await _brandService.GetById(id);
                return View(brand);

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> BrandDetails(long id)
        {
            bool isSuccess = false;
            string message = string.Empty;
            var brandDetails = new BrandViewModel();
            try
            {
                brandDetails = await _brandService.BrandDetailsService(id);
                if (brandDetails != null)
                {
                    isSuccess = true;
                }
                else
                {
                    message = "Brand not found!";
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return Json(new
            {
                Details = brandDetails, 
                IsSuccess = isSuccess,
                Message = message,
            });
        }


        [HttpGet]
        public async Task<ActionResult> Update(long id)
        {
            var updateBrand = new Brand();
            try
            {
                updateBrand = await _brandService.GetById(id);

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                ViewBag.Error = ex.Message;
            }
            return View(updateBrand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(long id, Brand brand)
        {
            try
            {

                if (ModelState.IsValid == false)
                {
                    return View(brand);
                }
                brand.ModifyBy = long.Parse(User.Identity.GetUserId());
                await _brandService.Update(brand.Id, brand);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                //ViewBag.Error = ex.Message;

            }
            TempData["AlertMessage"] = "Brand is updated successfully!";
            return RedirectToAction("Index");
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
                TempData["AlertMessage"] = "Brand is Deleted successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                //ViewBag.Error = ex.Message;
                //TempData["DeleteAlertMessage"] = "Brand is not found!";
            }
            return View(brand);

        }
    }
}