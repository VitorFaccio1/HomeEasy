using HomeEasy.Models;

namespace HomeEasy.Interfaces;

public interface IUserService
{
    Task CreateAsync(User user);

    Task<User?> LoginAsync(string email, string password);

    Task<User?> GetUserByIdAsync(string id);

    Task UpdateUserAsync(User user, EditUserModel editUserModel = null);
}