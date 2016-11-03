using System;
using System.Linq;
using System.Collections.Generic;
	
namespace WillHomework01.Models
{   
	public  class vwCustomerListRepository : EFRepository<vwCustomerList>, IvwCustomerListRepository
	{

	}

	public  interface IvwCustomerListRepository : IRepository<vwCustomerList>
	{

	}
}