using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using WillHomework01.Models;

namespace WillHomework01.Controllers
{
    

    public class CustomerController : Controller
    {
        客戶資料Repository customerRepo = RepositoryHelper.Get客戶資料Repository();

        // GET: Customer
        public ActionResult Index(string 客戶分類,string keyword="",string sortOrder ="",bool IsAsc=true)

        {
            ViewBag.客戶分類 = new SelectList(new string[] { "OAK", "Normal", "VIP" });

            var customers = customerRepo.Search(客戶分類,keyword, sortOrder, IsAsc);

            ViewBag.IsAsc = IsAsc;

            return View(customers);
        }

        // GET: Customer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = customerRepo.All().FirstOrDefault(x => x.Id == id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            ViewBag.客戶分類 = new SelectList(new string[] { "OAK","Normal","VIP"});

            return View();
        }

        // POST: Customer/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,客戶分類,統一編號,電話,傳真,地址,Email")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                customerRepo.Add(客戶資料);
                customerRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            ViewBag.客戶分類 = new SelectList(new string[] { "OAK", "Normal", "VIP" });
            return View(客戶資料);
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = customerRepo.All().FirstOrDefault(x => x.Id == id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶分類 = new SelectList(new string[] { "OAK", "Normal", "VIP" });
            return View(客戶資料);
        }

        // POST: Customer/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶名稱,客戶分類,統一編號,電話,傳真,地址,Email")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                var db = customerRepo.UnitOfWork.Context;
                db.Entry(客戶資料).State = EntityState.Modified;
                customerRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            ViewBag.客戶分類 = new SelectList(new string[] { "OAK", "Normal", "VIP" });
            return View(客戶資料);
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = customerRepo.All().FirstOrDefault(x => x.Id == id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶資料 customer = customerRepo.All().FirstOrDefault(x => x.Id == id);
            customer.是否已刪除 = true;

            //連動刪除
            foreach (var bank in customer.客戶銀行資訊)
            {
                bank.是否已刪除 = true;
            }
            foreach (var contact in customer.客戶聯絡人)
            {
                contact.是否已刪除 = true;
            }

            customerRepo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        public ActionResult ToExcel()
        {
            //http://www.cnblogs.com/luwenlong/p/3614286.html
            //创建Excel文件的对象
            //IWorkbook workbook = new XSSFWorkbook(); //-- XSSF 用來產生Excel 2007檔案（.xlsx）
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

            var properties = typeof(客戶資料).GetType().GetProperties(BindingFlags.Public);

            for (int i = 0; i < properties.Length; i++)
            {
                row1.CreateCell(i).SetCellValue(properties[i].Name);
            }
            //将数据逐步写入sheet1各个行
            var datas = customerRepo.All().ToList();

            //for (int i = 0; i <= properties.Length; i++)
            //{
            //    u_sheet.CreateRow(i+1);
            //    for (int x = 0; x < properties.Length; x++)
            //    {
            //        var val = datas[i].GetType().GetProperty(properties[x].Name).GetValue(datas[i], null);

            //        u_sheet.GetRow(i+1).CreateCell(x).SetCellValue(val as string);
            //    }
            //}

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "客戶資料.xls");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                customerRepo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
