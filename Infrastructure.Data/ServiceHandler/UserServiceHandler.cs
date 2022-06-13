using Infrastructure.Data.Entities.User;
using Infrastructure.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.ServiceHandler
{
    public class UserServiceHandler : IUserService
    {
        private readonly HomeEasyContext _context;

        public UserServiceHandler(HomeEasyContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(UserEntity request)
        {
            await _context.Users.AddAsync(request);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserEntity>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
    }
}