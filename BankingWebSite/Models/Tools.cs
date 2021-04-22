using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingDatabase.Entity;

namespace BankingWebSite.Models
{
	public class Tools
	{
		public static User ConvertUser(UserViewModel userViewModel)
		{
			if (userViewModel == null)
				return null;

			var user = new User
			{
				Id = userViewModel.Id,
				FirstName = userViewModel.FirstName,
				LastName = userViewModel.LastName,
				Birthday = userViewModel.Birthday,
				Address = userViewModel.Address
			};
			return user;
		}
		public static UserViewModel ConvertUser(User user)
		{
			if (user == null)
				return null;

			var userViewModel = new UserViewModel
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Birthday = user.Birthday,
				Address = user.Address,
			};

			userViewModel.UserAccountId = ((user.UserAccount != null) ? user.UserAccount.Id : 0);
			return userViewModel;
		}
		public static List<UserViewModel> ConvertUsers(List<User> users)
		{
			if (users == null)
				return null;

			var usersViewModel = new List<UserViewModel>();
			foreach (User u in users)
			{
				usersViewModel.Add(Tools.ConvertUser(u));
			}
			return usersViewModel;
		}

		public static Account ConvertAccount(AccountViewModel accountViewModel)
		{
			if (accountViewModel == null)
				return null;

			var account = new Account
			{
				Id = accountViewModel.Id,
				Number = accountViewModel.Number,
				Balance = accountViewModel.Balance
			};
			return account;
		}
		public static AccountViewModel ConvertAccount(Account account)
		{
			if (account == null)
				return null;

			var accountViewModel = new AccountViewModel
			{
				Id = account.Id,
				Number = account.Number,
				Balance = account.Balance
			};

			accountViewModel.UserAccountId = ((account.UserAccount != null) ? account.UserAccount.Id : 0); ;

			if (account.Transactions != null)
			{
				accountViewModel.Transactions = new List<TransactionViewModel> { };
				foreach (Transaction t in account.Transactions)
				{
					accountViewModel.Transactions.Add(ConvertTransaction(t));
				}
			}
			return accountViewModel;
		}
		public static List<AccountViewModel> ConvertAccounts(List<Account> accounts)
		{
			if (accounts == null)
				return null;

			var accountsViewModel = new List<AccountViewModel>();
			foreach (Account a in accounts)
			{
				accountsViewModel.Add(Tools.ConvertAccount(a));
			}
			return accountsViewModel;
		}

		public static UserAccount ConvertUserAccount(UserAccountViewModel userAccountViewModel)
		{
			if (userAccountViewModel == null)
				return null;

			var userAccount = new UserAccount
			{
				Id = userAccountViewModel.Id,
				UserId = userAccountViewModel.UserId,
				AccountId = userAccountViewModel.AccountId
			};
			return userAccount;
		}
		public static UserAccountViewModel ConvertUserAccount(UserAccount userAccount)
		{
			if (userAccount == null)
				return null;

			var userAccountViewModel = new UserAccountViewModel
			{
				Id = userAccount.Id,
				UserId = userAccount.UserId,
				AccountId = userAccount.AccountId
			};

			userAccountViewModel.User = ((userAccount.User != null) ? ConvertUser(userAccount.User) : null);
			userAccountViewModel.AccountNumber = (userAccount.Account?.Number);
			return userAccountViewModel;
		}
		public static List<UserAccountViewModel> ConvertUserAccounts(List<UserAccount> userAccounts)
		{
			if (userAccounts == null)
				return null;

			var useraccountsViewModel = new List<UserAccountViewModel>();
			foreach (UserAccount ua in userAccounts)
			{
				useraccountsViewModel.Add(Tools.ConvertUserAccount(ua));
			}
			return useraccountsViewModel;
		}

		public static Transaction ConvertTransaction(TransactionViewModel transactionViewModel)
		{
			if (transactionViewModel == null)
				return null;

			var transaction = new Transaction
			{
				Id = transactionViewModel.Id,
				AccountId = transactionViewModel.AccountId,
				Date = transactionViewModel.Date,
				Amount = transactionViewModel.Amount
			};
			return transaction;
		}

		public static TransactionViewModel ConvertTransaction(Transaction transaction)
		{
			if (transaction == null)
				return null;

			var transactionViewModel = new TransactionViewModel
			{
				Id = transaction.Id,
				AccountId = transaction.AccountId,
				Date = transaction.Date,
				Amount = transaction.Amount
			};

			transactionViewModel.AccountNumber = (transaction.Account?.Number);
			return transactionViewModel;
		}
		public static List<TransactionViewModel> ConvertTransactions(List<Transaction> transactions)
		{
			if (transactions == null)
				return null;

			var transactionsViewModel = new List<TransactionViewModel>();
			foreach (Transaction t in transactions)
			{
				transactionsViewModel.Add(Tools.ConvertTransaction(t));
			}
			return transactionsViewModel;
		}
	}
}
