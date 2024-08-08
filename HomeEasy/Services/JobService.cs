using HomeEasy.Data;
using HomeEasy.Interfaces;
using HomeEasy.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeEasy.Services;

public sealed class JobService : IJobService
{
    private readonly HomeEasyContext _context;

    public JobService(HomeEasyContext context)
    {
        _context = context;
    }

    public async Task<List<Job>> GetJobsAsync()
    {
        return await _context.Jobs.ToListAsync();
    }
}
