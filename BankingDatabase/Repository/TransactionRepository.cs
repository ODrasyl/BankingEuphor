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
	public class TransactionRepository : ITransactionRepository
	{
		private readonly BankingDbContext _context;

		public TransactionRepository(BankingDbContext context) => _context = context;

		public IQueryable<Transaction> GetTransactions()
		{
			IQueryable<Transaction> transactions = from t in _context.Transactions
												   .Include(t => t.Account)
												   select t;

			transactions = transactions.OrderBy(t => t.Date);
			return transactions;
		}

		public async Task<Transaction> GetTransaction(int id)
		{
			if (!TransactionExists(id))
				return null;

			Transaction transaction = await _context.Transactions
				.Include(t => t.Account)
				.FirstOrDefaultAsync(m => m.Id == id);

			return transaction;
		}

		public async Task<Account> GetAccount(int id)
		{
			if (!TransactionExists(id))
				return null;

			Transaction transaction = await _context.Transactions
				.Include(t => t.Account)
				.FirstOrDefaultAsync(m => m.Id == id);
			
			if (!AccountExists(transaction.AccountId))
				return null;

			Account account = await _context.Accounts
				.Include(t => t.Transactions)
				.FirstOrDefaultAsync(m => m.Id == transaction.AccountId);

			return account;
		}

		public async Task<Account> GetAccountWithAccountId(int id)
		{
			if (!AccountExists(id))
				return null;

			Account account = await _context.Accounts
				.Include(t => t.Transactions)
				.FirstOrDefaultAsync(m => m.Id == id);

			return account;
		}

		public async Task<int> CreateNewTransaction(Transaction transaction, bool isReceived = false)
		{
			if (transaction == null)
				return 0;

			transaction.Amount = ((isReceived) ? Math.Abs(transaction.Amount) : -Math.Abs(transaction.Amount));
			_context.Add(transaction);

			var account = await _context.Accounts.FindAsync(transaction.AccountId);
			account.Balance += transaction.Amount;
			_context.Update(account);

			await _context.SaveChangesAsync();

			if (!TransactionExists(transaction.Id))
				return 0;
			return transaction.Id;
		}

		public async Task<int> DeletTransaction(int id)
		{
			if (!TransactionExists(id))
				return 0;

			var transaction = await GetTransaction(id);
			var accountId = transaction.AccountId;
			_context.Transactions.Remove(transaction);
			await _context.SaveChangesAsync();

			if (TransactionExists(id))
				return 0;
			return accountId;
		}

		public bool TransactionExists(int id)
		{
			return _context.Transactions.Any(e => e.Id == id);
		}

		public bool AccountExists(int id)
		{
			return _context.Accounts.Any(e => e.Id == id);
		}
	}
}
