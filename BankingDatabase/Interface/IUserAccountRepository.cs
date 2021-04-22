using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingDatabase.Entity;

namespace BankingDatabase.Interface
{
	public interface IUserAccountRepository
	{
		public IQueryable<UserAccount> GetUserAccounts(); //TODO make async
		public Task<UserAccount> GetUserAccount(int id);
		public Task<User> GetUser(int id);
		public Task<Account> GetAccount(int id);
		public Task<int> CreateNewAccount(User user);
		public Task<bool> DeletUserAccount(int id);
		public bool UserAccountExists(int id);
	}
}
