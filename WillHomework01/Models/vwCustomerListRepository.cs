using System;
using System.Linq;
using System.Collections.Generic;
	
namespace WillHomework01.Models
{
    public class vwCustomerListRepository : EFRepository<vwCustomerList>, IvwCustomerListRepository
    {
        internal IQueryable<vwCustomerList> Search(string keyword)
        {
            var customerList = this.All().AsQueryable();

            if (!String.IsNullOrEmpty(keyword))
            {
            //    customerList = customerList.Where(x => x.�Ȥ�W��.Contains(keyword));
            }

            return customerList;
        }
    }

    public  interface IvwCustomerListRepository : IRepository<vwCustomerList>
	{

	}
}