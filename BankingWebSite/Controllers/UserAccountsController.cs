using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BankingWebSite.Models;
using System.Diagnostics;

namespace BankingWebSite.Controllers
{
	public class UserAccountsController : Controller
	{
		private readonly BankingContext _context;

		public UserAccountsController(BankingContext context)
		{
			_context = context;
		}

		// GET: UserAccounts
		public async Task<IActionResult> Index()
		{
			var bankingContext = _context.UserAccounts.Include(u => u.Account).Include(u => u.User);
			return View(await bankingContext.ToListAsync());
		}

		// GET: UserAccounts/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var userAccount = await _context.UserAccounts
				.Include(u => u.Account)
				.Include(u => u.User)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (userAccount == null)
			{
				return NotFound();
			}

			return View(userAccount);
		}

		// GET: UserAccounts/NewAccount
		public IActionResult NewAccount()
		{
			return View();
		}

		// POST: UserAccounts/NewAccount
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> NewAccount([Bind("Id,FirstName,LastName,Birthday,Address")] User user)
		{
			if (ModelState.IsValid)
			{
				_context.Add(user);
				var account = new Account();
				account.Number = CreateNewAccountNumber();
				account.Balance = 0.0f;
				_context.Add(account);
				await _context.SaveChangesAsync();
				var userAccount = new UserAccount();
				userAccount.UserId = user.Id;
				userAccount.AccountId = account.Id;
				_context.Add(userAccount);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(user);
		}

		private string CreateNewAccountNumber()
		{
			Random r = new Random();
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

		// GET: UserAccounts/Create
		public IActionResult Create()
		{
			ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Id");
			ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
			return View();
		}

		// POST: UserAccounts/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,UserId,AccountId")] UserAccount userAccount)
		{
			if (ModelState.IsValid)
			{
				_context.Add(userAccount);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Id", userAccount.AccountId);
			ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userAccount.UserId);
			return View(userAccount);
		}

		// GET: UserAccounts/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var userAccount = await _context.UserAccounts.FindAsync(id);
			if (userAccount == null)
			{
				return NotFound();
			}
			ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Id", userAccount.AccountId);
			ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userAccount.UserId);
			return View(userAccount);
		}

		// POST: UserAccounts/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,AccountId")] UserAccount userAccount)
		{
			if (id != userAccount.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(userAccount);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!UserAccountExists(userAccount.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Id", userAccount.AccountId);
			ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userAccount.UserId);
			return View(userAccount);
		}

		// GET: UserAccounts/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var userAccount = await _context.UserAccounts
				.Include(u => u.Account)
				.Include(u => u.User)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (userAccount == null)
			{
				return NotFound();
			}

			return View(userAccount);
		}

		// POST: UserAccounts/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var userAccount = await _context.UserAccounts.FindAsync(id);
			_context.UserAccounts.Remove(userAccount);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool UserAccountExists(int id)
		{
			return _context.UserAccounts.Any(e => e.Id == id);
		}
	}
}
