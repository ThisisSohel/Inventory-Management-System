﻿using IMS.Entity.Entities;
using IMS.Entity.EntityViewModels;
using IMS.Service;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
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
        public async Task<ActionResult> Create(SupplierViewModel supplierViewModel)
        {
            string message = string.Empty;
            bool isValid = false;
            try
            {
                if(supplierViewModel != null)
                {
                    await _supplierService.CreateAsync(supplierViewModel);
                    isValid = true;
                    message = "Supplier is added successfully!";
                }
                else
                {
                    message = "Something is wrong! Please try again!";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return Json(new
            {
                IsValid = isValid,
                Message = message
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Load()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> LoadSupplierData()
        {
            var supplierViewModelList = new List<SupplierViewModel>();
            string message = string.Empty;
            bool isValid = false;

            try
            {
                var supplierList = await _supplierService.GetAllAsync();
                if(supplierList != null)
                {
                    foreach (var supplier in supplierList)
                    {
                        var supplierViewModel = new SupplierViewModel
                        {
                            Id = supplier.Id,
                            SupplierName = supplier.SupplierName,
                            SupplierNumber = supplier.SupplierNumber,
                            EmailAddress = supplier.EmailAddress,
                            SupplierAddress = supplier.SupplierAddress,
                            CreatedBy = supplier.CreatedBy,
                            CreatedDate = supplier.CreatedDate,
                            ModifyBy = supplier.ModifyBy,
                            ModifyDate = supplier.ModifyDate,
                        };
                        supplierViewModelList.Add(supplierViewModel);
                    }
                    isValid = true;
                }
                else
                {
                    message = "No supplier is found!";
                }
            }
            catch (Exception ex)
            {
                message = "Internal server error!";
            }

            return Json(new
            {
                SupplierList = supplierViewModelList,
                IsValid = isValid,
                Message = message
            }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Details(long id)
        {
            bool isSuccess = false;
            string message = string.Empty;
            var supplierDetails = new SupplierViewModel();

            try
            {
                supplierDetails = await _supplierService.Details(id);
                if (supplierDetails != null)
                {
                    isSuccess = true;
                }else
                {
                    message = "Supplier not found!";
                }
            }
            catch (Exception ex)
            {
                message = "Internal server error!";
            }
            return Json(new
            {
                Details = new
                {
                    supplierDetails.CreatedBy,
                    CreatedDate = supplierDetails.CreatedDate?.ToString("yyyy-MM-dd HH:mm:ss tt"),
                    supplierDetails.ModifyBy,
                    ModifyDate = supplierDetails.ModifyDate?.ToString("yyyy-MM-dd HH:mm:ss tt")
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
            var supplierToUpdate = new SupplierViewModel();
            try
            {
                supplierToUpdate = await _supplierService.GetById(id);

                if (supplierToUpdate == null)
                {
                    message = "Supplier is not found!";
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
                UpdateSupplierData = supplierToUpdate,
                IsSuccess = isSuccess,
                Message = message,
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> Update(long id, SupplierViewModel supplierViewModel)
        {
            string message = string.Empty;
            bool isSuccess = false;

            if (supplierViewModel == null)
            {
                message = "Suppler is not found! Try again";
            }
            else
            {
                try
                {
                    supplierViewModel.ModifyBy = 200;
                    await _supplierService.UpdateAsync(id, supplierViewModel);
                    isSuccess = true;
                    message = "Supplier is updated successfully!";
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
            var supplier = await _supplierService.GetById(id);

            if (supplier == null)
            {
                message = "Supplier not found to delete!";
            }
            else
            {
                try
                {
                    if (supplier.Id != 0)
                    {
                        await _supplierService.DeleteAsync(id);
                        message = "Supplier is deleted successfully!";
                        isSuccess = true;
                    }
                    else
                    {
                        message = "Supplier is not found!";
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