using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingDatabase.Entity;

namespace BankingDatabase.Interface
{
	public interface ITransactionRepository
	{
		public IQueryable<Transaction> GetTransactions(); //TODO make async
		public Task<Transaction> GetTransaction(int id);
		public Task<Account> GetAccount(int id);
		public Task<Account> GetAccountWithAccountId(int id);
		public Task<int> CreateNewTransaction(Transaction transaction, bool isReceived = false);
		public Task<int> DeletTransaction(int id);
		public bool TransactionExists(int id);
		public bool AccountExists(int id);
	}
}
