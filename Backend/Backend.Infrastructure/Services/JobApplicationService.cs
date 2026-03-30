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
    public async Task<IEnumerable<JobApplication>> GetAllAsync(string? status, string? company)
    {
        return await _repository.GetAllAsync(status, company);
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
