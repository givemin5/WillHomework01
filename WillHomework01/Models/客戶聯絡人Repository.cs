using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace WillHomework01.Models
{
    public class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
    {
        public override IQueryable<客戶聯絡人> All()
        {
            return base.All().Where(x => x.是否已刪除 != true);
        }

        internal IQueryable<客戶聯絡人> Search(string 職稱, string keyword)
        {
            var contacts = this.All().Include(客 => 客.客戶資料);

            if (!String.IsNullOrEmpty(職稱))
            {
                contacts = contacts.Where(x => x.職稱.Contains(職稱));
            }

            if (!String.IsNullOrEmpty(keyword))
            {
                contacts = contacts.Where(x => x.客戶資料.客戶名稱.Contains(keyword));
            }

            return contacts;
        }
    }

    public interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
    {

    }
}