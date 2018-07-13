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
    public class CustomerContactController : BaseController
    {
        //private 客戶資料Entities db = new 客戶資料Entities();

        客戶資料Repository CustomerRepo;
        客戶聯絡人Repository CustomerContactRepo;

        public CustomerContactController()
        {
            CustomerRepo = RepositoryHelper.Get客戶資料Repository();
            CustomerContactRepo = RepositoryHelper.Get客戶聯絡人Repository(CustomerRepo.UnitOfWork);
        }
        // GET: CustomerContact
        public ActionResult Index(string IsExport)
        {
            var data = CustomerContactRepo.All().Include(p => p.客戶資料);
            //var 客戶聯絡人 = db.客戶聯絡人.Include(客 => 客.客戶資料);
            if (!string.IsNullOrEmpty(IsExport))
            {

                JArray jObjects = new JArray();

                foreach (var item in data)
                {
                    var jo = new JObject();
                    jo.Add("職稱", item.職稱);
                    jo.Add("姓名", item.姓名);
                    jo.Add("Email", item.Email);
                    jo.Add("手機", item.手機);
                    jo.Add("電話", item.電話);
                    
                    jObjects.Add(jo);
                }

                var exportSpource = jObjects;
                var dt = JsonConvert.DeserializeObject<DataTable>(exportSpource.ToString());

                var exportFileName = string.Concat(
                    "客戶聯絡人_",
                    DateTime.Now.ToString("yyyyMMddHHmmss"),
                    ".xlsx");

                return new ExportExcelResult
                {
                    SheetName = "客戶聯絡人",
                    FileName = exportFileName,
                    ExportData = dt
                };
            }
            return View(data);
        }

        public ActionResult Search(string Keyword)
        {
            var data = CustomerContactRepo.Search(Keyword);

            return View("Index", data);
        }

        // GET: CustomerContact/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = CustomerContactRepo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // GET: CustomerContact/Create
        public ActionResult Create()
        {
            ViewBag.客戶Id = new SelectList(CustomerRepo.All(), "Id", "客戶名稱");
            return View();
        }

        // POST: CustomerContact/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                CustomerContactRepo.Add(客戶聯絡人);
                CustomerContactRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(CustomerRepo.All(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: CustomerContact/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = CustomerContactRepo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(CustomerRepo.All(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // POST: CustomerContact/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                var db = CustomerContactRepo.UnitOfWork.Context;
                db.Entry(客戶聯絡人).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.客戶Id = new SelectList(CustomerRepo.All(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: CustomerContact/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = CustomerContactRepo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // POST: CustomerContact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶聯絡人 客戶聯絡人 = CustomerContactRepo.Find(id);
            CustomerContactRepo.Delete(客戶聯絡人);
            CustomerContactRepo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CustomerContactRepo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
