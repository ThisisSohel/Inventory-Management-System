﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IMS.Entity.Entities;
using IMS.Entity.EntityViewModels;
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
        public async Task<ActionResult> Create(CustomerViewModel customerViewModel)
        {
            string message = string.Empty;
            bool isValid = false;

            try
            {
                if (customerViewModel != null)
                {
                    await _customerService.CreateAsync(customerViewModel);
                    isValid = true;
                    message = "Customer is added successfully!";
                }
                else
                {
                    message = "Something is wrong! Please try again!";
                }

            }
            catch (Exception ex)
            {
                message = "Internal server error!";
            }

            return Json(new 
            { 
                Message = message, 
                IsValid = isValid 
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Load()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> LoadCustomerData()
        {
            var customerViewModelList = new List<CustomerViewModel>();
            string message = string.Empty;
            bool isValid = false;
            try
            { 

                var sku = await _customerService.GetAllAsync();

                if (sku != null)
                {
                    customerViewModelList = sku.Select(b => new CustomerViewModel
                    {
                        Id = b.Id,
                        CustomerName = b.CustomerName,
                        CustomerNumber = b.CustomerNumber,
                        CustomerAddress = b.CustomerAddress,
                        EmailAddress = b.CustomerAddress,
                        CreatedBy = b.CreatedBy,
                        CreatedDate = b.CreatedDate,
                        ModifyBy = b.ModifyBy,
                        ModifyDate = b.ModifyDate
                    }).ToList();
                    isValid = true;
                }
                else
                {
                    message = "No customer is available!";
                }
            }
            catch (Exception ex)
            {
                message = "Internal Server Error!";
            }

            return Json(new
            {
                CustomerList = customerViewModelList,
                IsValid = isValid,
                Message = message
            },
            JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Details(long id)
        {
            bool isSuccess = false;
            string message = string.Empty;
            var customerDetails = new CustomerViewModel();
            try
            {
                customerDetails = await _customerService.CustomerDetails(id);

                if (customerDetails != null)
                {
                    isSuccess = true;
                }
                else
                {
                    message = "Customer not found!";
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
                    customerDetails.CreatedBy,
                    CreatedDate = customerDetails.CreatedDate?.ToString("yyyy-MM-dd HH:mm:ss tt"),
                    customerDetails.ModifyBy,
                    ModifyDate = customerDetails.ModifyDate?.ToString("yyyy-MM-dd HH:mm:ss tt")
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
            var customerToUpdate = new CustomerViewModel();
            try
            {
                customerToUpdate = await _customerService.GetById(id);
                if (customerToUpdate == null)
                {
                    message = "Customer is not found!";
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
                UpdateCustomerData = customerToUpdate,
                IsSuccess = isSuccess,
                Message = message,
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> Update(long id, CustomerViewModel customerViewModel)
        {
            string message = string.Empty;
            bool isSuccess = false;
            if (customerViewModel == null)
            {
                message = "Customer is not found! Try again";
            }
            else
            {
                try
                {
                    //brand.ModifyBy = long.Parse(User.Identity.GetUserId());
                    customerViewModel.ModifyBy = 200;
                    await _customerService.UpdateAsync(id, customerViewModel);
                    isSuccess = true;
                    message = "Customer is updated successfully!";
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
            var customer = await _customerService.GetById(id);
            if (customer == null)
            {
                message = "Customer not found to delete!";
            }
            else
            {
                try
                {
                    if (customer.Id != 0)
                    {
                        await _customerService.DeleteAsync(id);
                        message = "Customer is deleted successfully!";
                        isSuccess = true;
                    }
                    else
                    {
                        message = "Customer is not found!";
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