﻿using System;
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
    public class CustomerInformationController : BaseController
    {
        private 客戶資料Entities db2 = new 客戶資料Entities();
        客戶資料Repository CustomerRepo = RepositoryHelper.Get客戶資料Repository();
        // GET: CustomerInformation
        public ActionResult Index(string IsExport)
        {
            var data = CustomerRepo.All();
            if (!string.IsNullOrEmpty(IsExport)) {

                JArray jObjects = new JArray();

                foreach (var item in data)
                {
                    var jo = new JObject();
                    jo.Add("客戶名稱", item.客戶名稱);
                    jo.Add("統一編號", item.統一編號);
                    jo.Add("電話", item.電話);
                    jo.Add("傳真", item.傳真);
                    jo.Add("地址", item.地址);
                    jo.Add("Email", item.Email);
                    jObjects.Add(jo);
                }

                var exportSpource = jObjects;
                var dt = JsonConvert.DeserializeObject<DataTable>(exportSpource.ToString());

                var exportFileName = string.Concat(
                    "客戶資料_",
                    DateTime.Now.ToString("yyyyMMddHHmmss"),
                    ".xlsx");

                return new ExportExcelResult
                {
                    SheetName = "客戶資料",
                    FileName = exportFileName,
                    ExportData = dt
                };
            }
            return View(data);
        }

        public ActionResult Search(string Keyword, string IsExport)
        {
            var data = CustomerRepo.Search(Keyword);
            
            return View("Index", data);
        }

        public ActionResult Index2(string IsExport)
        {
            var data = db2.客戶資料
                .OrderByDescending(p => p.客戶名稱)
                .Select(p => new TestViewModel() {
                    客戶名稱 = p.客戶名稱,
                    聯絡人數量 = db2.客戶聯絡人.Count(d => d.客戶Id == p.Id) ,
                    銀行帳戶數量 = db2.客戶銀行資訊.Count(b => b.客戶Id == p.Id)
                });
            if (!string.IsNullOrEmpty(IsExport))
            {

                JArray jObjects = new JArray();

                foreach (var item in data)
                {
                    var jo = new JObject();
                    jo.Add("客戶名稱", item.客戶名稱);
                    jo.Add("聯絡人數量", item.聯絡人數量);
                    jo.Add("銀行帳戶數量", item.銀行帳戶數量);
                    jObjects.Add(jo);
                }

                var exportSpource = jObjects;
                var dt = JsonConvert.DeserializeObject<DataTable>(exportSpource.ToString());

                var exportFileName = string.Concat(
                    "報表_",
                    DateTime.Now.ToString("yyyyMMddHHmmss"),
                    ".xlsx");

                return new ExportExcelResult
                {
                    SheetName = "客戶資料",
                    FileName = exportFileName,
                    ExportData = dt
                };
            }
            return View(data);
        }


        public ActionResult Search2(string Keyword)
        {
            var data = db2.客戶資料
                .OrderByDescending(p => p.客戶名稱)
                .Select(p => new TestViewModel()
                {
                    客戶名稱 = p.客戶名稱,
                    聯絡人數量 = db2.客戶聯絡人.Count(d => d.客戶Id == p.Id),
                    銀行帳戶數量 = db2.客戶銀行資訊.Count(b => b.客戶Id == p.Id)
                });

            if (!String.IsNullOrEmpty(Keyword))
            {
                data = data.Where(p => p.客戶名稱.Contains(Keyword)).OrderByDescending(p => p.客戶名稱)
                    .Select(p => new TestViewModel() {
                        客戶名稱 = p.客戶名稱,
                        聯絡人數量 = p.聯絡人數量,
                        銀行帳戶數量 = p.銀行帳戶數量
                    });
            }

            return View("Index2", data);
        }
        // GET: CustomerInformation/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = CustomerRepo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // GET: CustomerInformation/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerInformation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                CustomerRepo.Add(客戶資料);
                CustomerRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(客戶資料);
        }

        // GET: CustomerInformation/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = CustomerRepo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: CustomerInformation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                var db = CustomerRepo.UnitOfWork.Context;
                db.Entry(客戶資料).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(客戶資料);
        }

        // GET: CustomerInformation/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = CustomerRepo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: CustomerInformation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶資料 客戶資料 = CustomerRepo.Find(id);
            CustomerRepo.Delete(客戶資料);
            CustomerRepo.UnitOfWork.Commit();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CustomerRepo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
