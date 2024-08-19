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

            user.AvaiableAds -= 1;

            await _context.SaveChangesAsync();
        }

        public async Task<(List<Ad> Ads, int TotalCount)> GetAdsWithFiltersAsync(UserType userType, int page = 1, int size = 6, string job = "", int rating = 0, bool expired = false, string userId = "")
        {
            var query = FilterAdsQuery(job, rating, expired, userType, userId);

            int totalCount = await query.CountAsync();

            var ads = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            return (ads, totalCount);
        }

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

        private IQueryable<Ad> FilterAdsQuery(string job, int rating, bool expired, UserType userType, string userId)
        {
            IQueryable<Ad> query = _context.Ads.Include(ad => ad.User.Reviews);

            query = string.IsNullOrEmpty(userId)
                ? query.Where(ad => ad.User.Type == userType)
                : query.Where(ad => ad.User.Id.ToString() == userId);

            query = !expired
                ? query.Where(ad => ad.EndDate >= DateTime.Now)
                : query.Where(ad => ad.EndDate < DateTime.Now);

            if (!string.IsNullOrEmpty(job))
                query = query.Where(ad => ad.Job == job);

            if (rating != 0)
                query = query.Where(ad => (int)ad.User.Reviews.Average(review => review.Rating) == rating);

            return query;
        }
    }
}