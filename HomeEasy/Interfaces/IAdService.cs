using HomeEasy.Models;

namespace HomeEasy.Interfaces
{
    public interface IAdService
    {
        Task CreateAsync(Ad ad, User user);

        Task<Ad?> GetAdAsync(Guid? id);

        Task EditAdAsync(Ad ad);

        Task DeleteAdAsync(Ad ad);
    }
}