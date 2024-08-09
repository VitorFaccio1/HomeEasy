using HomeEasy.Interfaces;
using HomeEasy.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HomeEasy.Controllers;

public sealed class UsersController : Controller
{
    private readonly IUserService _userService;
    private readonly IJobService _jobService;

    public UsersController(IUserService userService, IJobService jobService)
    {
        _jobService = jobService;
        _userService = userService;
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Photo,Email,Password,Name,Type")] User user)
    {
        if (ModelState.IsValid)
        {
            await _userService.CreateAsync(user);

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
                new(ClaimTypes.SerialNumber, user.Id.ToString()),
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.Role, user.Type.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    public async Task<IActionResult> GetWorkers()
    {
        var workers = await _userService.GetWorkersAsync();

        return workers != null
            ? View(workers)
            : NotFound();
    }

    [Authorize]
    public async Task<IActionResult> Details()
    {
        return await UserView();
    }

    [Authorize]
    public async Task<IActionResult> MyAds()
    {
        var user = await _userService.GetUserByIdAsync(User.FindFirst(ClaimTypes.SerialNumber)?.Value);

        return user != null
            ? View(user.Ads)
            : NotFound();
    }

    [Authorize]
    public async Task<IActionResult> Edit()
    {
        return await UserView();
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string email, string name)
    {
        var user = await _userService.GetUserByIdAsync(User.FindFirst(ClaimTypes.SerialNumber)?.Value);

        if (ModelState.IsValid)
        {
            user.Email = email;
            user.Name = name;

            await _userService.UpdateUserAsync(user);

            return RedirectToAction(nameof(Details));
        }

        return View(user);
    }

    [Authorize]
    public async Task<IActionResult> EditPhoto()
    {
        return await UserView();
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPhoto(string photo)
    {
        var user = await _userService.GetUserByIdAsync(User.FindFirst(ClaimTypes.SerialNumber)?.Value);

        if (!string.IsNullOrEmpty(photo))
        {
            user.Photo = photo;

            await _userService.UpdateUserAsync(user);

            return RedirectToAction(nameof(Details));
        }
        else
        {
            ModelState.Clear();
            ModelState.AddModelError("Photo", "A foto é obrigatória.");

            return View(user);
        }
    }

    private async Task<IActionResult> UserView()
    {
        var user = await _userService.GetUserByIdAsync(User.FindFirst(ClaimTypes.SerialNumber)?.Value);

        return user != null
            ? View(user)
            : NotFound();
    }
}