using HomeEasy.Enums;
using HomeEasy.Models;

namespace HomeEasy.Interfaces;

public interface IAdService
{
    Task CreateAsync(Ad ad, User user);

    Task<List<Ad>> GetClientsAdsAsync(int page, int size);

    Task<List<Ad>> GetWorkersAdsAsync(int page, int size);

    Task<int> GetAdsTotalCountByUserTypeAsync(UserType userType);

    Task<List<Ad>> GetUserAdsAsync(int page, int size, string userId);

    Task<int> GetUserAdsTotalCountAsync(string userId);

    Task<Ad?> GetAdAsync(Guid? id);

    Task EditAdAsync(Ad ad);

    Task DeleteAdAsync(Ad ad);
}