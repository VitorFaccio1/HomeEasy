using Infrastructure.Data.Entities.User;

namespace Infrastructure.Data.Interfaces
{
    public interface IUserService
    {
        Task AddUserAsync(UserEntity request);

        Task<IEnumerable<UserEntity>> GetUsersAsync();

        Task<UserEntity> GetUserAsync(long id);

        Task UpdateUserAsync(UserEntity user);
    }
}