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
                if (brand != null)
                {
                    await _brandService.CreateBrandService(brand);
                    isValid = true;
                    message = "Brand is added successfully!";
                }
                else
                {
                    message = "Something is wrong! Please try again!";
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
        public async Task<ActionResult> BrandDetails(long id)
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
                //_logger.Error(ex.Message);
            }
            return Json(new
            {
                Details = new
                {
                    brandDetails.CreatedBy,
                    CreatedDate = brandDetails.CreatedDate?.ToString("yyyy-MM-dd HH:mm:ss tt"),
                    brandDetails.ModifyBy,
                    ModifyDate = brandDetails.ModifyDate?.ToString("yyyy-MM-dd HH:mm:ss tt")
                },
                IsSuccess = isSuccess,
                Message = message,
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public async Task<ActionResult> Update(long id)
        {
            bool isSuccess = false;
            string message = string.Empty;
            var updateBrand = new BrandViewModel();
            try
            {
                updateBrand = await _brandService.GetById(id);
                if (updateBrand == null)
                {
                    message = "Brand is not found!";
                }
                else
                {
                    isSuccess = true;
                }

            }
            catch (Exception ex)
            {
                message = "Internal server error!";
            }
            return Json(new
            {
                UpdateBrandData = updateBrand,
                IsSuccess = isSuccess,
                Message = message,
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public async Task<ActionResult> Update(long id, BrandViewModel brand)
        {
            string message = string.Empty;
            bool isSuccess = false;
            if (brand == null)
            {
                message = "Brand is not found! Try again";
            }
            else{
                try
                {
                    //brand.ModifyBy = long.Parse(User.Identity.GetUserId());
                    brand.ModifyBy = 200;
                    await _brandService.Update(brand.Id, brand);
                    isSuccess = true;
                    message = "Brand is updated successfully!";
                }
                catch (Exception ex)
                {
                    message = "Internal server error!";
                }
            }
            return Json(new
            {
                IsSuccess = isSuccess,
                Message = message,
            });
        }


        [HttpPost]
        public async Task<ActionResult> Delete(long id)
        {
            string message = string.Empty;
            bool isSuccess = false;
            var brand = await _brandService.GetById(id);
            if (brand == null)
            {
                message = "Brand not found to delete!";
            }
            else
            {
                try
                {
                    if (brand.Id != 0)
                    {
                        await _brandService.DeleteAsync(id);
                        message = "Brand is deleted successfully!";
                        isSuccess = true;
                    }
                    else
                    {
                        message = "Brand is not found!";
                    }
                }
                catch (Exception ex)
                {
                    message = "Internal server error!";
                }
            }
            return Json(new
            {
                Message = message,
                IsSuccess = isSuccess
            }, JsonRequestBehavior.AllowGet);
        }
    }
}