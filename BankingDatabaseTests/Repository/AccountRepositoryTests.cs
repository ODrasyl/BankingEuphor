using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingDatabase.Repository;
using BankingDatabase.Interface;
using BankingWebSite.Models;

namespace BankingDatabase.Repository.Tests
{
	[TestClass()]
	public class AccountRepositoryTests
	{
        [TestMethod()]
        public void GetAccounts_NotNull()
        {
            //Arrange
            var context = DbInitializer.GetDatabaseContext();
            IAccountRepository accountRepository = new AccountRepository(context);

            //Act
            var account = accountRepository.GetAccounts();

            //Assert
            Assert.IsNotNull(account);
        }

        [TestMethod()]
        public void GetAccounts_VaildNumberAccount()
        {
            //Arrange
            var context = DbInitializer.GetDatabaseContext();
            IAccountRepository accountRepository = new AccountRepository(context);

            //Act
            var account = accountRepository.GetAccounts();

            //Assert
            Assert.AreEqual(account.Count(), context.Accounts.Count());
        }

        //[TestMethod]
        //public void Withdraw_ValidAmount_ChangesBalance()
        //{
        //    IAccountRepository accountRepo = new AccountRepository();
        //    // arrange
        //    double currentBalance = 10.0;
        //    double withdrawal = 1.0;
        //    double expected = 9.0;
        //    var account = new CheckingAccount("JohnDoe", currentBalance);

        //    // act
        //    account.Withdraw(withdrawal);

        //    // assert
        //    Assert.AreEqual(expected, account.Balance);
        //}

        //[TestMethod]
        //public void Withdraw_AmountMoreThanBalance_Throws()
        //{
        //    // arrange
        //    var account = new CheckingAccount("John Doe", 10.0);

        //    // act and assert
        //    Assert.ThrowsException<System.ArgumentException>(() => account.Withdraw(20.0));
        //}
    }
}