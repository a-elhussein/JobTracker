using Backend.Core.Models;

namespace Backend.Core.Services;

public interface IJobApplicationService
{
    Task<IEnumerable<JobApplication>> GetAllAsync(string? status, string? company);
    Task<JobApplication?> GetByIdAsync(Guid id);
    Task<JobApplication> CreateAsync(JobApplication application);
    Task<JobApplication?> UpdateAsync(JobApplication application);
    Task<bool> DeleteAsync(Guid id);
}