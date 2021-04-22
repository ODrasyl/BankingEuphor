using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingDatabase.Entity
{
	public class Transaction
	{
		public int Id { get; set; }
		public int AccountId{ get; set; }
		public DateTime Date { get; set; }
		public decimal Amount { get; set; }

		public Account Account { get; set; }
	}
}
