using Backend.Core.DTOs;
using Backend.Core.Models;

namespace Backend.Core.Repositories;

public interface IJobApplicationRepository
{
    Task<(IEnumerable<JobApplication> Items, int TotalCount)> GetAllAsync(
        string? status,
        string? company,
        PaginationParams pagination);
    Task<JobApplication?> GetByIdAsync(Guid id);
    Task<JobApplication> AddAsync(JobApplication application);
    Task<JobApplication> UpdateAsync(JobApplication application);
    Task<bool> DeleteAsync(Guid id);
}