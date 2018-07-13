using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HomeWork1.Models;
using HomeWork1.Models.CustomResults;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HomeWork1.Controllers
{
    public class CustomerBankInformationController : BaseController
    {
        //private 客戶資料Entities db = new 客戶資料Entities();
        客戶資料Repository CustomerRepo;
        客戶銀行資訊Repository CustomerBankRepo;

        public CustomerBankInformationController() {
            CustomerRepo = RepositoryHelper.Get客戶資料Repository();
            CustomerBankRepo = RepositoryHelper.Get客戶銀行資訊Repository(CustomerRepo.UnitOfWork);
        }

        // GET: CustomerBankInformation
        public ActionResult Index(string IsExport)
        {
            var data = CustomerBankRepo.All().Include(p => p.客戶資料);
            //var 客戶銀行資訊 = db.客戶銀行資訊.Include(客 => 客.客戶資料);
            if (!string.IsNullOrEmpty(IsExport))
            {

                JArray jObjects = new JArray();

                foreach (var item in data)
                {
                    var jo = new JObject();
                    var Custom = CustomerRepo.Where(p => p.Id == item.客戶Id).Select(p => new { p.客戶名稱 });

                    jo.Add("銀行名稱", item.銀行名稱);
                    jo.Add("銀行代碼", item.銀行代碼);
                    jo.Add("分行代碼", item.分行代碼);
                    jo.Add("帳戶名稱", item.帳戶名稱);
                    jo.Add("帳戶號碼", item.帳戶號碼);
                    //jo.Add("客戶名稱", Custom.ToString());
                    jObjects.Add(jo);
                }

                var exportSpource = jObjects;
                var dt = JsonConvert.DeserializeObject<DataTable>(exportSpource.ToString());

                var exportFileName = string.Concat(
                    "客戶銀行資訊_",
                    DateTime.Now.ToString("yyyyMMddHHmmss"),
                    ".xlsx");

                return new ExportExcelResult
                {
                    SheetName = "客戶銀行資訊",
                    FileName = exportFileName,
                    ExportData = dt
                };
            }
            return View(data);
        }

        public ActionResult Search(string Keyword)
        {
            var data = CustomerBankRepo.Search(Keyword);

            return View("Index", data);
        }

        // GET: CustomerBankInformation/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = CustomerBankRepo.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // GET: CustomerBankInformation/Create
        public ActionResult Create()
        {
            ViewBag.客戶Id = new SelectList(CustomerRepo.All(), "Id", "客戶名稱");
            return View();
        }

        // POST: CustomerBankInformation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                CustomerBankRepo.Add(客戶銀行資訊);
                CustomerBankRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
           
            ViewBag.客戶Id = new SelectList(CustomerRepo.All(), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: CustomerBankInformation/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = CustomerBankRepo.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }

            ViewBag.客戶Id = new SelectList(CustomerRepo.All(), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // POST: CustomerBankInformation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                var db = CustomerBankRepo.UnitOfWork.Context;
                db.Entry(客戶銀行資訊).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.客戶Id = new SelectList(CustomerRepo.All(), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: CustomerBankInformation/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = CustomerBankRepo.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // POST: CustomerBankInformation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶銀行資訊 客戶銀行資訊 = CustomerBankRepo.Find(id);
            CustomerBankRepo.Delete(客戶銀行資訊);
            CustomerBankRepo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CustomerBankRepo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
