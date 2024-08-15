using HomeEasy.Models;

namespace HomeEasy.Interfaces;

public interface IReviewService
{
    Task CreateAsync(Review review, User valuer, User valued, Contract contractId, bool anonymous);

    Task<IEnumerable<Review>> GetUserReviews(Guid userId);
}
