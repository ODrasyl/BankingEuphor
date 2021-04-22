using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingWebSite.Models
{
	public class TransactionViewModel
	{
		public int Id { get; set; }
		public int AccountId { get; set; }
		public DateTime Date { get; set; }
		public decimal Amount { get; set; }

		public string AccountNumber { get; set; }

		public bool IsReceived { get; set; }
	}
}
