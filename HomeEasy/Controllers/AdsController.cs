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
                var user = await _userService.GetUserByEmailAsync(User.FindFirst(ClaimTypes.Email)?.Value);

                await _adService.CreateAsync(ad, user);

                return RedirectToAction("GetWorkers", "Users");
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
        public async Task<IActionResult> Edit(Guid id, [Bind("Title,Description,Job")] Ad ad)
        {
            if (id != ad.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _adService.EditAdAsync(ad);

                return RedirectToAction("GetWorkers", "Users");
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
                _adService.DeleteAdAsync(ad);

            return RedirectToAction("GetWorkers", "Users");
        }
    }
}