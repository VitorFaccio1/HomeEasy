using HomeEasy.Data;
using HomeEasy.Enums;
using HomeEasy.Interfaces;
using HomeEasy.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeEasy.Services
{
    public sealed class AdService : IAdService
    {
        private readonly HomeEasyContext _context;

        public AdService(HomeEasyContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Ad ad, User user)
        {
            ad.Id = Guid.NewGuid();
            ad.StartDate = DateTime.Now;
            ad.EndDate = DateTime.Now.AddDays(30);
            ad.User = user;

            _context.Ads.Add(ad);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Ad>> GetClientsNotExpiredAdsAsync(int page, int size) =>
            await _context.Ads.Include(ad => ad.User.Reviews)
                .Where(ad => ad.User.Type == UserType.Client && ad.EndDate >= DateTime.Now)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

        public async Task<List<Ad>> GetWorkersNotExpiredAdsAsync(int page, int size) =>
            await _context.Ads.Include(ad => ad.User.Reviews)
                .Where(ad => ad.User.Type == UserType.Worker && ad.EndDate >= DateTime.Now)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

        public async Task<int> GetNotExpiredAdsTotalCountByUserTypeAsync(UserType userType) =>
            await _context.Ads.Include(ad => ad.User)
                .Where(ad => ad.EndDate >= DateTime.Now && ad.User.Type == userType).CountAsync();

        public async Task<List<Ad>> GetUserNotExpiredAdsAsync(int page, int size, string userId) =>
            await _context.Ads
                .Include(ad => ad.User)
                .Where(ad => ad.User.Id.ToString() == userId && ad.EndDate >= DateTime.Now)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

        public async Task<List<Ad>> GetUserExpiredAdsAsync(int page, int size, string userId) =>
            await _context.Ads
                .Include(ad => ad.User)
                .Where(ad => ad.User.Id.ToString() == userId && ad.EndDate < DateTime.Now)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

        public async Task<int> GetUserNotExpiredAdsTotalCountAsync(string userId) =>
            await _context.Ads
                .Include(ad => ad.User)
                .Where(ad => ad.User.Id.ToString() == userId && ad.EndDate >= DateTime.Now)
                .CountAsync();

        public async Task<int> GetUserExpiredAdsTotalCountAsync(string userId) =>
            await _context.Ads
                .Include(ad => ad.User)
                .Where(ad => ad.User.Id.ToString() == userId && ad.EndDate < DateTime.Now)
                .CountAsync();

        public async Task<Ad?> GetAdAsync(Guid? id) =>
            await _context.Ads.Include(ad => ad.User.Reviews).FirstOrDefaultAsync(m => m.Id == id);

        public async Task DeleteAdAsync(Ad ad)
        {
            _context.Ads.Remove(ad);

            await _context.SaveChangesAsync();
        }

        public async Task EditAdAsync(Ad ad)
        {
            var existingAd = await _context.Ads.FindAsync(ad.Id);

            if (existingAd != null)
            {
                existingAd.Title = ad.Title;
                existingAd.Description = ad.Description;
                existingAd.Job = ad.Job;

                _context.Ads.Update(existingAd);

                await _context.SaveChangesAsync();
            }
        }
    }
}