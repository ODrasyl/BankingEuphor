using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BankingDatabase.Interface;
using BankingWebSite.Models;

namespace BankingWebSite.Controllers
{
	public class UserAccountsController : Controller
	{
		private readonly IUserAccountRepository _userAccountRepository;

		public UserAccountsController(IUserAccountRepository userAccountRepository)
			=> (_userAccountRepository) = (userAccountRepository);

		// GET: UserAccounts
		public async Task<IActionResult> Index()
		{
			var userAccounts = _userAccountRepository.GetUserAccounts();
			return View(Tools.ConvertUserAccounts(await userAccounts.AsNoTracking().ToListAsync()));
		}

		// GET: UserAccounts/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
				return NotFound();

			var userAccount = Tools.ConvertUserAccount(await _userAccountRepository.GetUserAccount(((id.HasValue) ? (int)id : 0)));
			if (userAccount == null)
				return NotFound();

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
		public async Task<IActionResult> NewAccount([Bind("FirstName,LastName,Birthday,Address")] UserViewModel userViewModel)
		{
			var user = Tools.ConvertUser(userViewModel);
			if (ModelState.IsValid)
			{
				int userAccountId = await _userAccountRepository.CreateNewAccount(user);
				if (userAccountId == 0)
					return View(userViewModel); //TODO display error in htlm
				return RedirectToAction("Details", "UserAccounts", await _userAccountRepository.GetUserAccount(userAccountId));
			}
			return View(userViewModel); //TODO display error in htlm
		}

		// GET: Transactions/Delete/5
		public async Task<IActionResult> DeleteAccount(int? id) //TODO delet account With popup instead of delet page
		{
			if (id == null)
				return NotFound();

			var userAccount = Tools.ConvertUserAccount(await _userAccountRepository.GetUserAccount(((id.HasValue) ? (int)id : 0)));
			if (userAccount == null)
				return NotFound();

			return View(userAccount);
		}

		// POST: Transactions/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id) //TODO delet account With popup instead of delet page
		{
			if (!await _userAccountRepository.DeletUserAccount(id))
				return RedirectToAction("Details", "Accounts", await _userAccountRepository.GetAccount(id));
			return RedirectToAction("Index", "Accounts");
		}
	}
}
