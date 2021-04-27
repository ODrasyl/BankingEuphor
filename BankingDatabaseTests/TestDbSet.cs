using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingDatabase.Repository;
using BankingWebSite;
using Microsoft.EntityFrameworkCore;

namespace BankingDatabase.Tests
{
	public class TestDbSet<T> : DbSet<T>, IQueryable, IEnumerable<T>
		where T : class
	{
	}
}