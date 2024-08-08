using HomeEasy.Interfaces;
using HomeEasy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeEasy.Controllers
{
    [Authorize]
    public sealed class JobsController : Controller
    {
        private readonly IJobService _jobService;

        public JobsController(IJobService jobService)
        {
            _jobService = jobService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _jobService.GetJobsAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Job job)
        {
            if (ModelState.IsValid)
            {
                await _jobService.CreateJobAsync(job);

                return RedirectToAction(nameof(Index));
            }

            return View(job);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var job = await _jobService.GetJobAsync(id);

            return job != null
                ? View(job)
                : NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name")] Job job)
        {
            if (id != job.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                await _jobService.UpdateJobAsync(job);

                return RedirectToAction(nameof(Index));
            }

            return View(job);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            var job = await _jobService.GetJobAsync(id);

            return job != null
                ? View(job)
                : NotFound();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var job = await _jobService.GetJobAsync(id);

            if (job != null)
                await _jobService.RemoveJobAsync(job);

            return RedirectToAction(nameof(Index));
        }
    }
}