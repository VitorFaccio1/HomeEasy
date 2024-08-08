using HomeEasy.Models;

namespace HomeEasy.Interfaces
{
    public interface IUserService
    {
        void CreateAsync(User user);

        Task<User> LoginAsync(string email, string password);

        Task<List<User>> GetWorkersAsync();

        Task<User> GetUserByEmailAsync(string email);
    }
}
