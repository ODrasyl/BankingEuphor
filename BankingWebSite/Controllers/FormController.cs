using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BankingWebSite.Models;
using BankingDatabase.Interface;
using System.Text.Encodings.Web;

namespace BankingWebSite.Controllers
{
	public class FormController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly ITransactionRepository _transactionRepository;

        public FormController(IAccountRepository accountRepository, IUserAccountRepository userAccountRepository, ITransactionRepository transactionRepository)
            => (_accountRepository, _userAccountRepository, _transactionRepository) = (accountRepository, userAccountRepository, transactionRepository);

        // GET: /HelloWorld/
        public IActionResult Index()
		{
			return View();
		}

		// 
		// GET: /HelloWorld/Welcome/ 
		public string Welcome(int name, int ID = 1)
		{
			return HtmlEncoder.Default.Encode($"Hello {name}, ID: {ID}");
        }

        // GET: Accounts/NewTransaction/5
        public IActionResult Create(int? accountId)
        {
            if (accountId == null)
                return NotFound();

            if (_accountRepository.AccountExists(((accountId.HasValue) ? (int)accountId : 0)))
                return NotFound();

            ViewData["AccountId"] = accountId;
            return View();
        }

        // POST: Accounts/NewTransaction/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int accountId, [Bind("Id,AccountId,Date,Amount")] TransactionViewModel transactionViewModel)
        {
            if (accountId != transactionViewModel.AccountId)
                return NotFound();

            var transaction = Tools.ConvertTransaction(transactionViewModel);
            if (ModelState.IsValid)
            {
                await _transactionRepository.CreateNewTransaction(transaction);
                return RedirectToAction(nameof(Index));
            }

            ViewData["AccountId"] = accountId;
            return View(transaction);
        }
    }
}
