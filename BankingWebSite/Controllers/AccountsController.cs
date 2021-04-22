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
	public class AccountsController : Controller
	{
		private readonly IAccountRepository _accountRepository;

		public AccountsController(IAccountRepository accountRepository)
			=> (_accountRepository) = (accountRepository);

		// GET: Accounts
		public async Task<IActionResult> Index()
		{
			var accounts = _accountRepository.GetAccounts();
			return View(Tools.ConvertAccounts(await accounts.AsNoTracking().ToListAsync()));
		}

		// GET: Accounts/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
				return NotFound();

			var account = Tools.ConvertAccount(await _accountRepository.GetAccount(((id.HasValue) ? (int)id : 0)));
			if (account == null)
				return NotFound();

			return View(account);
		}
	}
}
