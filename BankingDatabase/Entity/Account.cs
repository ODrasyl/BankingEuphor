using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingDatabase.Entity
{
	public class Account
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public decimal Balance { get; set; }

        public UserAccount UserAccount { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
