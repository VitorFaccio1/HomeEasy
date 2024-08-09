using HomeEasy.Data;
using HomeEasy.Interfaces;
using HomeEasy.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace HomeEasy.Services
{
    public sealed class AdService : IAdService
    {
        private readonly IMemoryCache _cache;
        private readonly string _TotalAdsCacheKey = "TotalAds";
        private readonly HomeEasyContext _context;

        public AdService(
            IMemoryCache cache,
            HomeEasyContext context)
        {
            _cache = cache;
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

            UpdateTotalAdsCache(+1);
        }

        public async Task<(List<Ad> Ads, int TotalCount)> GetAdsWithCountAsync(int page, int size, string userId = "")
        {
            IQueryable<Ad> query = _context.Ads.Include(ad => ad.User);

            int totalCount;

            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(m => m.User.Id.ToString() == userId);

                totalCount = await query.CountAsync();
            }
            else
            {
                if (!_cache.TryGetValue(_TotalAdsCacheKey, out totalCount))
                {
                    totalCount = await query.CountAsync();
                    _cache.Set(_TotalAdsCacheKey, totalCount);
                }
            }

            var ads = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            return (ads, totalCount);
        }

        public async Task<Ad?> GetAdAsync(Guid? id)
        {
            return await _context.Ads.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task DeleteAdAsync(Ad ad)
        {
            _context.Ads.Remove(ad);

            await _context.SaveChangesAsync();

            UpdateTotalAdsCache(-1);
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

        private void UpdateTotalAdsCache(int change)
        {
            if (_cache.TryGetValue(_TotalAdsCacheKey, out int totalAds))
            {
                totalAds += change;
                _cache.Set(_TotalAdsCacheKey, totalAds);
            }
        }
    }
}