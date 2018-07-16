using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HomeWork1.Models.CustomResults
{
    public class CusExcelResult : ActionResult
    {
        public string SheetName { get; set; }
        public string FileName { get; set; }
        public IQueryable Queryable { get; set; }

        public CusExcelResult() {

        }

        public override void ExecuteResult(ControllerContext context){

            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sample Sheet");
            worksheet.Cell("A1").Value = "Hello World!";
            workbook.SaveAs("HelloWorld.xlsx");



            // return this.File();
        }
    }
}