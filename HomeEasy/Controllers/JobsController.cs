using HomeEasy.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HomeEasy.Controllers;

public sealed class JobsController : Controller
{
    private readonly IJobService _jobService;

    public JobsController(IJobService jobInterface)
    {
        _jobService = jobInterface;
    }

    // GET: Jobs
    public async Task<IActionResult> Index()
    {
        return View(await _jobService.GetJobsAsync());
    }
}