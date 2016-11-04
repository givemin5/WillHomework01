using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace WillHomework01.Models
{   
	public  class 客戶銀行資訊Repository : EFRepository<客戶銀行資訊>, I客戶銀行資訊Repository
	{
		public override IQueryable<客戶銀行資訊> All()
		{
			return base.All().Where(x=>x.是否已刪除!=true);
		}

        internal IQueryable<客戶銀行資訊> Search(string keyword)
        {
            var banks = this.All().Include(客 => 客.客戶資料);

            if (!String.IsNullOrEmpty(keyword))
            {
                banks = banks.Where(x => x.客戶資料.客戶名稱.Contains(keyword));
            }

            return banks;
        }
    }

	public  interface I客戶銀行資訊Repository : IRepository<客戶銀行資訊>
	{

	}
}