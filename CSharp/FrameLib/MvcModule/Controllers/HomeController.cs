using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcModule.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "/logger.txt";
            ViewBag.logInfo = System.IO.File.ReadAllText(filePath);
            return View();
        }
        public ActionResult Aaditya()
        {
            return View();
        }
    }
}