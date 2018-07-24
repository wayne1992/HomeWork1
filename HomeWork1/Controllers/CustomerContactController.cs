using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using HomeWork1.Models;
using HomeWork1.Models.CustomResults;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HomeWork1.Controllers
{
    public class CustomerContactController : BaseController
    {
        //private 客戶資料Entities db = new 客戶資料Entities();

        //客戶資料Repository CustomerRepo;
        //客戶聯絡人Repository CustomerContactRepo;

        public CustomerContactController()
        {
            //CustomerRepo = RepositoryHelper.Get客戶資料Repository();
           // CustomerContactRepo = RepositoryHelper.Get客戶聯絡人Repository(CustomerRepo.UnitOfWork);
        }
        // GET: CustomerContact
        public ActionResult Index()
        {
            var data = CustomerContactRepo.All().Include(p => p.客戶資料);
            
            return View(data);
        }

        [HandleError(ExceptionType = typeof(DbEntityValidationException), View = "Error_DbEntityValidationException")]
        public ActionResult BatchUpdate(BatchUpdateContactVM[] data)
        {
            if (ModelState.IsValid)
            {
                foreach (var vm in data)
                {
                    var Contact = CustomerContactRepo.Find(vm.Id);
                    Contact.職稱 = vm.職稱;
                    Contact.手機 = vm.手機;
                    Contact.電話 = vm.電話;
                }

                CustomerContactRepo.UnitOfWork.Commit();

                return RedirectToAction("Index");
            }

            ViewData.Model = CustomerContactRepo.All();

            return View("Index");
        }

        public ActionResult Search(string Keyword)
        {
            var data = CustomerContactRepo.Search(Keyword);

            return View("Index", data);
        }

        public ActionResult CustomerExcel(string sheetName, string fileName)
        {
            if (String.IsNullOrEmpty(sheetName))
            {
                sheetName = "工作表1";
            }
            if (String.IsNullOrEmpty(fileName))
            {
                fileName = string.Concat(DateTime.Now.ToString("yyyyMMddHHmmss"), ".xlsx");
            }

            var data = CustomerContactRepo.All().Select(p => new { p.職稱, p.姓名, p.Email, p.手機, p.電話, p.客戶資料.客戶名稱 });
            var workbook = new XLWorkbook();
            var MymemoryStream = new MemoryStream();
            //設置默認Style
            var style = workbook.Style;
            style.Font.FontName = "Microsoft YaHei";
            style.Font.FontSize = 16;
            var worksheet = workbook.Worksheets.Add(sheetName);
            worksheet.Cell(1, 1).Value = "職稱";
            worksheet.Cell(1, 2).Value = "姓名";
            worksheet.Cell(1, 3).Value = "Email";
            worksheet.Cell(1, 4).Value = "手機";
            worksheet.Cell(1, 5).Value = "電話";
            worksheet.Cell(1, 6).Value = "客戶名稱";
            worksheet.Cell(2, 1).InsertData(data);

            workbook.SaveAs(MymemoryStream);

            return File(MymemoryStream.ToArray(), "application/vnd.ms-excel", fileName);
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
