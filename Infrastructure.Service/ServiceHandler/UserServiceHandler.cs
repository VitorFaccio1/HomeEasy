using Infrastructure.Service.Interfaces;
using Infrastructure.Service.Services.User;

namespace Infrastructure.Service.ServiceHandler
{
    public class UserServiceHandler : IUserService
    {
        private readonly HomeEasyContext _context;

        public UserServiceHandler(HomeEasyContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(UserServiceRequest request)
        {
            await _context.Users.AddAsync(request);
        }
    }
}