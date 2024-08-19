using HomeEasy.Data;
using HomeEasy.Extensions;
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

    public async Task CreateAsync(User user)
    {
        if (await UserExistsAsync(user.Email))
            throw new InvalidOperationException();

        user.Id = new Guid();
        user.Password = HashPassword(user);
        user.Phone = user.Phone.NormalizePhoneNumber();

        _context.Users.Add(user);

        await _context.SaveChangesAsync();
    }

    public async Task<User?> LoginAsync(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

        if (user == null)
            return null;

        var result = new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, password);

        return result == PasswordVerificationResult.Failed
            ? null
            : user;
    }

    public async Task<User?> GetUserByIdAsync(string id) =>
        await _context.Users
        .Include(user => user.Reviews)
        .FirstOrDefaultAsync(user => user.Id.ToString() == id);

    public async Task<int> GetUserAvailableAds(string id) =>
        (await _context.Users
        .FirstOrDefaultAsync(user => user.Id.ToString() == id))?.AvaiableAds ?? 0;

    public async Task SetUserAvailableAds(string id)
    {
        var user = _context.Users.FirstOrDefault(user => user.Id.ToString() == id);

        user.AvaiableAds += 1;

        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user, EditUserModel editUserModel = null)
    {
        if (editUserModel != null)
        {
            bool changeEmail = user.Email.ToLower() != editUserModel.Email.ToLower();

            if (changeEmail && await UserExistsAsync(editUserModel.Email))
                throw new InvalidOperationException();

            user.Email = editUserModel.Email;
            user.State = editUserModel.State;
            user.City = editUserModel.City;
            user.Country = editUserModel.Country;
            user.Phone = editUserModel.Phone.NormalizePhoneNumber();
            user.DateOfBirth = editUserModel.DateOfBirth;
            user.Name = editUserModel.Name;
            user.Gender = editUserModel.Gender;
        }

        _context.Users.Update(user);

        await _context.SaveChangesAsync();
    }

    private string HashPassword(User user) =>
        new PasswordHasher<User>().HashPassword(user, user.Password);


    private async Task<bool> UserExistsAsync(string email) =>
        await _context.Users.AnyAsync(user => user.Email.ToLower() == email.ToLower());
}