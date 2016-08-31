using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Session["Name"] = "zhuqinfeng";
            return View();
        }
        public ActionResult QueryName()
        {
            string name = Session["Name"].ToString();
            ViewBag.Name = name;
            return View();
        }
    }
}
