using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingDatabase.Entity;

namespace BankingDatabase.Interface
{
	public interface IUserRepository
	{
		public void EnsureDbCreated();
		public bool UsersAny();
		public IQueryable<User> GetUsers(); //TODO make async
		public Task<User> GetUser(int id);
		public Task<UserAccount> GetUserAccount(int id);
		public Task<bool> UpdateUser(User user);
		public bool UserExists(int id);
	}
}
