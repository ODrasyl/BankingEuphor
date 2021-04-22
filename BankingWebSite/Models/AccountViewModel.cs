using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingWebSite.Models
{
	public class AccountViewModel
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public decimal Balance { get; set; }

        public int UserAccountId { get; set; }
        public ICollection<TransactionViewModel> Transactions { get; set; }
    }
}
