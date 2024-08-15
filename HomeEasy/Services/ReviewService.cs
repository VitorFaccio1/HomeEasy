using HomeEasy.Data;
using HomeEasy.Interfaces;
using HomeEasy.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeEasy.Services;

public sealed class ReviewService : IReviewService
{
    private readonly HomeEasyContext _context;

    public ReviewService(HomeEasyContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Review review, User valuer, User valued, Contract contract, bool anonymous)
    {
        review.Date = DateOnly.FromDateTime(DateTime.Now.Date);
        review.Id = Guid.NewGuid();
        review.ValuerName = !anonymous ? valuer.Name : null;

        valued.Reviews.Add(review);

        _context.Reviews.Add(review);

        if (valuer == contract.Contractee)
            contract.ContracteeReviewed = true;
        else
            contract.ContractorReviewed = true;

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Review>> GetUserReviews(Guid userId) =>
        (await _context.Users.Include(user => user.Reviews)
            .FirstOrDefaultAsync(user => user.Id == userId)).Reviews.OrderBy(review => review.Date);
}