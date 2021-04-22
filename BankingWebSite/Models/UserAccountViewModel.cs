using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingWebSite.Models
{
	public class UserAccountViewModel
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public int AccountId { get; set; }

		public UserViewModel User { get; set; }
		public string AccountNumber { get; set; }
	}
}
