using Backend.Core.DTOs;

namespace Backend.Core.Services;

public interface IJobApplicationService
{
    Task<PagedResult<JobApplicationResponseDto>> GetAllAsync(string? status, string? company, PaginationParams pagination);
    Task<JobApplicationResponseDto> GetByIdAsync(Guid id);
    Task<JobApplicationResponseDto> CreateAsync(CreateJobApplicationDto dto);
    Task<JobApplicationResponseDto> UpdateAsync(Guid id, UpdateJobApplicationDto dto);
    Task<bool> DeleteAsync(Guid id);
}