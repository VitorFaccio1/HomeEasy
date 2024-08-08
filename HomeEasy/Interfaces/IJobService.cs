using HomeEasy.Models;

namespace HomeEasy.Interfaces;

public interface IJobService
{
    Task<List<Job>> GetJobsAsync();
}