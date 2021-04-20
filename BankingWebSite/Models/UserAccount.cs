using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingWebSite.Models
{
	public class UserAccount
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public int AccountId { get; set; }

		public User User { get; set; }
		public Account Account { get; set; }
	}
}
