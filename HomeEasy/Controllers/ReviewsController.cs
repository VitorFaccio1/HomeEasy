using HomeEasy.Interfaces;
using HomeEasy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HomeEasy.Controllers;

[Authorize]
public class ReviewsController : Controller
{
    private readonly IUserService _userService;
    private readonly IReviewService _reviewService;
    private readonly IContractService _contractService;

    public ReviewsController(
        IUserService userService,
        IReviewService reviewService,
        IContractService contractService)
    {
        _userService = userService;
        _reviewService = reviewService;
        _contractService = contractService;
    }

    public async Task<IActionResult> Create(Guid id)
    {
        var contract = await _contractService.GetContractById(id);

        ViewBag.ContractId = contract.Id;
        ViewBag.Valued = contract.Contractor.Id.ToString() == User.FindFirstValue(ClaimTypes.SerialNumber)
            ? contract.Contractee
            : contract.Contractor;

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Rating,Comment")] Review review, Guid contractId, bool anonymous)
    {
        var contract = await _contractService.GetContractById(contractId);

        var userId = User.FindFirstValue(ClaimTypes.SerialNumber);

        User? valuer;
        User? valued;

        SetValuerAndValued(contract, userId, out valuer, out valued);

        if (ModelState.IsValid)
        {
            await _reviewService.CreateAsync(review, valuer, valued, contract, anonymous);

            return RedirectToAction("Completeds", "Contracts");
        }

        ViewBag.ContractId = contractId;
        ViewBag.Valued = valued;

        return View(review);
    }

    private static void SetValuerAndValued(Contract? contract, string? userId, out User? valuer, out User valued)
    {
        if (contract.Contractor.Id.ToString() == userId)
        {
            valuer = contract.Contractor;
            valued = contract.Contractee;
        }
        else
        {
            valuer = contract.Contractee;
            valued = contract.Contractor;
        }
    }

    public async Task<IActionResult> UserReviews(Guid id)
    {
        var reviews = await _reviewService.GetUserReviews(id);

        return View(reviews);
    }
}