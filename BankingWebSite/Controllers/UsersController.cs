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
#nullable enable
namespace BankingWebSite.Controllers
{
	public class UsersController : Controller
	{
		private readonly IUserRepository _userRepository;

		public UsersController(IUserRepository userRepository)
			=> (_userRepository) = (userRepository);

		// GET: Users
		public async Task<IActionResult> Index()
		{
			var users = _userRepository.GetUsers();
			return View(Tools.ConvertUsers(await users.AsNoTracking().ToListAsync()));
		}

		// GET: Users/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
				return NotFound();

			var user = Tools.ConvertUser(await _userRepository.GetUser((int)id));
			if (user == null)
				return NotFound();

			return View(user);
		}

		// GET: Users/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
				return NotFound();

			var user = Tools.ConvertUser(await _userRepository.GetUser((int)id));
			if (user == null)
				return NotFound();

			return View(user);
		}

		// POST: Users/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Birthday,Address")] UserViewModel userViewModel)
		{
			if (id != userViewModel.Id)
				return NotFound();

			var user = Tools.ConvertUser(userViewModel);

			if (ModelState.IsValid)
			{
				try
				{
					if (!await _userRepository.UpdateUser(user))
						return View(userViewModel); //TODO display error in htlm
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!_userRepository.UserExists(user.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction("Details", "Users", await _userRepository.GetUser(user.Id));
			}
			return View(userViewModel); //TODO display error in htlm
		}
	}
}
