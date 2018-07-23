using HomeWork1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using X.PagedList;


namespace HomeWork1.Controllers
{
    public abstract class BaseController : Controller
    {
        public 客戶資料Repository CustomerRepo;
        public 客戶銀行資訊Repository CustomerBankRepo;
        public 客戶聯絡人Repository CustomerContactRepo;

        protected int pageSize = 1;

        public BaseController()
        {
            CustomerRepo = RepositoryHelper.Get客戶資料Repository();
            CustomerBankRepo = RepositoryHelper.Get客戶銀行資訊Repository(CustomerRepo.UnitOfWork);
            CustomerContactRepo = RepositoryHelper.Get客戶聯絡人Repository(CustomerRepo.UnitOfWork);
        }

        protected override void HandleUnknownAction(string actionName)
        {
            this.RedirectToAction("Index").ExecuteResult(this.ControllerContext);
        }
    }
}