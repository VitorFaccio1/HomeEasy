using HomeEasy.Data;
using HomeEasy.Interfaces;
using HomeEasy.Models;

namespace HomeEasy.Services;

public sealed class ReviewService : IReviewService
{
    private readonly HomeEasyContext _context;

    public ReviewService(HomeEasyContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Review review, User valuer, User valued, Contract contract)
    {
        review.Date = DateOnly.FromDateTime(DateTime.Now.Date);
        review.Id = Guid.NewGuid();
        review.Reviewer = valuer;

        valued.Reviews.Add(review);

        _context.Reviews.Add(review);

        if (valuer == contract.Contractee)
            contract.ContracteeReviewed = true;
        else
            contract.ContractorReviewed = true;

        await _context.SaveChangesAsync();
    }
}