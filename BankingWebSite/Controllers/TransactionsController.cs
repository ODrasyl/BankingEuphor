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
    public class TransactionsController : Controller
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionsController(ITransactionRepository transactionRepository)
            => (_transactionRepository) = (transactionRepository);

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var transactions = _transactionRepository.GetTransactions();
            return View(Tools.ConvertTransactions(await transactions.AsNoTracking().ToListAsync()));
        } 

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var transaction = Tools.ConvertTransaction(await _transactionRepository.GetTransaction(((id.HasValue) ? (int)id : 0)));
            if (transaction == null)
                return NotFound();

            return View(transaction);
        }

        // GET: Accounts/NewTransaction/5
        public IActionResult NewTransaction(int? accountId)
        {
            if (accountId == null)
                return NotFound();

            if (!_transactionRepository.AccountExists(((accountId.HasValue) ? (int)accountId : 0)))
                return NotFound();

            ViewData["AccountId"] = accountId;
            return View();
        }

        // POST: Accounts/NewTransaction/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewTransaction(int accountId, [Bind("AccountId,Amount,IsReceived")] TransactionViewModel transactionViewModel)
        {
            if (accountId != transactionViewModel.AccountId)
                return NotFound();

            var transaction = Tools.ConvertTransaction(transactionViewModel);
            if (ModelState.IsValid)
            {
                int transactionId = await _transactionRepository.CreateNewTransaction(transaction, transactionViewModel.IsReceived);
                if (transactionId == 0)
                {
                    ViewData["AccountId"] = accountId;
                    return View(transactionViewModel); //TODO display error in htlm
                }
                return RedirectToAction("Details", "Accounts", await _transactionRepository.GetAccount(transactionId));
            }

            ViewData["AccountId"] = accountId;
            return View(transactionViewModel); //TODO display error in htlm
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var transaction = Tools.ConvertTransaction(await _transactionRepository.GetTransaction(((id.HasValue) ? (int)id : 0)));
            if (transaction == null)
                return NotFound();

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var accountId = await _transactionRepository.DeletTransaction(id);
            if (!_transactionRepository.AccountExists(accountId))
			{
                return RedirectToAction("Index", "Accounts");
            }
            return RedirectToAction("Details", "Accounts", await _transactionRepository.GetAccountWithAccountId(accountId));
        }
    }
}
