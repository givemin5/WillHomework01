using System;
using System.Linq;
using System.Collections.Generic;
	
namespace WillHomework01.Models
{   
	public  class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
	{
		public override IQueryable<客戶資料> All()
		{
			return base.All().Where(x => x.是否已刪除 != true);
		}
	}

	public  interface I客戶資料Repository : IRepository<客戶資料>
	{
		
	}
}