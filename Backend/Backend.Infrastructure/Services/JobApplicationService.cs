using Backend.Core.DTOs;
using Backend.Core.Models;
using Backend.Core.Repositories;
using Backend.Core.Services;

namespace Backend.Infrastructure.Services;

public class JobApplicationService: IJobApplicationService
{
    private readonly IJobApplicationRepository _repository;

    public JobApplicationService(IJobApplicationRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<JobApplicationResponseDto>> GetAllAsync(string? status, string? company, PaginationParams pagination)
    {
        if (pagination.Page < 1)
            throw new ArgumentOutOfRangeException(nameof(pagination), "Page must be at least 1.");
        
        var validSortFields = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "dateApplied", "company", "role", "status"
        };

        if (!validSortFields.Contains(pagination.SortBy))
            pagination.SortBy = "dateApplied"; 

        var (items, totalCount) = await _repository.GetAllAsync(status, company, pagination);

        var dtos = items.Select(j => new JobApplicationResponseDto
        {
            Id = j.Id,
            Company = j.Company,
            Role = j.Role,
            Status = j.Status,
            DateApplied = j.DateApplied,
            Notes = j.Notes,
            SalaryRange = j.SalaryRange,
            Source = j.Source
        });

        return new PagedResult<JobApplicationResponseDto>
        {
            Data = dtos,
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<JobApplication?> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<JobApplication> CreateAsync(JobApplication application)
    {
        return await _repository.AddAsync(application);
    }

    public async Task<JobApplication?> UpdateAsync(JobApplication application)
    {
        return await _repository.UpdateAsync(application);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }
}
