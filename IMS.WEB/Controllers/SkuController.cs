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
        public static readonly ILog _logger = LogManager.GetLogger(typeof(SkuController));
        public SkuController()
        {
            _skuService = new SkuService();
        }

        [Authorize]
        public ActionResult CreateSKU()
        {
            return View(new SkuViewModel());
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateSKU(SkuViewModel skuViewModel)

        {
            string message = string.Empty;
            bool isValid = false;

            try
            {
                if (skuViewModel != null)
                {
                    skuViewModel.CreatedBy = User.Identity.Name;
                    skuViewModel.ModifyBy = User.Identity.Name;
                    await _skuService.CreateSkuService(skuViewModel);
                    isValid = true;
                    message = "SKU is added successfully!";
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

        [Authorize]
        [HttpGet]
        public ActionResult Load()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> LoadSkuData()
        {
            var skuViewModelList = new List<SkuViewModel>();

            try
            {
                var sku = await _skuService.GetAll();

                skuViewModelList = sku.Select(b => new SkuViewModel
                {
                    Id = b.Id,
                    SKUsName = b.SKUsName,
                    CreatedDate = b.CreatedDate,
                    ModifyDate = b.ModifyDate
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return Json(new
            {
                recordsTotal = skuViewModelList.Count,
                recordsFiltered = skuViewModelList.Count,
                data = skuViewModelList,
            },
            JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpGet]
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
                message = "Something went wrong!";
                _logger.Error(ex.Message);
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

        [Authorize]
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
                _logger.Error(message, ex);
                message = "Something went wrong!";
            }

            return Json(new
            {
                UpdateSkuData = sku,
                IsSuccess = isSuccess,
                Message = message,
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Update(long id, SkuViewModel sKU)
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
                    sKU.ModifyBy = User.Identity.Name;
                    await _skuService.UpdateAsync(id, sKU);
                    isSuccess = true;
                    message = "SKU is updated successfully!";
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
            }

            return Json(new
            {
                IsSuccess = isSuccess,
                Message = message,
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Delete(long id)
        {
            string message = string.Empty;
            bool isSuccess = false;
            var sku = new SkuViewModel();

            try
            {
                sku = await _skuService.GetById(id);

                if (sku != null)
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
                _logger.Error(message, ex);
                message = "Something went wrong!";
            }


            return Json(new
            {
                Message = message,
                IsSuccess = isSuccess
            }, JsonRequestBehavior.AllowGet);
        }
    }
}