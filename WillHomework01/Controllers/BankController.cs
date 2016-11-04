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
    public class BankController : Controller
    {
        客戶銀行資訊Repository bankRepo = RepositoryHelper.Get客戶銀行資訊Repository();
        客戶資料Repository customerRepo = RepositoryHelper.Get客戶資料Repository();

        // GET: Bank
        public ActionResult Index(string keyword="")
        {
            IQueryable<客戶銀行資訊> banks = bankRepo.Search(keyword);

            return View(banks.ToList());
        }

        // GET: Bank/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = bankRepo.All().FirstOrDefault(x=>x.Id==id);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // GET: Bank/Create
        public ActionResult Create()
        {
            ViewBag.客戶Id = new SelectList(customerRepo.Where(x => x.是否已刪除 == false), "Id", "客戶名稱");
            return View();
        }

        // POST: Bank/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                bankRepo.Add(客戶銀行資訊);
                bankRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(customerRepo.Where(x => x.是否已刪除 == false), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: Bank/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = bankRepo.All().FirstOrDefault(x=>x.Id==id);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(customerRepo.Where(x => x.是否已刪除 == false), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // POST: Bank/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                var db = bankRepo.UnitOfWork.Context;
                db.Entry(客戶銀行資訊).State = EntityState.Modified;
                bankRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            ViewBag.客戶Id = new SelectList(customerRepo.All(), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: Bank/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = bankRepo.All().FirstOrDefault(x => x.Id == id);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            
            return View(客戶銀行資訊);
        }

        // POST: Bank/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var db = bankRepo.UnitOfWork.Context;
            客戶銀行資訊 客戶銀行資訊 = bankRepo.All().FirstOrDefault(x => x.Id == id);
            客戶銀行資訊.是否已刪除 = true;
            db.SaveChanges();
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
