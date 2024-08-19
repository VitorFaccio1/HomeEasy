using HomeEasy.Interfaces;
using HomeEasy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HomeEasy.Controllers;

[Authorize]
public class ContractsController : Controller
{
    private readonly IAdService _adService;
    private readonly IUserService _userService;
    private readonly IContractService _contractService;

    public ContractsController(
        IAdService adService,
        IUserService userService,
        IContractService contractService)
    {
        _adService = adService;
        _userService = userService;
        _contractService = contractService;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Guid adId)
    {
        var ad = await _adService.GetAdAsync(adId);

        var contractor = await _userService.GetUserByIdAsync(User.FindFirstValue(ClaimTypes.SerialNumber));

        var contract = new Contract(ad, contractor);

        await _contractService.CreateAsync(contract);

        return RedirectToAction(nameof(Pendings));
    }

    public async Task<IActionResult> Approveds(int page = 1)
    {
        var size = 3;

        var userId = User.FindFirst(ClaimTypes.SerialNumber)?.Value;

        var contracts = await _contractService.GetUserContractsFilteredAsync(userId, page, size, approved: true);

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(contracts.TotalCount / (double)size);

        return View(contracts.Contracts);
    }

    public async Task<IActionResult> Pendings(int page = 1)
    {
        var size = 3;

        var userId = User.FindFirst(ClaimTypes.SerialNumber)?.Value;

        var contracts = await _contractService.GetUserContractsFilteredAsync(userId, page, size);

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(contracts.TotalCount / (double)size);

        return View(contracts.Contracts);
    }

    public async Task<IActionResult> Completeds(int page = 1)
    {
        var size = 3;

        var userId = User.FindFirst(ClaimTypes.SerialNumber)?.Value;

        var contracts = await _contractService.GetUserContractsFilteredAsync(userId, page, size, completed: true);

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(contracts.TotalCount / (double)size);

        return View(contracts.Contracts);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var contract = await _contractService.GetContractById(id);

        ViewBag.LoggedUserId = User.FindFirst(ClaimTypes.SerialNumber)?.Value;

        return View(contract);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApproveContract(Guid id)
    {
        await _contractService.ApproveContractAsync(id);

        return RedirectToAction(nameof(Approveds), new { userId = User.FindFirstValue(ClaimTypes.SerialNumber) });
    }

    public async Task<IActionResult> CompleteContract(Guid id)
    {
        await _contractService.CompleteContractAsync(id);

        return RedirectToAction("Create", "Reviews", new { id });
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var contract = await _contractService.GetContractById(id);

        return contract != null
            ? View(contract)
            : NotFound();
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _contractService.DeleteContractAsync(id);

        return RedirectToAction(nameof(Pendings), new { userId = User.FindFirstValue(ClaimTypes.SerialNumber) });
    }
}