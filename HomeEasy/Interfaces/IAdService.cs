using HomeEasy.Enums;
using HomeEasy.Models;

namespace HomeEasy.Interfaces;

public interface IAdService
{
    Task CreateAsync(Ad ad, User user);

    Task<(List<Ad> Ads, int TotalCount)> GetAdsWithFiltersAsync(UserType userType, int page = 1, int size = 6, string job = "", int rating = 0, bool expired = false, string userId = "");

    Task<Ad?> GetAdAsync(Guid? id);

    Task EditAdAsync(Ad ad);

    Task DeleteAdAsync(Ad ad);
}