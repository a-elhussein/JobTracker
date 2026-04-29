using Backend.Core.DTOs;

namespace Backend.Core.Services;

public interface IAnalyticsServices
{
    Task<IEnumerable<StatusBreakdownDto>> GetStatusBreakdownAsync();
    Task<IEnumerable<WeeklyApplicationsDto>> GetWeeklyApplicationsAsync(int weeks);
}