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
	public class AccountRepository : IAccountRepository
	{
		private readonly BankingDbContext _context;

		public AccountRepository(BankingDbContext context) => _context = context;

		public IQueryable<Account> GetAccounts()
		{
			IQueryable<Account> accounts = from a in _context.Accounts
										   .Include(ua => ua.UserAccount)
										   .Include(t => t.Transactions)
										   .AsNoTracking()
										   select a;

			accounts = accounts.OrderBy(a => a.Number);
			return accounts;
		}

		public async Task<Account> GetAccount(int id)
		{
			if (!AccountExists(id))
				return null;

			Account account =  await _context.Accounts
				.Include(ua => ua.UserAccount)
				.Include(t => t.Transactions)
				.AsNoTracking()
				.FirstOrDefaultAsync(m => m.Id == id);

			return account;
		}

		public bool AccountExists(int id)
		{
			return _context.Accounts.Any(e => e.Id == id);
		}
	}
}
