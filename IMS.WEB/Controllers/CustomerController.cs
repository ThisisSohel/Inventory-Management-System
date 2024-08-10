using IMS.CustomException;
using IMS.Entity.EntityViewModels;
using IMS.Service;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

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

        [Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Create(CustomerViewModel customerViewModel)
        {
            string message = string.Empty;
            bool isValid = false;

            try
            {
                if (customerViewModel != null)
                {
                    customerViewModel.CreatedBy = User.Identity.Name;
                    customerViewModel.ModifyBy = User.Identity.Name;    
                    await _customerService.CreateAsync(customerViewModel);
                    isValid = true;
                    message = "Customer is added successfully!";
                }
                else
                {
                    message = "Something is wrong! Please try again!";
                }

            }
            catch (InvalidNameException ex)
            {
                message = ex.Message;
            }
            catch (InvalidExpressionException ex)
            {
                message = ex.Message;
            }
            catch (Exception ex)
            {
                message = "Something went wrong!";
                _logger.Error(message, ex);
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
        public async Task<ActionResult> LoadCustomerData()
        {
            var customerViewModelList = new List<CustomerViewModel>();

            try
            {
                var customer = await _customerService.GetAllAsync();

                customerViewModelList = customer.Select(b => new CustomerViewModel
                {
                    Id = b.Id,
                    CustomerName = b.CustomerName,
                    CustomerNumber = b.CustomerNumber,
                    EmailAddress = b.EmailAddress,
                    CustomerAddress = b.CustomerAddress,
                    CreatedBy = b.CreatedBy,
                    CreatedDate = b.CreatedDate,
                    ModifyBy = b.ModifyBy,
                    ModifyDate = b.ModifyDate
                }).ToList();

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }

            return Json(new
            {
                recordsTotal = customerViewModelList.Count,
                recordsFiltered = customerViewModelList.Count,
                data = customerViewModelList,
            },
            JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpGet]
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
                _logger.Error(ex.Message, ex);
                message = "Something went wrong!";
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

        [Authorize]
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
                _logger.Error(message, ex);
                message = "Something went wrong!";
            }

            return Json(new
            {
                UpdateCustomerData = customerToUpdate,
                IsSuccess = isSuccess,
                Message = message,
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
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
                    customerViewModel.ModifyBy = User.Identity.Name;
                    await _customerService.UpdateAsync(id, customerViewModel);
                    isSuccess = true;
                    message = "Customer is updated successfully!";
                }
                catch (InvalidNameException ex)
                {
                    message = ex.Message;
                }
                catch (InvalidExpressionException ex)
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
            var customer = new CustomerViewModel();

            try
            {
                customer = await _customerService.GetById(id);

                if (customer !=  null)
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