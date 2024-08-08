using HomeEasy.Models;

namespace HomeEasy.Interfaces;

public interface IJobService
{
    Task<List<Job>> GetJobsAsync();

    Task<Job> GetJobAsync(Guid? id);

    Task CreateJobAsync(Job job);

    Task RemoveJobAsync(Job job);

    Task UpdateJobAsync(Job job);
}