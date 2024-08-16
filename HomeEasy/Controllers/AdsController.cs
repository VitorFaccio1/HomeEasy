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

    public async Task<IActionResult> Clients(string job, int page = 1)
    {
        var size = 9;

        var clientsAds = await _adService.GetClientsNotExpiredAdsAsync(page, size, job);

        var clientAdsTotalCount = clientsAds.Any()
            ? await _adService.GetNotExpiredAdsTotalCountByUserTypeAsync(UserType.Client, job)
            : 0;

        ViewBag.CurrentPage = page;
        ViewBag.Jobs = await _jobService.GetJobsAsync();
        ViewBag.TotalPages = (int)Math.Ceiling(clientAdsTotalCount / (double)size);
        if (!string.IsNullOrEmpty(job))
            ViewBag.Job = job;

        return View(clientsAds);
    }

    public async Task<IActionResult> Workers(string job, int page = 1)
    {
        var size = 3;

        var workerAds = await _adService.GetWorkersNotExpiredAdsAsync(page, size, job);

        var workersAdsTotalCount = workerAds.Any()
            ? await _adService.GetNotExpiredAdsTotalCountByUserTypeAsync(UserType.Worker, job)
            : 0;

        ViewBag.CurrentPage = page;
        ViewBag.Jobs = await _jobService.GetJobsAsync();
        ViewBag.TotalPages = (int)Math.Ceiling(workersAdsTotalCount / (double)size);
        if (!string.IsNullOrEmpty(job))
            ViewBag.Job = job;

        return View(workerAds);
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

            return RedirectToAction("MyAds", "Ads", new { Page = page });
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
                ? RedirectToAction("UserAds", "Ads")
                : RedirectToAction("MyExpiredAds", "Ads");
        }

        return View(ad);
    }

    public async Task<IActionResult> UserAds(string id, int page = 1)
    {
        var size = 3;

        var user = await _userService.GetUserByIdAsync(id);

        var userAds = await _adService.GetUserNotExpiredAdsAsync(page, size, id);

        var userAdsTotalCount = userAds.Any() ? await _adService.GetUserNotExpiredAdsTotalCountAsync(id) : 0;

        ViewBag.TotalPages = (int)Math.Ceiling(userAdsTotalCount / (double)size);
        ViewBag.CurrentPage = page;
        ViewBag.User = user;

        return View(userAds);
    }

    public async Task<IActionResult> MyExpiredAds(int page = 1)
    {
        var size = 3;

        var userId = User.FindFirst(ClaimTypes.SerialNumber)?.Value;

        var userExpiredAds = await _adService.GetUserExpiredAdsAsync(page, size, userId);

        var userExpiredAdsTotalCount = userExpiredAds.Any() ? await _adService.GetUserExpiredAdsTotalCountAsync(userId) : 0;

        ViewBag.TotalPages = (int)Math.Ceiling(userExpiredAdsTotalCount / (double)size);
        ViewBag.CurrentPage = page;

        return View(userExpiredAds);
    }
}