using Backend.Core.DTOs;
using Backend.Core.Exceptions;
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

        public async Task<JobApplicationResponseDto> GetByIdAsync(Guid id)
    {
        var application = await _repository.GetByIdAsync(id);

        if (application is null)
            throw new NotFoundException(nameof(JobApplication), id);

        return MapToResponseDto(application);
    }

    public async Task<JobApplicationResponseDto> CreateAsync(CreateJobApplicationDto dto)
    {
        var entity = new JobApplication
        {
            Id          = Guid.NewGuid(),
            Company     = dto.Company,
            Role        = dto.Role,
            Status      = dto.Status,
            DateApplied = dto.DateApplied,
            Notes       = dto.Notes,
            SalaryRange = dto.SalaryRange,
            Source      = dto.Source
        };

        var created = await _repository.AddAsync(entity);
        return MapToResponseDto(created);
    }

    public async Task<JobApplicationResponseDto> UpdateAsync(Guid id, UpdateJobApplicationDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);

        if (existing is null)
            throw new NotFoundException(nameof(JobApplication), id);

        existing.Company     = dto.Company;
        existing.Role        = dto.Role;
        existing.Status      = dto.Status;
        existing.DateApplied = dto.DateApplied;
        existing.Notes       = dto.Notes;
        existing.SalaryRange = dto.SalaryRange;
        existing.Source      = dto.Source;

        var updated = await _repository.UpdateAsync(existing);
        return MapToResponseDto(updated!);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await _repository.GetByIdAsync(id);

        if (existing is null)
            throw new NotFoundException(nameof(JobApplication), id);

        return await _repository.DeleteAsync(id);
    }

    private static JobApplicationResponseDto MapToResponseDto(JobApplication j) => new()
    {
        Id          = j.Id,
        Company     = j.Company,
        Role        = j.Role,
        Status      = j.Status,
        DateApplied = j.DateApplied,
        Notes       = j.Notes,
        SalaryRange = j.SalaryRange,
        Source      = j.Source
    };
    
}