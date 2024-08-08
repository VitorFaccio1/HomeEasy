using HomeEasy.Data;
using HomeEasy.Interfaces;
using HomeEasy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HomeEasy.Services;

public sealed class UserService : IUserService
{
    private readonly HomeEasyContext _context;

    public UserService(HomeEasyContext context)
    {
        _context = context;
    }

    public async void CreateAsync(User user)
    {
        user.Id = new Guid();

        user.Password = new PasswordHasher<User>().HashPassword(user, user.Password);

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

    public async Task<List<User>> GetWorkersAsync() =>
        await _context.Users
            .Include(user => user.Ads)
            .Where(user => user.Ads.Any() && (user.Type == UserType.Worker || user.Type == UserType.Admin))
            .ToListAsync();

    public async Task<User> GetUserByEmailAsync(string email) =>
        await _context.Users.FirstOrDefaultAsync(user => user.Email.ToLower() == email.ToLower());
}