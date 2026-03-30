using Backend.Core.Models;
using Backend.Core.Repositories;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class JobApplicationRepository: IJobApplicationRepository
{
    private readonly ApplicationDbContext _context;

    public JobApplicationRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<JobApplication>> GetAllAsync(string? status, string? company)
    {
        var query = _context.JobApplications.AsQueryable();

        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(x => x.Status == status);
        }

        if (!string.IsNullOrEmpty(company))
        {
            query = query.Where(x => x.Company == company);
        }
        return await query
            .OrderByDescending(x => x.DateApplied)
            .ToListAsync();
    }

    public async Task<JobApplication?> GetByIdAsync(Guid id)
    {
        return await _context.JobApplications.FindAsync(id);
    }

    public async Task<JobApplication> AddAsync(JobApplication application)
    {
        _context.JobApplications.Add(application);
        await _context.SaveChangesAsync();
        return application;
    }

    public async Task<JobApplication?> UpdateAsync(JobApplication application)
    {
        var existingApplication = await _context.JobApplications.FindAsync(application.Id);

        if (existingApplication == null)
            return null;
        
        existingApplication.Company = application.Company;
        existingApplication.Role = application.Role;
        existingApplication.Status = application.Status;
        existingApplication.DateApplied = application.DateApplied;
        existingApplication.Notes = application.Notes;
        existingApplication.SalaryRange = application.SalaryRange;
        existingApplication.Source = application.Source;
        
        await _context.SaveChangesAsync();
        return existingApplication;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var application = await _context.JobApplications.FindAsync(id);
        if (application ==null)
            return false;
        _context.JobApplications.Remove(application);
        await _context.SaveChangesAsync();
        
        return true;
    }
}