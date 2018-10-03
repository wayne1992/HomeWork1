using HomeWork1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HomeWork1.Controllers
{
    public class LoginController : BaseController
    {
        // GET: Login
        public ActionResult Login()
        {
 
            return View();
        }

        [HttpPost]
        public ActionResult Login([Bind(Include = "Account, Password")] LoginVM LoginData)
        {
            if (ModelState.IsValid)
            {
                var Result = CustomerRepo.Login(LoginData.Account, LoginData.Password);
                if (Result.Count() > 0) {
                    FormsAuthentication.RedirectFromLoginPage(LoginData.Account, true);
                    return Redirect("/Dashboard/Index");
                }     
            }

            return View(LoginData);
        }

        public ActionResult CheckLogin()
        {
            var Result = "未登入";

            if (User.Identity.IsAuthenticated)
            {
                Result = User.Identity.Name + "您現在是已登入狀態。";
            }
            return Content(Result);
        }

        public ActionResult Loginout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}