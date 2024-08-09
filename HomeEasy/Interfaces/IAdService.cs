using HomeEasy.Models;

namespace HomeEasy.Interfaces;

public interface IAdService
{
    Task CreateAsync(Ad ad, User user);

    Task<(List<Ad> Ads, int TotalCount)> GetAdsWithCountAsync(int page, int size, string userId = "");

    Task<Ad?> GetAdAsync(Guid? id);

    Task EditAdAsync(Ad ad);

    Task DeleteAdAsync(Ad ad);
}