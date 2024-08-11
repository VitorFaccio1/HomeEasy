using HomeEasy.Enums;
using HomeEasy.Interfaces;
using HomeEasy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HomeEasy.Controllers
{
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

        public async Task<IActionResult> Clients(int page = 1)
        {
            var size = 9;

            var clientsAds = await _adService.GetClientsAdsAsync(page, size);

            var clientAdsTotalCount = await _adService.GetAdsTotalCountByUserTypeAsync(UserType.Client);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(clientAdsTotalCount / (double)size);

            return View(clientsAds);
        }

        public async Task<IActionResult> Workers(int page = 1)
        {
            var size = 9;

            var workerAds = await _adService.GetWorkersAdsAsync(page, size);
            var workersAdsTotalCount = await _adService.GetAdsTotalCountByUserTypeAsync(UserType.Worker);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(workersAdsTotalCount / (double)size);

            return View(workerAds);
        }

        public async Task<IActionResult> Details(Guid? id, int page, UserType viewType)
        {
            var ad = await _adService.GetAdAsync(id);

            ViewBag.Page = page;

            SetGoBack(viewType);

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

                return RedirectToAction(nameof(Index), new { userType = user.Type });
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

                return RedirectToAction("MyAds", "Users", new
                {
                    Page = page
                });
            }

            return View(ad);
        }

        public async Task<IActionResult> Delete(Guid? id, int page = 1)
        {
            var ad = await _adService.GetAdAsync(id);
            ViewBag.Page = page;

            SetGoBack(null);

            return ad != null
                ? View(ad)
                : NotFound();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, int page = 1)
        {
            var ad = await _adService.GetAdAsync(id);

            if (ad != null)
            {
                await _adService.DeleteAdAsync(ad);

                return RedirectToAction("MyAds", "Users", new
                {
                    Page = page
                });
            }

            return View(ad);
        }

        private void SetGoBack(UserType? viewType)
        {
            ViewBag.Action = Path.GetFileName(Request.Headers.Referer.ToString());

            if (ViewBag.Action.Contains("MyAds"))
            {
                ViewBag.Controller = "Users";
                ViewBag.Action = "MyAds";
            }
            else
            {
                ViewBag.Controller = "Ads";
                ViewBag.Action = viewType == UserType.Client
                    ? "Clients"
                    : "Workers";
            }
        }
    }
}