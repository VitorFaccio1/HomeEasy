using HomeEasy.Data;
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

            user.Ads.Add(ad);
            _context.Ads.Add(ad);

            await _context.SaveChangesAsync();
        }

        public async Task<Ad?> GetAdAsync(Guid? id)
        {
            return await _context.Ads.FirstOrDefaultAsync(m => m.Id == id);
        }

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