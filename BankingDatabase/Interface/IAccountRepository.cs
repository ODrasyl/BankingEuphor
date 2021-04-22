using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingDatabase.Entity;

namespace BankingDatabase.Interface
{
	public interface IAccountRepository
	{
		public IQueryable<Account> GetAccounts(); //TODO make async
		public Task<Account> GetAccount(int id);
		public bool AccountExists(int id);
	}
}
