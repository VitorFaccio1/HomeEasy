using HomeEasy.Models;

namespace HomeEasy.Interfaces;

public interface IReviewService
{
    Task CreateAsync(Review review, User reviewer, User reviewed, Contract contractId);
}
