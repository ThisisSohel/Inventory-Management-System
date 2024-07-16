using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IMS.WEB.Controllers
{
    public class HomeController : Controller
    {
        private static readonly ILog logger = LoggerConfiguration.GetLogger();
        public ActionResult Index()
        {
            logger.Info("Index page visited.");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "A small description about our Inventory Management System";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Please contact us if you have any queries!";

            return View();
        }
    }
}