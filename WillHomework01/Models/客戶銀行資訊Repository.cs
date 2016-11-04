using System;
using System.Linq;
using System.Collections.Generic;
	
namespace WillHomework01.Models
{   
	public  class 客戶銀行資訊Repository : EFRepository<客戶銀行資訊>, I客戶銀行資訊Repository
	{
		public override IQueryable<客戶銀行資訊> All()
		{
			return base.All().Where(x=>x.是否已刪除!=true);
		}
	}

	public  interface I客戶銀行資訊Repository : IRepository<客戶銀行資訊>
	{

	}
}