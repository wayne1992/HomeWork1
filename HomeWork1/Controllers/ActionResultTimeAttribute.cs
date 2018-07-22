using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HomeWork1.Controllers
{
    public class ActionResultTimeAttribute : ActionFilterAttribute
    {
        public DateTime Executing;
        public DateTime Executed;

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            Executing = DateTime.Now;

            base.OnResultExecuting(filterContext);
        }


        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            Executed = DateTime.Now;

            var ResultRange = (Executed - Executing);
            Debug.Print(ResultRange.ToString());

            base.OnResultExecuted(filterContext);
        }
        
    }
}