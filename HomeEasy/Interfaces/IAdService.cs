using HomeEasy.Enums;
using HomeEasy.Models;

namespace HomeEasy.Interfaces;

public interface IAdService
{
    Task CreateAsync(Ad ad, User user);

    Task<List<Ad>> GetClientsNotExpiredAdsAsync(int page, int size);

    Task<List<Ad>> GetWorkersNotExpiredAdsAsync(int page, int size);

    Task<int> GetNotExpiredAdsTotalCountByUserTypeAsync(UserType userType);

    Task<List<Ad>> GetUserNotExpiredAdsAsync(int page, int size, string userId);

    Task<List<Ad>> GetUserExpiredAdsAsync(int page, int size, string userId);

    Task<int> GetUserNotExpiredAdsTotalCountAsync(string userId);

    Task<int> GetUserExpiredAdsTotalCountAsync(string userId);

    Task<Ad?> GetAdAsync(Guid? id);

    Task EditAdAsync(Ad ad);

    Task DeleteAdAsync(Ad ad);
}