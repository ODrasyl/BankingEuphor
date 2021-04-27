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
	public class UserRepositoryTests
	{
		[TestMethod()]
		public void GetUsers_NotNull()
		{
			//Arrange
			var context = DbInitializer.GetDatabaseContext();
			IUserRepository userRepository = new UserRepository(context);

			//Act
			var users = userRepository.GetUsers();

			//Assert
			Assert.IsNotNull(users);
		}
	}
}