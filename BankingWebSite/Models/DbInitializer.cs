using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingDatabase;
using BankingDatabase.Entity;
using BankingDatabase.Interface;

namespace BankingWebSite.Models
{
	public class DbInitializer
	{
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
			}
			context.SaveChanges();

			var accounts = new Account[]
			{
			new Account{Number="11564678521641",Balance=1280},
			new Account{Number="34343543453451",Balance=1520},
			new Account{Number="14354345433454",Balance=150},
			new Account{Number="15434535454545",Balance=4152},
			new Account{Number="53455923945865",Balance=1650},
			new Account{Number="45345347856631",Balance=162},
			new Account{Number="36589678536361",Balance=6152},
			new Account{Number="19546412476534",Balance=620}
			};
			foreach (Account a in accounts)
			{
				context.Accounts.Add(a);
			}
			context.SaveChanges();

			var userAccounts = new UserAccount[]
			{
			new UserAccount{UserId=1,AccountId=1},
			new UserAccount{UserId=2,AccountId=2},
			new UserAccount{UserId=3,AccountId=3},
			new UserAccount{UserId=4,AccountId=4},
			new UserAccount{UserId=5,AccountId=5},
			new UserAccount{UserId=6,AccountId=6},
			new UserAccount{UserId=7,AccountId=7},
			new UserAccount{UserId=8,AccountId=8},
			};
			foreach (UserAccount ua in userAccounts)
			{
				context.UserAccounts.Add(ua);
			}
			context.SaveChanges();

			var transactions = new Transaction[]
			{
			new Transaction{AccountId=5,Date=DateTime.Parse("2021-03-05 14:30"),Amount=-1000},
			new Transaction{AccountId=1,Date=DateTime.Parse("2021-03-05 14:30"),Amount=1000},
			new Transaction{AccountId=5,Date=DateTime.Parse("2021-05-02 11:30"),Amount=550},
			new Transaction{AccountId=8,Date=DateTime.Parse("2021-07-01 16:30"),Amount=100},
			new Transaction{AccountId=7,Date=DateTime.Parse("2021-09-01 10:30"),Amount=200},
			};
			foreach (Transaction t in transactions)
			{
				context.Transactions.Add(t);
			}
			context.SaveChanges();
		}
	}
}