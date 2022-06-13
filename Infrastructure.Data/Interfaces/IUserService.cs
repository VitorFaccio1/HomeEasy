using Infrastructure.Data.Entities.User;

namespace Infrastructure.Data.Interfaces
{
    public interface IUserService
    {
        Task AddUserAsync(UserEntity request);

        Task<List<UserEntity>> GetUsersAsync();
    }
}