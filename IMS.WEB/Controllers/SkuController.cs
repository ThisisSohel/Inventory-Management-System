using IMS.CustomException;
using IMS.Entity.Entities;
using IMS.Entity.EntityViewModels;
using IMS.Service;
using log4net;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
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


        public ActionResult CreateSKU()
        {
            return View(new SkuViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> CreateSKU(SkuViewModel skuViewModel)

        {
            string message = string.Empty;
            bool isValid = false;

            try
            {
                if (skuViewModel != null)
                {
                    await _skuService.CreateSkuService(skuViewModel);
                    isValid = true;
                    message = "SKU is added successfully!";
                }
                else
                {
                    message = "Something is wrong! Please try again!";
                }

            }
            catch(InvalidNameException ex)
            {
                message = ex.Message;
            }
            catch (Exception ex)
            {
                message = "Internal server error!";
            }
            return Json(new { Message = message, IsValid = isValid }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Load()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> LoadSkuData()
        {
            var skuViewModelList = new List<SkuViewModel>();
            string message = string.Empty;
            bool isValid = false;
            try
            {

                var sku = await _skuService.GetAll();

                if (sku != null)
                {
                    skuViewModelList = sku.Select(b => new SkuViewModel
                    {
                        Id = b.Id,
                        SKUsName = b.SKUsName,
                        CreatedDate = b.CreatedDate,
                        ModifyDate = b.ModifyDate
                    }).ToList();
                    isValid = true;
                }
                else
                {
                    message = "No Sku is available!";
                }
            }
            catch (Exception ex)
            {
                message = "Internal Server Error!";
            }

            return Json(new
            {
                SkuList = skuViewModelList,
                IsValid = isValid,
                Message = message
            },
            JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> SkuDetails(long id)
        {
            bool isSuccess = false;
            string message = string.Empty;
            var skuDetails = new SkuViewModel();
            try
            {
                skuDetails = await _skuService.SkuDetailsService(id);

                if (skuDetails != null)
                {
                    isSuccess = true;
                }
                else
                {
                    message = "SKU not found!";
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
                    skuDetails.CreatedBy,
                    CreatedDate = skuDetails.CreatedDate?.ToString("yyyy-MM-dd HH:mm:ss tt"),
                    skuDetails.ModifyBy,
                    ModifyDate = skuDetails.ModifyDate?.ToString("yyyy-MM-dd HH:mm:ss tt")
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
            var sku = new SkuViewModel();
            try
            {
                sku = await _skuService.GetById(id);
                if (sku == null)
                {
                    message = "SKU is not found!";
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
                UpdateSkuData = sku,
                IsSuccess = isSuccess,
                Message = message,
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> Update(long id, SkuViewModel sKU )
        {
            string message = string.Empty;
            bool isSuccess = false;
            if (sKU == null)
            {
                message = "SKU is not found! Try again";
            }
            else
            {
                try
                {
                    //brand.ModifyBy = long.Parse(User.Identity.GetUserId());
                    sKU.ModifyBy = 200;
                    await _skuService.UpdateAsync(id, sKU);
                    isSuccess = true;
                    message = "SKU is updated successfully!";
                }
                catch(InvalidNameException ex)
                {
                    message = ex.Message;
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

        public async Task<ActionResult> Delete(long id)
        {
            string message = string.Empty;
            bool isSuccess = false;
            var sku = await _skuService.GetById(id);
            if (sku == null)
            {
                message = "SKU not found to delete!";
            }
            else
            {
                try
                {
                    if (sku.Id != 0)
                    {
                        await _skuService.DeleteAsync(id);
                        message = "SKU is deleted successfully!";
                        isSuccess = true;
                    }
                    else
                    {
                        message = "SKU is not found!";
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