using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BankingWebSite.Models;

namespace BankingWebSite.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly BankingContext _context;

        public TransactionsController(BankingContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var bankingContext = _context.Transactions.Include(t => t.Account);
            return View(await bankingContext.ToListAsync());
        } 

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Account)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Accounts/NewTransaction/5
        public async Task<IActionResult> NewTransaction(int? accountId)
        {
            if (accountId == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = accountId;
            return View();
        }

        // POST: Accounts/NewTransaction/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewTransaction(int accountId, [Bind("Id,AccountId,Date,Amount")] TransactionViewModel transaction)
        {
            if (accountId != transaction.AccountId)
            {
                return NotFound();
            }

            var newTransaction = new Transaction
            {
                AccountId = transaction.AccountId,
                Date = DateTime.UtcNow,
                Amount = transaction.Amount
            };

            if (ModelState.IsValid)
            {
                newTransaction.Amount = -newTransaction.Amount;
                _context.Add(newTransaction);
                var account = await _context.Accounts.FindAsync(accountId);
                account.Balance += newTransaction.Amount;
                _context.Update(account);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Accounts", await _context.Accounts.FindAsync(accountId));
            }
            ViewData["AccountId"] = accountId;
            return View(transaction);
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Id");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccountId,Date,Amount")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Id", transaction.AccountId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Account)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }
    }
}
