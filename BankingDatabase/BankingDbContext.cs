using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BankingDatabase.Entity;

namespace BankingDatabase
{
	public class BankingDbContext : DbContext
	{
		public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options)
		{
		}

		public DbSet<User> Users { get; set; }
		public DbSet<Account> Accounts { get; set; }
		public DbSet<UserAccount> UserAccounts { get; set; }
		public DbSet<Transaction> Transactions { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>().ToTable("User");
			modelBuilder.Entity<Account>().ToTable("Account");
			modelBuilder.Entity<UserAccount>().ToTable("UserAccount");
			modelBuilder.Entity<Transaction>().ToTable("Transaction");
		}
	}
	//TODO Suppr BankingDbManager
	/*
	public class BankingDbManager
	{
		public enum UsersIncludeType
		{
			UserAccount,
			None
		}

		public enum AccountsIncludeType
		{
			Transactions,
			UserAccount,
			All,
			None
		}

		public enum UserAccountsIncludeType
		{
			User,
			Account,
			All,
			None
		}

		public enum TransactionsIncludeType
		{
			Account,
			None
		}

		public enum TransactionType
		{
			Send,
			Received
		}

		private readonly BankingDbContext _context;

		public BankingDbManager(BankingDbContext context)
		{
			_context = context;
		}

		public void EnsureDbCreated() //TODO make async
		{
			_context.Database.EnsureCreated();
		}

		public bool UsersAny() //TODO make async
		{
			return _context.Users.Any();
		}

		// Users Bdd interaction
		public IQueryable<User> GetUsers(UsersIncludeType type = UsersIncludeType.None) //TODO make async
		{
			IQueryable<User> users = type switch
			{
				UsersIncludeType.UserAccount => from u in _context.Users
											   .Include(ua => ua.UserAccount)
											   .AsNoTracking()
											   select u,
				UsersIncludeType.None => from u in _context.Users
										select u,
				_ => from u in _context.Users
					 select u,
			};
			users = users.OrderBy(users => users.LastName);
			return users;
		}

		public async Task<User> GetUser(int? id, UsersIncludeType type = UsersIncludeType.None)
		{
			if (id == null)
			{
				return null;
			}
			User user = type switch
			{
				UsersIncludeType.UserAccount => await _context.Users
											   .Include(ua => ua.UserAccount)
											   .AsNoTracking()
											   .FirstOrDefaultAsync(m => m.Id == id),
				UsersIncludeType.None => await _context.Users
										.FirstOrDefaultAsync(m => m.Id == id),
				_ => await _context.Users
					 .FirstOrDefaultAsync(m => m.Id == id),
			};
			return user;
		}

		public async Task<bool> UpdateUser(User user)
		{
			_context.Update(user);
			await _context.SaveChangesAsync();
			return true;
		}

		public bool UserExists(int id)
		{
			return _context.Users.Any(e => e.Id == id);
		}

		// Accounts Bdd interaction
		public IQueryable<Account> GetAccounts(AccountsIncludeType type = AccountsIncludeType.None) //TODO make async
		{
			IQueryable<Account> accounts = type switch
			{
				AccountsIncludeType.All => from a in _context.Accounts
										   .Include(ua => ua.UserAccount)
										   .Include(t => t.Transactions)
										   .AsNoTracking()
										   select a,
				AccountsIncludeType.UserAccount => from a in _context.Accounts
													.Include(ua => ua.UserAccount)
													.AsNoTracking()
													select a,
				AccountsIncludeType.Transactions => from a in _context.Accounts
													.Include(t => t.Transactions)
													.AsNoTracking()
													select a,
				AccountsIncludeType.None => from a in _context.Accounts
											select a,
				_ => from a in _context.Accounts
					 select a,
			};
			accounts = accounts.OrderBy(a => a.Number);
			return accounts;
		}

		public async Task<Account> GetAccount(int? id, AccountsIncludeType type = AccountsIncludeType.None)
		{
			if (id == null)
				return null;

			Account account = type switch
			{
				AccountsIncludeType.All => await _context.Accounts
													.Include(ua => ua.UserAccount)
													.Include(t => t.Transactions)
													.AsNoTracking()
													.FirstOrDefaultAsync(m => m.Id == id),
				AccountsIncludeType.UserAccount => await _context.Accounts
													.Include(ua => ua.UserAccount)
													.AsNoTracking()
													.FirstOrDefaultAsync(m => m.Id == id),
				AccountsIncludeType.Transactions => await _context.Accounts
													.Include(t => t.Transactions)
													.AsNoTracking()
													.FirstOrDefaultAsync(m => m.Id == id),
				AccountsIncludeType.None => await _context.Accounts
											.FirstOrDefaultAsync(m => m.Id == id),
				_ => await _context.Accounts
					 .FirstOrDefaultAsync(m => m.Id == id),
			};
			return account;
		}

		public bool AccountExists(int? id)
		{
			if (id == null)
				return false;
			return _context.Accounts.Any(e => e.Id == id);
		}

		// UserAccounts Bdd interaction
		public IQueryable<UserAccount> GetUserAccounts(UserAccountsIncludeType type = UserAccountsIncludeType.None) //TODO make async
		{
			IQueryable<UserAccount> userAccounts = type switch
			{
				UserAccountsIncludeType.User => from ua in _context.UserAccounts
												.Include(u => u.User)
												.AsNoTracking()
												select ua,
				UserAccountsIncludeType.Account => from ua in _context.UserAccounts
												   .Include(a => a.Account)
 												   .AsNoTracking()
												   select ua,
				UserAccountsIncludeType.All => from ua in _context.UserAccounts
											   .Include(u => u.User)
											   .Include(a => a.Account)
											   .AsNoTracking()
											   select ua,
				UserAccountsIncludeType.None => from ua in _context.UserAccounts
												select ua,
				_ => from ua in _context.UserAccounts
					 select ua,
			};
			return userAccounts;
		}

		public async Task<UserAccount> GetUserAccount(int? id, UserAccountsIncludeType type = UserAccountsIncludeType.None)
		{
			if (id == null)
				return null;

			UserAccount userAccount = type switch
			{
				UserAccountsIncludeType.User => await _context.UserAccounts
												.Include(u => u.User)
												.AsNoTracking()
												.FirstOrDefaultAsync(m => m.Id == id),
				UserAccountsIncludeType.Account => await _context.UserAccounts
												   .Include(a => a.Account)
												   .AsNoTracking()
												   .FirstOrDefaultAsync(m => m.Id == id),
				UserAccountsIncludeType.All => await _context.UserAccounts
											   .Include(u => u.User)
											   .Include(a => a.Account)
											   .AsNoTracking()
											   .FirstOrDefaultAsync(m => m.Id == id),
				UserAccountsIncludeType.None => await _context.UserAccounts
												.FirstOrDefaultAsync(m => m.Id == id),
				_ => await _context.UserAccounts
					 .FirstOrDefaultAsync(m => m.Id == id),
			};
			return userAccount;
		}

		public async Task<int> CreateNewAccount(User user)
		{

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

		public async Task<bool> DeletUser(int id)
		{
			var user = await GetUser(id, BankingDbManager.UsersIncludeType.UserAccount);
			var userAccount = await GetUserAccount(user.UserAccount.Id, BankingDbManager.UserAccountsIncludeType.Account);
			var account = await GetAccount(userAccount.Account.Id);
			_context.Users.Remove(user);
			_context.Accounts.Remove(account);
			await _context.SaveChangesAsync();
			return true;
		}
		public async Task<bool> DeletAccount(int id)
		{
			var account = await GetAccount(id, BankingDbManager.AccountsIncludeType.All);
			var userAccount = await GetUserAccount(account.UserAccount.Id, BankingDbManager.UserAccountsIncludeType.User);
			var user = await GetUser(userAccount.User.Id);
			_context.Users.Remove(user);
			_context.Accounts.Remove(account);
			await _context.SaveChangesAsync();
			return true;
		}
		public async Task<bool> DeletUserAccount(int id)
		{
			var userAccount = await GetUserAccount(id, BankingDbManager.UserAccountsIncludeType.All);
			var user = await GetUser(userAccount.User.Id, BankingDbManager.UsersIncludeType.UserAccount);
			var account = await GetAccount(userAccount.Account.Id);
			_context.Users.Remove(user);
			_context.Accounts.Remove(account);
			await _context.SaveChangesAsync();
			return true;
		}

		public bool UserAccountExists(int id)
		{
			return _context.UserAccounts.Any(e => e.Id == id);
		}

		// Transactions Bdd interaction
		public IQueryable<Transaction> GetTransactions(TransactionsIncludeType type = TransactionsIncludeType.None) //TODO make async
		{
			IQueryable<Transaction> transactions = type switch
			{
				TransactionsIncludeType.Account => from t in _context.Transactions
												   .Include(t => t.Account)
												   select t,
				TransactionsIncludeType.None => from t in _context.Transactions
												select t,
				_ => from t in _context.Transactions
					 select t,
			};

			transactions = transactions.OrderBy(t => t.Date);
			return transactions;
		}

		public async Task<Transaction> GetTransaction(int? id, TransactionsIncludeType type = TransactionsIncludeType.None)
		{
			if (id == null)
				return null;
			Transaction transaction = type switch
			{
				TransactionsIncludeType.Account => await _context.Transactions
												   .Include(t => t.Account)
												   .FirstOrDefaultAsync(m => m.Id == id),
				TransactionsIncludeType.None => await _context.Transactions
												.FirstOrDefaultAsync(m => m.Id == id),
				_ => await _context.Transactions
					 .FirstOrDefaultAsync(m => m.Id == id),
			};
			return transaction;
		}

		public async Task<int> CreateNewTransaction(Transaction transaction, TransactionType type = TransactionType.Send)
		{
			transaction.Amount = ((type == TransactionType.Received) ? Math.Abs(transaction.Amount) : -Math.Abs(transaction.Amount));
			_context.Add(transaction);

			var account = await _context.Accounts.FindAsync(transaction.AccountId);
			account.Balance += transaction.Amount;
			_context.Update(account);

			await _context.SaveChangesAsync();
			return transaction.Id;
		}

		public async Task<int> DeletTransaction(int id)
		{
			var transaction = await GetTransaction(id);
			var accountId = transaction.AccountId;
			_context.Transactions.Remove(transaction);
			await _context.SaveChangesAsync();
			return accountId;
		}

		public bool TransactionExists(int? id)
		{
			if (id == null)
				return false;
			return _context.Transactions.Any(e => e.Id == id);
		}
	}
	*/
}