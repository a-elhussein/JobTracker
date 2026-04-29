using Backend.Core.DTOs;
using Backend.Core.Services;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Services;

public class AnalyticsService: IAnalyticsServices
{
    private readonly ApplicationDbContext _context;

    public AnalyticsService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<StatusBreakdownDto>> GetStatusBreakdownAsync()
    {
        return await _context.JobApplications
            .GroupBy(j => j.Status)
            .Select(g => new StatusBreakdownDto
            {
                Status = g.Key,
                Count = g.Count()
            }).OrderByDescending(s => s.Count)
            .ToListAsync();
    }

    public async Task<IEnumerable<WeeklyApplicationsDto>> GetWeeklyApplicationsAsync(int weeks)
    {
        var startDate = DateTime.UtcNow.AddDays(-(weeks * 7));

        var applications = await _context.JobApplications
            .Where(j => j.DateApplied >= startDate)
            .ToListAsync();

        return applications
            .GroupBy(j => GetWeekStart(j.DateApplied))
            .Select(g => new WeeklyApplicationsDto
            {
                Week  = g.Key.ToString("yyyy-MM-dd"),
                Count = g.Count()
            })
            .OrderBy(w => w.Week)
            .ToList();
    }

    private static DateTime GetWeekStart(DateTime date)
    {
        var diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
        return date.AddDays(-diff).Date;
    }
}
    
        
    
