using System.Security.Claims;
using HomeEasy.Interfaces;
using HomeEasy.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace HomeEasy.Controllers;

public sealed class UsersController : Controller
{
	private readonly IUserService _userService;

	public UsersController(IUserService userService)
	{
		_userService = userService;
	}

	public IActionResult Create()
	{
		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create([Bind("Id,Email,Password,Name,Type")] User user)
	{
		if (ModelState.IsValid)
		{
			_userService.CreateAsync(user);

			return RedirectToAction("Login");
		}

		return View(user);
	}

	public IActionResult Login()
	{
		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Login(string email, string password)
	{
		if (ModelState.IsValid)
		{
			var user = await _userService.LoginAsync(email, password);

			if (user == null)
			{
				ModelState.AddModelError("", "Email ou senha inválidos.");

				return View();
			}

			var claims = new List<Claim>
			{
				new(ClaimTypes.Email, user.Email),
				new(ClaimTypes.Name, user.Name),
				new(ClaimTypes.Role, user.Type.ToString())
			};

			var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

			return RedirectToAction("Index", "Home");
		}

		return View();
	}

	public async Task<IActionResult> Logout()
	{
		await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

		return RedirectToAction("Index", "Home");
	}
}