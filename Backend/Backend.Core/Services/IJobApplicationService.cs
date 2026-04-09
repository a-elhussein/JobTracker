using Backend.Core.DTOs;
using Backend.Core.Models;

namespace Backend.Core.Services;

public interface IJobApplicationService
{
    Task<PagedResult<JobApplicationResponseDto>> GetAllAsync(string? status, string? company, PaginationParams pagination);
    Task<JobApplication?> GetByIdAsync(Guid id);
    Task<JobApplication> CreateAsync(JobApplication application);
    Task<JobApplication?> UpdateAsync(JobApplication application);
    Task<bool> DeleteAsync(Guid id);
}