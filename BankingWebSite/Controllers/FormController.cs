using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BankingWebSite.Models;
using System.Text.Encodings.Web;

namespace BankingWebSite.Controllers
{
	public class FormController : Controller
    {
        private readonly BankingContext _context;

        public FormController(BankingContext context)
        {
            _context = context;
        }

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
        public async Task<IActionResult> Create(int? accountId)
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
            ViewData["AccountId"] = accountId;//new SelectList(_context.Accounts, "Id", "Id");
            return View();
        }

        // POST: Accounts/NewTransaction/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int accountId, [Bind("Id,AccountId,Date,Amount")] Transaction transaction)
        {
            if (accountId != transaction.AccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = accountId;//new SelectList(_context.Accounts, "Id", "Id", transaction.AccountId);
            return View(transaction);
        }
    }
}
