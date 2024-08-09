using HomeEasy.Data;
using HomeEasy.Interfaces;
using HomeEasy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace HomeEasy.Services;

public sealed class UserService : IUserService
{
    private readonly IMemoryCache _cache;
    private readonly HomeEasyContext _context;

    public UserService(IMemoryCache cache, HomeEasyContext context)
    {
        _cache = cache;
        _context = context;
    }

    public async Task CreateAsync(User user)
    {
        user.Id = new Guid();

        user.Password = HashPassword(user);

        _context.Users.Add(user);

        await _context.SaveChangesAsync();
    }

    public async Task<User> LoginAsync(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

        if (user == null)
            return null;

        var result = new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, password);

        if (result == PasswordVerificationResult.Failed)
            return null;

        return user;
    }

    public async Task<List<User>> GetWorkersAsync()
    {

        return await _context.Users
               .Where(user => user.Ads.Any() && (user.Type == UserType.Worker || user.Type == UserType.Admin))
               .Include(user => user.Ads)
               .ToListAsync();

    }

    public async Task<User> GetUserByIdAsync(string id)
    {
        return await _context.Users.Include(user => user.Ads)
                .FirstOrDefaultAsync(user => user.Id.ToString() == id);
    }

    public async Task UpdateUserAsync(User user)
    {
        _context.Users.Update(user);

        await _context.SaveChangesAsync();
    }

    private string HashPassword(User user)
    {
        return new PasswordHasher<User>().HashPassword(user, user.Password);
    }
}