using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BankingDatabase.Entity;
using BankingDatabase.Interface;

namespace BankingDatabase.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly BankingDbContext _context;

		public UserRepository(BankingDbContext context) => _context = context;

		public void EnsureDbCreated()
		{
			_context.Database.EnsureCreated();
		}
		public bool UsersAny()
		{
			return _context.Users.Any();
		}

		public IQueryable<User> GetUsers()
		{
			IQueryable<User> users = from u in _context.Users
									  .Include(ua => ua.UserAccount)
									  .AsNoTracking()
									  select u;

			users = users.OrderBy(users => users.LastName);
			return users;
		}

		public async Task<User> GetUser(int id)
		{
			if (!UserExists(id))
				return null;

			User user = await _context.Users
				.Include(ua => ua.UserAccount)
				.AsNoTracking()
				.FirstOrDefaultAsync(m => m.Id == id);

			return user;
		}

		public async Task<UserAccount> GetUserAccount(int id)
		{
			if (!UserExists(id))
				return null;

			User user = await GetUser(id);

			if (!_context.UserAccounts.Any(e => e.Id == user.UserAccount.Id))
				return null;

			UserAccount userAccount = await _context.UserAccounts
				.Include(u => u.User)
				.Include(a => a.Account)
				.AsNoTracking()
				.FirstOrDefaultAsync(m => m.Id == user.UserAccount.Id);

			return userAccount;
		}

		public async Task<bool> UpdateUser(User user)
		{
			if (user == null || !UserExists(user.Id))
				return false;

			_context.Update(user);
			await _context.SaveChangesAsync();
			return true;
		}

		public bool UserExists(int id)
		{
			return _context.Users.Any(e => e.Id == id);
		}
	}
}
