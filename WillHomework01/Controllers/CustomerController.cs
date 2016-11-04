using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WillHomework01.Models;

namespace WillHomework01.Controllers
{
    

    public class CustomerController : Controller
    {
        客戶資料Repository customerRepo = RepositoryHelper.Get客戶資料Repository();

        // GET: Customer
        public ActionResult Index(string 客戶分類,string keyword="")

        {
            ViewBag.客戶分類 = new SelectList(new string[] { "OAK", "Normal", "VIP" });

            var customers = customerRepo.Search(客戶分類,keyword);

            

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
