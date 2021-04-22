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
	public class UserAccountRepository : IUserAccountRepository
	{
		private readonly BankingDbContext _context;

		public UserAccountRepository(BankingDbContext context) => _context = context;

		public IQueryable<UserAccount> GetUserAccounts()
		{
			IQueryable<UserAccount> userAccounts = from ua in _context.UserAccounts
													.Include(u => u.User)
													.Include(a => a.Account)
													.AsNoTracking()
													select ua;

			return userAccounts;
		}

		public async Task<UserAccount> GetUserAccount(int id)
		{
			if (!UserAccountExists(id))
				return null;

			UserAccount userAccount = await _context.UserAccounts
				.Include(u => u.User)
				.Include(a => a.Account)
				.AsNoTracking()
				.FirstOrDefaultAsync(m => m.Id == id);

			return userAccount;
		}

		public async Task<User> GetUser(int id)
		{
			if (!UserAccountExists(id))
				return null;

			UserAccount userAccount = await GetUserAccount(id);

			if (!_context.Users.Any(e => e.Id == userAccount.UserId))
				return null;

			User user = await _context.Users
				.Include(ua => ua.UserAccount)
				.AsNoTracking()
				.FirstOrDefaultAsync(m => m.Id == userAccount.UserId);

			return user;
		}

		public async Task<Account> GetAccount(int id)
		{
			if (!UserAccountExists(id))
				return null;

			UserAccount userAccount = await GetUserAccount(id);

			if (!_context.Accounts.Any(e => e.Id == userAccount.AccountId))
				return null;

			Account account = await _context.Accounts
				.Include(t => t.Transactions)
				.AsNoTracking()
				.FirstOrDefaultAsync(m => m.Id == userAccount.AccountId);

			return account;
		}

		public async Task<int> CreateNewAccount(User user)
		{
			if (user == null || UserAccountExists(user.Id))
				return 0;

			_context.Add(user);

			var account = new Account
			{
				Number = CreateNewAccountNumber(),
				Balance = 0
			};
			_context.Add(account);

			await _context.SaveChangesAsync();

			var userAccount = new UserAccount
			{
				UserId = user.Id,
				AccountId = account.Id
			};
			_context.Add(userAccount);

			await _context.SaveChangesAsync();
			
			if (!UserAccountExists(userAccount.Id) ||
				!_context.Users.Any(e => e.Id == user.Id) ||
				!_context.Accounts.Any(e => e.Id == account.Id))
			{
				if (_context.Users.Any(e => e.Id == user.Id))
					_context.Users.Remove(user);
				if (_context.Accounts.Any(e => e.Id == account.Id))
					_context.Accounts.Remove(account);
				await _context.SaveChangesAsync();
				if (UserAccountExists(userAccount.Id))
				{
					_context.UserAccounts.Remove(userAccount);
					await _context.SaveChangesAsync();
				}
				return 0;
			}
			return userAccount.Id;
		}
		private string CreateNewAccountNumber()
		{
			var r = new Random();
			string accountNumber;
			while (CheckAccountNumber(accountNumber = ((long)(r.NextDouble() * 100000000000000)).ToString())) ;
			return accountNumber;
		}
		private bool CheckAccountNumber(string accountNumber)
		{
			foreach (var num in _context.Accounts)
			{
				if (accountNumber == num.Number)
					return true;
			}
			return false;
		}

		public async Task<bool> DeletUserAccount(int id)
		{
			if (!UserAccountExists(id))
				return false;

			var userAccount = await GetUserAccount(id);
			var user = await _context.Users
				.FirstOrDefaultAsync(m => m.Id == userAccount.User.Id);
			var account = await _context.Accounts
				.FirstOrDefaultAsync(m => m.Id == userAccount.Account.Id);
			_context.Users.Remove(user);
			_context.Accounts.Remove(account);
			await _context.SaveChangesAsync();
			if (UserAccountExists(userAccount.Id))
			{
				_context.UserAccounts.Remove(userAccount);
				await _context.SaveChangesAsync();
			}
			if (_context.Accounts.Any(e => e.Id == account.Id))
				return false;
			return true;
		}

		public bool UserAccountExists(int id)
		{
			return _context.UserAccounts.Any(e => e.Id == id);
		}
	}
}
