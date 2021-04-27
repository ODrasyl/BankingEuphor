using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingDatabase;
using Microsoft.EntityFrameworkCore;
using BankingDatabase.Entity;
using BankingDatabase.Interface;

namespace BankingWebSite.Models
{
	public class DbInitializer
	{
		public static BankingDbContext GetDatabaseContext()
		{
			var options = new DbContextOptionsBuilder<BankingDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;
			var context = new BankingDbContext(options);
			Initialize(context);
			return context;
		}

		public static void Initialize(IUserRepository userRepository, IUserAccountRepository userAccountRepository, ITransactionRepository transactionRepository)
		{
			userRepository.EnsureDbCreated();

			// Look for any students.
			if (userRepository.UsersAny())
				return;   // DB has been seeded

			var users = new UserViewModel[]
			{
			new UserViewModel{FirstName="Carson",LastName="Alexander",Birthday=DateTime.Parse("1985-09-01"), Address="180 chemin de la grange, 01360 Béligneux"},
			new UserViewModel{FirstName="Meredith",LastName="Alonso",Birthday=DateTime.Parse("1986-09-01"), Address="181 chemin de la grange, 01360 Béligneux"},
			new UserViewModel{FirstName="Arturo",LastName="Anand",Birthday=DateTime.Parse("1978-09-01"), Address="182 chemin de la grange, 01360 Béligneux"},
			new UserViewModel{FirstName="Gytis",LastName="Barzdukas",Birthday=DateTime.Parse("1961-09-01"), Address="183 chemin de la grange, 01360 Béligneux"},
			new UserViewModel{FirstName="Yan",LastName="Li",Birthday=DateTime.Parse("1996-09-01"), Address="184 chemin de la grange, 01360 Béligneux"},
			new UserViewModel{FirstName="Peggy",LastName="Justice",Birthday=DateTime.Parse("1973-09-01"), Address="185 chemin de la grange, 01360 Béligneux"},
			new UserViewModel{FirstName="Laura",LastName="Norman",Birthday=DateTime.Parse("1979-09-01"), Address="186 chemin de la grange, 01360 Béligneux"},
			new UserViewModel{FirstName="Nino",LastName="Olivetto",Birthday=DateTime.Parse("1985-09-01"), Address="187 chemin de la grange, 01360 Béligneux"}
			};
			foreach (UserViewModel u in users)
			{
				var user = Tools.ConvertUser(u);
				_ = userAccountRepository.CreateNewAccount(user);
			}

			var transactions = new TransactionViewModel[]
			{
			new TransactionViewModel{AccountId=1,Date=DateTime.Parse("2021-03-05 14:30"),Amount=5641},
			new TransactionViewModel{AccountId=2,Date=DateTime.Parse("2021-03-05 14:30"),Amount=752},
			new TransactionViewModel{AccountId=3,Date=DateTime.Parse("2021-05-02 11:30"),Amount=1887},
			new TransactionViewModel{AccountId=4,Date=DateTime.Parse("2021-07-01 16:30"),Amount=625},
			new TransactionViewModel{AccountId=5,Date=DateTime.Parse("2021-09-01 10:30"),Amount=986},
			new TransactionViewModel{AccountId=6,Date=DateTime.Parse("2021-05-02 11:30"),Amount=12000},
			new TransactionViewModel{AccountId=7,Date=DateTime.Parse("2021-07-01 16:30"),Amount=2547},
			new TransactionViewModel{AccountId=8,Date=DateTime.Parse("2021-09-01 10:30"),Amount=120},
			};
			foreach (TransactionViewModel t in transactions)
			{
				var transaction = Tools.ConvertTransaction(t);
				_ = transactionRepository.CreateNewTransaction(transaction, true);
			}
		}

		public static void Initialize(BankingDbContext context)
		{
			context.Database.EnsureCreated();

			// Look for any students.
			if (context.Users.Any())
			{
				return;   // DB has been seeded
			}

			var users = new User[]
			{
			new User{FirstName="Carson",LastName="Alexander",Birthday=DateTime.Parse("1985-09-01"), Address="180 chemin de la grange, 01360 Béligneux"},
			new User{FirstName="Meredith",LastName="Alonso",Birthday=DateTime.Parse("1986-09-01"), Address="181 chemin de la grange, 01360 Béligneux"},
			new User{FirstName="Arturo",LastName="Anand",Birthday=DateTime.Parse("1978-09-01"), Address="182 chemin de la grange, 01360 Béligneux"},
			new User{FirstName="Gytis",LastName="Barzdukas",Birthday=DateTime.Parse("1961-09-01"), Address="183 chemin de la grange, 01360 Béligneux"},
			new User{FirstName="Yan",LastName="Li",Birthday=DateTime.Parse("1996-09-01"), Address="184 chemin de la grange, 01360 Béligneux"},
			new User{FirstName="Peggy",LastName="Justice",Birthday=DateTime.Parse("1973-09-01"), Address="185 chemin de la grange, 01360 Béligneux"},
			new User{FirstName="Laura",LastName="Norman",Birthday=DateTime.Parse("1979-09-01"), Address="186 chemin de la grange, 01360 Béligneux"},
			new User{FirstName="Nino",LastName="Olivetto",Birthday=DateTime.Parse("1985-09-01"), Address="187 chemin de la grange, 01360 Béligneux"}
			};
			foreach (User u in users)
			{
				context.Users.Add(u);

				var account = new Account
				{
					Number = CreateNewAccountNumber(context),
					Balance = 0
				};
				context.Accounts.Add(account);

				context.SaveChanges();

				var userAccount = new UserAccount
				{
					UserId = u.Id,
					AccountId = account.Id
				};
				context.UserAccounts.Add(userAccount);
			}
			context.SaveChanges();

			var transactions = new Transaction[]
			{
			new Transaction{AccountId=1,Date=DateTime.Parse("2021-03-05 14:30"),Amount=5641},
			new Transaction{AccountId=2,Date=DateTime.Parse("2021-03-05 14:30"),Amount=752},
			new Transaction{AccountId=3,Date=DateTime.Parse("2021-05-02 11:30"),Amount=1887},
			new Transaction{AccountId=4,Date=DateTime.Parse("2021-07-01 16:30"),Amount=625},
			new Transaction{AccountId=5,Date=DateTime.Parse("2021-09-01 10:30"),Amount=986},
			new Transaction{AccountId=6,Date=DateTime.Parse("2021-05-02 11:30"),Amount=12000},
			new Transaction{AccountId=7,Date=DateTime.Parse("2021-07-01 16:30"),Amount=2547},
			new Transaction{AccountId=8,Date=DateTime.Parse("2021-09-01 10:30"),Amount=120},

			new Transaction{AccountId=5,Date=DateTime.Parse("2021-03-05 14:30"),Amount=-1000},
			new Transaction{AccountId=1,Date=DateTime.Parse("2021-03-05 14:30"),Amount=1000},
			new Transaction{AccountId=5,Date=DateTime.Parse("2021-05-02 11:30"),Amount=550},
			new Transaction{AccountId=8,Date=DateTime.Parse("2021-07-01 16:30"),Amount=100},
			new Transaction{AccountId=7,Date=DateTime.Parse("2021-09-01 10:30"),Amount=200},
			};
			foreach (Transaction t in transactions)
			{
				context.Transactions.Add(t);

				var account = context.Accounts
					.FirstOrDefault(m => m.Id == t.AccountId);
				account.Balance += t.Amount;
				context.Accounts.Update(account);
			}
			context.SaveChanges();
		}

		private static string CreateNewAccountNumber(BankingDbContext context)
		{
			var r = new Random();
			string accountNumber;
			while (CheckAccountNumber(accountNumber = ((long)(r.NextDouble() * 100000000000000)).ToString(), context)) ;
			return accountNumber;
		}
		private static bool CheckAccountNumber(string accountNumber, BankingDbContext context)
		{
			foreach (var num in context.Accounts)
			{
				if (accountNumber == num.Number)
					return true;
			}
			return false;
		}
	}
}