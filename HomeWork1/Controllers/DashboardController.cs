using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HomeWork1.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        [NeedLogin]
        public ActionResult Index()
        {
            return View();
        }
    }
}