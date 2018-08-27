using System.Collections.Generic;
using WebApplication.Web.Models.Account;

namespace WebApplication.Web.Models
{
	public class Profile
    {
		public User User { get; set; }
		public IList<Report> Reports { get; set; }
		
    }
}
