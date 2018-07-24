using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using HomeWork1.Models;
using HomeWork1.Models.CustomResults;
using Microsoft.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using X.PagedList;

namespace HomeWork1.Controllers
{
    public class CustomerInformationController : BaseController
    {
        private 客戶資料Entities db2 = new 客戶資料Entities();
        Dictionary<int, string> classification = new Dictionary<int, string>();
        //客戶資料Repository CustomerRepo = RepositoryHelper.Get客戶資料Repository();
        // GET: CustomerInformation
        public CustomerInformationController(){
            
            classification.Add(1,"科技業");
            classification.Add(2, "傳產業");
            classification.Add(3, "航太業");
        }

        [ActionResultTime]
        public ActionResult Index(string Keyword, int? 客戶分類, string sortType, string colName, int page = 1)
        {
            //var data = CustomerRepo.All(sortType, colName).ToPagedList(Page, pageSize);
            var data = CustomerRepo.Search(Keyword, 客戶分類, sortType, colName);
            var items = (from p in classification
                         select p.Key)
                         .Distinct()
                         .OrderBy(p => p)
                         .Select(p => new SelectListItem()
                         {
                             Text = classification[p].ToString(),
                             Value = p.ToString()
                         });
            ViewBag.客戶分類 = new SelectList(items, "Value", "Text");
            ViewBag.classification = classification;

            ViewBag.Keyword = Keyword;
            ViewBag.客戶分類Id = 客戶分類;
            var Result = data.ToPagedList(page, pageSize);
            return View(Result);
        }


        [ChildActionOnly]//限定由頁面呼叫存取
        public ActionResult CustomerContactList(int id)
        {
            ViewData.Model = CustomerRepo.Find(id).客戶聯絡人.ToList(); 

            return PartialView("CustomerContactList");
        }

        public ActionResult CustomerExcel(string sheetName, string fileName)
        {
            if (String.IsNullOrEmpty(sheetName)) {
                sheetName = "工作表1";
            }
            if (String.IsNullOrEmpty(fileName))
            {
                fileName = string.Concat(DateTime.Now.ToString("yyyyMMddHHmmss"), ".xlsx");
            }
            
            var data = CustomerRepo.All().Select( p => new {p.客戶名稱, p.統一編號, p.電話, p.傳真, p.地址, p.Email } );
            var workbook = new XLWorkbook();
            var MymemoryStream = new MemoryStream();
            //設置默認Style
            var style = workbook.Style;
            style.Font.FontName = "Microsoft YaHei";
            style.Font.FontSize = 16;
            var worksheet = workbook.Worksheets.Add(sheetName);
            worksheet.Cell(1, 1).Value = "客戶名稱";
            worksheet.Cell(1, 2).Value = "統一編號";
            worksheet.Cell(1, 3).Value = "電話";
            worksheet.Cell(1, 4).Value = "傳真";
            worksheet.Cell(1, 5).Value = "地址";
            worksheet.Cell(1, 6).Value = "Email";
            worksheet.Cell(2, 1).InsertData(data);
            
            workbook.SaveAs(MymemoryStream);

            return File(MymemoryStream.ToArray(), "application/vnd.ms-excel", fileName);
        }

        
        //public ActionResult Search(string Keyword, int? 客戶分類, int Page = 1)
        //{
            //var data = CustomerRepo.Search(Keyword, 客戶分類);
            //var items = (from p in classification
            //             select p.Key)
            //             .Distinct()
            //             .OrderBy(p => p)
            //             .Select(p => new SelectListItem()
            //             {
            //                 Text = classification[p].ToString(),
            //                 Value = p.ToString()
            //             });
            //ViewBag.客戶分類 = new SelectList(items, "Value", "Text");
            //ViewBag.classification = classification;
            //ViewBag.Keyword = Keyword;
            //ViewBag.客戶分類Id = 客戶分類;
            //var Result = data.ToPagedList(Page, pageSize);

            //return View("Index", Result);
        //}
        [AjaxOnly]
        public ActionResult AjaxResort(string sortType, string colName)
        {

            return View();
        }
        public ActionResult Index2(string IsExport)
        {
            //var data = db2.客戶資料
            //    .OrderByDescending(p => p.客戶名稱)
            //    .Select(p => new TestViewModel() {
            //        客戶名稱 = p.客戶名稱,
            //        聯絡人數量 = db2.客戶聯絡人.Count(d => d.客戶Id == p.Id) ,
            //        銀行帳戶數量 = db2.客戶銀行資訊.Count(b => b.客戶Id == p.Id)
            //    });

            var data = CustomerRepo.All()
                .OrderByDescending(p => p.客戶名稱)
                .Select(p => new TestViewModel()
                {
                    客戶名稱 = p.客戶名稱,
                    聯絡人數量 = p.客戶聯絡人.Count(d => d.客戶Id == p.Id),
                    銀行帳戶數量 = p.客戶銀行資訊.Count(b => b.客戶Id == p.Id)
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
            var items = (from p in classification
                         select p.Key)
                         .Distinct()
                         .OrderBy(p => p)
                         .Select(p => new SelectListItem()
                         {
                             Text = classification[p].ToString(),
                             Value = p.ToString()
                         });
            ViewBag.客戶分類 = new SelectList(items, "Value", "Text");

            return View();
        }

        // POST: CustomerInformation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類")] 客戶資料 客戶資料)
        {

            if (ModelState.IsValid)
            {
                CustomerRepo.Add(客戶資料);
                CustomerRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            var items = (from p in classification
                         select p.Key)
                         .Distinct()
                         .OrderBy(p => p)
                         .Select(p => new SelectListItem()
                         {
                             Text = classification[p].ToString(),
                             Value = p.ToString()
                         });
            ViewBag.客戶分類 = new SelectList(items, "Value", "Text");
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

            var items = (from p in classification
                         select p.Key)
                         .Distinct()
                         .OrderBy(p => p)
                         .Select(p => new SelectListItem()
                         {
                             Text = classification[p].ToString(),
                             Value = p.ToString()
                         });
            ViewBag.客戶分類 = new SelectList(items, "Value", "Text" , 客戶資料.客戶分類);

            return View(客戶資料);
        }

        // POST: CustomerInformation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                var db = CustomerRepo.UnitOfWork.Context;
                db.Entry(客戶資料).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var items = (from p in classification
                         select p.Key)
                        .Distinct()
                        .OrderBy(p => p)
                        .Select(p => new SelectListItem()
                        {
                            Text = classification[p].ToString(),
                            Value = p.ToString()
                        });
            ViewBag.客戶分類 = new SelectList(items, "Value", "Text", 客戶資料.客戶分類);

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
