using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HomeWork1.Controllers
{
    public class NeedLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult("/Login/Login");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}