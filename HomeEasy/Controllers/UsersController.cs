using HomeEasy.Extensions;
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
    private readonly IAdService _adService;

    public UsersController(
         IAdService adService,
        IUserService userService)
    {
        _adService = adService;
        _userService = userService;
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Photo,Email,Password,Name,Phone,Country,State,City,Gender,DateOfBirth,Type")] User user)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _userService.CreateAsync(user);
            }
            catch (Exception)
            {
                ModelState.AddModelError("Email", "O e-mail fornecido já está em uso. Por favor, tente um e-mail diferente.");

                return View(user);
            }

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
                new(ClaimTypes.Role, user.Type.GetDescription())
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
    public async Task<IActionResult> Details()
    {
        var user = await _userService.GetUserByIdAsync(User.FindFirst(ClaimTypes.SerialNumber)?.Value);

        return View(user);
    }

    [Authorize]
    public async Task<IActionResult> MyAds(int page = 1)
    {
        var size = 3;

        var userId = User.FindFirst(ClaimTypes.SerialNumber)?.Value;

        var userAds = await _adService.GetUserNotExpiredAdsAsync(page, size, userId);

        var userAdsTotalCount = await _adService.GetUserNotExpiredAdsTotalCountAsync(userId);

        ViewBag.TotalPages = (int)Math.Ceiling(userAdsTotalCount / (double)size);
        ViewBag.CurrentPage = page;

        return View(userAds);
    }

    [Authorize]
    public async Task<IActionResult> MyExpiredAds(int page = 1)
    {
        var size = 3;

        var userId = User.FindFirst(ClaimTypes.SerialNumber)?.Value;

        var userExpiredAds = await _adService.GetUserExpiredAdsAsync(page, size, userId);

        var userExpiredAdsTotalCount = await _adService.GetUserExpiredAdsTotalCountAsync(userId);

        ViewBag.TotalPages = (int)Math.Ceiling(userExpiredAdsTotalCount / (double)size);
        ViewBag.CurrentPage = page;

        return View(userExpiredAds);
    }

    [Authorize]
    public async Task<IActionResult> Edit()
    {
        var user = await _userService.GetUserByIdAsync(User.FindFirst(ClaimTypes.SerialNumber)?.Value);

        return user != null
            ? View(user)
            : NotFound();
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditUserModel editUserModel)
    {
        var user = await _userService.GetUserByIdAsync(User.FindFirst(ClaimTypes.SerialNumber)?.Value);

        if (ModelState.IsValid)
        {
            try
            {
                await _userService.UpdateUserAsync(user, editUserModel);
            }
            catch (Exception)
            {
                ModelState.AddModelError("Email", "O e-mail fornecido já está em uso. Por favor, tente um e-mail diferente.");

                return View(user);
            }

            return RedirectToAction(nameof(Details));
        }

        return View(user);
    }

    [Authorize]
    public async Task<IActionResult> EditPhoto()
    {
        var user = await _userService.GetUserByIdAsync(User.FindFirst(ClaimTypes.SerialNumber)?.Value);

        return user != null
            ? View(user)
            : NotFound();
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
}