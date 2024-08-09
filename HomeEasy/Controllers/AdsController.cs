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

        public async Task<IActionResult> Index(int page = 1)
        {
            var size = 9;

            var result = await _adService.GetAdsWithCountAsync(page, size);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(result.TotalCount / (double)size);

            return View(result.Ads);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            var ad = await _adService.GetAdAsync(id);

            SetGoBack();

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

                return RedirectToAction("Index", "Ads");
            }

            return RedirectToAction(nameof(Create));
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            ViewBag.Jobs = await _jobService.GetJobsAsync();

            var ad = await _adService.GetAdAsync(id);

            return ad != null
                ? View(ad)
                : NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id, Title,Description,Job")] Ad ad, Guid id)
        {
            if (id != ad.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                await _adService.EditAdAsync(ad);

                return RedirectToAction("MyAds", "Users");
            }

            return View(ad);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            var ad = await _adService.GetAdAsync(id);

            SetGoBack();

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

                return RedirectToAction("MyAds", "Users");
            }

            return View(ad);
        }

        private void SetGoBack()
        {
            ViewBag.Action = Path.GetFileName(Request.Headers.Referer.ToString());

            if (ViewBag.Action.Equals("MyAds"))
            {
                ViewBag.Controller = "Users";
            }
            else
            {
                ViewBag.Action = "Index";
                ViewBag.Controller = "Ads";
            }
        }
    }
}