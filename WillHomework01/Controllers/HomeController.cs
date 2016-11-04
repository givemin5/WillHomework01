using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WillHomework01.Models;

namespace WillHomework01.Controllers
{
    public class HomeController : Controller
    {
        
        // GET: Home
        public ActionResult Index(string keyword="")
        {
            vwCustomerListRepository vwRepo = RepositoryHelper.GetvwCustomerListRepository();

            var customerList = vwRepo.All().AsQueryable();

            if (!String.IsNullOrEmpty(keyword))
            {
                customerList = customerList.Where(x => x.客戶名稱.Contains(keyword));
            }


            return View(customerList);
        }
    }
}