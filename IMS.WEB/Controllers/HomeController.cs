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
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}