using HomeEasy.Enums;
using HomeEasy.Extensions;
using HomeEasy.Interfaces;
using HomeEasy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HomeEasy.Controllers;

[Authorize]
public class AdsController : Controller
{
    private readonly IAdService _adService;
    private readonly IJobService _jobService;
    private readonly IUserService _userService;

    public AdsController(
        IAdService adService,
        IJobService jobService,
        IUserService userService)
    {
        _jobService = jobService;
        _adService = adService;
        _userService = userService;
    }

    public async Task<IActionResult> Clients(string job = "", int rating = 0, int page = 1)
    {
        var size = 3;

        var clientsAds = await _adService.GetAdsWithFiltersAsync(UserType.Client, page, size, job, rating);

        ViewBag.CurrentPage = page;
        ViewBag.Jobs = await _jobService.GetJobsAsync();
        ViewBag.TotalPages = (int)Math.Ceiling(clientsAds.TotalCount / (double)size);
        ViewBag.UserAvailableAds = await _userService.GetUserAvailableAds(User.FindFirst(ClaimTypes.SerialNumber)?.Value);

        if (rating != 0)
            ViewBag.Rating = rating;

        if (!string.IsNullOrEmpty(job))
            ViewBag.Job = job;

        return View(clientsAds.Ads);
    }

    public async Task<IActionResult> Workers(string job = "", int rating = 0, int page = 1)
    {
        var size = 3;

        var workerAds = await _adService.GetAdsWithFiltersAsync(UserType.Worker, page, size, job, rating);

        ViewBag.CurrentPage = page;
        ViewBag.Jobs = await _jobService.GetJobsAsync();
        ViewBag.TotalPages = (int)Math.Ceiling(workerAds.TotalCount / (double)size);
        ViewBag.UserAvailableAds = await _userService.GetUserAvailableAds(User.FindFirst(ClaimTypes.SerialNumber)?.Value);

        if (rating != 0)
            ViewBag.Rating = rating;

        if (!string.IsNullOrEmpty(job))
            ViewBag.Job = job;

        return View(workerAds.Ads);
    }

    public async Task<IActionResult> Details(Guid? id)
    {
        var ad = await _adService.GetAdAsync(id);

        return ad != null
            ? View(ad)
            : NotFound();
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Jobs = await _jobService.GetJobsAsync();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,Description,Job")] Ad ad)
    {
        if (ModelState.IsValid)
        {
            var user = await _userService.GetUserByIdAsync(User.FindFirst(ClaimTypes.SerialNumber)?.Value);

            await _adService.CreateAsync(ad, user);

            return User.IsInRole(UserType.Worker.GetDescription())
                ? RedirectToAction(nameof(Workers))
                : RedirectToAction(nameof(Clients));
        }

        return RedirectToAction(nameof(Create));
    }

    public async Task<IActionResult> Edit(Guid? id, int page = 1)
    {
        ViewBag.Jobs = await _jobService.GetJobsAsync();
        ViewBag.Page = page;

        var ad = await _adService.GetAdAsync(id);

        return ad != null
            ? View(ad)
            : NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([Bind("Id, Title,Description,Job")] Ad ad, Guid id, int page = 1)
    {
        if (id != ad.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            await _adService.EditAdAsync(ad);

            return RedirectToAction("UserAds", "Ads", new { Id = User.FindFirstValue(ClaimTypes.SerialNumber), Page = page });
        }

        return View(ad);
    }

    public async Task<IActionResult> Delete(Guid? id)
    {
        var ad = await _adService.GetAdAsync(id);

        return ad != null
            ? View(ad)
            : NotFound();
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var ad = await _adService.GetAdAsync(id);

        if (ad != null)
        {
            await _adService.DeleteAdAsync(ad);

            return ad.EndDate >= DateTime.Now
                ? RedirectToAction("UserAds", "Ads", new { Id = User.FindFirstValue(ClaimTypes.SerialNumber) })
                : RedirectToAction("MyExpiredAds", "Ads");
        }

        return View(ad);
    }

    public async Task<IActionResult> UserAds(string id, int page = 1)
    {
        var size = 3;

        var user = await _userService.GetUserByIdAsync(id);

        var userAds = await _adService.GetAdsWithFiltersAsync(UserType.Client, page, size, userId: id);

        ViewBag.UserAvailableAds = await _userService.GetUserAvailableAds(User.FindFirst(ClaimTypes.SerialNumber)?.Value);
        ViewBag.TotalPages = (int)Math.Ceiling(userAds.TotalCount / (double)size);
        ViewBag.CurrentPage = page;
        ViewBag.User = user;

        return View(userAds.Ads);
    }

    public async Task<IActionResult> MyExpiredAds(int page = 1)
    {
        var size = 3;

        var userId = User.FindFirst(ClaimTypes.SerialNumber)?.Value;

        var userExpiredAds = await _adService.GetAdsWithFiltersAsync(UserType.Client, page, size, expired: true, userId: userId);

        ViewBag.TotalPages = (int)Math.Ceiling(userExpiredAds.TotalCount / (double)size);
        ViewBag.CurrentPage = page;

        return View(userExpiredAds.Ads);
    }

    public IActionResult Buy()
    {
        return Redirect($"https://buy.stripe.com/test_cN2cPGgg32zObss288?prefilled_email={User.FindFirstValue(ClaimTypes.Email)}");
    }

    public async Task<IActionResult> BuyConfirmed()
    {
        var userId = User.FindFirstValue(ClaimTypes.SerialNumber);

        await _userService.SetUserAvailableAds(userId);

        return RedirectToAction(nameof(UserAds), new { Id = userId });
    }
}