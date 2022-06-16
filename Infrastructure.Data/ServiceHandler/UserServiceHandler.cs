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

        public async Task<IEnumerable<UserEntity>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<UserEntity> GetUserAsync(long id)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task UpdateUserAsync(UserEntity user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}