using Infrastructure.Service.Services.User;

namespace Infrastructure.Service.Interfaces
{
    public interface IUserService
    {
        Task AddUserAsync(UserServiceRequest request);
    }
}
