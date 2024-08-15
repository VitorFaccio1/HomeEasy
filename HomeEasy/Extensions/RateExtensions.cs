using HomeEasy.Models;

namespace HomeEasy.Extensions;

public static class RateExtensions
{
    public static int GetUserRating(this List<Review> reviews)
    {
        if (!reviews?.Any() ?? true)
            return 0;

        return (int)Math.Round(reviews.Select(review => review.Rating).Average());
    }
}