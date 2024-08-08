using HomeEasy.Data;
using HomeEasy.Interfaces;
using HomeEasy.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace HomeEasy.Services;

public sealed class JobService : IJobService
{
    private readonly IMemoryCache _cache;
    private readonly HomeEasyContext _context;

    public JobService(HomeEasyContext context, IMemoryCache cache)
    {
        _cache = cache;
        _context = context;
    }

    public async Task<List<Job>> GetJobsAsync()
    {
        var jobsCacheKey = "jobs";

        if (!_cache.TryGetValue(jobsCacheKey, out List<Job> jobs))
        {
            jobs = await _context.Jobs.ToListAsync();

            _cache.Set(jobsCacheKey, jobs, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
            });
        }

        return jobs;
    }

    public async Task<Job> GetJobAsync(Guid? id)
    {
        if (id == null)
            return null;
        else
            return await _context.Jobs.FindAsync(id);
    }

    public async Task CreateJobAsync(Job job)
    {
        if (!await JobAlreadyExists(job.Name))
        {
            _context.Jobs.Add(job);

            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteJobAsync(Job job)
    {
        _context.Jobs.Remove(job);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateJobAsync(Job job)
    {
        var existingJob = await _context.Jobs.FindAsync(job.Id);

        if (existingJob != null)
        {
            existingJob.Name = job.Name;

            _context.Jobs.Update(existingJob);

            await _context.SaveChangesAsync();
        }
    }

    private async Task<bool> JobAlreadyExists(string name) =>
        await _context.Jobs.AnyAsync(job => job.Name.ToLower() == name.ToLower());
}
